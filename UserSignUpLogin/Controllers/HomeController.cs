using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using UserSignUpLogin.Models;

namespace UserSignUpLogin.Controllers
{
    public class HomeController : Controller
    {
        private readonly DBuserSignupLoginEntities db = new DBuserSignupLoginEntities();


        // GET: Home/Index
        public ActionResult Index()
        {
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["IdUsSS"]);

            var currentUser = db.TBLUserInfoes.Find(userId);

            if (currentUser == null)
            {
                Session.Clear();
                return RedirectToAction("Login");
            }

            return View(currentUser);
        }

        // GET: Home/ChangePassword
        public ActionResult ChangePassword()
        {
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["IdUsSS"]);

            var currentUser = db.TBLUserInfoes.Find(userId);

            if (currentUser == null)
            {
                Session.Clear();
                return RedirectToAction("Login");
            }

            return View(currentUser);
        }

        // POST: Home/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(TBLUserInfo model)
        {
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["IdUsSS"]);

            var currentUser = db.TBLUserInfoes.Find(userId);

            if (currentUser == null)
            {
                Session.Clear();
                return RedirectToAction("Login");
            }

            if (ModelState.IsValid)
            {
                if (model.PassWordUs == model.rePassWordUs)
                {
                    currentUser.PassWordUs = model.PassWordUs;
                    db.Entry(currentUser).Property(u => u.PassWordUs).IsModified = true;
                    db.SaveChanges();
                    return Content("Senha atualizada com sucesso!");
                }
                else
                {
                    return Content("As senhas não coincidem");
                }
            }
            return View(currentUser);
        }


        // GET: Home/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.FirstOrDefault(x => x.UsernameUS.Equals(tBLUserInfo.UsernameUS) && x.PassWordUs.Equals(tBLUserInfo.PassWordUs));
            if (checkLogin != null)
            {
                Session["IdUsSS"] = checkLogin.IDUs.ToString();
                Session["UsernameSS"] = checkLogin.UsernameUS.ToString();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Notification = "Usuário ou senha inválidos";
                return View();
            }
        }

        // GET: Home/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        // GET: Home/Signup
        public ActionResult Signup()
        {
            return View();
        }

        // POST: Home/Signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(TBLUserInfo tBLUserInfo)
        {
            if (db.TBLUserInfoes.Any(x => x.UsernameUS == tBLUserInfo.UsernameUS))
            {
                ViewBag.Notification = "Essa conta já existe";
                return View();
            }
            else
            {
                db.TBLUserInfoes.Add(tBLUserInfo);
                db.SaveChanges();

                Session["IdUsSS"] = tBLUserInfo.IDUs.ToString();
                Session["UsernameSS"] = tBLUserInfo.UsernameUS.ToString();
                return RedirectToAction("Index");
            }
        }

        // GET: Home/Details
        public ActionResult Details()
        {
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            int userId = Convert.ToInt32(Session["IdUsSS"]);

            var currentUser = db.TBLUserInfoes.Find(userId);

            if (currentUser == null)
            {
                Session.Clear();
                return RedirectToAction("Login");
            }

            return View(currentUser);
        }

        // GET: Home/Edit
        public ActionResult Edit()
        {
            // Verifica se o usuário está autenticado
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            // Obtém o ID do usuário da sessão
            int userId = Convert.ToInt32(Session["IdUsSS"]);

            // Busca o usuário no banco de dados pelo ID
            var currentUser = db.TBLUserInfoes.Find(userId);

            // Verifica se o usuário existe
            if (currentUser == null)
            {
                // Se não existir, limpa a sessão e redireciona para a página de login
                Session.Clear();
                return RedirectToAction("Login");
            }

            // Retorna a view de edição com o usuário encontrado
            return View(currentUser);
        }

        // POST: Home/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TBLUserInfo model)
        {
            // Verifica se o usuário está autenticado
            if (Session["IdUsSS"] == null)
            {
                return RedirectToAction("Login");
            }

            // Verifica se o modelo é válido
            if (ModelState.IsValid)
            {
                // Atualiza as informações do usuário no banco de dados
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                // Redireciona para a página de detalhes do usuário
                return RedirectToAction("Index");
            }

            // Se houver erros de validação, retorna a view de edição com o modelo
            return View(model);
        }


    }
}
