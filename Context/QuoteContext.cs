using Microsoft.EntityFrameworkCore;
using QuoteOTD___Service.Model;

namespace QuoteOTD___Service.Context
{
    public class QuoteContext : DbContext
    {
        public QuoteContext(DbContextOptions<QuoteContext> options) : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }
    }
   
}
