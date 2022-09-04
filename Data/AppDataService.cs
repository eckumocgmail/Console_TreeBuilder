using Microsoft.EntityFrameworkCore;

using Mvc_Apteka;
using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

using WpfGrid.Data.Resources;
using WpfGrid.Data.Services.Xml;
using WpfGrid.MVVM;

namespace WpfGrid.Data.Services
{
    public interface ICrudProductInfoService
    {
        Task<MethodResult<IEnumerable<ProductActivity>>> GetHistory();
     
        Task AddOrUpdate(ProductCatalog TargetCatalog);
        Task<bool> AddOrUpdate(ProductInfo info);
        Task<bool> AddOrUpdate(string productName, float productPrice, int productCount);
        void Clear();
        Task<int> Create(ProductInfo productInfo);
        bool Equals(ProductInfo ProductInfo, string ProductName, float ProductPrice, float ProductCount);
        Task<ProductCatalog> GetProductCatalog(string ProductCatalogName);
        Task<ProductInfo> GetProductInfo(string ProductName);
        Task<bool> HasProductWithName(string Name);
        Task<IEnumerable<ProductInfo>> ListProducts();
        Task<IEnumerable<ProductInfo>> ProductCountInRange(int min, int max);
        Task<IEnumerable<ProductInfo>> ProductCountInRange(IEnumerable<ProductInfo> products, int min, int max);
        Task<IEnumerable<ProductInfo>> ProductPriceInRange(float min, float max);
        Task<IEnumerable<ProductInfo>> ProductPriceInRange(IEnumerable<ProductInfo> products, float min, float max);
        Task<IEnumerable<ProductInfo>> ProductsSearch(IEnumerable<ProductInfo> products, int minCount, int maxCount, float minPrice, float maxPrice);
        Task<int> Remove(ProductInfo productInfo);
        Task<int> Update(ProductInfo productInfo);
    }

    public class AppDataService      
    {
        


        public async Task<byte[]> GetDataForExport( )
        {
            using (var db = new AppDbContext())
            {
                var ctrl = new ProductsJsonController( );
                return await ctrl.GetDataForExport();
            }
        }

        public int Create(ProductInfo productInfo)
        {
            using (var context = new AppDbContext())
            {
                context.ProductInfos.Add(productInfo);
                return context.SaveChanges();
            }
        }

