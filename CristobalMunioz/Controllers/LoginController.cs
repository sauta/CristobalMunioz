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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string cEmail, string cPassword)
        {
            var userInfo = await (from emp in _context.People
                                  where emp.EmailAddresses.First().EmailAddress1 == cEmail
                                  select new
                                  {
                                      IDEmployee = emp.BusinessEntityId,
                                      Nombre = emp.FirstName,
                                      Apellido = emp.LastName,
                                      Email = emp.EmailAddresses.First().EmailAddress1,
                                      Password = emp.Password
                                      //IDsPermiso = emp.Idpermisos.Select(x => x.Idpermiso),
                                      //NombrePermiso = emp.Idpermisos.Select(x => x.Descripcion)

                                  }).SingleOrDefaultAsync();

            if (userInfo != null)
            {
                if (userInfo != null || Argon2PasswordHasher.VerifyHashedPassword(userInfo.Email, cPassword)) //se cambia verificación
                {
                    var claims = new List<Claim>();
                    /*
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInfo.Usuario.Email),
                        new Claim(ClaimTypes.NameIdentifier, userInfo.Usuario.Email.ToString()),
                        new Claim(ClaimTypes.Role, userInfo.TipoUsuario.Descripcion.ToString())
                    };

                    var allPermisos = userInfo.PermisosTipoUsuario.Concat(userInfo.PermisosUsuario).Distinct();
                    foreach (var permiso in allPermisos)
                    {
                        claims.Add(new Claim("Permiso", permiso));
                    }
                    */
                    var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");

                    await HttpContext.SignInAsync(
                        "CookieAuth",
                        new ClaimsPrincipal(claimsIdentity));

                    _logger.LogInformation("User: {} successfully logged in", userInfo.Email);

                    return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Index", "Home");
        }

    }
}
