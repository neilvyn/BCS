using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BCSTech.Models;
using BCSTech.Services.Network;
using BCSTech.Services.Predefined;
using BCSTech.Services.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace BCSTech.ViewModels
{
    public class SearchPageViewModel : ViewModelBase, IResponseConnector
    {
        // key: rpro, datatype: SearchPageModel, property: ClassProperty
        private SearchPageModel _ClassProperty = new SearchPageModel();
        public SearchPageModel ClassProperty { get { return _ClassProperty; } set { _ClassProperty = value; this.RaisePropertyChanged(nameof(ClassProperty)); } }

        #region events and delegates
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<object> ItemTappedCommand { get; set; }
        #endregion

        #region variables
        private INavigationService navigationService;
        private readonly RestService restService;
        private readonly NetworkHelper networkHelper;
        private IUserDialogs UserDialogs = Acr.UserDialogs.UserDialogs.Instance;
        CancellationTokenSource cts;
        #endregion

        public SearchPageViewModel(INavigationService _navigationService, RestService _restService, NetworkHelper _networkHelper) : base(_navigationService)
        {
            navigationService = _navigationService;
            restService = _restService;
            networkHelper = _networkHelper;
            restService.RestResponseDelegate = this;

            SearchCommand = new DelegateCommand(SearchControl);
            ItemTappedCommand = new DelegateCommand<object>(ItemTappedAction);
        }

        private void ItemTappedAction(object obj)
        {
            var cust = obj as CustomerModel;
            LogConsole.AsyncOutput(this, cust.GuestName);

            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("CustomerItem", cust);

            navigationService.NavigateAsync(Constants.CustomerDetailPage, parameters: navParams, animated: true);
        }

        async private void SearchControl()
        {
            ClassProperty.Customers = new System.Collections.ObjectModel.ObservableCollection<CustomerModel>();
            ClassProperty.IsValidParkCode = !((string.IsNullOrEmpty(ClassProperty.CodeKey) || string.IsNullOrWhiteSpace(ClassProperty.CodeKey)));
            ClassProperty.IsValidDate = !((ClassProperty.SelectedDate > DateTime.Today));

            if(ClassProperty.IsValidParkCode && ClassProperty.IsValidDate)
            {
                await InvokeSearchQuery();
            }
        }

        private async Task InvokeSearchQuery()
        {
            if(networkHelper.HasInternet())
            {
                if(await networkHelper.IsHostReachable())
                {
                    cts = new CancellationTokenSource();
                    UserDialogs.ShowLoading();
                    try
                    {
                        string url = Constants.URL_CUSTOMERS + "?parkCode=" + ClassProperty.CodeKey + "&arriving=" + Convert.ToDateTime(ClassProperty.SelectedDate).ToString("yyyy-MM-dd");
                        await restService.Request(EnumHttpMethod.Get, url, ctoken: cts.Token, ws_query: "search_keyword");
                    }
                    catch (OperationCanceledException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
                    catch (TimeoutException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
                    catch (Exception ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
                    cts = null;
                }
                else
                    UserDialogs.Alert(Constants.HOST_UNREACHABLE.Description, Constants.HOST_UNREACHABLE.Title, Constants.AlertPositiveLabel);
            }
            else
                UserDialogs.Alert(Constants.NO_CONNECTION.Description, Constants.NO_CONNECTION.Title, Constants.AlertPositiveLabel);
        }

        async public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("IsToRefresh"))
            {
                if (parameters.GetValue<bool>("IsToRefresh"))
                {
                    UserDialogs.Alert(Constants.DefaultSuccessAlert, null, Constants.AlertPositiveLabel);
                    await InvokeSearchQuery();
                }
            }
        }

        public void ReceiveError(string title, string error, string ws_query) => Acr.UserDialogs.UserDialogs.Instance.Alert(error, title, Constants.AlertPositiveLabel);

        public void ReceiveJSONData(string jsonString, string ws_query)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.HideLoading());
            var jsonData = Controls.Utilities.StringUtil.ToJObject(jsonString);

            if (jsonData != null)
            {
                switch (ws_query)
                {
                    case "search_keyword":
                        if (jsonData.ContainsKey("obj"))
                        {
                            ClassProperty.Customers = JsonConvert.DeserializeObject<ObservableCollection<CustomerModel>>(jsonData["obj"].ToString());
                        }
                        break;
                    default:
                        break;
                }
                
            }

            ClassProperty.HasData = ClassProperty.Customers.Count > 0;
        }
    }
}
