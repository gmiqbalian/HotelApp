using HotelApp.Data;
using HotelApp.System;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    public class Controller
    {
        private AppDbContext dbContext;
        private IController _controller;
        private IBookingManager _bookingManager;
        private IRoomManager _roomManager;
        private IGuestManager _guestManager;
        public Controller(AppDbContext context) 
        {
            dbContext = context;            
            _roomManager = new RoomManager(dbContext);
            _guestManager = new GuestManager(dbContext);
            _bookingManager = new BookingManager(dbContext, _roomManager);
        }
        public IController GetController(string controllerType)
        {   
            switch (controllerType)
            {
                case "booking":
                    _controller = new BookingController(dbContext, _bookingManager, _roomManager);
                    break;
                case "room":
                    _controller = new RoomController(dbContext, _roomManager);
                    break;
                case "guest":
                    _controller = new GuestController(dbContext, _guestManager);
                    break;
                default:
                    break;
            }
            return _controller;
        }
    }
}
