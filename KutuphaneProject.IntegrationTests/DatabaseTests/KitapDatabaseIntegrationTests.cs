using Xunit;
using Microsoft.EntityFrameworkCore;
using KutuphaneProject.EFCore;
using KutuphaneProject.Models;

namespace KutuphaneProject.IntegrationTests.DatabaseTests
{
    // Bu mümkün olan en basit Integration Test
    public class KitapDatabaseIntegrationTests
    {
        [Fact]
        public void Can_Connect_To_InMemory_Database()
        {
            // 1. InMemory Database oluştur
            var options = new DbContextOptionsBuilder<KutuphaneDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB_Connection")
                .Options;

            // 2. Bağlanmayı dene
            using var context = new KutuphaneDbContext(options);
            var canConnect = context.Database.CanConnect();

            // 3. Bağlantının başarılı olduğundan emin ol
            Assert.True(canConnect);
        }
    }
}