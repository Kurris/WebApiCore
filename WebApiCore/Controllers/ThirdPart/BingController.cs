using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore.Lib.Utils;
using WebApiCore.Lib.Utils.Extensions;

namespace WebApiCore.Controllers.ThirdPart
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BingController : ControllerBase
    {
        private readonly IHttpClientFactory _httpfactory;
        private readonly string _baseUrl = "https://www.bing.com";

        public BingController(IHttpClientFactory httpfactory)
        {
            this._httpfactory = httpfactory;
        }


        [HttpGet]
        public async Task<string> GetDayImage()
        {
            string json = await _httpfactory.GetClient().GetStringAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
            var jobj = JsonHelper.ToJObject(json);
            string url = jobj["images"][0].Value<string>("url");

            string imgUrl= $"{_baseUrl}{url}";
            return imgUrl;
        }
    }
}
