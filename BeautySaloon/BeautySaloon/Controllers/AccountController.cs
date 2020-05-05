using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BeautySaloon.Models;
using BeautySaloon.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeautySaloon.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;
        public AccountController(ApplicationContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            //return Content("<a href = Login");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var login = "Admin";
            var pass = "Admin";
            if (ModelState.IsValid)
            {
                if (model.Email == login && model.Password == pass)
                {
                    return RedirectToAction("Index", "OrderService");
                }
                else
                {
                    User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == GetHashString(model.Password));
                    if (user != null)
                    {
                        await Authenticate(model.Email); // аутентификация

                        return RedirectToAction("Home", "Home");
                    }
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(cl => cl.Email == model.Email);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    db.Users.Add(new User
                    {
                        Surname = model.Surname,
                        Name = model.Name,
                        Patronymic = model.Patronymic,
                        Date = model.Date,
                        Phone = model.Phone,
                        Email = model.Email,
                        Password = GetHashString(model.Password)
                    });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Login", "Account");
                }
                else
                    
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        string GetHashString(string s)
        {
            //переводим строку в байт-массив  
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim - список claims - набор данных, которые шифруются и добавляются в аутентификационные куки
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}

/*Для установки кук применяется асинхронный метод контекста HttpContext.SignInAsync(). В качестве параметра он принимает схему аутентификации, которая была использована при установки middleware app.UseCookieAuthentication в классе Startup. То есть в нашем случае это строка "Cookies". А в качестве второго параметра передается объект ClaimsPrincipal, который представляет пользователя.

Для правильного создания и настройки объекта ClaimsPrincipal вначале создается список claims - набор данных, которые шифруются и добавляются в аутентификационные куки. Каждый такой claim принимает тип и значение. В нашем случае у нас только один claim, который в качестве типа принимает константу ClaimsIdentity.DefaultNameClaimType, а в качестве значения - email пользователя.

Далее создается объект ClaimsIdentity, который нужен для инициализации ClaimsPrincipal. В ClaimsIdentity передается:

Ранее созданный список claims

Тип аутентификации, в данном случае "ApplicationCookie"

Тип данных в списке claims, который преставляет логин пользователя. То есть при добавлении claimа мы использовали в качестве типа ClaimsIdentity.DefaultNameClaimType, поэтому и тут нам надо указать то же самое значение. Мы, конечно, можем указать и разные значения, но тогда система не сможет связать различные claim с логином пользователя.

Тип данных в списке claims, который представляет роль пользователя. Хотя у нас такого claim нет, который бы представлял роль пользователя, но но опционально мы можем указать константу ClaimsIdentity.DefaultRoleClaimType. В данном случае она ни на что не влияет.

И после вызова метода расширения HttpContext.SignInAsync в ответ клиенту будут отправляться аутентификационные куки, которые при последующих запросах будут передаваться обратно на сервер, десериализоваться и использоваться для аутентификации пользователя.

Метод Register сделан аналогично методу Login, только теперь мы получаем данные регистрации через объект RegisterModel и перед аутентификацией сохраняем эти данные в базу данных.
*/
