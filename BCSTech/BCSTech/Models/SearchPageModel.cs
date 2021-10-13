using System;
using System.Collections.ObjectModel;
using BCSTech.Services.Predefined;
using Prism.Mvvm;

namespace BCSTech.Models
{
    public class SearchPageModel : BindableBase
    {
        // key: opro, datatype: string, property: CodeKey
        private string _CodeKey = "";
        public string CodeKey { get { return _CodeKey; } set { _CodeKey = value; this.RaisePropertyChanged(nameof(CodeKey)); } }

        // key: opro, datatype: bool, property: IsValidParkCode
        private bool _IsValidParkCode = true;
        public bool IsValidParkCode { get { return _IsValidParkCode; } set { _IsValidParkCode = value; this.RaisePropertyChanged(nameof(IsValidParkCode)); } }

        // key: opro, datatype: DateTime, property: SelectedDate
        private DateTime _SelectedDate = DateTime.Today;
        public DateTime SelectedDate { get { return _SelectedDate; } set { _SelectedDate = value; this.RaisePropertyChanged(nameof(SelectedDate)); } }

        // key: opro, datatype: bool, property: IsValidDate
        private bool _IsValidDate = true;
        public bool IsValidDate { get { return _IsValidDate; } set { _IsValidDate = value; this.RaisePropertyChanged(nameof(IsValidDate)); } }

        // key: opro, datatype: ObservableCollection<CustomerModel>, property: Customers
        private ObservableCollection<CustomerModel> _Customers;
        public ObservableCollection<CustomerModel> Customers { get { return _Customers; } set { _Customers = value; this.RaisePropertyChanged(nameof(Customers)); } }

        // key: opro, datatype: bool, property: HasData
        private bool _HasData = false;
        public bool HasData { get { return _HasData; } set { _HasData = value; this.RaisePropertyChanged(nameof(HasData)); } }
    }
}
