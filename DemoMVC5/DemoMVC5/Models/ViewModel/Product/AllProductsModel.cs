using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase = DemoMVC5.Models.DataBase;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель списка всех товаров
    /// </summary>
    public class AllProductsModel
    {
        /// <summary>
        /// Id товара
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Наименование товара
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Количество товара
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Дата последнего изменения товара
        /// </summary>
        public string Modified { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект товара из базы данных</param>
        public AllProductsModel(DataBase.Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Count = product.Count;
            this.Modified = product.Modified.ToShortDateString();
        }
    }
}