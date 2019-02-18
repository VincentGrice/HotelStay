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
    }
}