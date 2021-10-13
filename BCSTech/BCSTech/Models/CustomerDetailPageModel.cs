using System;
using System.Collections.ObjectModel;
using BCSTech.Services.Predefined;
using Prism.Mvvm;

namespace BCSTech.Models
{
    public class CustomerDetailPageModel : BindableBase
    {
        // key: opro, datatype: CustomerModel, property: Customer
        private CustomerModel _Customer;
        public CustomerModel Customer { get { return _Customer; } set { _Customer = value; this.RaisePropertyChanged(nameof(Customer)); } }

        // key: opro, datatype: bool, property: IsValidEmail
        private bool _IsValidEmail = true;
        public bool IsValidEmail { get { return _IsValidEmail; } set { _IsValidEmail = value; this.RaisePropertyChanged(nameof(IsValidEmail)); } }

        // key: opro, datatype: bool, property: IsUpdated
        private bool _IsUpdated = false;
        public bool IsUpdated { get { return _IsUpdated; } set { _IsUpdated = value; this.RaisePropertyChanged(nameof(IsUpdated)); } }
    }
}
