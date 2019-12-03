using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Conversion
{
    public class FixerIO
    {
        //Assign string value of api url.
        private const string BaseUri = "http://data.fixer.io/api/";

        //store api key
        private static string _apiKey;

        /// <summary>
        /// Get API key from user. Check if key has been set.
        /// </summary>
        private static string ApiKey
        {
            //get key from user, or fail and ask for key.
            get => !string.IsNullOrWhiteSpace(_apiKey) ? _apiKey : throw new InvalidOperationException("Fixer.io now requires an API key! Call .SetApiKey(\"key\") first");
            set => _apiKey = value;
        }//end method

        /// <summary>
        /// Method for getting conversion amount.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="amount"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static double Convert(string from, string to, double amount, DateTime? date = null)
        {
            return GetRate(from, to, date).Convert(amount);
        }//end method

        /// <summary>
        /// Method for getting conversion rate.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static Exchange Rate(string from, string to, DateTime? date = null)
        {
            return GetRate(from, to, date);
        }//end method

        /// <summary>
        /// Method for setting API key for Fixer.IO.
        /// </summary>
        /// <param name="apiKey"></param>
        public static void SetApiKey(string apiKey)
        {
            ApiKey = apiKey;
        }//end method

        /// <summary>
        /// Method for communication with API, and Exchange. 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static Exchange GetRate(string from, string to, DateTime? date = null)
        {
            //convert user input to upper case for api use
            from = from.ToUpper();
            to = to.ToUpper();

            //check if symbols are valid
            if (!CSymbols.IsValid(from))
                throw new ArgumentException("Symbol not found for provided currency", "from");
            if (!CSymbols.IsValid(to))
                throw new ArgumentException("Symbol not found for provided currency", "to");

            //assign url with date
            var url = GetFixerUrl(date);

            //create instance for communicating with api
            using (var client = new HttpClient())
            {
                //assign response value to determine response from api
                var response = client.GetAsync(url).Result;
                //determine if success of faliure
                response.EnsureSuccessStatusCode();
                //return response from api connection
                return ParseData(response.Content.ReadAsStringAsync().Result, from, to);
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
            var returnedDate = DateTime.ParseExact(root.Value<string>("date"), "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture);
            //return exchange
            return new Exchange(from, to, rate, returnedDate);
        }//end method

        /// <summary>
        /// Method for getting url from fixer.io api.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private static string GetFixerUrl(DateTime? date = null)
        {
            var dateString = date.HasValue ? date.Value.ToString("yyyy-MM-dd") : "latest";
            //php side of api query
            return $"{BaseUri}{dateString}?access_key={ApiKey}";
        }//end method
    }//end class
}//end namespace
