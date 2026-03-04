using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Components;
using ProjectManager.Data.Model;
using ProjectManager.Migrations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Proponent> Proponents { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<ProjectModel> Projects { get; set; }

        public DbSet<ContactPerson> ContactPersons { get; set; }

        public DbSet<TrackingModel> Trackings { get; set; }

        public DbSet<ReviewModel> Reviews { get; set; }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var now = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries<ContactPerson>())
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.onUpdate = now;
                }
                else if (entry.State == EntityState.Added)
                {
                    entry.Entity.createdOn = now;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Proponent>()
                .HasMany(c => c.Contacts)
                .WithOne(c => c.Proponent)
                .HasForeignKey(c => c.ProponentID);

            modelBuilder.Entity<Proponent>()
                .HasMany(c => c.Projects)
                .WithOne(c => c.Proponent)
                .HasForeignKey(c => c.ProponentID);

            modelBuilder.Entity<ProjectModel>()
                .HasMany(t => t.Trackings)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectID);

            modelBuilder.Entity<Status>()
                .HasMany(t => t.Trackings)
                .WithOne(t => t.Status)
                .HasForeignKey(t => t.StatusID);

            modelBuilder.Entity<ReviewModel>()
                .HasOne(r => r.Tracking)
                .WithOne(t => t.Review)
                .HasPrincipalKey<TrackingModel>(t => t.Id);

            modelBuilder.Entity<Status>().HasData(
    new Status
    {
        ID = Guid.Parse("a3f1c9e2-7b4d-4c91-8f2a-1d6e9b7c3f10"),
        Name = "Not Started",
        Color = "#FF0000"
    },
    new Status
    {
        ID = Guid.Parse("b7e2d4f9-3a11-42c7-9c8d-5f2a6b1e4d22"),
        Name = "In Progress",
        Color = "#FFFF00"
    },
    new Status
    {
        ID = Guid.Parse("c9a5f3d1-88b2-4a6e-91d3-7e4f2b6c8a33"),
        Name = "Completed",
        Color = "#00FF00"
    },
    new Status
    {
        ID = Guid.Parse("d1f7b3a9-55c4-4f8a-b2e1-9a6c3d7e5f44"),
        Name = "On Hold",
        Color = "#0000FF"
    },
    new Status
    {
        ID = Guid.Parse("e4c2a8f6-19d3-4b7f-a9c2-3f8d6e1b2a55"),
        Name = "Cancelled",
        Color = "#808080"
    }
);
        }
    }
}
