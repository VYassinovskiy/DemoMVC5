using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель редактирования данных о пользователе
    /// </summary>
    public class EditUserModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [Required]
        public string FIO { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [Required]
        public string Login { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Конструктор без параметров
        /// </summary>
        public EditUserModel()
        {
        }

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="product">Объект пользователя из базы данных</param>
        public EditUserModel(DataBase.User user)
        {
            this.Id = user.Id;
            this.Login = user.Login;
            this.FIO = user.FIO;
            this.Password = user.Password;
        }
    }
}