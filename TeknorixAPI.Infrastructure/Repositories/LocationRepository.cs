using Microsoft.Extensions.Configuration;
using TeknorixAPI.Application.Abstractions;
using TeknorixAPI.Domain.Models;

namespace TeknorixAPI.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly IConfiguration _configuration;

        public DBContext AppContext
        {
            get
            {
                return _dbContext as DBContext;
            }
        }

        public LocationRepository(DBContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _configuration = configuration;
        }
    }
}