        public async Task<int> CreateAsync(ProductInfo productInfo)
        {
            using (var context = new AppDbContext())
            {
                context.ProductInfos.Add(productInfo);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<int> Update(ProductInfo productInfo)
        {
            using (var context = new AppDbContext())
            {
                context.Update(productInfo);
                return await context.SaveChangesAsync();
            }
        }

       
        public async Task InitData( )
        {
            var DrugList = new List<LS>();
 

                                    
            using (var context = new AppDbContext())
            using (var stream = new StringReader(InputData.InputDataXml))
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(stream);
                foreach (DataRow row in dataset.Tables[0].Rows)
                {
                    DrugList.Add(new LS()
                    {
                        MNN = row[0].ToString(),
                        LS_Id = int.Parse(row[1].ToString())
                    });
                }
                foreach (DataRow row in dataset.Tables[1].Rows)
                {

                    int catalogId = int.Parse(row[3].ToString());
                    LS catalog = DrugList.Where(x => x.LS_Id == catalogId).FirstOrDefault();
                    catalog.Products.Add(new DATA()
                    {
                        NAME = row[0].ToString(),
                        COUNT = row[2].ToString(),
                        PRICE = row[1].ToString()
                    });
                }



                foreach (LS next in DrugList)
                {
                    var ProductCatalog = new ProductCatalog()
                    {
                        ProductCatalogName = next.MNN,
                        ProductCatalogNumber = next.LS_Id
                    };

                    foreach (DATA record in next.Products)
                    {
                        int count = (int)Math.Floor(float.Parse(record.COUNT.Replace(".", ",")));
                        float price = float.Parse(record.PRICE.Replace(".", ","));
                        var info = new ProductInfo()
                        {
                            ProductCatalogID = ProductCatalog.ID,
                            ProductName = record.NAME,
                            ProductCount = 0,
                            ProductPrice = 0
                        };
                        await AddOrUpdate(context, info);
                        context.SaveChanges();

                        info.ProductCount = count;
                        info.ProductPrice = price;

                        context.SaveChanges();

                    }

                    //context.AddOrUpdate(ProductCatalog);
                }
            }
            
 
        }
        

        public List<ProductInfo> GetProducts()
        {
            using (var context = new AppDbContext())
            {                
                return context.ProductInfos.ToList();
            }
        }

        public async Task<int> Remove(ProductInfo productInfo)
        {
            using (var context = new AppDbContext())
            {
                context.Update(productInfo);
                return await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProductInfo>> ProductsSearch(IEnumerable<ProductInfo> products, int minCount, int maxCount, float minPrice, float maxPrice)
        {
            var productsInCountingRange = await ProductCountInRange(products, minCount, maxCount);
            var productsInPriceRange = await ProductPriceInRange(productsInCountingRange, minPrice, maxPrice);
            return productsInPriceRange;
        }

        public Task<IEnumerable<ProductInfo>> ProductCountInRange(IEnumerable<ProductInfo> products, int min, int max)
            => Task.FromResult(products.Where(p => p.ProductCount >= min && p.ProductCount <= max));

        public async Task Import(IEnumerable<ProductInfo> products)
        {
            foreach(var product in products)
            {
                this.AddOrUpdate(product);
            }
        }

        public Task<IEnumerable<ProductInfo>> ProductPriceInRange(IEnumerable<ProductInfo> products, float min, float max)
            => Task.FromResult(products.Where(p => p.ProductPrice >= min && p.ProductPrice <= max));

        public Task<IEnumerable<ProductInfo>> ProductCountInRange(int min, int max)
        {
            using (var context = new AppDbContext())
            {
                return this.ProductCountInRange(context.ProductInfos, min, max);
            }
        }

        public Task<IEnumerable<ProductInfo>> ProductPriceInRange(float min, float max)
        {
            using(var db = new AppDbContext())
            {
                return this.ProductPriceInRange(db.ProductInfos, min, max);
            }
        }
        
        public Task<bool> HasProductWithName(string Name)
        {
            using (var context = new AppDbContext())
            {
                return Task.FromResult(context.ProductInfos.AsNoTracking().Any(p => p.ProductName.ToUpper() == Name.ToUpper()));
            }
        }

        public Task<ProductCatalog> GetProductCatalog(string ProductCatalogName)
        {
            using (var context = new AppDbContext())
            {
                return context.ProductCatalogs.AsNoTracking().Where(p => p.ProductCatalogName == ProductCatalogName).FirstOrDefaultAsync();
            }
        }
    
        public async Task AddOrUpdate( ProductCatalog TargetCatalog)
        {
            using (var context = new AppDbContext())
            {
                await AddOrUpdate(context, TargetCatalog);
            }
        }
        public async Task AddOrUpdate(AppDbContext context, ProductCatalog TargetCatalog)
        {                       
            var CurrentCatalog = context.ProductCatalogs.AsNoTracking().Where(p => p.ProductCatalogName == TargetCatalog.ProductCatalogName).FirstOrDefault();
            if (CurrentCatalog == null)
            {
                context.ProductCatalogs.Add(TargetCatalog);
                context.SaveChanges();
            }
            else
            {
                var CurrentProducts = context.ProductInfos.Where(p => p.ProductCatalogID == TargetCatalog.ID);
                var TargetProducts = TargetCatalog.Products;

                HashSet<string> CurrentProductNames = CurrentProducts.Select(p => p.ProductName).ToHashSet();
                HashSet<string> TargetProductNames = TargetProducts.Select(p => p.ProductName).ToHashSet();

                HashSet<string> CurrentExpectTarget = new HashSet<string>();
                HashSet<string> TargetExpectCurrent = new HashSet<string>();
                HashSet<string> TargetInspectCurrent = new HashSet<string>();
                foreach (var ProductName in TargetProductNames.Intersect(CurrentProductNames))
                    TargetInspectCurrent.Add(ProductName);
                foreach (var ProductName in TargetProductNames.Except(CurrentProductNames))
                    TargetExpectCurrent.Add(ProductName);
                foreach (var ProductName in CurrentProductNames.Except(TargetProductNames))
                    CurrentExpectTarget.Add(ProductName);


                /// удаляем записи которых нет в текущем наборе
                foreach (var Product in TargetProducts.Where(p => TargetExpectCurrent.Contains(p.ProductName)).ToList())
                    context.ProductInfos.Add(Product);
                int ProductsAdded = context.SaveChanges();

                /// удаляем записи которых нет в целевом наборе
                foreach (var Product in CurrentProducts.Where(p => CurrentExpectTarget.Contains(p.ProductName)).ToList())
                    context.ProductInfos.Remove(Product);
                int ProductsRemoved = context.SaveChanges();

                int ProductsUpdated = 0;
                /// остальные записи сравниваем и обновляем
                foreach (var Product in CurrentProducts.Where(p => TargetInspectCurrent.Contains(p.ProductName)).ToList())
                {
                    var TargetProduct = TargetProducts.Where(p => p.ProductName == Product.ProductName).First();
                    if (await this.AddOrUpdate(context, Product.ProductName, TargetProduct.ProductPrice, TargetProduct.ProductCount))
                        ProductsUpdated++;
                }
            }
            

        }

        public ProductInfo GetProductInfo(string ProductName)
        {
            using (var context = new AppDbContext())
            {
                return context.ProductInfos.AsNoTracking().Where(p => p.ProductName == ProductName).FirstOrDefault();
            }
        }

        public async Task<ProductInfo> GetProductInfoAsync(string ProductName)
        {
            using (var context = new AppDbContext())
            {
                return await context.ProductInfos.AsNoTracking().Where(p => p.ProductName == ProductName).FirstOrDefaultAsync();
            }
        }





        public async Task<bool> AddOrUpdate(ProductInfo info)
        {
            using(var db = new AppDbContext())
            {
                return await AddOrUpdate(db, info);
            }
        }
        public async Task<bool> AddOrUpdate(AppDbContext db, ProductInfo info)        
        {
            return await AddOrUpdate(db, info.ProductName, info.ProductPrice, info.ProductCount);
        }

        public async Task<bool> AddOrUpdate(AppDbContext context,  string productName, float productPrice, int productCount)
        {
          
            ProductInfo p = this.GetProductInfo(productName);
            if (p == null)
            {
                context.ProductInfos.Add(new ProductInfo()
                {
                    ProductName = productName,
                    ProductCount = productCount,
                    ProductPrice = productPrice
                });
            }
            else
            {
                if (Equals(p, productName, productPrice, productCount) == false)
                {
                    p.ProductName = productName;
                    p.ProductCount = productCount;
                    p.ProductPrice = productPrice;
                }
            }

            await context.SaveChangesAsync();
            return true;
    
        }

        public bool Equals(ProductInfo ProductInfo, string ProductName, float ProductPrice, float ProductCount)
        =>
            ProductInfo.ProductName != ProductName ? false :
            ProductInfo.ProductPrice != ProductPrice ? false :
            ProductInfo.ProductCount != ProductCount ? false : true;

        public void Clear()
        {
            using (var context = new AppDbContext())
            {
                foreach (var act in context.Activities.ToList())
                    context.Remove(act);
                foreach (var info in context.ProductInfos.ToList())
                    context.Remove(info);
                foreach (var catalog in context.ProductCatalogs.ToList())
                    context.Remove(catalog);
                context.SaveChanges();
            }

        }

        public MethodResult<IEnumerable<ProductInfo>> GetProductInfos()
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    return MethodResult<IEnumerable<ProductInfo>>.FromResult( context.ProductInfos.ToList());
                }

            }catch (Exception ex)
            {
                return MethodResult<IEnumerable<ProductInfo>>.FromException(ex);
            }
        }

