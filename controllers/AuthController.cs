using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoCompleto.Services;
using ProjetoCompleto.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoCompleto.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // GET: Auth/Cadastro
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Cadastro()
        {
            return View();
        }

        // POST: Auth/Cadastro
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(CadastroViewModel model)
        {
            if (! ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _authService.CadastrarUsuario(model);

            if (resultado.Sucesso)
            {
                TempData["Sucesso"] = resultado.Mensagem;
                return RedirectToAction(nameof(Login));
            }

            ModelState.AddModelError(string.Empty, resultado.Mensagem);
            return View(model);
        }

        // GET: Auth/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var resultado = await _authService.Login(model);

            if (resultado.Sucesso)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, resultado.Usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, resultado.Usuario.Nome),
                    new Claim(ClaimTypes.Email, resultado.Usuario.Email),
                    new Claim(ClaimTypes.Role, resultado. Usuario.Perfil)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.LembrarMe,
                    ExpiresUtc = model.LembrarMe 
                        ? System.DateTimeOffset.UtcNow.AddDays(30) 
                        : System.DateTimeOffset.UtcNow.AddHours(2)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);

                if (! string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, resultado. Mensagem);
            return View(model);
        }

        // POST: Auth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        // GET: Auth/AcessoNegado
        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}