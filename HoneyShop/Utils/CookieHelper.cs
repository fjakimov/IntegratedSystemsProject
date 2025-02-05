using Microsoft.AspNetCore.Http;
using System;

namespace HoneyShop.Web.Utils
{
    public static class CookieHelper
    {
        public static void SetCookie(HttpResponse response, string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddDays(7); // Default expiration (1 week)

            response.Cookies.Append(key, value, option);
        }

        public static string? GetCookie(HttpRequest request, string key)
        {
            request.Cookies.TryGetValue(key, out string? value);
            return value;
        }

        public static void RemoveCookie(HttpResponse response, string key)
        {
            response.Cookies.Delete(key);
        }
    }
}
