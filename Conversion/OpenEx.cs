using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Conversion
{
    public class OpenEx
    {
        //Assign string value of api url.
        private const string URL = "https://openexchangerates.org/api/latest.json";

        //store api key
        private static string APIKey;

        /// <summary>
        /// Get API key from user. Check if key has been set.
        /// </summary>
        private static string Key
        {
            //get key from user, or fail and ask for key.
            get => !string.IsNullOrWhiteSpace(APIKey) ? APIKey : throw new InvalidOperationException("API key required.");
            set => APIKey = value;
        }//end key

        /// <summary>
        /// Method for getting conversion amount.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static double Convert(string from, string to, double amount)
        {
            return GetRate(from, to).Convert(amount);
        }//end method

        /// <summary>
        /// Method for getting conversion amount.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static Exchange Rate(string from, string to)
        {
            return GetRate(from, to);
        }//end method

        /// <summary>
        /// Method for setting API key for Fixer.IO.
        /// </summary>
        /// <param name="apiKey"></param>
        public static void SetApiKey(string apiKey)
        {
            Key = apiKey;
        }//end method

        /// <summary>
        /// Method for communication with API, and Exchange. 
        /// </summary>
        /// <param name="conFrom"></param>
        /// <param name="conTo"></param>
        /// <returns></returns>
        private static Exchange GetRate(string conFrom, string conTo)
        {
            //convert user input to upper case for api us
            conFrom = conFrom.ToUpper();
            conTo = conTo.ToUpper();

            //check if symbols are valid
            if (!CSymbols.IsValid(conFrom))
                throw new ArgumentException("Symbol not found.");

            if (!CSymbols.IsValid(conTo))
                throw new ArgumentException("Symbol not found.");

            //assign url to var
            var url = GetOpenExUrl();

            //create instance for communicating with api
            using (var client = new HttpClient())
            {
                var clientResponse = client.GetAsync(url).Result;
                clientResponse.EnsureSuccessStatusCode();
                return ParseData(clientResponse.Content.ReadAsStringAsync().Result, conFrom, conTo);
            }//end using
        }//end method

        /// <summary>
        /// Method for api communication using through Newtonsoft.Json.Linq | Required for communication. 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private static Exchange ParseData(string data, string from, string to)
        {
            //get objects from api | date, from, to and rate
            var root = JObject.Parse(data);
            var rates = root.Value<JObject>("rates");
            var fromRate = rates.Value<double>(from);
            var toRate = rates.Value<double>(to);
            var rate = toRate / fromRate;
            //return exchange
            return new Exchange(from, to, rate, DateTime.Now);
        }//end method

        /// <summary>
        /// Method for defining url.
        /// </summary>
        /// <returns></returns>
        private static string GetOpenExUrl()
        {
            //php query structure.
            return $"{URL}?app_id={Key}";
        }//end method
    }//end class
}//end namespace
