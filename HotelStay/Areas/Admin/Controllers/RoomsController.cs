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
                System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + RoomsVM.Rooms.Id + "jpg");
                roomsFromDb.Image = @"\" + SD.ImageFolder + RoomsVM.Rooms.Id + "jpg";

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

        //POST : Edit Room
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit (int id)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var roomFromDb = _db.Rooms.Where(m => m.Id == RoomsVM.Rooms.Id).FirstOrDefault();

                if (files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(roomFromDb.Image);

                    if (System.IO.File.Exists(Path.Combine(uploads, RoomsVM.Rooms.Id + extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, RoomsVM.Rooms.Id + extension_old));
                    }
                    using (var filestream = new FileStream(Path.Combine(uploads, RoomsVM.Rooms.Id + extension_new), FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }
                    RoomsVM.Rooms.Image = @"\" + SD.ImageFolder + @"\" + RoomsVM.Rooms.Id + extension_new;
                }

                if (RoomsVM.Rooms.Image != null)
                {
                    roomFromDb.Image = RoomsVM.Rooms.Image;
                }

                roomFromDb.Name = RoomsVM.Rooms.Name;
                roomFromDb.Price = RoomsVM.Rooms.Price;
                roomFromDb.Available = RoomsVM.Rooms.Available;
                roomFromDb.RoomTypeId = RoomsVM.Rooms.RoomTypeId;
                roomFromDb.RoomTagsID = RoomsVM.Rooms.RoomTagsID;
                roomFromDb.NonSmoking = RoomsVM.Rooms.NonSmoking;
                await _db.SaveChangesAsync();
                


                return RedirectToAction(nameof(Index));
            }

            return View(RoomsVM);
        }

        //GET : Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomsVM.Rooms = await _db.Rooms.Include(m => m.RoomTags).Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (RoomsVM.Rooms == null)
            {
                return NotFound();
            }

            return View(RoomsVM);
        }

        //GET : Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            RoomsVM.Rooms = await _db.Rooms.Include(m => m.RoomTags).Include(m => m.RoomTypes).SingleOrDefaultAsync(m => m.Id == id);

            if (RoomsVM.Rooms == null)
            {
                return NotFound();
            }

            return View(RoomsVM);
        }

        //POST : Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            Rooms rooms = await _db.Rooms.FindAsync(id);

            if(rooms == null)
            {
                return NotFound();
            }
            else
            {
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(rooms.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, rooms.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, rooms.Id + extension));
                }
                _db.Rooms.Remove(rooms);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

    }
}