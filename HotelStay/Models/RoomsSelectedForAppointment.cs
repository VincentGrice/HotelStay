using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models
{
    public class RoomsSelectedForAppointment
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public virtual Reservations Reservations { get; set; }

        public int RoomId { get; set; }

        [ForeignKey("RoomId")]
        public virtual Rooms Rooms { get; set; }

    }
}
