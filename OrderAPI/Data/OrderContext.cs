using Microsoft.EntityFrameworkCore;
using OrderAPI.Model;

namespace OrderAPI.Data
{
    public class OrderContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public OrderContext(DbContextOptions options) : base(options)
        {

        }
    }
}
