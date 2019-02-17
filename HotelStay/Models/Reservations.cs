using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models
{
    public class Reservations
    {
        public int Id { get; set; }

        public DateTime ReservationDate { get; set; }

        [NotMapped]
        public DateTime ReservationTime { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public bool isConfirmed { get; set; }
    }
}
