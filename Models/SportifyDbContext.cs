using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sportify_Back.Models;

namespace Sportify_back.Models
{
    public class SportifyDbContext : IdentityDbContext<ApplicationUser> // Cambiado a ApplicationUser
    {
        public SportifyDbContext(DbContextOptions<SportifyDbContext> options) : base(options)
        {
        }
        

        public DbSet<Activities> Activities { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<Licenses> Licenses { get; set; }
        public DbSet<Plans> Plans { get; set; }
        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<Programmings> Programmings { get; set; }
        public DbSet<Teachers> Teachers { get; set; }

        public DbSet<Payments> Payments { get; set; }

        public DbSet<PaymentMethod> PaymentMethod { get; set; }

        public DbSet<ProgrammingUsers> ProgrammingUsers { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Plans)               // Un usuario tiene un plan
            .WithMany(p => p.Users)             // Un plan puede tener muchos usuarios
            .HasForeignKey(u => u.PlansId)      // Clave externa en ApplicationUser
            .OnDelete(DeleteBehavior.SetNull);  // Si el plan se elimina, los usuarios no se borran.


            modelBuilder.Entity<Classes>()
                .HasOne(c => c.Teachers)
                .WithMany(t => t.Classes)
                .HasForeignKey(c => c.TeachersId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.HasKey(login => new { login.LoginProvider, login.ProviderKey });
                entity.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity<Payments>()
        .HasOne(up => up.ApplicationUser)
        .WithMany()  // o .WithOne() dependiendo de la relación
        .HasForeignKey(up => up.UsersId)
        .HasPrincipalKey(au => au.Id);

            modelBuilder.Entity<ProgrammingUsers>()
                .HasOne(pu => pu.User)
                .WithMany()
                .HasForeignKey(pu => pu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgrammingUsers>()
                .HasOne(pu => pu.Class)
                .WithMany()
                .HasForeignKey(pu => pu.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
