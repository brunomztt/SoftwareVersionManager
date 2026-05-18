using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SoftwareVersionManager.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = "Server=localhost;Port=3306;Database=software_version_manager_dev;User=root;Password=;";

        try
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        catch
        {
            // Fallback to InMemory if MySQL is not available
            optionsBuilder.UseInMemoryDatabase("SoftwareVersionManager");
        }

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}


