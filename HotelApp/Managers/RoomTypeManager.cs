using HotelApp.Data;
using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public class RoomTypeManager
    {
        private AppDbContext dbContext;
        public RoomTypeManager(AppDbContext context)
        {
            dbContext = context;
        }
        public RoomType GetRoomType(string roomType)
        {
            if (roomType.ToLower() == "single")
                return dbContext.RoomTypes.First(rt=> rt.Id == "Single");
            else if (roomType.ToLower() == "double")
                return dbContext.RoomTypes.First(rt => rt.Id == "Double");

            return null;
        }
    }
}
