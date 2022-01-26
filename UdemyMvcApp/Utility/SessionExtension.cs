using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace UdemyMvcApp.Utility
{
    public static class SessionExtension
    {
       public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            var val = session.GetString(key);
            return val == null ? default : JsonSerializer.Deserialize<T>(val);
        }


    }
}
