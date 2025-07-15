using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalResearch.DAL.DataContext
{
    public static class DALRegistrator
    {
        public static void RegisterService(IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddDbContext<MedicalResearchDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
            );

        }
    }
}
