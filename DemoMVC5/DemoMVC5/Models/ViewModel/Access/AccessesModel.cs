using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoMVC5.Models.Permission;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель для отображения списка доступов к даным
    /// </summary>
    public class AccessesModel
    {
        /// <summary>
        /// Id пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// Сервис
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Id сервиса
        /// </summary>
        public int ServiceId { get; set; }
        /// <summary>
        /// Доступ на создание
        /// </summary>
        public bool Create { get; set; }
        /// <summary>
        /// Доступ на чтение
        /// </summary>
        public bool Read { get; set; }
        /// <summary>
        /// Доступ на изменение
        /// </summary>
        public bool Update { get; set; }
        /// <summary>
        /// Доступ на удаление
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// Конструктор с параметрами для инициализации начальных данных в модели вывода
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service">Сервис</param>
        /// <param name="serviceId">Id сервиса</param>
        /// <param name="create">Доступ на создание</param>
        /// <param name="read">Доступ на чтение</param>
        /// <param name="update">Доступ на изменение</param>
        /// <param name="delete">Доступ на удаление</param>
        public AccessesModel(DataBase.User user, string service, int serviceId, bool create, bool read, bool update, bool delete)
        {
            this.UserId = user.Id;
            this.User = user.Login;
            this.Service = service;
            this.ServiceId = serviceId;
            this.Create = create;
            this.Read = read;
            this.Update = update;
            this.Delete = delete;
        }
    }
}