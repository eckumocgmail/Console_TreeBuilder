using Microsoft.EntityFrameworkCore.ChangeTracking;

using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Apteka.Entities
{
    /// <summary>
    /// Журнал поступления продукции, изм. цены 
    /// </summary>
    public class ProductActivity
    {         
        [Key]
        public int ID { get; set; }


        /// <summary>
        /// ИД продукции
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Наименование изменённой продукции
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Время фиксации
        /// </summary>
        public DateTime BeginDate { get; set; } = DateTime.Now;
        
        [Display(Name = "Изменение объёма продукции на складе")]
        [Required(ErrorMessage = "Укажите изменение объёма продукции на складе")]
        public int ProductCountDev { get; set; }

        [Display(Name = "Изменение цены продукции за единицу")]
        [Required(ErrorMessage = "Необходимо указать изменение цены продукции за единицу")]
        public float ProductPriceDev { get; set; }


        [Display(Name = "Текущий объём продукции с учётом изменений")]
        [Required(ErrorMessage = "Необходимо указать текущий объём продукции с учётом изменений")]
        public int ProductCount { get; set; }

        [Display(Name = "Текущее значение цены продукции за единицу с учётом изменений")]
        [Required(ErrorMessage = "Необходимо указать текущее значение цены продукции за единицу с учётом изменений")]
        public float ProductPrice { get; set; }
    }
}
