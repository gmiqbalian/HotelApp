using HotelApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.System
{
    public interface IGuestManager
    {
        public Guest GetGuestData(Guest guest);
        public bool HasBookig(Guest guestToDelete);
    }
}
