using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using DemoMVC5.Models.DataBase;
using DemoMVC5.Models.ViewModel;
using DemoMVC5.Models.Permission;

namespace DemoMVC5.Controllers
{
    public class AccessController : BaseController
    {
        /// <summary>
        /// Функция вывода списка со всеми доступами конкретного пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns>Список доступов</returns>
        public ActionResult Accesses(int? id)
        {
            if (!User.HasPermission(Services.User, Actions.Read)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            List<AccessesModel> AllAccesses = new List<AccessesModel>(); // Список доступов для вывода
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                User user = db.User.FirstOrDefault(u => u.Id == id); // Находим пользователя, для которого нужно отобразить доступы
                foreach (Services service in Enum.GetValues(typeof(Services))) // Проходим по перечислению существующих сервисов
                {
                    Service dbService = db.Service.FirstOrDefault(s => s.Id == (int)service); // Получаем сервис по его id из базы данных
                    // Добавляем в список новое значение содержащее сервис и доступы к нему
                    AllAccesses.Add(new AccessesModel(user, dbService.Name, (int)service,
                        user.HasPermission(service, Actions.Create), user.HasPermission(service, Actions.Read),
                        user.HasPermission(service, Actions.Update), user.HasPermission(service, Actions.Delete)));
                }
            }           
            return View(AllAccesses); // Передаем список доступов в представление
        }

        /// <summary>
        /// Функция редактирования прав доступа пользователя
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="service">Id сервиса</param>
        /// <returns>Представление для редактирования прав доступа</returns>
        public ActionResult Edit(int? id, int? service)
        {
            if (!User.HasPermission(Services.User, Actions.Update)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            Services uService = (Services)service; // По Id сервиса преобразуем к сответствующему элементу перечисления
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                User user = db.User.FirstOrDefault(u => u.Id == id); // Получаем пользователя из базы по его Id
                Service dbService = db.Service.FirstOrDefault(s => s.Id == (int)service); // Получаем сервис из базы по его Id
                // Создаем модель для редактирования прав доступа пользователя и выводим ее
                return View(new EditAccessesModel(user, dbService.Name, user.HasPermission(uService, Actions.Create),
                    user.HasPermission(uService, Actions.Read), user.HasPermission(uService, Actions.Update),
                    user.HasPermission(uService, Actions.Delete)));
            }
        }

        /// <summary>
        /// Функция методом пост сохраняет результат редактирования прав доступа пользователя в базе
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="service">Id сервиса</param>
        /// <param name="model">Модель редактирования</param>
        /// <returns>Результат сохранения пользователя в базе</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, int?service, EditAccessesModel model)
        {
            if (ModelState.IsValid)
            {
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    User user = db.User.FirstOrDefault(u => u.Id == id); // Получаем текущего пользователя из базы
                    List<Actions> ModelActions = model.GetActionsList(); // Получаем список доступов которые необходимо предоставить (выбранные чекбоксы в представлении)
                    foreach (Actions action in Enum.GetValues(typeof(Actions))) // Цикл по всем видам доступов из перечисления
                    {
                        // Получаем строку из базы для указанного пользователя, сервиса и доступа
                        Access access = db.Access.Where(u => u.User == id && u.Service == service && u.Action == (int)action).FirstOrDefault();
                        if (access == null && ModelActions.Contains(action)) // Если доступа нет, но нужно предоставить
                            db.Access.Add(new Access { User = (int)id, Service = (int)service, Action = (int)action }); // Добавляем строку доступа в базе
                        else if (access != null && !ModelActions.Contains(action)) // Если доступ есть, но предоставлять не нужно
                            db.Access.Remove(access); // Удаляем доступ из базы
                        db.SaveChanges(); // Сохраняем изменения
                    }
                    return RedirectToAction("Accesses", "Access", new { id = id }); // Возвращаемся к списку доступов
                }
            }
            return View();
        }
    }
}