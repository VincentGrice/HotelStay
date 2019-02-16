using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models.ViewModels
{
    public class RoomsViewModel
    {
        public Rooms Rooms { get; set; }
        public IEnumerable<RoomTypes> RoomTypes { get; set; }
        public IEnumerable<RoomTags> RoomTags { get; set; }
    }
}
