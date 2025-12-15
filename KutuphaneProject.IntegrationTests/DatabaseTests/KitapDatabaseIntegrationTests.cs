using Xunit;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.DatabaseTests
{
    // هذا أبسط Integration Test ممكن
    public class KitapDatabaseIntegrationTests
    {
        [Fact]
        public void Can_Connect_To_InMemory_Database()
        {
            // 1. أنشئ InMemory Database
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Connection")
                .Options;

            // 2. جرب تتصل
            using var context = new KutuphaneDbContext(options);
            var canConnect = context.Database.CanConnect();

            // 3. تأكد أن الاتصال ناجح
            Assert.True(canConnect);
        }
    }
}