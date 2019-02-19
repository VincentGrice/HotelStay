using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models.ViewModels
{
    public class ReservationDetailsViewModel
    {
        public Reservations Reservation { get; set; }
        public List<ApplicationUser> SalesPerson { get; set; }
        public List<Rooms> Rooms { get; set; }
    }
}
