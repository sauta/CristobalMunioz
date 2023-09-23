using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using CristobalMunioz.Helpers;
using System.Data;
using System.Security.Claims;
using CristobalMunioz.Models;
using Newtonsoft.Json;


namespace CristobalMunioz.Controllers
{
    public class LoginController : Controller
    {
        private readonly AdventureWorks2019Context _context;
        private readonly ILogger<LoginController> _logger;

        public LoginController(AdventureWorks2019Context context, ILogger<LoginController> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cEmail"></param>
        /// <param name="cPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string cEmail, string cPassword)
        {
            var userInfo = await (from P in _context.People
                                  join EA in _context.EmailAddresses on P.BusinessEntityId equals EA.BusinessEntityId
                                  join PS in _context.Passwords on P.BusinessEntityId equals PS.BusinessEntityId
                                  where EA.EmailAddress1 == cEmail
                                  select new
                                  {
                                      IDEmployee = P.BusinessEntityId,
                                      Nombre = P.FirstName,
                                      Apellido = P.LastName,
                                      Email = EA.EmailAddress1,
                                      Password = PS.PasswordHash,
                                      IDsPermiso = P.Permisos.Select(x => x.PermisoId),
                                      NombrePermisos = P.Permisos.Select(x => x.Descripcion),
                                  }).SingleOrDefaultAsync();

            if (userInfo != null)
            {
                if (Argon2PasswordHasher.VerifyHashedPassword(userInfo.Password, cPassword)) //se cambia verificación
                {
                    var claims = new List<Claim>();

                    claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.IDEmployee.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userInfo.IDEmployee.ToString()),
                        new Claim(ClaimTypes.GivenName, userInfo.Nombre),
                        new Claim(ClaimTypes.Name, userInfo.Nombre),
                    };

                    var allPermisos = userInfo.IDsPermiso;

                    foreach (var permiso in allPermisos)
                    {
                        claims.Add(new Claim("Permiso", permiso.ToString()));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                    await HttpContext.SignInAsync(
                        "CookieAuth",
                        new ClaimsPrincipal(claimsIdentity));

                    _logger.LogInformation("User: {} successfully logged in", userInfo.Email);

                    return RedirectToAction("Index", "Home");
                }
                TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
                return RedirectToAction("Index", "Login");
            }
            TempData["ErrorMessage"] = "Email o Contraseña Incorrectos";
            return RedirectToAction("Index", "Login");
        }
        //public async Task<IActionResult> Login(string hash)
        //{ 
        //    var a = Argon2PasswordHasher.HashPassword(hash);

        //    var info = a.ToString();

        //    return RedirectToAction("Index", "Home");
        //}

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

    }
}
