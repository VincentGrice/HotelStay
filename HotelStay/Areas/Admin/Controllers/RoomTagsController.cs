using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelStay.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        //Dependency Injection
        public RoomTagsController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View(_db.RoomTags.ToList());
        }

        //GET Create Action Method
        public IActionResult Create()
        {
            return View();
        }

        //POST Create Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoomTags roomTags)
        {
            if (ModelState.IsValid)
            {
                _db.Add(roomTags);
                //use await keyword when using async methods
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomTags);
        }

        //GET Edit Action Method
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomTag = await _db.RoomTags.FindAsync(id);
            if (roomTag == null)
            {
                return NotFound();
            }

            return View(roomTag);
        }

        //POST Edit Action Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RoomTags roomTags)
        {
            if (id != roomTags.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Update(roomTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(roomTags);
        }

        //GET Details Action Method
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomTag = await _db.RoomTags.FindAsync(id);
            if (roomTag == null)
            {
                return NotFound();
            }

            return View(roomTag);
        }

        //GET Delete Action Method
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var roomTag = await _db.RoomTags.FindAsync(id);
            if (roomTag == null)
            {
                return NotFound();
            }

            return View(roomTag);
        }

        //POST Delete Action Method
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var roomTags = await _db.RoomTags.FindAsync(id);
            _db.RoomTags.Remove(roomTags);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}