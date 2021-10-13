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
    public class CustomerDetailPageViewModel : ViewModelBase, IResponseConnector
    {
        // key: rpro, datatype: CustomerDetailPageModel, property: ClassProperty
        private CustomerDetailPageModel _ClassProperty = new CustomerDetailPageModel();
        public CustomerDetailPageModel ClassProperty { get { return _ClassProperty; } set { _ClassProperty = value; this.RaisePropertyChanged(nameof(ClassProperty)); } }

        #region events and delegates
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; }
        #endregion

        #region variables
        private INavigationService navigationService;
        private readonly RestService restService;
        private readonly NetworkHelper networkHelper;
        CancellationTokenSource cts;
        private IUserDialogs UserDialogs = Acr.UserDialogs.UserDialogs.Instance;
        #endregion

        public CustomerDetailPageViewModel(INavigationService _navigationService, RestService _restService, NetworkHelper _networkHelper) : base(_navigationService)
        {
            navigationService = _navigationService;
            restService = _restService;
            networkHelper = _networkHelper;
            restService.RestResponseDelegate = this;

            BackCommand = new DelegateCommand(BackControl);
            UpdateCommand = new DelegateCommand(UpdateControl);
        }

        async private void UpdateControl()
        {
            ClassProperty.IsValidEmail = true;

            if (!Controls.Utilities.StringUtil.EmailNetIsValid(ClassProperty.Customer.UserEmail))
                ClassProperty.IsValidEmail = false;
            else if (!Controls.Utilities.StringUtil.EmailIsValid(ClassProperty.Customer.UserEmail))
                ClassProperty.IsValidEmail = false;
            else if(string.IsNullOrEmpty(ClassProperty.Customer.UserEmail) || string.IsNullOrWhiteSpace(ClassProperty.Customer.UserEmail))
                ClassProperty.IsValidEmail = false;

            if (ClassProperty.IsValidEmail)
            {
                await InvokeQuery();
            }
        }

        private async Task InvokeQuery()
        {
            if(networkHelper.HasInternet())
            {
                if(await networkHelper.IsHostReachable())
                {
                    cts = new CancellationTokenSource();
                    UserDialogs.ShowLoading();
                    try
                    {
                        string url = Constants.URL_RESPONSE;
                        var json = JsonConvert.SerializeObject(new
                        {
                            ResId = ClassProperty.Customer.ReservationId,
                            UserEmail = ClassProperty.Customer.UserEmail
                        });
                        await restService.Request(EnumHttpMethod.Post, url, ctoken: cts.Token, ws_query: "update_email", dictionary: json);
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

        private void BackControl()
        {
            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("IsToRefresh", ClassProperty.IsUpdated);

            navigationService.GoBackAsync(animated: true, parameters: navParams);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("CustomerItem"))
            {
                ClassProperty.Customer = parameters.GetValue<CustomerModel>("CustomerItem");
            }
        }

        public void ReceiveError(string title, string error, string ws_query) => Acr.UserDialogs.UserDialogs.Instance.Alert(error, title, Constants.AlertPositiveLabel);

        public void ReceiveJSONData(string jsonString, string ws_query)
        {
            Device.BeginInvokeOnMainThread(() => UserDialogs.HideLoading());

            switch (ws_query)
            {
                case "update_email":
                    ClassProperty.IsUpdated = true;
                    BackControl();
                    break;
                default:
                    break;
            }
        }
    }
}
