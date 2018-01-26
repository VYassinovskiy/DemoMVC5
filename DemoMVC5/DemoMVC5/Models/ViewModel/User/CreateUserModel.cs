using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель создания пользователя
    /// </summary>
    public class CreateUserModel
    {
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
        /// Подтверждение пароля пользователя
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}