using Mvc_Apteka;
using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.Text;

using WpfGrid.Data;

namespace ConsoleApp1_Tree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var service = new ProductCatalogService();
                
                using (var db = new AppDbContext())
               
                {
                    db.Database.EnsureCreated();

                    var dataset = ProductCatalogs_Init.GetPrimaryProductCatalogs();                                       
                    var root = service.GetRoot(dataset);
                    service.Print(Console.Out, db, root);
                    service.AddHier(db, new List<ProductCatalog>(dataset));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
