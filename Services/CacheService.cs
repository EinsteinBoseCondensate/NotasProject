using Newtonsoft.Json;
using NotasProject.Models.Jsons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NotasProject.Services
{
    public static class CacheService
    {
        private static readonly string keyCacheSeparator = "_$__%___&___%__$_";
        private static readonly HttpContext context = HttpContext.Current;

        public static void SetSessionItem<TClass>(string key, TClass value)
        {
            try {
            byte[] IV = CryptoService.SetUnique16BitAES_IV();
            context.Session[key + keyCacheSeparator + Convert.ToBase64String(IV)] = CryptoService.AES_Encrypt(JsonConvert.SerializeObject(value), IV);
            }
            catch (Exception e)
            {
                LoggerService.LogException(e);
            }
        }
        public static TClass GetSessionItem<TClass>(string key) where TClass : new()
        {
            Tuple<string, byte[]> tuple = TupleFromKey(key);
            return JsonConvert.DeserializeObject<TClass>(CryptoService.AES_Decrypt(context.Session[tuple.Item1].ToString(), tuple.Item2));
        }
        public static void SetString(string key, string value)
        {
            SetSessionItem<object>(key, new { value });
        }
        public static string GetString(string key)
        {
            return GetSessionItem<SinglePropJson>(key).Value;
        }
        public static Tuple<string, byte[]> TupleFromKey(string key)
        {
            var checker = 0;
            string[] aux = null;
            foreach (var a in context.Session.Keys)
            {
                aux[checker] = a.ToString();
                checker++;
            }
            var truekey = aux.FirstOrDefault(x => x.ToString().Contains(key)).ToString();
            Tuple<string, byte[]> tuple = new Tuple<string, byte[]>(truekey, Convert.FromBase64String(truekey.Split(new string[] { keyCacheSeparator }, StringSplitOptions.None)[1]));
            return tuple;
        }
    }
}