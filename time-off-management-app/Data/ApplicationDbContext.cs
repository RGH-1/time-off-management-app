using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using time_off_management_app.Models;

namespace time_off_management_app.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<Reason> Reason { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<TimeOffForm>()
                .HasOne(t => t.ApprovedBy)
                .WithMany()
                .HasForeignKey(t => t.ApprovedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
