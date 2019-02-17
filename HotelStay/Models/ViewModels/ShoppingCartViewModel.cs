using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Rooms> Rooms { get; set; }

        public Reservations Reservations { get; set; }

    }
}
