using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknorixAPI.Application.IServices;

namespace TeknorixAPI.Application.Services
{
    public class ServiceInjector
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IJobService, JobService>();
        }
    }
}
