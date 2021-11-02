using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
// using ApplicationCore

namespace Infrastructure.Data
{
    public class ZemartDBContext : DbContext
    {
        public ZemartDBContext(DbContextOptions<ZemartDBContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { set; get; }
        public DbSet<Catergory> Catergories { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // var Faker = new FakeData();
            // Faker.Generate(30);
            // modelBuilder.Entity<Catergory>().HasData(Faker.Catergories);
            // modelBuilder.Entity<Product>().HasData(Faker.Products);

            // dotnet ef migrations add AddPersonModel
        }



    }

    // public class FakeData
    // {
    //     public List<Product> Products = new List<Product>();
    //     public List<Catergory> Catergories = new List<Catergory>();
    //     private Faker f;


    //     public void Generate(int InstanceNumber)
    //     {
    //         f = new Faker();

    //         GeneargateCategory();
    //         GenerateProduct(InstanceNumber);
    //     }


    //     public void GenerateProduct(int instanceNumber)
    //     {
    //         var productID = 1;
    //         for (int i = 0; i < instanceNumber; i++)
    //         {
    //             var randomCate = Catergories[f.Random.Number(Catergories.Count - 1)];
    //             var product = new Product { ID = productID, Description = f.Commerce.ProductDescription(), Price = 100, Catergory = randomCate, CatergoryID = randomCate.ID };
    //         }
    //     }

    //     public void GeneargateCategory()
    //     {
    //         var catId = 1;
    //         foreach (var item in f.Commerce.Categories(10))
    //         {
    //             var cat = new Catergory { Name = item, ID = catId, Description = f.Commerce.ProductAdjective() };
    //             Catergories.Add(cat);
    //             catId++;
    //         }
    //     }
    // }
}