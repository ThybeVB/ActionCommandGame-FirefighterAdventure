using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ActionCommandGame.Repository
{
    public class ActionButtonGameDbContextFactory : IDesignTimeDbContextFactory<ActionButtonGameDbContext>
    {
        public ActionButtonGameDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ActionButtonGameDbContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=FiremanAdventure;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False");

            return new ActionButtonGameDbContext(optionsBuilder.Options);
        }
    }
}
