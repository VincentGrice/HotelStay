using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Models;
using HotelStay.Models.ViewModels;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelStay.Controllers
{
    [Area("Admin")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly HostingEnvironment _hostingEnvironment;

        [BindProperty]
        public RoomsViewModel RoomsVM { get; set; }
        //Dependency Injection
        //Constructor for Rooms controller
        public RoomsController(ApplicationDbContext db, HostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            RoomsVM = new RoomsViewModel()
            {
                RoomTypes = _db.RoomTypes.ToList(),
                RoomTags = _db.RoomTags.ToList(),
                Rooms = new Models.Rooms()
            };
        }
        

        public async Task<IActionResult> Index()
        {
            var rooms = _db.Rooms.Include(m => m.RoomTypes).Include(m => m.RoomTags);

            return View(await rooms.ToListAsync());
        }

        //Get : Rooms Create
        public IActionResult Create()
        {
            return View(RoomsVM);
        }

        //POST : Rooms Create
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(RoomsVM);
            }

            _db.Rooms.Add(RoomsVM.Rooms);
            await _db.SaveChangesAsync();

            //Image being saved

            string webRootPath = _hostingEnvironment.WebRootPath;
        }
    }
}