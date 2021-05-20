namespace Solid.Core
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class CoreExtensions
    {
        public static T DeepCopy<T>(this T source) =>
            JsonConvert.DeserializeObject<T>(
                JsonConvert.SerializeObject(source));

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
            collection == null || collection.Any() == false;

        public static bool ContainsIgnoreCase(this string value, string compareWith) =>
            value?.Contains(compareWith, StringComparison.InvariantCultureIgnoreCase) ?? false;

        public static string ToJson<T>(this T value) =>
            value == null
            ? default
            : JsonConvert.SerializeObject(value);

        public static T FromJson<T>(this string value) =>
            string.IsNullOrEmpty(value)
            ? default
            : JsonConvert.DeserializeObject<T>(value);
    }
}
