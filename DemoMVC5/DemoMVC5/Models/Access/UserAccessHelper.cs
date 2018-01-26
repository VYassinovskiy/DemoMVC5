using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoMVC5.Models.DataBase;

namespace DemoMVC5.Models.Permission
{
    /// Перечисления являются простым и удобным к использованию способом перечислить сревисы и операции над ними
    /// Возможен более сложный вариант с использованием значений действий и сервисов напрямую из базы, он более сложен, менее нагляден, но более устойчив к изменениям


    /// <summary>
    /// Список действий доступных пользователю над сервисами
    /// </summary>
    public enum Actions
    {
        Create=1,
        Read=2,
        Update=3,
        Delete=4
    }

    /// <summary>
    /// Список сервисов
    /// </summary>
    public enum Services
    {
        User=1,
        Product=2
    }

    /// <summary>
    /// Добавил два метода-расширения для того, чтобы рализовать проверку прав удобно
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Метод-расширение проверяет наличие прав доступа у пользователя для авторизованного пользователя, наследника IPrincipal
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service">Сервис, к которому необходим доступ</param>
        /// <param name="action">Уровень доступа определяется действием, которое необходимо выполнить, используется стандартная модель CRUD</param>
        /// <returns></returns>
        public static bool HasPermission(this System.Security.Principal.IPrincipal user, Services service, Actions action)
        {
            return HasPermission(user.Identity.Name, service, action);
        }

        /// <summary>
        /// Метод-расширение проверяет наличие прав доступа у пользователя для пользователя из базы
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service">Сервис, к которому необходим доступ</param>
        /// <param name="action">Уровень доступа определяется действием, которое необходимо выполнить, используется стандартная модель CRUD</param>
        /// <returns></returns>
        public static bool HasPermission(this DataBase.User user, Services service, Actions action)
        {
            return HasPermission(user.Login, service, action);
        }


        /// <summary>
        /// Метод проверяет наличие прав доступа у пользователя по логину
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="service">Сервис, к которому необходим доступ</param>
        /// <param name="action">Уровень доступа определяется действием, которое необходимо выполнить, используется стандартная модель CRUD</param>
        /// <returns></returns>
        public static bool HasPermission(string login, Services service, Actions action)
        {
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                // Проверка, есть ли данный пользователь в базе данных
                User dbUser = db.User.FirstOrDefault(u => u.Login == login);
                if (dbUser != null)
                {
                    // Проверка, есть ли для указанного пользователя необходимые настройки доступа
                    Access access = db.Access.FirstOrDefault(a => a.User == dbUser.Id && a.Service == (int)service && a.Action == (int)action);
                    if (access != null) return true; // если доступ есть
                }

            }
            return false; // если доступа нет
        }
    }
}