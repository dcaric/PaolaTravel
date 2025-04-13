using Microsoft.AspNetCore.Mvc;
using Travel.Web.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Travel.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7172/");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ApplicationUser model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Registration failed.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
                if (result != null)
                {
                    HttpContext.Session.SetString("JWToken", result.Token);
                    Console.WriteLine($"JWToken: {result.Token} ");
                    //var stored = HttpContext.Session.GetString("JWToken");
                    //Console.WriteLine("Retrieved from session: " + stored);
                    TempData["JwtToken"] = result.Token;

                    // Extract claims from token
                    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var token = handler.ReadJwtToken(result.Token);
                    var isAdmin = token.Claims.FirstOrDefault(c => c.Type == "isAdmin")?.Value == "true";
                    // Redirect based on role
                    if (isAdmin)
                        return RedirectToAction("Profile", "ApplicationUser");
                    else
                        return RedirectToAction("UserProfile", "ApplicationUser");
                }
            }

            ModelState.AddModelError("", "Login failed.");
            return View(model);
        }


        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // remove all session data, including JWToken
            return RedirectToAction("Login", "Account");
        }

    }
}
