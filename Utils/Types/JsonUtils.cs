// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using System;
using Newtonsoft.Json;

namespace Common_Library.Utils
{
    public static class JsonUtils
    {
        public static T DeserializeObject<T>(String json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static Boolean TryDeserializeObject<T>(String json, out T result, out Exception error)
        {
            result = default;

            if (String.IsNullOrWhiteSpace(json))
            {
                error = new ArgumentException(@"NullOrWhiteSpace", nameof(json));
                return false;
            }

            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                result = default;
                error = e;
                return false;
            }

            error = null;
            return result != null;
        }
    }
}