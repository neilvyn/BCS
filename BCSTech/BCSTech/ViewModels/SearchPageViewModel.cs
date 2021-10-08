using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using BCSTech.Models;
using BCSTech.Services.Predefined;
using BCSTech.Services.Rest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace BCSTech.ViewModels
{
    public class SearchPageViewModel : ViewModelBase, IRestConnector
    {
        // key: rpro, datatype: SearchPageModel, property: ClassProperty
        private SearchPageModel _ClassProperty = new SearchPageModel();
        public SearchPageModel ClassProperty { get { return _ClassProperty; } set { _ClassProperty = value; this.RaisePropertyChanged(nameof(ClassProperty)); } }

        #region events and delegates
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<object> ItemTappedCommand { get; set; }
        #endregion

        #region variables
        INavigationService navigationServices;
        CancellationTokenSource cts = new CancellationTokenSource();
        RestService restService = new RestService();
        #endregion

        public SearchPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            navigationServices = navigationService;
            restService.RestServiceDelegate = this;

            SearchCommand = new DelegateCommand(SearchControl);
            ItemTappedCommand = new DelegateCommand<object>(ItemTappedAction);
        }

        private void ItemTappedAction(object obj)
        {
            var cust = obj as CustomerModel;
            LogConsole.AsyncOutput(this, cust.GuestName);

            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("CustomerItem", cust);

            navigationServices.NavigateAsync(Constants.CustomerDetailPage, parameters: navParams, animated: true);
        }

        async private void SearchControl()
        {
            ClassProperty.Customers = new System.Collections.ObjectModel.ObservableCollection<CustomerModel>();
            ClassProperty.IsValidParkCode = !((string.IsNullOrEmpty(ClassProperty.CodeKey) || string.IsNullOrWhiteSpace(ClassProperty.CodeKey)));
            ClassProperty.IsValidDate = !((ClassProperty.SelectedDate > DateTime.Today));

            if(ClassProperty.IsValidParkCode && ClassProperty.IsValidDate)
            {
                await InvokeQuery();
            }
        }

        private async Task InvokeQuery()
        {
            cts = new CancellationTokenSource();
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading();
            try
            {
                string url = Constants.URL_CUSTOMERS + "?parkCode=" + ClassProperty.CodeKey + "&arriving=" + Convert.ToDateTime(ClassProperty.SelectedDate).ToString("yyyy-MM-dd");
                await restService.GetRequest(url, cts.Token, wsType: 1);
                //await restService.GetRequest(url, cts.Token, wsType: 1, authHeader: Constants.AUTH_HEADER);
            }
            catch (OperationCanceledException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            catch (TimeoutException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            catch (Exception ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            cts = null;
        }

        async public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("IsToRefresh"))
            {
                if (parameters.GetValue<bool>("IsToRefresh"))
                {
                    Acr.UserDialogs.UserDialogs.Instance.Alert(Constants.DefaultSuccessAlert, null, Constants.AlertPositiveLabel);
                    await InvokeQuery();
                }
            }
        }

        public void ReceiveError(string title, string error, int wsType) => Acr.UserDialogs.UserDialogs.Instance.Alert(error, title, Constants.AlertPositiveLabel);

        public void ReceiveJSONData(string jsonString, int wsType)
        {
            Device.BeginInvokeOnMainThread(() => Acr.UserDialogs.UserDialogs.Instance.HideLoading());
            var jsonData = Controls.Utilities.StringUtil.ToJObject(jsonString);

            if (jsonData != null)
            {
                switch (wsType)
                {
                    case 1:
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
