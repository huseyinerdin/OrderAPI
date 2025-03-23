using OrderAPI.Domain.Entities;

namespace OrderAPI.Persistence.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                var rnd = new Random();
                var categories = new[] { "Elektronik", "Giyim", "Kırtasiye", "Gıda", "Temizlik" };

                var products = Enumerable.Range(1, 1000).Select(i => new Product
                {
                    Description = $"Ürün {i}",
                    Category = categories[rnd.Next(categories.Length)],
                    Unit = "Adet",
                    UnitPrice = (decimal)Math.Round(rnd.NextDouble() * 1000, 2),
                    Status = true,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                });

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}
