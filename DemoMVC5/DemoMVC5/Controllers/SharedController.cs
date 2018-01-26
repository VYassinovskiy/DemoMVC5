using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DemoMVC5.Controllers
{
    public class SharedController : BaseController
    {
        /// <summary>
        /// Представление для вывода информации об отсутствии прав доступа на действие
        /// </summary>
        /// <returns>Представление</returns>
        public ActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        /// Представление для вывода информации об отсутствии запрашиваемой страницы
        /// </summary>
        /// <returns>Представление</returns>
        public ActionResult PageNotFound()
        {
            return View();
        }

        /// <summary>
        /// Представление для вывода информации об отсутствии пользователя в базе
        /// </summary>
        /// <returns>Представление</returns>
        public ActionResult UserNotFound()
        {
            return View();
        }

        /// <summary>
        /// Представление для вывода информации об ошибке
        /// </summary>
        /// <returns>Представление</returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}