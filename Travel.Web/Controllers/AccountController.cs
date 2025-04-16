﻿using Microsoft.AspNetCore.Mvc;
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
                // TempData is used to transffer data to the nest redirection place
                TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Registration failed. Please try again.";
            return View(model);
        }


        // When user insertes usernam / password it sends /Account/Login POST with { username, passwor } in the payload
        // it comes here and this function sends request towards BACKEND as api/auth/login
        // 
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model);

            // response is response from the backend
            if (response.IsSuccessStatusCode) // check if 200 OK result code
            {

                // when 200 OK is passed it reads now detail part of response and this is response.Content
                var result = await response.Content.ReadFromJsonAsync<JwtResponse>();
                if (result != null)
                {
                    HttpContext.Session.SetString("JWToken", result.Token);
                    // TempData is used to transffer data to the nest redirection place
                    // this is also .Net specific feature
                    TempData["SuccessMessage"] = "Successful log in.";

                    // this is .Net redirection function meaning: "Dashboard" is razor page (cshtml) and "Home" is folder where
                    // Dashboard.cshtml is located
                    // SO IF BACKEND CONFIRMS USER'S CREDENTIOALS (Usernam + password) IT RETURNS 200 OK
                    // JWT token also and code comes here where Home/Dashboard,cshtml is loaded
                    return RedirectToAction("Dashboard", "Home");
                }
            }

            TempData["ErrorMessage"] = "Login failed. Invalid username or password.";
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
