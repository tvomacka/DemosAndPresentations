using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RefactorMe
{
    public class Exercise
    {
        /*
         * 1. Zkuste odhadnout, co metoda ParseResponseSingleLine dělá.
         * 2. Použijte ReSharper opakovaně na metodu ParseResponseSingleLine, dokud sám neoznačí metodu za "hotovou".
         * 3. Porovnejte výsledný kód metody s původním kódem. Jak se liší, co pro vás tento výsledek představuje?
         */

        /// <summary>
        /// Parses the information obtained from server from json format to array.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static int[] ParseResponseSingleLine(string elevationResponse)
        {
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(elevationResponse);

            if (result.ContainsKey("statusCode") && (int)result["statusCode"] != 200)
                throw new WebException(result["statusDescription"].ToString());

            object[] resourceSets = result["resourceSets"] as object[];
            if (resourceSets != null && resourceSets.Length > 0)
            {
                if (resourceSets[0] is Dictionary<string, object> && (resourceSets[0] as Dictionary<string, object>).ContainsKey("resources"))
                {
                    object[] resources = (resourceSets[0] as Dictionary<string, object>)["resources"] as object[];
                    if (resources != null && resources.Length > 0)
                    {
                        if (resources[0] is Dictionary<string, object> && (resources[0] as Dictionary<string, object>).ContainsKey("elevations"))
                        {
                            object[] elevations = (resources[0] as Dictionary<string, object>)["elevations"] as object[];
                            if (elevations != null && elevations.Length > 0)
                            {
                                int[] elevationsArray = elevations.Select(x => (int)x).ToArray();
                                return elevationsArray;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}