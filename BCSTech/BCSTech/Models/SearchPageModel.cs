using System;
using System.Collections.ObjectModel;
using BCSTech.Services.Predefined;

namespace BCSTech.Models
{
    public class SearchPageModel : BindablePropertyBase
    {
        // key: opro, datatype: string, property: CodeKey
        private string _CodeKey = "";
        public string CodeKey { get { return _CodeKey; } set { _CodeKey = value; this.OnPropertyChanged(nameof(CodeKey)); } }

        // key: opro, datatype: bool, property: IsValidParkCode
        private bool _IsValidParkCode = true;
        public bool IsValidParkCode { get { return _IsValidParkCode; } set { _IsValidParkCode = value; this.OnPropertyChanged(nameof(IsValidParkCode)); } }

        // key: opro, datatype: DateTime, property: SelectedDate
        private DateTime _SelectedDate = DateTime.Today;
        public DateTime SelectedDate { get { return _SelectedDate; } set { _SelectedDate = value; this.OnPropertyChanged(nameof(SelectedDate)); } }

        // key: opro, datatype: bool, property: IsValidDate
        private bool _IsValidDate = true;
        public bool IsValidDate { get { return _IsValidDate; } set { _IsValidDate = value; this.OnPropertyChanged(nameof(IsValidDate)); } }

        // key: opro, datatype: ObservableCollection<CustomerModel>, property: Customers
        private ObservableCollection<CustomerModel> _Customers;
        public ObservableCollection<CustomerModel> Customers { get { return _Customers; } set { _Customers = value; this.OnPropertyChanged(nameof(Customers)); } }

        // key: opro, datatype: bool, property: HasData
        private bool _HasData = false;
        public bool HasData { get { return _HasData; } set { _HasData = value; this.OnPropertyChanged(nameof(HasData)); } }
    }
}
