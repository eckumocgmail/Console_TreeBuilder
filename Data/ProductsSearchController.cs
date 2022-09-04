 
using Mvc_Apteka.Entities;

using System;
using System.Collections.Generic;
using System.Linq;

using WpfGrid.Data.Services;

namespace Mvc_Apteka
{
    /// <summary>
    /// Страница поиска с карточным отображением и фильтрами по цене и объёму продукции
    /// </summary>
    public class ProductsSearchController 
    {


        public class SearchModel
        {
            public string SearchQuery { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int TotalResults { get; set; }
            public List<ProductInfo> SearchResults { get; set; }
 
        }

        /// <summary>
        /// Выбор данных
        /// </summary>
        public virtual SearchModel OnSearch(
            AppDataService context,
            string searchInput = "",
            float minPrice = 0,
            float maxPrice = 1000000,
            int minCount = 0,
            int maxCount = 1000000,

            int PageNumber = 1,
            int PageSize = 10)
        {
            var result = context.GetProductInfos();
            if (!result.Success)
                throw new Exception($"Ошибка при получении сведений о продукции: {result.Message}");

            IEnumerable<ProductInfo> infos = null;
            if ((String.IsNullOrWhiteSpace(searchInput)))
            {

                infos = result.Result;
            }
            else
            {
                infos = result.Result.Where(p => p.ProductName.ToUpper().IndexOf(searchInput.ToUpper()) != -1);
            }
            infos = context.ProductsSearch(infos, minCount, maxCount, minPrice, maxPrice).Result;
            return new SearchModel()
            {
                PageNumber= PageNumber,
                PageSize= PageSize,
                SearchQuery= searchInput,
                TotalResults= infos.Count(),
                SearchResults= infos.Skip((PageNumber - 1) * PageSize).Take(PageSize).ToList()
            };
        }


        /// <summary>
        /// Запрос терминов для автоподстановки в строке поиска
        /// </summary>           
        public virtual object OnInput(AppDbContext context, string value)
        {            
            var products =
                String.IsNullOrWhiteSpace(value) ?
                context.ProductInfos :
                context.ProductInfos.Where(p => p.ProductName.ToUpper().IndexOf(value.ToUpper()) != -1);
            return new
            {
                Options = products.Select(p => p.ProductName).ToList()
            };
        }
    }
}
