﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Models
{
    public class RoomType
    {
        [Key]
        public string Id { get; set; }
        public int Bed { get; set; }
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
