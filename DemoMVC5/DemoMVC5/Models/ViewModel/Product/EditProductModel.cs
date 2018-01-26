using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель для редактирования данных о товаре
    /// </summary>
    public class EditProductModel
    {
        /// <summary>
        /// Id товара
        /// </summary>
        public int Id { get; set; }
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

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public EditProductModel()
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект товара из базы данных</param>
        public EditProductModel(DataBase.Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Count = product.Count;
        }
    }
}