
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Mvc_Apteka
{

    /// <summary>
    /// Контекст EFCore 
    /// </summary>
    public class AppDbContext : DbContext  
    {
     

        public virtual DbSet<ProductCatalog> ProductCatalogs { get; set; }
        public virtual DbSet<ProductInfo> ProductInfos { get; set; }
        public virtual DbSet<ProductActivity> Activities { get; set; }
        public string ConnectionString { get; set; }

        public AppDbContext() : base()
        {
            ConnectionString = InitConnectionString();
        }
             
        /// <summary>
        /// Конфигурация параметров соединения
        /// </summary>        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Console.WriteLine(ConnectionString);
            if (optionsBuilder.IsConfigured == false)
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        private string InitConnectionString()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = "AGENT\\KILLER";
            builder.TrustServerCertificate = false;
            builder.IntegratedSecurity = true;
            builder.InitialCatalog = "mdb";
            return builder.ToString();
        }


        /// <summary>
        /// Обнаружение изменений сведений о продукции на складе,
        /// при обнаружении выполняется запись в журнал
        /// </summary>
        public void BeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();         
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if(entry.Entity.ToString() == typeof(Entities.ProductInfo).FullName)
                {
                    var activity = new ProductActivity();
                    bool save = false;
                    foreach (PropertyEntry property in entry.Properties)
                    {
                        if (property.IsModified)
                        {
                            
                            Console.WriteLine(property.OriginalValue + "=>" + property.CurrentValue);
                            switch (property.Metadata.Name)
                            {
                                case nameof(ProductInfo.ProductCount):
                                    save = true;
                                    activity.ProductCount = (int)property.CurrentValue;
                                    activity.ProductCountDev = (int)property.CurrentValue - (int)property.OriginalValue;
                                    break;
                                case nameof(ProductInfo.ProductPrice):
                                    save = true;
                                    activity.ProductPrice = (float)property.CurrentValue;
                                    activity.ProductPriceDev = (float)property.CurrentValue - (float)property.OriginalValue;
                                    break;
                            }

                        }
                        switch (property.Metadata.Name)
                        {
                            case nameof(ProductInfo.ProductName):
                                activity.ProductName = (string)property.CurrentValue;                                
                                break;
                            case nameof(ProductInfo.ID):
                                activity.ProductID = (int)property.CurrentValue;                                
                                break;
                        }
                    }
                    if(save)
                        this.Activities.Add(activity);
                }                                
            }
        }
        
        /// <summary>
        /// Фиксация изменений 
        /// </summary>
        public override int SaveChanges()
        {
            this.BeforeSaveChanges();
            return base.SaveChanges();
        }

        /// <summary>
        /// Фиксация изменений 
        /// </summary>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {           
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
         
    }
}
