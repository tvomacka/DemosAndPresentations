using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RefactorMe
{
    public class Exercise1
    {
        /*
         * Cviceni 1
         *
         * 1. Zkuste odhadnout, co metoda ParseResponseSingleLine dělá.
         * 2. Použijte ReSharper opakovaně na metodu ParseResponseSingleLine, dokud sám neoznačí metodu za "hotovou".
         * 3. Porovnejte výsledný kód metody s původním kódem. Jak se liší, co pro vás tento výsledek představuje?
         */

        /// <summary>
        /// Parses the information obtained from server from json format to array.
        /// </summary>
        public static int[] ParseResponseSingleLine(string elevationResponse)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(elevationResponse);

            if (result == null)
                return null;

            if (result.ContainsKey("statusCode") && (long)result["statusCode"] != 200)
                throw new WebException(result["statusDescription"].ToString());

            if (!(result["resourceSets"] is JArray) || ((JArray)result["resourceSets"]).Count == 0)
                return null;

            return ((JArray)result["resourceSets"])[0]["resources"]?[0]?["elevations"]?.Select(x => (int)x).ToArray();

        }

    }
}