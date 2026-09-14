namespace Data
{
    public class DbContextFactory
    {
        public ApplicationDbContext CreateDbContext(string connectionString)
        {
            return new ApplicationDbContext(connectionString);
        }
    }
}
