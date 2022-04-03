using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProgramUp;
using Pyrros.Repository.DataContext;
using Microsoft.EntityFrameworkCore;

[assembly: WebJobsStartup(typeof(Program))]
namespace ProgramUp
{
    public class Program : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("ConnectionStrings:PyrrosDB");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
