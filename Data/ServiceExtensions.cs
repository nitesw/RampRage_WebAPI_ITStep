using Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class ServiceExtensions
    {
        public static void AddRampRageDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RampRageDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
        }
    }
}
