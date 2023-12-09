using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PremiumDeluxeMotorSports_v1.Models;

namespace PremiumDeluxeMotorSports_v1.Data
{
    public class PremiumDeluxeMotorSports_v1Context : DbContext
    {
        public PremiumDeluxeMotorSports_v1Context (DbContextOptions<PremiumDeluxeMotorSports_v1Context> options)
            : base(options)
        {
        }

        public DbSet<PremiumDeluxeMotorSports_v1.Models.User> Users { get; set; } = default!;

        public DbSet<PremiumDeluxeMotorSports_v1.Models.Vehicule>? Vehicule { get; set; }

        public DbSet<PremiumDeluxeMotorSports_v1.Models.Reservation>? Reservation { get; set; }
        public DbSet<PremiumDeluxeMotorSports_v1.Models.Commande>? Commande { get; set; }

        public DbSet<PremiumDeluxeMotorSports_v1.Models.Custom>? Custom { get; set; }

        public DbSet<PremiumDeluxeMotorSports_v1.Models.Role>? Role { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuration de la relation User - Reservation
            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .OnDelete(DeleteBehavior.Cascade);
                
            
            // Configuration de la relation Vehicule - Custom
            modelBuilder.Entity<Vehicule>()
                .HasMany(v => v.Customs)
                .WithOne(cu => cu.Vehicule)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configuration de la relation User - Commande
            modelBuilder.Entity<User>()
                .HasMany(u => u.Commandes)
                .WithOne(c => c.User)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configuration de la relation Vehicule - Commande
            modelBuilder.Entity<Vehicule>()
                .HasMany(v => v.Commandes)
                .WithOne(c => c.Vehicule)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Configuration de la relation Custom - Commande
            modelBuilder.Entity<Custom>()
                .HasMany(cu => cu.Commandes)
                .WithOne(co => co.Custom)
                .OnDelete(DeleteBehavior.Cascade);


            // Configuration de la relation Vehicule - Reservation

            modelBuilder.Entity<Vehicule>()
                  .HasOne(v => v.Reservation)
                  .WithOne(r => r.Vehicule)
                  .HasForeignKey<Reservation>(r => r.VehiculeId);
             

            modelBuilder.Entity<Vehicule>()
                .HasIndex(v => v.ReservationId)
                .IsUnique();

            // Configuration de la relation User - Role

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleID);
            
        }        
    }
}