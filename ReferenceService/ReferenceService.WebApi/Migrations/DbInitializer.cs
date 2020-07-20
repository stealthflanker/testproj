using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;

namespace ReferenceService.WebApi.Migrations
{
    public class DbInitializer : CreateDatabaseIfNotExists<Repositories.ReferenceRepository.Context>
    {
        public override void InitializeDatabase(Repositories.ReferenceRepository.Context context)
        {
            DbMigrator dbMigrator = new DbMigrator(new Configuration
            {                
                TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, "Npgsql")
            });
            
        }
    }
}
