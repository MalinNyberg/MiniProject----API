using Microsoft.EntityFrameworkCore;
using MiniProject____API.Data;
using MiniProject____API.Services;

namespace MiniProject____API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            string connectionString = builder.Configuration.GetConnectionString("ApplicationContext");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            var app = builder.Build();

            // Get and Post people endpoints
            app.MapGet("/people", ApiHandler.GetPeople);
            app.MapPost("/people", ApiHandler.AddPerson);

            // Get and Post Interests endpoints
            app.MapGet("/interests", ApiHandler.GetInterests);
            app.MapPost("/interests", ApiHandler.AddInterest);

            // Get and Post People and interests endpoints
            app.MapGet("/people/{personId}/interests", ApiHandler.GetPersonInterests);
            app.MapPost("/people/{personId}/interests/{interestId}", ApiHandler.ConnectPersonAndInterest);

            // Get and posts Interests link endpoints
            app.MapGet("/people/{personId}/interests/links", ApiHandler.GetLinksConnectedToPerson);
            app.MapPost("/people/{personId}/interests/{interestId}/links/", ApiHandler.AddInterestLink);


            app.Run();
        }
    }
}