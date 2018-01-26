using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель создания товара
    /// </summary>
    public class CreateProductModel
    {
        /// <summary>
        /// Наименование товара
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Описание товара
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Количество товара
        /// </summary>
        [Required]
        public int Count { get; set; }

    }
}