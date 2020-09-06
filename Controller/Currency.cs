using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;
using CoinPress.Model;

namespace CoinPress
{
    public class Currency
    {
        #region Fields
        /// <summary>
        /// Instance of Currency Class with create method.
        /// </summary>
        private static Currency instance;
        public static Currency Instance
        {
            get
            {
                if (instance == null)
                    instance = new Currency();
                return instance;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Currency()
        {
            client = new HttpClient();
        }
        #endregion

        #region Create Method
        public async void Create(string wpid, string snky, string title, string info, string abbrev, string exchange, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wpid);
                dict.Add("snky", snky);
                dict.Add("title", title);
                dict.Add("info", info);
                dict.Add("abbrev", abbrev);
                dict.Add("exchange", exchange);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/coinpress/v1/user/wallet/currencies", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(result);

                bool success = token.status == "success" ? true : false;
                string data = token.status == "success" ? result : token.message;
                callback(success, data);
            }
            else
            {
                callback(false, "Network Error! Check your connection.");
            }
        }
        #endregion
    }
}
