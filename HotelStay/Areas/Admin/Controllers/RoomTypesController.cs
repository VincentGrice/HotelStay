using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomTypesController : Controller
    {
        private readonly ApplicationDbContext _db;
        //Dependency Injection
        public RoomTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View(_db.RoomTypes.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomTypes roomTypes)
        {
            if (ModelState.IsValid)
            {
                _db.Add(roomTypes);
                //use await keyword when using async methods
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypes);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var roomType = await _db.RoomTypes.FindAsync(id);
            if(roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomTypes roomTypes)
        {
            if (id != roomTypes.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(roomTypes);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomTypes);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomType = await _db.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomType = await _db.RoomTypes.FindAsync(id);
            if (roomType == null)
            {
                return NotFound();
            }

            return View(roomType);
        }

        //POST Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            var roomTypes = await _db.RoomTypes.FindAsync(id);
            _db.RoomTypes.Remove(roomTypes);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));          
            
        }
    }
}