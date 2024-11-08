using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknorixAPI.Application.Abstractions;
using TeknorixAPI.Domain.Models;

namespace TeknorixAPI.Infrastructure.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly IConfiguration _configuration;

        public DBContext AppContext
        {
            get
            {
                return _dbContext as DBContext;
            }
        }

        public DepartmentRepository(DBContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            _configuration = configuration;
        }
    }
}
