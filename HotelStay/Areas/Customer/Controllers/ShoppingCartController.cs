using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Extensions;
using HotelStay.Models;
using HotelStay.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelStay.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                Rooms = new List<Models.Rooms>()
            };
        }

        //GET Index Shopping Cart
        public async Task<IActionResult> Index()
        {
            List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                foreach(int cartItem in lstShoppingCart)
                {
                    Rooms room = _db.Rooms.Include(r => r.RoomTags).Include(r=>r.RoomTypes).Where(r => r.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.Rooms.Add(room);
                }
            }
            return View(ShoppingCartVM);
        }

        //POST Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            ShoppingCartVM.Reservations.ReservationDate = ShoppingCartVM.Reservations.ReservationDate
                .AddHours(ShoppingCartVM.Reservations.ReservationTime.Hour)
                .AddMinutes(ShoppingCartVM.Reservations.ReservationTime.Minute);

            Reservations reservations = ShoppingCartVM.Reservations;
            _db.Reservations.Add(reservations);
            _db.SaveChanges();

            int reservationId = reservations.Id;

            foreach(int roomId in lstCartItems)
            {
                RoomsSelectedForReservations roomsSelectedForReservation = new RoomsSelectedForReservations()
                {
                    ReservationId = reservationId,
                    RoomId = roomId
                };
                _db.RoomsSelectedForReservations.Add(roomsSelectedForReservation);

            }
            _db.SaveChanges();
            lstCartItems = new List<int>();
            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("ReservationConfirmation","ShoppingCart", new { Id = reservationId });
        }

        //Delete Action Method to remove cart item
        public IActionResult Remove(int id)
        {
            List<int> lstCartItems = HttpContext.Session.Get<List<int>>("ssShoppingCart");

            if(lstCartItems.Count > 0)
            {
                if (lstCartItems.Contains(id))
                {
                    lstCartItems.Remove(id);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }

        //GET Action Method for Reservations
        public IActionResult ReservationConfirmation(int id)
        {
            ShoppingCartVM.Reservations = _db.Reservations.Where(r => r.Id == id).FirstOrDefault();
            List<RoomsSelectedForReservations> objRoomList = _db.RoomsSelectedForReservations.Where(r => r.ReservationId == id).ToList();

            foreach(RoomsSelectedForReservations roomResObj in objRoomList)
            {
                ShoppingCartVM.Rooms.Add(_db.Rooms.Include(r => r.RoomTypes).Include(r => r.RoomTags).Where(r => r.Id == roomResObj.RoomId).FirstOrDefault());
            }

            return View(ShoppingCartVM);
        }
    }
}