        public async Task<IEnumerable<ProductInfo>> GetProductInfosAsync()
        {
            await Task.CompletedTask;
            using (var context = new AppDbContext())
                return await context.ProductInfos.ToListAsync();
        }

        public async Task<MethodResult<IEnumerable<ProductActivity>>> GetHistory()
        {
            try
            {
                await Task.CompletedTask;
                using (var context = new AppDbContext())
                    return MethodResult<IEnumerable<ProductActivity>>.FromResult(context.Activities.ToList());
            }catch (Exception ex)
            {
                return MethodResult<IEnumerable<ProductActivity>>.FromException(ex);
            }
        }
    }



    /// <summary>
    /// Первичные структуры 
    /// </summary>
    namespace Xml
    {
        /// <summary>
        /// Сведения о лекарственном препарате
        /// </summary>
        public class LS
        {
            public System.String MNN { get; set; }
            public System.Int32 LS_Id { get; set; }

            [XmlIgnore]
            public List<DATA> Products { get; set; } = new List<DATA> { };
        }


        /// <summary>
        /// Продажи лекарств
        /// </summary>
        public class DATA
        {
            public System.String NAME { get; set; }
            public System.String PRICE { get; set; }
            public System.String COUNT { get; set; }
            public System.Int32 LS_Id { get; set; }
        }
    }
}
