using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ConsoleTest
{
    public class CookieTest
    {
        public CookieTest()
        {
            Console.WriteLine(new DateTimeOffset(DateTime.Now, TimeSpan.FromHours(9)).ToString("r"));
            /*IOS*/
            string cookie = "idsrv.session=24L3oUugPdBfvVn9uTuAbg; path=/; secure; samesite=none, .AspNetCore.Identity.Application=CfDJ8DU3rRjrXZBDv0GO64RceIIUK; expires=Tue, 03 Nov 2020 05:32:56 GMT; path=/; secure; samesite=none; httponly";
            /*ANDRODI*/
            cookie = ".AspNetCore.Antiforgery.eB4w8d0gFhc=CODE,idsrv.session=JoZChRZTqzKn1rdOlAfFDg,.AspNetCore.Identity.Application=CODE";
            /*DeleteCookie*/
            cookie = "idsrv.session=.; expires=Tue, 22 Oct 2019 08:42:21 GMT; path=/; secure, samesite=none,.AspNetCore.Identity.Application=, expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; secure; samesite=none, httponly,Identity.External=, expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; secure; samesite=none, httponly,Identity.TwoFactorUserId=, expires=Thu, 01 Jan 1970 00:00:00 GMT; path=/; secure; samesite=lax; httponly";

            /*err*/
            cookie = "Identity.External=; expires=Sun, 25 Oct 2020 06:31:55 GMT,Identity.TwoFactorUserId=; expires=Sun, 25 Oct 2020 06:31:56 GMT,.AspNetCore.Identity.Application=CfDJ8P9ohNpFaC9GnOhfONFk1tNQL5MFsa1CzGahPS-6S1l715aEkA-B_cXitqBsbSjW0NCbSBgIT1w7r8ImHK_Qt5ipowkePF_wUZycUqqILw5NgdlqWL2LpvgM3lAycIpQ-AmbnyxO6_LkOlSLJ87shwpVR6PNVZu2BV8Sw7mU0DvnI_tOlOxnC7w_4vvbpLBzR8z-ZB9brct4BAgISclKjITjN_ppzaM6YzGu2FAPIC8r5M0-TLanrqtti5fh9f5HRek-cO-3V7y1V8x3ypWy7f3G7LQOqxY2S6DExReP2_z3xQv15B4Z45TJI3-CfhHe5PyZkadRJpAlMqqqppHZFRA02XJq61kYFJw0WqIUoBCXxsw8C3KRGGDuRC_WgrpcTatmD7fbQ1yoLcP5lQaJyqf39zLOEffbjLOqSeErsqnr9jZrGXOZI5oGcXHkAf8k_TZJ5UHotV3g8kfBcOpZiIH4B3FzYB1kZ0yRX5NyXBLfQsr9JZPTYXJOfR5aTlKKwx4ErspqgNGXOZTYXFoiBUHco71L3bdmWGAj10gRyP0REQDIkZS3evzK4iaODOjFhWe-taDLl2efi-IcCGCHUCmncLTs841XvLn9jUPaGplV4gBWNWsGZxikX4dMw5JTckvvxjOLIZ-qI4_C23-fTefLx0zpA5vajTUOW4E4Gp3ueH_SfamZiyL334NRdqOx4g; expires=Sat, 24 Oct 2020 21:32:01 GMT,";

            foreach (var c in SplitCookies(cookie))
            {
                Console.WriteLine("----------------------------");
                Console.WriteLine(c);
            }

            while (true) ;
        }


        private static List<string> SplitCookies(string cookie)
        {
            List<string> result = new List<string>();
            var pairs = cookie.Split(';');
            for (int i = 0; i < pairs.Length; i++)
            {
                if (pairs[i].Contains(','))
                {
                    string[] pairs2 = pairs[i].Split(',');
                    for (int j = 0; j < pairs2.Length; j++)
                    {
                        if (j > 0 && !pairs[j].Contains("=") && pairs2[j - 1].ToLower().StartsWith("expires="))
                        {
                            string v = result[result.Count - 1] + "," + pairs2[j];
                            if (DateTime.TryParse(v, out _))
                            {
                                result[result.Count - 1] = v;
                            }
                            else
                            {
                                AddResult(pairs2[j]);
                            }
                        }
                        else
                        {
                            AddResult(pairs2[j]);
                        }
                    }
                }
                else
                {
                    AddResult(pairs[i]);
                }
            };
            return result;

            void AddResult(string v)
            {
                if (string.IsNullOrWhiteSpace(v)) return;
                if (v[0] == ' ') v = v.Substring(1);
                if (string.IsNullOrWhiteSpace(v)) return;

                int idx = v.IndexOf('=');
                if (idx == -1)
                {
                    result[result.Count - 1] += $"; {v}";
                }
                else
                {
                    var key = v.Substring(0, idx);
                    switch (key.ToLower())
                    {
                        case "expires":
                        case "max-age":
                        case "maxage":
                        case "domain":
                        case "path":
                        case "secure":
                        case "httponly":
                        case "samesite":
                            result[result.Count - 1] += $"; {v}";
                            break;
                        default:
                            result.Add(v);
                            break;
                    }
                }
            }
        }
    }
}