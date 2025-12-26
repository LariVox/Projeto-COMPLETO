using Microsoft.AspNetCore.Authentication; // QUEM você é, login/ogout, autenticação  
using Microsoft.AspNetCore. Authentication.Cookies; // usar cookies para manter usuario logado 
using Microsoft.AspNetCore.Authorization; // O QUE voce pode fazer, controla acesso e permissões
using Microsoft.AspNetCore.Mvc; // fornece as ferramentas para  
using ProjetoCompleto.Services;
using ProjetoCompleto.ViewModels;
using System.Security.Claims; // representa informações sobre o usuário autenticado

namespace ProjetoCompleto.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro(CadastroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var sucesso = await _authService.CadastrarUsuarioAsync(model);

            if (! sucesso)
            {
                ModelState.AddModelError("", "Este email já está cadastrado.");
                return View(model);
            }

            TempData["Mensagem"] = "Cadastro realizado com sucesso!  Faça login. ";
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _authService. AutenticarAsync(model. Email, model.Senha);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Email ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario. Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.LembrarMe,
                ExpiresUtc = model.LembrarMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}