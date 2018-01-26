using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DemoMVC5.Models.DataBase;
using DemoMVC5.Models.ViewModel;
using DemoMVC5.Models.Permission;

namespace DemoMVC5.Controllers
{
    public class ProductController : BaseController
    {
        /// <summary>
        /// Представление списка всех товаров
        /// </summary>
        /// <returns>Представление списка всех товаров</returns>
        public ActionResult Products()
        {
            if (!User.HasPermission(Services.Product, Actions.Read)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            // Получение списка товаров из базы данных
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                List<Product> Products = db.Product.Where(u => u.Id > 0).ToList(); // Список всех товаров
                List<AllProductsModel> AllProducts = new List<AllProductsModel>(); // Список модели для вывода 
                foreach (Product product in Products)
                    AllProducts.Add(new AllProductsModel(product)); // Добавление товаров из базы в список модели для вывода
                return View(AllProducts); // Вывод списка товаров в представление
            }
        }

        /// <summary>
        /// Функция для добавления нового товара в базу
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (!User.HasPermission(Services.Product, Actions.Create)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа

            return View();
        }


        /// <summary>
        /// Сохранение информации о добавленном товаре в базе
        /// </summary>
        /// <param name="model">Модель с информацией о товаре</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProductModel model)
        {
            if (ModelState.IsValid)
            {
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    User user = db.User.FirstOrDefault(u => u.Login == User.Identity.Name); // Получение информации о пользователе из базы
                    // Добавление нового товара в базу
                    db.Product.Add(new Product { Name = model.Name, Description = model.Description, Count = model.Count, Modified = DateTime.Today, ModifiedBy = user.Id });
                    db.SaveChanges(); // Сохранение изменений
                    return RedirectToAction("Products", "Product"); // Переход к списку всех товаров
                }
            }
            return View(model);
        }

        /// <summary>
        /// Функция редактирования товара
        /// </summary>
        /// <param name="id">Id товара</param>
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            if (!User.HasPermission(Services.Product, Actions.Update)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {               
                Product product = db.Product.Find(id); // Получаем информацию о товаре из базы данных
                if (product == null) // Если информация не найдена
                    RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой
                return View(new EditProductModel(product)); // Создаем модель с отображением информации о выбранном товаре и выводим модель
            }
        }

        /// <summary>
        /// Функция сохраняет в базе изменненный товар
        /// </summary>
        /// <param name="id">Id товара</param>
        /// <param name="model">Модель с информацией о товаре</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, EditProductModel model)
        {
            if (ModelState.IsValid)
            {
                using (DemoMVC5Entities db = new DemoMVC5Entities())
                {
                    User user = db.User.FirstOrDefault(u => u.Login == User.Identity.Name);
                    // Изменение товара
                    if (user != null)
                    {
                        Product product = db.Product.Find(id); // Получаем информацию о товаре
                        if (product == null) // Если информация не найдена
                            RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой

                        product.Name = model.Name;
                        product.Description = model.Description;
                        product.Count = model.Count;
                        product.Modified = DateTime.Today;
                        product.ModifiedBy = user.Id;
                        db.SaveChanges(); // Заполняем информацию о товаре и сохраняем изменения в базе
                        return RedirectToAction("Products", "Product"); // Переходим на страницу со списком товаров

                    }
                }
            }
            return View();
        }

        /// <summary>
        /// Функция отображает информацию о товаре
        /// </summary>
        /// <param name="id">Id товара</param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (!User.HasPermission(Services.Product, Actions.Read)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {               
                Product product = db.Product.Find(id); // Находим товар в базе данных
                if (product == null) // Если информация не найдена
                    RedirectToAction("PageNotFound", "Shared"); // Переходим на страницу с ошибкой
                return View(new DetailsProductModel(product)); // Создаем модель с отображением товара на основе товара из базы и выводим информацию о товаре 
            }
        }

        /// <summary>
        /// Удаление товара из базы
        /// </summary>
        /// <param name="id">Id товара</param>
        /// <returns></returns>
        public ActionResult Delete(int? id)
        {
            if (!User.HasPermission(Services.Product, Actions.Delete)) //Проверка наличия необходимых прав доступа
                return RedirectToAction("AccessDenied", "Shared"); // Если доступа нет, перейти на страницу с ошибкой доступа
            if (id == null) // Проверка наличия id в адресе запрашиваемой страницы
                RedirectToAction("PageNotFound", "Shared"); // Если id не указан, перейти на страницу с сообщением о том, что запрашиваемая страница не найдена

            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                Product product = db.Product.Find(id); // Выполняем запрос к базе по поиску товара с указанным 
                if (product != null) // Если товар найден
                {
                    db.Product.Remove(product); // Удаляем товар из базы
                    db.SaveChanges(); // Сохраняем изменения
                    return RedirectToAction("Products"); // Переходим к списку всех товаров
                }
            }
            return View();
        }

        /// <summary>
        /// Функция экспортирует в Excel список всех товаров из базы данных. 
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            using (DemoMVC5Entities db = new DemoMVC5Entities())
            {
                GridView gv = new GridView();
                // Здесь я вывожу в Excel таблицу продуктов из базы, по аналогии можно выводить все что угодно, преобразуя к List
                gv.DataSource = db.Product.ToList(); // Сохраняем в GridView gv список всех товаров из базы
                gv.DataBind();
                // Создаем в Response объект типа приложение excel с именем Products.xls и загружаем файл пользователю
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Products.xls");
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                StringWriter sw = new StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                gv.RenderControl(htw);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                return RedirectToAction("Products"); // Возвращаемся к списку товаров
            }
        }
    }
}