using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель просмотра информации о товаре
    /// </summary>
    public class DetailsProductModel
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
        /// Описание товара
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Дата последнего изменения товара
        /// </summary>
        public string Modified { get; set; }
        /// <summary>
        /// Автор последних изменений товара
        /// </summary>
        public string ModifiedBy { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект товара из базы данных</param>
        public DetailsProductModel(DataBase.Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Description = product.Description;
            this.Modified = product.Modified.ToShortDateString();
            this.ModifiedBy = product.User.Login;
        }
    }
}