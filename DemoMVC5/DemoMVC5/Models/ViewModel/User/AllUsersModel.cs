using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataBase = DemoMVC5.Models.DataBase;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель для просмотра всех пользователей
    /// </summary>
    public class AllUsersModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string FIO { get; set; }
        /// <summary>
        /// Дата последнего изменения данных о пользователе
        /// </summary>
        public string Modified { get; set; }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект пользователя из базы данных</param>
        public AllUsersModel(DataBase.User user)
        {
            this.Id = user.Id;
            this.Login = user.Login;
            this.Password = user.Password;
            this.FIO = user.FIO;
            this.Modified = user.Modified.ToShortDateString();
        }
    }
}