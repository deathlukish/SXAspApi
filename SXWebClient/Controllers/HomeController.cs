﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibPhoneBook;
using System.Text;
using System.Text.Json;

namespace SXWebClient.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<PhoneBook>? Notes { get; set; }
        private readonly HttpClient _httpClient;
        public PhoneBookDetail PhoneBook { get; set; } = new();
        public HomeController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                Notes = await _httpClient.GetFromJsonAsync<List<PhoneBook>>("webapi/PhoneBooks");
            }
            catch (Exception ex)
            {
                return View("Error", ex);
            }
            finally
            {
                _httpClient?.Dispose();
            }
            if (Notes != null)
            {
                return View(Notes);
            }
            return NotFound();
        }
        public async Task<IActionResult> Edit(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description,Adres")] PhoneBookDetail note)
        {
            var serialize = JsonSerializer.Serialize(note);
            var requestContent = new StringContent(serialize, Encoding.UTF8, "application/json-patch+json");
            await _httpClient.PatchAsync($"webapi/PhoneBooks/", requestContent);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Remove(int id) => View(await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}"));
        [HttpPost, ActionName("Remove")]
        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"webapi/PhoneBooks/{id}");
            return RedirectToAction("index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            PhoneBook = await _httpClient.GetFromJsonAsync<PhoneBookDetail>($"webapi/PhoneBooks/{id}");
            return View(PhoneBook);
        }
        public async Task<IActionResult> Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(int id, [Bind("Id,FirstName,MiddleName,LastName,Phone,Description, Adres")] PhoneBookDetail note)
        {
            await _httpClient.PostAsJsonAsync("webapi/PhoneBooks", note);
            return Redirect("index");
        }
        [HttpGet]
        public async Task<IActionResult> Users()
        { 
            var a = await _httpClient.GetAsync("webapi/PhoneBooks/Test");
            if (a.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return View("Auth");
            }
            return View("UserManager");
        }
        [HttpPost]
        public async Task<IActionResult> Users(User users)
        {
            if (ModelState.IsValid)
            {
                //message will collect the String value from the model method.
                string message = "fds";
                //RedirectToAction("actionName/ViewName_ActionResultMethodName", "ControllerName");
                if (message.Equals("1"))
                {

                }
                else
                {
                    ViewBag.ErrorMessage = message;
                }
            }
            await _httpClient.PostAsJsonAsync("webapi/UserAuth", users);
           
            return View("Auth",users);
        }

    }
}
