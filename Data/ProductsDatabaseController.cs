
using System;

using WpfGrid.MVVM;

namespace Mvc_Apteka.Controllers
{

    /// <summary>
    /// Управление структурой БД
    /// </summary>
    public class ProductsDatabaseController
    {


        public bool Config(string ConnectionString)
        {
            using (var appDb = new AppDbContext() {
                ConnectionString = ConnectionString 
            }) {

                
                return appDb.Database.CanConnect();
                 
            }
        
            
        }


        /// <summary>
        /// Обновление структуры данных
        /// </summary>
        public MethodResult CreateDatabase(AppDbContext context)
        {
            bool result = false;
            try
            {
                result = context.Database.EnsureCreated();
                return MethodResult.FromResult(  result);
            }
            catch (Exception ex)
            {
                return MethodResult.FromException(ex);
            }
        }

        /// <summary>
        /// Уничтожение структуры данных
        /// </summary>
        public MethodResult DeleteDatabase(AppDbContext context)
        {
            bool result = false;
            try
            {
                return MethodResult.FromResult(result = context.Database.EnsureDeleted());
            }
            catch (Exception ex)
            {
                return MethodResult.FromException(ex);
            }
        }
    }
}
