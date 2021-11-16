using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using ApplicationCore.Entities;

namespace Infrastructure.Data
{
    public class SeederModel
    {
        public int numberOfProduct { set; get; }
        public int numberOfCategory { set; get; }

        public int categoryDeepLevel { set; get; }
        public int numberOfCategoryPerDeep { set; get; }

    }

    public static class ShopDBInitializer
    {

        public static void Initialize(ShopDBContext context, IServiceProvider services, IWebHostEnvironment webHost)
        {
            var logger = services.GetRequiredService<ILogger<ShopDBContext>>();
            context.Database.EnsureCreated();
            if (context.Products.Any())
            {
                logger.LogInformation("The database was already seeded");

                return;
            }
            logger.LogInformation("Start seeding the database.");
            var Faker = new FakeData();
            Faker.Generate(new SeederModel
            {
                numberOfCategory = 12,
                numberOfProduct = 200,
                numberOfCategoryPerDeep = 4,
                categoryDeepLevel = 2

            }, webHost);
            context.Catergories.AddRange(Faker.Catergories);
            context.Products.AddRange(Faker.Products);
            context.SaveChanges();


        }

        public static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                // TODO What is scope.ServiceProvider;
                var services = scope.ServiceProvider;
                try
                {
                    var dbcontext = services.GetRequiredService<ShopDBContext>();
                    var webHost = services.GetService<IWebHostEnvironment>();
                    var applocationDomain = services.GetService<IApplicationBuilder>();
                    ShopDBInitializer.Initialize(dbcontext, services, webHost);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("An error occurred while seeding the database" + ex.ToString());
                }
            }
        }
    }
    public class FakeData
    {

        public List<Product> Products = new List<Product>();
        public List<Catergory> Catergories = new List<Catergory>();
        private Faker f;
        private IWebHostEnvironment webHost;

        private SeederModel seederModel;


        public void Generate(SeederModel seederModelConfig, IWebHostEnvironment webHostConfig)
        {
            f = new Faker();
            webHost = webHostConfig;
            seederModel = seederModelConfig;
            GeneargateCategory(seederModel.numberOfCategory);

            // Create 
            GenerateCategoryWithSubCategory(seederModel.categoryDeepLevel, seederModel.numberOfCategoryPerDeep);
            GenerateProduct(seederModel.numberOfProduct);
        }

        private List<string> grabImgURL()
        {
            var wwwrootPath = webHost.WebRootPath;


            var FilePath = Path.Combine(wwwrootPath, "images");
            string[] ImgPaths = Directory.GetFiles(FilePath);
            List<string> validMockImg = new List<string>();
            foreach (var item in ImgPaths)
            {
                if (item.Contains("big"))
                {
                    // TODO Fix this
                    validMockImg.Add(Path.Combine("images", Path.GetFileName(item)));
                }
            }
            return validMockImg;
            // System.Read
        }


        public void GenerateProduct(int instanceNumber)
        {
            List<string> mockValidImgPaths = grabImgURL();
            var productID = 1;
            for (int i = 0; i < instanceNumber; i++)
            {
                var randomCate = Catergories[f.Random.Number(Catergories.Count - 1)];
                var imgPath = mockValidImgPaths[f.Random.Number(mockValidImgPaths.Count - 1)];
                var product = new Product
                {
                    // ID = productID,
                    Name = f.Commerce.ProductName(),
                    Description = f.Commerce.ProductDescription(),
                    ImageURL = imgPath,
                    Price = f.Random.Number(300),
                    PromotionPrice = f.Random.Number(5, 20),
                    Catergory = randomCate,
                    CatergoryID = randomCate.ID

                };
                Products.Add(product);
                productID++;
            }
        }

        public void GeneargateCategory(int numberOfCategory)
        {
            var catId = 1;
            foreach (var item in f.Commerce.Categories(numberOfCategory))
            {
                var cat = new Catergory { Name = item, Description = f.Commerce.ProductAdjective() };
                Catergories.Add(cat);
                catId++;
            }

        }

        int OGArrayLength = 0;


        List<int> listOfCategoryLayer = new List<int>();

        private void populateLayer(int subDeptLevel, int numberOfSubPerDept, int rootCategoryLength)
        {
            var previousDistance = 1;
            listOfCategoryLayer.Add(rootCategoryLength);
            for (int i = 0; i < subDeptLevel; i++)
            {
                if (i == 0)
                {
                    previousDistance = rootCategoryLength;
                }
                var distance = previousDistance * numberOfSubPerDept + previousDistance;
                listOfCategoryLayer.Add(distance);
                previousDistance = distance;
            }
        }
        public void GenerateCategoryWithSubCategory(int subDeptLevel, int numberOfSubPerDept)
        {
            populateLayer(subDeptLevel, numberOfSubPerDept, Catergories.Count);
            var baseIndex = 0;
            for (int deep = 0; deep < subDeptLevel; deep++)
            {
                var deepDistance = listOfCategoryLayer[deep];
                for (int i = baseIndex; i < deepDistance; i++)
                {
                    insertToRootCat(numberOfSubPerDept, i);
                }
                baseIndex = deepDistance;
            }
            System.Console.WriteLine("");
            // Create Cat with parrent Ref

        }



        private void insertToRootCat(int subCount, int parrentIndex)
        {

            foreach (var item in f.Commerce.Categories(subCount).Select((value, i) => (value, i)))
            {
                var cat = new Catergory
                {
                    Parrent = Catergories[parrentIndex],
                    ParrentId = parrentIndex,
                    Name = item.value,
                    Description = f.Commerce.ProductAdjective()
                };
                // Add Child ref to Pattrent
                Catergories.Add(cat);
            }
        }



    }
}