using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HotelStay.Data;
using HotelStay.Models;
using HotelStay.Models.ViewModels;
using HotelStay.Utility;
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
            var files = HttpContext.Request.Form.Files;


            var roomsFromDb = _db.Rooms.Find(RoomsVM.Rooms.Id);

            if (files.Count != 0)
            {
                //Image has been uploaded
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads, RoomsVM.Rooms.Id + extension),FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }
                roomsFromDb.Image = @"\" + SD.ImageFolder + @"\" + RoomsVM.Rooms.Id + extension;
            }
            else
            {
                //when user does not upload image
                var uploads = Path.Combine(webRootPath,SD.ImageFolder+@"\" + SD.DefaultRoomImage);
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + RoomsVM.Rooms.Id + "jpeg");
                roomsFromDb.Image = @"\" + SD.ImageFolder + RoomsVM.Rooms.Id + "jepg";

            }
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //GET : Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomsVM.Rooms = await _db.Rooms.Include(m => m.RoomTags).Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.Id == id);

            if(RoomsVM.Rooms == null)
            {
                return NotFound();
            }

            return View(RoomsVM);
        }
    }
}