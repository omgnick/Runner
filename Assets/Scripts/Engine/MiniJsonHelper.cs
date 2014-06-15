using System;
using System.Collections.Generic;

namespace Swrve.Helpers
{
    public static class MiniJsonHelper
    {
        public static int GetInt (Dictionary<string, object> json, string key)
        {
            return GetInt (json, key, 0);
        }
     
        public static int GetInt (Dictionary<string, object> json, string key, int defaultValue)
        {
            object val = json [key];
            if (val is int) {
                return (int)val;
            } else if (val is long) {
                return (int)(long)val;
            }
         
            return defaultValue;
        }
     
        public static long GetLong (Dictionary<string, object> json, string key)
        {
            return GetLong (json, key, 0);
        }
     
        public static long GetLong (Dictionary<string, object> json, string key, long defaultValue)
        {
            object val = json [key];
            if (val is long) {
                return (long)val;
            } else if (val is int) {
                return (long)(int)val;
            }
         
            return defaultValue;
        }
     
        public static float GetFloat (Dictionary<string, object> json, string key)
        {
            return GetFloat (json, key, 0);
        }
         
        public static float GetFloat (Dictionary<string, object> json, string key, float defaultValue)
        {
            object val = json [key];
            if (val is float) {
                return (float)val;
            } else if (val is long) {
                return (float)(long)val;
            }

            return defaultValue;
        }
     
        public static bool GetBool (Dictionary<string, object> json, string key)
        {
            return GetBool (json, key, false);
        }
     
        public static bool GetBool (Dictionary<string, object> json, string key, bool defaultValue)
        {
            object val = json [key];
            if (val is bool) {
                return (bool)val;
            }
         
            return defaultValue;
        }
     
        public static string GetString (Dictionary<string, object> json, string key)
        {
            return GetString (json, key, null);
        }
     
        public static string GetString (Dictionary<string, object> json, string key, string defaultValue)
        {
            object val = json [key];
            if (val is string) {
                return (string)val;
            }
         
            return defaultValue;
        }
    }
}

