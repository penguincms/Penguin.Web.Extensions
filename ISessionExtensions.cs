using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;

namespace Penguin.Web.Extensions
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class ISessionExtensions
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        internal static JsonSerializerSettings DefaultSettings
        {
            get
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.Objects
                };

                return settings;
            }
        }

        /// <summary>
        /// Gets a Typed value from the session
        /// </summary>
        /// <typeparam name="T">The type of the value to get</typeparam>
        /// <param name="session">The session to retrieve it from</param>
        /// <param name="key">The object key</param>
        /// <returns>The typed object</returns>
        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            return value == null ? default :
                JsonConvert.DeserializeObject<T>(value, DefaultSettings);
        }

        /// <summary>
        /// Gets a typed object from the session by the class type
        /// </summary>
        /// <typeparam name="T">The type of the value to get</typeparam>
        /// <param name="session">The session to retrieve it from</param>
        /// <returns>The typed object</returns>
        public static T Get<T>(this ISession session)
        {
            string value = session.GetString(typeof(T).FullName);
            return value == null ? default :
                JsonConvert.DeserializeObject<T>(value, DefaultSettings);
        }

        /// <summary>
        /// Gets a typed object from the session by the class type
        /// </summary>
        /// <param name="session">The session to retrieve it from</param>
        /// <param name="key">The object key</param>
        /// <param name="t">The type of the value to get</param>
        /// <returns>        /// <returns>The typed object</returns></returns>
        public static object Get(this ISession session, string key, System.Type t)
        {
            string value = session.GetString(key);
            return value == null ? null : JsonConvert.DeserializeObject(value, t, DefaultSettings);
        }

        /// <summary>
        /// Sets an object to the session, using the type name as the key
        /// </summary>
        /// <param name="session">The session to set the object to</param>
        /// <param name="value">The object to set</param>
        public static void Set(this ISession session, object value)
        {
            if (value is null)
            {
                throw new System.ArgumentNullException(nameof(value));
            }

            session.SetString(value.GetType().FullName, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// Sets an object to the session
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="session">The session to add the object to</param>
        /// <param name="key">The key to add it with</param>
        /// <param name="value">The actual object to add</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}