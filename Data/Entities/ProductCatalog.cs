using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Apteka.Entities
{
    /// <summary>
    /// Группировка продукции ( 1 ко многим )   
    /// </summary>
    //[Index(nameof(ProductCatalogName),nameof(ProductCatalogNumber))]
    public class ProductCatalog
    {
        
        [Key]
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public ProductCatalog Parent { get; set; }
    




        [Required]
        public int ProductCatalogNumber { get; set; }

        [Required]
        public string ProductCatalogName { get; set; }
        public virtual ICollection<ProductInfo> Products { get; set; }


        

        

        public ProductCatalog()
        {
            this.Products = new List<ProductInfo>();
        }

        public ProductCatalog(ProductCatalog parent, string name)
        {
            this.ProductCatalogName = name;
            this.Parent = parent;
        }
    }
}
