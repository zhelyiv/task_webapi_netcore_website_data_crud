using DAL.Ef.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Enums;
using System; 
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Ef
{
    public class WebsiteManagementDbContext : DbContext, IWebsiteManagementDbContext
    {
        protected IDbContextTransaction DbContextTransaction { set; get; }

        #region tables

        public DbSet<Website> Website { get; set; }
        public DbSet<WebsiteCategory> WebsiteCategory { get; set; }
        public DbSet<WebsiteField> WebsiteField { get; set; }
        public DbSet<WebsiteHomepageSnapshot> WebsiteHomepageSnapshot { get; set; }
        public DbSet<WebsiteLogin> WebsiteLogin { get; set; }
        public DbSet<WebsiteStatus> WebsiteStatus { get; set; }

        #endregion

        public WebsiteManagementDbContext() { }
        public WebsiteManagementDbContext(DbContextOptions<WebsiteManagementDbContext> options) : base(options) { }

        #region DbContext_override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDeleteBehavior(modelBuilder, DeleteBehavior.Restrict);

            SetWebsiteEntityDeleteBehavior(modelBuilder, DeleteBehavior.Cascade);

            SeedWebsiteStatuses(modelBuilder);

            SeedWebsiteCategories(modelBuilder);

            AddConstraints(modelBuilder); 

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            { 
                options.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=WebsiteManagement;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True");
            }
        }

        #endregion

        #region IWebsiteManagementDbContext_implementation

        public async Task<bool> SaveChangesAsync()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified))
                .ToArray();

            foreach (var entityEntry in modifiedEntries)
            {
                var entity = entityEntry.Entity as BaseEntity;
                if (entity is null)
                    continue;

                if (entityEntry.State == EntityState.Added)
                {
                    entity.DateCreatedUtc = DateTime.UtcNow;
                    entity.DateUpdatedUtc = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    entity.DateUpdatedUtc = DateTime.UtcNow;
                }
            }

            var rc = await base.SaveChangesAsync();
            return rc > 0;
        }

        public async Task BeginTransactionAsync()
        {
            if (DbContextTransaction != null)
                throw new Exception($"There is incomplete transaction: {DbContextTransaction.TransactionId}");

            DbContextTransaction = await Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (DbContextTransaction != null)
            {
                await DbContextTransaction.CommitAsync();
                DbContextTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (DbContextTransaction != null)
            {
                await DbContextTransaction.RollbackAsync();
                DbContextTransaction = null;
            }
        }

        #endregion

        #region Privates

        private void SetDeleteBehavior(ModelBuilder modelBuilder, DeleteBehavior deleteBehavior)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = deleteBehavior;
            }
        }

        private void SetWebsiteEntityDeleteBehavior(ModelBuilder modelBuilder, DeleteBehavior deleteBehavior)
        {
            modelBuilder.Entity<Website>()
                .HasMany(p => p.Fields)
                .WithOne(x => x.Website)
                .OnDelete(deleteBehavior);

            modelBuilder.Entity<Website>()
                 .HasMany(p => p.Logins)
                 .WithOne(x => x.Website)
                 .OnDelete(deleteBehavior);
        }

        private void SeedWebsiteStatuses(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebsiteStatus>()
                .HasData(WebsiteManagementDbContextSeedHelper.GetWebsiteStatusesFromEnum());
        }
        private void SeedWebsiteCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebsiteCategory>()
                .HasData(WebsiteManagementDbContextSeedHelper.GetWebsiteCategoriesFromEnum());
        }
        private void AddConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WebsiteField>()
                .HasIndex(x => new { x.WebsiteId, x.FieldName })
                .IsUnique();

            modelBuilder.Entity<WebsiteLogin>()
               .HasIndex(x => new { x.WebsiteId, x.Email })
               .IsUnique();

            modelBuilder.Entity<Website>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Website>()
                .HasIndex(x => x.Url)
                .IsUnique();

            modelBuilder.Entity<WebsiteCategory>()
                .HasIndex(x => new { x.Name })
                .IsUnique();

            modelBuilder.Entity<WebsiteStatus>()
                .HasIndex(x => new { x.Name })
                .IsUnique();
        }


        #endregion
    }
}
