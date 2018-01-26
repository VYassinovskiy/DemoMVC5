using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DemoMVC5.Models.Permission;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC5.Models.ViewModel
{
    /// <summary>
    /// Модель редактирования прав доступа пользователя
    /// </summary>
    public class EditAccessesModel
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
        /// Метод возвращает список прав доступа, доступных данном пользователю для данного сервиса
        /// </summary>
        /// <returns>Список прав доступа</returns>
        public List<Actions> GetActionsList()
        {
            List<Actions> Actions = new List<Actions>();
            if (Create) Actions.Add(Permission.Actions.Create);
            if (Read) Actions.Add(Permission.Actions.Read);
            if (Update) Actions.Add(Permission.Actions.Update);
            if (Delete) Actions.Add(Permission.Actions.Delete);
            return Actions;
        }

        /// <summary>
        /// Конструктор по умолчанию, для инициализации модели без параметров
        /// </summary>
        public EditAccessesModel()
        {
        }

        /// <summary>
        /// Конструктор с параметрами для инициализации начальных данных в модели вывода
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service">Сервис</param>
        /// <param name="create">Доступ на создание</param>
        /// <param name="read">Доступ на чтение</param>
        /// <param name="update">Доступ на изменение</param>
        /// <param name="delete">Доступ на удаление</param>
        public EditAccessesModel(DataBase.User user, string service, bool create, bool read, bool update, bool delete)
        {
            this.UserId = user.Id;
            this.User = user.Login;
            this.Service = service;
            this.Create = create;           
            this.Read = read;
            this.Update = update;
            this.Delete = delete;
        }
    }
}