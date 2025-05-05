using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models.Dto;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient client;
        public ProductController(HttpClient _client)
        {
            client = _client;
        }
        public async Task<IActionResult> Index(string token)
        {
            if (token == null)
            {
                return Content("yetkisiz..");
            }

            client.DefaultRequestHeaders.Add("Authorization", token);
            var urunler = await client.GetFromJsonAsync<List<ProductDto>>("http://localhost:5043/api/Products");
            return View(urunler);
        }
    }
}
