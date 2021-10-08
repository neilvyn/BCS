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
    public class CustomerDetailPageViewModel : ViewModelBase, IRestConnector
    {
        // key: rpro, datatype: CustomerDetailPageModel, property: ClassProperty
        private CustomerDetailPageModel _ClassProperty = new CustomerDetailPageModel();
        public CustomerDetailPageModel ClassProperty { get { return _ClassProperty; } set { _ClassProperty = value; this.RaisePropertyChanged(nameof(ClassProperty)); } }

        #region events and delegates
        public DelegateCommand BackCommand { get; set; }
        public DelegateCommand UpdateCommand { get; set; }
        #endregion

        #region variables
        INavigationService navigationServices;
        CancellationTokenSource cts = new CancellationTokenSource();
        RestService restService = new RestService();
        #endregion

        public CustomerDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            navigationServices = navigationService;
            restService.RestServiceDelegate = this;

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
            cts = new CancellationTokenSource();
            Acr.UserDialogs.UserDialogs.Instance.ShowLoading();
            try
            {
                string url = Constants.URL_RESPONSE;
                var json = JsonConvert.SerializeObject(new
                {
                    ResId = ClassProperty.Customer.ReservationId,
                    UserEmail = ClassProperty.Customer.UserEmail
                });
                await restService.PostRequestAsync(url, json, cts.Token, 1);
                //await restService.PostRequestAsync(url, json, cts.Token, 2, authHeader: Constants.AUTH_HEADER);
            }
            catch (OperationCanceledException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            catch (TimeoutException ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            catch (Exception ex) { LogConsole.AsyncOutput(this, "Error : " + ex); }
            cts = null;
        }

        private void BackControl()
        {
            NavigationParameters navParams = new NavigationParameters();
            navParams.Add("IsToRefresh", ClassProperty.IsUpdated);

            navigationServices.GoBackAsync(animated: true, parameters: navParams);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("CustomerItem"))
            {
                ClassProperty.Customer = parameters.GetValue<CustomerModel>("CustomerItem");
            }
        }

        public void ReceiveError(string title, string error, int wsType) => Acr.UserDialogs.UserDialogs.Instance.Alert(error, title, Constants.AlertPositiveLabel);

        public void ReceiveJSONData(string jsonString, int wsType)
        {
            Device.BeginInvokeOnMainThread(() => Acr.UserDialogs.UserDialogs.Instance.HideLoading());
            switch (wsType)
            {
                case 1:
                    ClassProperty.IsUpdated = true;
                    BackControl();
                    break;
                default:
                    break;
            }
        }
    }
}
