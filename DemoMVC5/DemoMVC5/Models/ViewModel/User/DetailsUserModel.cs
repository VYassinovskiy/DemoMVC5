using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель просмотра информации о пользователе
    /// </summary>
    public class DetailsUserModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string FIO { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект пользователя из базы данных</param>
        public DetailsUserModel(DataBase.User user)
        {
            this.Id = user.Id;
            this.Login = user.Login;
            this.FIO = user.FIO;
        }
    }
}