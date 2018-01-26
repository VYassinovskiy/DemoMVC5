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
    public class AccountController : BaseController
    {
        /// <summary>
        /// Отображение страницы логина
        /// </summary>
        /// <returns>Отображение страницы логина</returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Обработка логина пользователя в систему
        /// </summary>
        /// <param name="model">Модель логина</param>
        /// <returns>Представления для логина в систему</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid) // Если состояние модели корректное, данные заполнены верно
            {              
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    User user = db.User.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                    // Проверка, есть ли авторизуемый пользователь в базе данных
                    if (user != null)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, true); // Записываем авторизованного пользователя в куки
                        return RedirectToAction("Products", "Product"); // Перенаправляем на страницу товаров
                    }
                    else
                        RedirectToAction("UserNotFound", "Shared"); // Перенаправляем на страницу - Пользователь не найден
                }
            }
            return View(model);
        }

        /// <summary>
        /// Отображение списка всех пользователей системы
        /// </summary>
        /// <returns>Представление со списком всех пользователей системы</returns>
        public ActionResult Users()
        {
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                if (!User.HasPermission(Services.User, Actions.Read)) //Проверка наличия необходимых прав доступа
                    return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа

                List<User> Users = db.User.ToList(); // Список всех пользователей
                List<AllUsersModel> AllUsers = new List<AllUsersModel>(); // Список пользователй в виде модели для вывода
                foreach (User user in Users)
                    AllUsers.Add(new AllUsersModel(user)); // Создаем модель пользователя на основе пользователя из базы и добавляем в список для вывода в представление 
                return View(AllUsers);
            }
        }

        /// <summary>
        /// Представление для создания нового пользователя
        /// </summary>
        /// <returns>Представление для создания нового пользователя</returns>
        public ActionResult Create()
        {
            if (!User.HasPermission(Services.User, Actions.Create)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            return View();
        }

        /// <summary>
        /// Сохранение нового пользователя в баззе данных
        /// </summary>
        /// <param name="model">Модель с данными о новом пользователе</param>
        /// <returns>Представление сохранения</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    // Добавляем нового пользователя в базу даннгых
                    db.User.Add(new User { Login = model.Login, Password = model.Password, FIO = model.FIO, Modified = DateTime.Today });
                    db.SaveChanges(); // Сохраняем изменения
                    return RedirectToAction("Users", "Account"); // Переходим к странице со всеми пользоваетлями
                }
            }
            return View(model);
        }

        /// <summary>
        /// Функция для редактирования пользователя
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Представление с данными пользователя</returns>
        public ActionResult Edit(int? id)
        {
            if (!User.HasPermission(Services.User, Actions.Update)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                User user = db.User.Find(id); // Находим пользователя в базе даных
                if (user == null) // Если информация не найдена
                    RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой
                return View(new EditUserModel(user)); // Возвращаем модель с данными о пользователе
            }
        }

        /// <summary>
        /// Сохранение редактируемых данных о пользователе в базе данных
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <param name="model">Модель с данными о пользователе</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, EditUserModel model)
        {
            if (ModelState.IsValid)
            {
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    // Получаем информацию о редактируемом пользователе из базы данных, обновляем данные из модели и сохраняем новую информацию в базе
                    User user = db.User.FirstOrDefault(p => p.Id == id);
                    if (user == null) // Если информация не найдена
                        RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой
                    user.Login = model.Login;
                    user.FIO = model.FIO;
                    user.Password = model.Password;
                    user.Modified = DateTime.Today;
                    db.SaveChanges(); // Сохраняем изменения
                    return RedirectToAction("Users", "Account"); // Возвращаемся к списку всех пользователей
                }
            }
            return View();
        }

        /// <summary>
        /// Функция для отображения информации о пользователе
        /// </summary>
        /// <param name="id">Id пользователея</param>
        /// <returns>Представление с информацией о пользователе</returns>
        public ActionResult Details(int? id)
        {
            if (!User.HasPermission(Services.User, Actions.Read)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                User user = db.User.Find(id); // Находим пользователя в базе данных
                if (user == null) // Если информация не найдена
                    RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой
                return View(new DetailsUserModel(user)); // Создаем модель для отображения данных на основе найденного пользователя и выводим ее в представление
            }
        }

        /// <summary>
        /// Функция удаляет выбранного пользователя и все его доступы из базы
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (!User.HasPermission(Services.User, Actions.Delete)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                User user = db.User.Find(id); // Выполняем запрос к базе по поиску пользователя с указанным id с указанным 
                if (user != null) // Если пользователь найден
                {
                    List<Access> Acesses = db.Access.Where(a => a.User == id).ToList(); // Получаем список доступов пользователя
                    if (Acesses.Any()) // Если доступы есть
                        db.Access.RemoveRange(Acesses); // Удаляем доступы из базы
                    db.User.Remove(user); // Удаляем пользователя из базы
                    db.SaveChanges(); // Сохраняем изменения
                    return RedirectToAction("Users"); //Возвращаемся к списку пользователей
                }
            }
            return View();
        }

    }
}