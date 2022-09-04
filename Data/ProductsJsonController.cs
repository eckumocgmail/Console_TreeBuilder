
using Microsoft.Win32;

using Mvc_Apteka.Controllers;
using Mvc_Apteka.Entities;
 

using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WpfGrid.MVVM;

namespace Mvc_Apteka 
{
    /// <summary>
    /// Реализация импорта-экспорта файлов в формате json
    /// </summary>
    public class ProductsJsonController
    {
        private readonly AppDbContext appDbContext;
        public async Task Download(string filename, string contenttype, byte[] bytes)
        {
            await System.IO.File.WriteAllBytesAsync(filename, bytes);
        }

        public ProductsJsonController(  )
        {                  
        }


        /// <summary>
        /// Экспорт файла с данными JSON
        /// </summary>
        public async Task<byte[]> GetDataForExport(  )
        {
            await Task.CompletedTask;
            string json = JsonConvert.SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return bytes;
        }

       

 
       /* private void ImportProducts(IEnumerable<ProductInfo> records)
        {
            foreach (var product in records)
            {
                ProductInfo info = appDbContext.GetProductInfo(product.ProductName);
                if (info == null)
                {
                    appDbContext.ProductInfos.Add(new ProductInfo()
                    {
                        ProductName = product.ProductName,
                        ProductCount = product.ProductCount,
                        ProductPrice = product.ProductPrice
                    });
                }
                else
                {
                    ProductInfo p = appDbContext.GetProductInfo(product.ProductName);
                    if (appDbContext.Equals(p, product.ProductName, product.ProductPrice, product.ProductCount) == false)
                    {
                        p.ProductName = product.ProductName;
                        p.ProductCount = product.ProductCount;
                        p.ProductPrice = product.ProductPrice;
                    }
                }
            }
            appDbContext.SaveChanges();
        }*/
    }
}
