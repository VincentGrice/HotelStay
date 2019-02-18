using System;
using System.Collections.Generic;
using System.Text;
using HotelStay.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelStay.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<RoomTypes> RoomTypes { get; set; }
        public DbSet<RoomTags> RoomTags { get; set; }
        public DbSet<Rooms> Rooms { get; set; }

        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<RoomsSelectedForReservations> RoomsSelectedForReservations { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}
