﻿using Book_Shop_WPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Book_Shop_WPF
{
    internal class Token
    {
        public static string token { get; set; }

        public async void RefreshToken()
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var httpClient = new HttpClient(httpClientHandler))
                {
                    dynamic data = JObject.Parse(Token.token);
                    string oldToken = data.access_token;
                    StringContent content = new StringContent(JsonConvert.SerializeObject(oldToken), Encoding.UTF8, "application/json");
                    using (var response = await httpClient.PostAsync(App.ip + "/Users/refresh_token?access_token=" + oldToken, null))
                    {
                        try
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                string apiRes = await response.Content.ReadAsStringAsync();
                                token = apiRes;
                            }
                            else
                            {
                                MessageBox.Show("Ошибка обновления токена!");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }
    }
}
