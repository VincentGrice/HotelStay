using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Models;
using HotelStay.Models.ViewModels;
using HotelStay.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelStay.Areas.Admin.Controllers
{
    //[Authorize(Roles= SD.AdminEndUser + "," + SD.SuperAdminEndUser)]
    [Area("Admin")]
    public class ReservationsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ReservationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string searchName = null, string searchEmail = null, string searchPhone = null, string searchDate = null)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ReservationViewModel reservationVM = new ReservationViewModel()
            {
                Reservations = new List<Models.Reservations>()
            };

            reservationVM.Reservations = _db.Reservations.Include(r => r.SalesPerson).ToList();
            if (User.IsInRole(SD.AdminEndUser))
            {
                reservationVM.Reservations = reservationVM.Reservations.Where(r => r.SalesPersonId == claim.Value).ToList();
            }

            if (searchName != null)
            {
                reservationVM.Reservations = reservationVM.Reservations.Where(r => r.CustomerName.ToLower().Contains(searchName.ToLower())).ToList();
            }
            if (searchEmail != null)
            {
                reservationVM.Reservations = reservationVM.Reservations.Where(r => r.CustomerEmail.ToLower().Contains(searchEmail.ToLower())).ToList();
            }
            if (searchPhone != null)
            {
                reservationVM.Reservations = reservationVM.Reservations.Where(r => r.CustomerPhoneNumber.ToLower().Contains(searchPhone.ToLower())).ToList();
            }
            if (searchDate != null)
            {
                try
                {
                    DateTime resDate = Convert.ToDateTime(searchDate);
                    reservationVM.Reservations = reservationVM.Reservations.Where(r => r.ReservationDate.ToShortDateString().Equals(resDate.ToShortDateString())).ToList();
                }
                catch (Exception ex)
                {

                }
            }

            return View(reservationVM);
        }

        //GET : Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var roomList = (IEnumerable<Rooms>)(from r in _db.Rooms
                                             join a in _db.RoomsSelectedForReservations
                                             on r.Id equals a.RoomId
                                             where a.ReservationId == id
                                             select r).Include("RoomTypes");

            ReservationDetailsViewModel objReservationVM = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservations.Include(r => r.SalesPerson).Where(r => r.Id == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Rooms = roomList.ToList()
            };

            return View(objReservationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReservationDetailsViewModel objReservationVM)
        {
            if (ModelState.IsValid)
            {
                objReservationVM.Reservation.ReservationDate = objReservationVM.Reservation.ReservationDate
                                                                .AddHours(objReservationVM.Reservation.ReservationTime.Hour)
                                                                .AddMinutes(objReservationVM.Reservation.ReservationTime.Minute);

                var reservationFromDb = _db.Reservations.Where(r => r.Id == objReservationVM.Reservation.Id).FirstOrDefault();

                reservationFromDb.CustomerName = objReservationVM.Reservation.CustomerName;
                reservationFromDb.CustomerEmail = objReservationVM.Reservation.CustomerEmail;
                reservationFromDb.CustomerPhoneNumber = objReservationVM.Reservation.CustomerPhoneNumber;
                reservationFromDb.ReservationDate = objReservationVM.Reservation.ReservationDate;
                reservationFromDb.isConfirmed = objReservationVM.Reservation.isConfirmed;

                if (User.Identity.IsAuthenticated)
                {
                    reservationFromDb.SalesPersonId = objReservationVM.Reservation.SalesPersonId;
                }

                 _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            return View(objReservationVM);
        }

        //GET : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomList = (IEnumerable<Rooms>)(from r in _db.Rooms
                                                join a in _db.RoomsSelectedForReservations
                                                on r.Id equals a.RoomId
                                                where a.ReservationId == id
                                                select r).Include("RoomTypes");

            ReservationDetailsViewModel objReservationVM = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservations.Include(r => r.SalesPerson).Where(r => r.Id == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Rooms = roomList.ToList()
            };

            return View(objReservationVM);
        }

        //GET : Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roomList = (IEnumerable<Rooms>)(from r in _db.Rooms
                                                join a in _db.RoomsSelectedForReservations
                                                on r.Id equals a.RoomId
                                                where a.ReservationId == id
                                                select r).Include("RoomTypes");

            ReservationDetailsViewModel objReservationVM = new ReservationDetailsViewModel()
            {
                Reservation = _db.Reservations.Include(r => r.SalesPerson).Where(r => r.Id == id).FirstOrDefault(),
                SalesPerson = _db.ApplicationUser.ToList(),
                Rooms = roomList.ToList()
            };

            return View(objReservationVM);
        }

        //POST : Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>DeleteConfirmed(int id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            _db.Reservations.Remove(reservation);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}