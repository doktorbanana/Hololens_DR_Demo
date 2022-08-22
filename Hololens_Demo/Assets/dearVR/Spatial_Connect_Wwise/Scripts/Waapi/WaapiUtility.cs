using System.Collections.Generic;
using OVRSimpleJSON;
using System.Text.RegularExpressions;

namespace SpatialConnect.Wwise
{
    public static class WaapiUtility
    {
        public static JSONArray ToJSONArray(string element1)
        {
            return ToJSONArray(new[] {element1});
        }

        public static JSONArray ToJSONArray(string element1, string element2)
        {
            return ToJSONArray(new[] {element1, element2});
        }

        public static JSONArray ToJSONArray(string element1, string element2, string element3)
        {
            return ToJSONArray(new[] {element1, element2, element3});
        }

        public static JSONArray ToJSONArray(IEnumerable<string> array)
        {
            var jsonArray = new JSONArray();
            foreach (var element in array)
                jsonArray.Add(element);
            return jsonArray;
        }

        public static string GetValueFromJson(string fieldName, string jsonContent)
        {
            return Regex.Match(jsonContent, @""+"\""+fieldName+"\":\\s*(.+?),").Groups[1].Value.Replace("\"", "");
        }
    }
}
