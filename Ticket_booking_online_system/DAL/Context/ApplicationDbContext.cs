using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Ticket_booking_online_system.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        //public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<TransportationService> TransportationServices { get; set; }
        public virtual DbSet<FlightService> FlightServices { get; set; }
        public virtual DbSet<HotelService> HotelServices { get; set; }
        public virtual DbSet<Airline> Airlines { get; set; }
        public virtual DbSet<Flight> Flights { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<RefundCancel> RefundsCancels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginLocation)
                .WithMany()
                .HasForeignKey(f => f.Origin_LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestLocation)
                .WithMany()
                .HasForeignKey(f => f.Dest_LocationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<RefundCancel>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.RefundCancel)
                .HasForeignKey<RefundCancel>(r => r.BookingID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}