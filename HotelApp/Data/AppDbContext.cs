using HotelApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Guest> Guests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }

        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //change the database name before submitting                
                optionsBuilder.UseSqlServer(@"Server=(localdb)\\MSSQLLocalDB;Database=HotelAppDb;Trusted_Connection=True;TrustServerCertificate=true;");
            }
        }
    }
}
