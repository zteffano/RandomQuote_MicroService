
using Microsoft.EntityFrameworkCore;
using QuoteOTD___Service.Context;
using QuoteOTD___Service.Model;

namespace QuoteOTD___Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            

            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("Docker");

            // Add services to the container.
            builder.Services.AddAuthorization();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<QuoteContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
            // Tilpas Kestrel til kun at bruge HTTP, når den kører i Docker
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8080); // Match den port, der er angivet i ASPNETCORE_HTTP_PORTS
            });

            var app = builder.Build();

            // Migrationer her
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<QuoteContext>();
                    context.Database.Migrate(); // Udfører pending migrationer

                    // Seed database efter migrations
                    SeedDatabase(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();

            app.UseAuthorization();

            // Pick random quote
            app.MapGet("/quote", async (QuoteContext context) =>
            {
                var quotes = await context.Quotes.ToListAsync();
                var random = new Random();
                var quote = quotes[random.Next(quotes.Count)];
                return Results.Ok(quote);

            })
            .WithName("GetRandomQuote")
            .WithOpenApi();

            app.Run();

            // Metode til at seede databasen
            void SeedDatabase(QuoteContext context)
            {
                // Kontroller om databasen allerede er seedet
                if (!context.Quotes.Any())
                {
                    var quotes = new List<Quote>
        {
            new Quote { Author = "Albert Einstein", Text = "Life is like riding a bicycle. To keep your balance, you must keep moving." },
            new Quote { Author = "Maya Angelou", Text = "You will face many defeats in life, but never let yourself be defeated." },
            new Quote { Author = "Winston Churchill", Text = "Success is not final, failure is not fatal: It is the courage to continue that counts." },
            new Quote { Author = "Confucius", Text = "It does not matter how slowly you go as long as you do not stop." },
            new Quote { Author = "Oscar Wilde", Text = "Be yourself; everyone else is already taken." },
            new Quote { Author = "Nelson Mandela", Text = "The greatest glory in living lies not in never falling, but in rising every time we fall." },
            new Quote { Author = "Steve Jobs", Text = "Your work is going to fill a large part of your life, and the only way to be truly satisfied is to do what you believe is great work. And the only way to do great work is to love what you do." },
            new Quote { Author = "Ralph Waldo Emerson", Text = "Do not go where the path may lead, go instead where there is no path and leave a trail." },
            new Quote { Author = "Theodore Roosevelt", Text = "Believe you can and you're halfway there." },
            new Quote { Author = "Helen Keller", Text = "The best and most beautiful things in the world cannot be seen or even touched - they must be felt with the heart." }
        };

                    context.Quotes.AddRange(quotes);
                    context.SaveChanges();
                }
            }
        }


    }
}
