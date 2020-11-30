﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SinGooCMS.Utility;
using SinGooCMS.Utility.Extension;

namespace CoreWebTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            /*
             * net core不自带httpcontext 需要在 Startup 注入
             * 1、在ConfigureServices 中 services.AddStaticHttpContext();
             * 2、在Configure 中 app.UseStaticHttpContext();
             */

            var builder = new StringBuilder("测试如下：\r\n");

            //WebUtils
            builder.Append($"pid:{WebUtils.GetQueryInt("pid")}\r\n"); //?pid=1
            //全url
            builder.Append($"全URL：{WebUtils.GetAbsoluteUri()}\r\n");

            //CacheUtils 缓存
            DateTime dateTime = DateTime.Now;
            var cache = new CacheUtils();

            var cacheDT = DateTime.Now;
            if (cache.ContainKey("time"))
                cacheDT = cache.Get<DateTime>("time");
            else
                cache.Insert<DateTime>("time", dateTime, 3600);

            builder.Append($"当前时间：{dateTime.ToFormatString()} \r\n");
            builder.Append($"缓存时间：{cacheDT.ToFormatString()} \r\n");

            //当前网站目录
            builder.Append($"当前网站目录：{SystemUtils.GetMapPath()} \r\n");
            builder.Append($"upload目录：{SystemUtils.GetMapPath("/upload")} \r\n");

            //cookie
            CookieUtils.SetCookie("username", "jsonlee");
            builder.Append($"username cookie: {CookieUtils.GetCookie("username")} \r\n");

            //session
            SessionUtils.SetSession("username", System.Web.HttpUtility.UrlEncode("刘备"));
            builder.Append($"username session: {System.Web.HttpUtility.UrlDecode(SessionUtils.GetSession("username"))} \r\n");

            return Content(builder.ToString());
        }
    }
}