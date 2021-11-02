using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationCore.Entities
{

    // TODO Render small and big img by using servies DI

    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { set; get; }

        public string Name { set; get; }
        public string Description { set; get; }

        public string ImageURL { set; get; }

        public int PromotionPrice { set; get; }
        public int Price { set; get; }


        // Product Preview
        // public int Price { get => this.getDefaultProductVariant().Price; }
        // public string ImageURL { set; get; }
        // public int PromotionPrice { get => this.getDefaultProductVariant().PromotionPrice; }
        // public int DiscountedPrice
        // {
        //     get => this.getDefaultProductVariant().DiscountedPrice;
        // }
        ///

        // Foregin key ref to Catergory
        public int CatergoryID { set; get; }
        public Catergory Catergory { set; get; }

        // ProductVariant Section 

        // TODO List of ProductVariant
        // TODO Learn to delare ref in DB model
        // public List<ProductVariant> ProductVariants { set; get; }
        // // return ProductVariant[0] as default selected ViewOption;

        // public ProductVariant getDefaultProductVariant(int index = 0)
        // {
        //     return ProductVariants[index];
        // }
    }

    public class ProductVariant
    {

        // REF
        public int ProductID { set; get; }
        public Product Product { set; get; }

        // Atribute
        // public List<string> listOfMedia { set; get; }
        public int PromotionPrice { set; get; }
        public int Price { set; get; }

        // HMM
        public int DiscountedPrice
        {
            get => Price - PromotionPrice;
        }

        public string Name { set; get; }

        public List<ProductOptionalVariant> ProductOptionalVariants { set; get; }

        public ProductOptionalVariant getDefaultProductOptionalVariant(int index = 0)
        {
            return ProductOptionalVariants[index];
        }
    }

    public class ProductOptionalVariant
    {
        public ProductVariant ProductVariant { set; get; }
        public int IncrementPrice { set; get; }
        public string Name { set; get; }

    }
}