using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HotelStay.Models
{
    public class Rooms
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool Available { get; set; }

        public string Image { get; set; }

        public bool NonSmoking { get; set; }


        [Display(Name="Room Type")]
        public int RoomTypeId { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomTypes {get; set;}



        [Display(Name = "Room Tag")]
        public int RoomTagsID { get; set; }

        [ForeignKey("RoomTagsID")]
        public virtual RoomTags RoomTags { get; set; }

    }
}
