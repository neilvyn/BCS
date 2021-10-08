using System;
using BCSTech.Services.Predefined;

namespace BCSTech.Models
{
    public class CustomerModel : BindablePropertyBase
    {
        // key: opro, datatype: string, property: ReservationId
        private string _ReservationId;
        public string ReservationId { get { return _ReservationId; } set { _ReservationId = value; this.OnPropertyChanged(nameof(ReservationId)); } }

        // key: opro, datatype: string, property: GuestName
        private string _GuestName;
        public string GuestName { get { return _GuestName; } set { _GuestName = value; this.OnPropertyChanged(nameof(GuestName)); } }

        // key: opro, datatype: string, property: GuestMobile
        private string _GuestMobile;
        public string GuestMobile { get { return _GuestMobile; } set { _GuestMobile = value; this.OnPropertyChanged(nameof(GuestMobile)); } }

        // key: opro, datatype: string, property: Arrived
        private string _Arrived;
        public string Arrived { get { return _Arrived; } set { _Arrived = value; this.OnPropertyChanged(nameof(Arrived)); } }

        // key: opro, datatype: string, property: Depart
        private string _Depart;
        public string Depart { get { return _Depart; } set { _Depart = value; this.OnPropertyChanged(nameof(Depart)); } }

        // key: opro, datatype: string, property: Category
        private string _Category;
        public string Category { get { return _Category; } set { _Category = value; this.OnPropertyChanged(nameof(Category)); } }

        // key: opro, datatype: string, property: Nights
        private string _Nights;
        public string Nights { get { return _Nights; } set { _Nights = value; this.OnPropertyChanged(nameof(Nights)); } }

        // key: opro, datatype: string, property: AreaName
        private string _AreaName;
        public string AreaName { get { return _AreaName; } set { _AreaName = value; this.OnPropertyChanged(nameof(AreaName)); } }

        // key: opro, datatype: int, property: PreviousNPS
        private int? _PreviousNPS;
        public int? PreviousNPS { get { return _PreviousNPS; } set { _PreviousNPS = value; this.OnPropertyChanged(nameof(PreviousNPS)); } }

        // key: opro, datatype: string, property: PreviousNPSComment
        private string _PreviousNPSComment;
        public string PreviousNPSComment { get { return _PreviousNPSComment; } set { _PreviousNPSComment = value; this.OnPropertyChanged(nameof(PreviousNPSComment)); } }

        // key: opro, datatype: string, property: UserEmail
        private string _UserEmail = "";
        public string UserEmail { get { return _UserEmail; } set { _UserEmail = value; this.OnPropertyChanged(nameof(UserEmail)); } }
    }
}
