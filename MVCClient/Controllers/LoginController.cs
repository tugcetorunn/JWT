using Microsoft.AspNetCore.Mvc;
using MVCClient.Models.Dto;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace MVCClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient client;
        public static string token = string.Empty; // Token'ı burada saklayabiliriz, ancak genellikle daha güvenli bir yerde saklanması önerilir.
        public LoginController(HttpClient _client)
        {
            client = _client;
        }

        public async Task<IActionResult> Index()
        {
            if (token == null)
            {
                return Content("yetkisiz..");
            }

            client.DefaultRequestHeaders.Add("Authorization", token);
            var urunler = await client.GetFromJsonAsync<List<ProductDto>>("http://localhost:5043/api/Products");

            return View(urunler);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await client.PostAsJsonAsync("http://localhost:5043/api/Login", dto);

                if (result.IsSuccessStatusCode)
                {
                    var jwt = await result.Content.ReadAsStringAsync(); // apiden gelen content token olduğu için token ı verir. json formatında gelir, stringe çeviririz.
                    token = "Bearer " + jwt; // Token'ı sakla

                    // Token'ı modelle (örnek: {"token":"xyz123"})
                    // var tokenData = JsonConvert.DeserializeObject<ViewModels.TokenResponse>(jwt);

                    // Token'ı Cookie'de sakla
                    // HttpContext.Response.Cookies.Append("JwtToken", tokenData.Token, new CookieOptions
                    //{
                    //    HttpOnly = true,
                    //    Secure = true,
                    //    SameSite = SameSiteMode.Strict,
                    //    Expires = DateTimeOffset.UtcNow.AddHours(1)
                    //});

                    return RedirectToAction("Index", "Product", token);
                }
            }
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View(dto);
        }
    }
}
