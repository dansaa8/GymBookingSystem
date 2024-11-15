using GymBooking.Data;
using GymBookingSystem.Data;

namespace GymBooking.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            // Skapar en scope:ad service provider (CreateScope). Detta försäkrar oss om att services...
            // som hämtas under seedning kasseras efter användning (t.ex ApplicationDbContext).
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                try
                {
                    // Skickar med context och services så seeddata kommer åt contexten och services som finns i DI-containern.
                    await SeedData.InitAsync(context, services);
                }
                catch (Exception)
                {

                    throw;
                }
            }

            return app;
        }
    }
}
