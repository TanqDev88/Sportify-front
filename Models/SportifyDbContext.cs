using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Sportify_back.Models{

    public class SportifyDbContext: IdentityDbContext <IdentityUser>
    //ahora la db context es de tipo "IdentityDbContext <IdentityUser>"
    {

        public SportifyDbContext(DbContextOptions<SportifyDbContext> options):base(options)
        {
        }

        public DbSet<Activities> Activities {get; set;}

        public DbSet<Classes> Classes {get; set;}

        public DbSet<Licenses> Licenses {get; set;}

        public DbSet<Plans> Plans {get; set;}

        public DbSet<Profiles> Profiles {get; set;}

        public DbSet<Programmings> Programmings {get; set;}

        public DbSet<Users> Users {get; set;}

        public DbSet<Teachers> Teachers {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder){
              base.OnModelCreating(modelBuilder);
              
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
        }
    }
}