using DAL.Ef.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DAL.Ef
{
    public interface IWebsiteManagementDbContext: IDisposable
    {
        DbSet<Website> Website { get; set; }
        DbSet<WebsiteCategory> WebsiteCategory { get; set; }
        DbSet<WebsiteField> WebsiteField { get; set; }
        DbSet<WebsiteHomepageSnapshot> WebsiteHomepageSnapshot { get; set; }
        DbSet<WebsiteLogin> WebsiteLogin { get; set; }
        DbSet<WebsiteStatus> WebsiteStatus { get; set; }

        Task<bool> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
