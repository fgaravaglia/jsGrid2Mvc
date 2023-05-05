
using System.Text.Json;
using System.Text.Json.Serialization;

namespace jsGrid2Mvc.Model.Extensions
{
    /// <summary>
    /// Extensions to manage model objects
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Converts the obect to json string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this IModelObject obj)
        {
            var serializationSettings = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = false
            };
            return JsonSerializer.Serialize(obj, serializationSettings);
        }
    }
}
