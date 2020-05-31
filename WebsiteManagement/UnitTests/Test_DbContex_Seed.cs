using DAL.Ef;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests
{
    public class Test_DbContex_Seed
    {
        [Fact]
        public async Task Test_Seeding_of_Statuses_and_Categories()
        {
            var statusesToCompare = WebsiteManagementDbContextSeedHelper.GetWebsiteStatusesFromEnum();
            var categoriesToCompare = WebsiteManagementDbContextSeedHelper.GetWebsiteCategoriesFromEnum();

            using (var db = await InstanceFactory.GetInstance<IWebsiteManagementDbContext>())
            {
                var statusesFromDb = await db.WebsiteStatus.ToArrayAsync();

                Assert.Equal(statusesToCompare.Count(), statusesFromDb.Count());
                foreach (var expectedItem in statusesToCompare)
                {
                    var dbItem = statusesFromDb.FirstOrDefault(x => x.Id == expectedItem.Id);
                    Assert.NotNull(dbItem);
                    Assert.Equal(expectedItem.Name, dbItem.Name);
                }

                var categoriesFromDb = await db.WebsiteCategory.ToArrayAsync();

                Assert.Equal(categoriesToCompare.Count(), categoriesFromDb.Count());
                foreach (var expectedItem in categoriesToCompare)
                {
                    var dbItem = categoriesFromDb.FirstOrDefault(x => x.Id == expectedItem.Id);
                    Assert.NotNull(dbItem);
                    Assert.Equal(expectedItem.Name, dbItem.Name);
                }
            }
        } 
    }
}
