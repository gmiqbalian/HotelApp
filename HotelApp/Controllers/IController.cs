using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Controllers
{
    public interface IController
    {
        public void Create();
        public void Update();
        public void Delete();
        public void ShowAll();
    }
}
