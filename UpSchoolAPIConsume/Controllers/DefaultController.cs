using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UpSchoolAPIConsume.Models;

namespace UpSchoolAPIConsume.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DefaultController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:44337/api/Category");
            if(responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsondata = await responseMessage.Content.ReadAsStringAsync();
                //jsonı oluşturduğum viewmodel üzerinden liste halinde döndürmek için
                var result = JsonConvert.DeserializeObject<List<CategoryViewVodel>>(jsondata);
                return View(result);
            }
            else
            {
                ViewBag.responseMessage = "Bir hata oluştu";
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryViewVodel p)
         {
             p.Status = true;
             var client = _httpClientFactory.CreateClient();
             var jsonData = JsonConvert.SerializeObject(p);
             //kendim veri gönderdiğim için serialize edeceğim
             StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
             var responseMessage = await client.PostAsync("https://localhost:44337/api/Category", content);
            //200 dönerse
             if (responseMessage.IsSuccessStatusCode)
             {
                 return RedirectToAction("Index");
             }
             else
             {
                 ViewBag.responseMessage = "Bir hata ile karşılaşıldı";
                 return View();
             }
         }
         [HttpGet]
         public async Task<IActionResult> UpdateCategory(int id)
         {
             var client = _httpClientFactory.CreateClient();
             var responseMessage = await client.GetAsync($"https://localhost:44337/api/Category/{id}");
             if (responseMessage.IsSuccessStatusCode)
             {
                 var jsonData = await responseMessage.Content.ReadAsStringAsync();
                 var result = JsonConvert.DeserializeObject<CategoryViewVodel>(jsonData);
                //bir tane veri getireceğiz
                 return View();
             }
             else
             {
                 ViewBag.responseMessage = "Bir hata ile karşılaşıldı";
                 return View();
             }
         }
         [HttpPost]
         public async Task<IActionResult> UpdateCategory(CategoryViewVodel p)
         {
             var client = _httpClientFactory.CreateClient();
            //veri gönderdiğimiz için pyi serialize ediyoruz
             var jsonData = JsonConvert.SerializeObject(p);
             var content = new StringContent(jsonData,Encoding.UTF8,"application/json");
             var responseMessage = await client.PutAsync("https://localhost:44337/api/Category", content);
             if (responseMessage.IsSuccessStatusCode)
             {
                 return RedirectToAction("Index");
             }
             else
             {
                 return View();
             }

         }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var client = _httpClientFactory.CreateClient();
            await client.DeleteAsync($"https://localhost:44337/api/Category/{id}");
            return RedirectToAction("Index");

        }
    }

}
