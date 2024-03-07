
namespace QuoteOTD___Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var quotes = new[]
            {
                new { Author = "Steve Jobs", Text = "The only way to do great work is to love what you do." },
                new { Author = "Mark Twain", Text = "The secret of getting ahead is getting started." },
                new { Author = "Michael Jordan", Text = "I’ve missed more than 9000 shots in my career. I’ve lost almost 300 games. 26 times I’ve been trusted to take the game-winning shot and missed. I’ve failed over and over and over again in my life. And that is why I succeed." },
                new { Author = "Vince Lombardi", Text = "It’s not whether you get knocked down; it’s whether you get up." },
                new { Author = "Unknown", Text = "Your limitation—it’s only your imagination." },
                new { Author = "Unknown", Text = "Push yourself, because no one else is going to do it for you." },
                new { Author = "Unknown", Text = "Great things never come from comfort zones." },
                new { Author = "Unknown", Text = "Dream it. Wish it. Do it." },
                new { Author = "Unknown", Text = "Success doesn’t just find you. You have to go out and get it." },
                new { Author = "Unknown", Text = "The harder you work for something, the greater you’ll feel when you achieve it." },
                new { Author = "Unknown", Text = "Dream bigger. Do bigger." },
                new { Author = "Unknown", Text = "Don’t stop when you’re tired. Stop when you’re done." },
                new { Author = "Unknown", Text = "Wake up with determination. Go to bed with satisfaction." },
                new { Author = "Unknown", Text = "Do something today that your future self will thank you for." },
                new { Author = "Unknown", Text = "Little things make big days." }
            };

            app.MapGet("/quote", () =>
            {
                //Get quote
                var quote = quotes[new Random().Next(quotes.Length)];
                return Results.Ok(quote);

            })
            .WithName("GetQuote")
            .WithOpenApi();

            app.Run();
        }
    }
}
