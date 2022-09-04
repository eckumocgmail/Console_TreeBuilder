 
using Mvc_Apteka.Entities;
 

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mvc_Apteka  
{

 /*   /// <summary>
    /// Импорт-экспорт файлов формата XML
    /// </summary>
    public class ProductsXmlController: FilesController
    {
        private readonly AppDbContext appDbContext;

        public ProductsXmlController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        /// <summary>
        /// Экспорт файла XML
        /// </summary>
        public IActionResult DownloadXml( AppDbContext appDbContext)
        {
            string json = SerializeObject(appDbContext.ProductInfos.ToList());
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            return Download("products.xml", "application/xml", bytes);
        }

        private string SerializeObject(List<ProductInfo> productInfos)
        {
            var serializer = new XmlSerializer(typeof(List<ProductInfo>));

            using(var stream = new MemoryStream())
            {
                serializer.Serialize(stream, productInfos);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
        public T DeserializeObject<T>(string text)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var stream = new StringReader(text))
            {
                var infos = (T)serializer.Deserialize(stream);
                return infos;
            }
        }

        /// <summary>
        /// Импорт файла XML
        /// </summary>        
        public async Task<object> UploadXml()
        {
            if (HttpContext.Request.ContentType == "multipart/form-data")
            {
                try
                {
                    var FileMessageModel = await Upload("applciation/xml", 1024 * 1024 * 1024);
                    string text = Encoding.UTF8.GetString(FileMessageModel.FileData);
                    var records = DeserializeObject<List<ProductInfo>>(text);
                    foreach (var product in records)
                    {
                      
                        appDbContext.AddOrUpdate(product);
                        appDbContext.SaveChanges();                         
                    }
                    appDbContext.SaveChanges();

                    return MethodResult.FromResult(true);
                }
                catch (Exception ex)
                {
                    return MethodResult.FromException(ex);
                }

            }
            else
            {
                return View();
            }
        }

        


        /// <summary>
        /// Считывание данных из XML0-файла
        /// </summary>
        /// <param name="filename">путь к файлу</param>    
        public IActionResult InitXml([FromServices] AppDbContext context, [FromServices] IWebHostEnvironment env)
        {

            var DrugList = new List<LS>();
            string filepath = Path.Combine(
                env.ContentRootPath,
                "Resources",
                "input.xml"
            ).ToString();
            using (var stream = new StreamReader(System.IO.File.OpenRead(filepath)))
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
                    var ProductCatalog = new Entities.ProductCatalog()
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
                        context.AddOrUpdate(info);
                        context.SaveChanges();

                        info.ProductCount = count;
                        info.ProductPrice = price;

                        context.SaveChanges();

                    }

                    //context.AddOrUpdate(ProductCatalog);
                }
            }
            return View(DrugList);
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
    }*/
}
