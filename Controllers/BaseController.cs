using Newtonsoft.Json;
using NotasProject.Models;
using NotasProject.Models.Config;
using NotasProject.Models.Jsons;
using NotasProject.Repositories;
using NotasProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using NotasProject.Properties;

namespace NotasProject.Controllers
{
    public class BaseController : Controller
    {
        private readonly string keyCacheSeparator = Resources.KeyCacheSeparator;
        public void SetSessionItem<TClass>(string key, TClass value)
        {
            try
            {
                byte[] IV = CryptoService.SetUnique16BitAES_IV();
                Session[key + keyCacheSeparator + Convert.ToBase64String(IV)] = CryptoService.AES_Encrypt(JsonConvert.SerializeObject(value), IV);
            }
            catch (Exception e)
            {
                LoggerService.LogException(e);
            }
        }
        public TClass GetSessionItem<TClass>(string key) where TClass : new()
        {
            Tuple<string, byte[]> tuple = TupleFromKey(key);
            var jsonObject = CryptoService.AES_Decrypt(Session[tuple.Item1].ToString(), tuple.Item2);
            return JsonConvert.DeserializeObject<TClass>(jsonObject);
        }
        public void SetString(string key, string value)
        {
            SetSessionItem<object>(key, new { value });
        }
        public string GetString(string key)
        {
            return GetSessionItem<SinglePropJson>(key).Value;
        }
        public ApplicationUser GetCurrentUser()
        {
            return ExistsKey(Resources.CurrentUserObject) ? GetSessionItem<ApplicationUser>(Resources.CurrentUserObject) : GetFromPrincipal();
        }
        public void DeleteUserItems()
        {
            RemoveFromSession(Resources.MisNotas);
            RemoveFromSession(Resources.CurrentUserObject);
        }
        private ApplicationUser GetFromPrincipal()
        {
            using (UserRepository _userService = new UserRepository(new ApplicationDbContext()))
            {
                ApplicationUser user = _userService.GetByEmail(User.Identity.Name);
                SetSessionItem(Resources.CurrentUserObject, user);
                return user;
            }
        }
        public void RemoveFromSession(string key)
        {
            IterateKeys(out string[] aux);
            if (aux.Any(k => k == key))
            {
                var keypart = aux.FirstOrDefault(x => x.ToString() == (key));
                var truekey = Session.Keys.Get(Array.IndexOf(aux, keypart));
                Session.Remove(truekey);
            }
        }
        public bool ExistsKey(string key)
        {
            return Session.Keys.Count == 0 ? false : IsContained(key);
        }
        public bool IsContained(string key)
        {
            IterateKeys(out string[] aux);
            return aux.Any(k => k == key);
        }
        public Tuple<string, byte[]> TupleFromKey(string key)
        {
            IterateKeys(out string[] aux);
            var keypart = aux.FirstOrDefault(x => x.ToString() == (key));
            var truekey = Session.Keys.Get(Array.IndexOf(aux, keypart));
            Tuple<string, byte[]> tuple = new Tuple<string, byte[]>(truekey, Convert.FromBase64String(truekey.Split(new string[] { keyCacheSeparator }, StringSplitOptions.None)[1]));
            return tuple;
        }
        public void IterateKeys(out string[] keys)
        {
            var checker = 0;
            var separator = new string[] { keyCacheSeparator };
            keys = new string[Session.Keys.Count];
            foreach (var a in Session.Keys)
            {
                keys.SetValue(a.ToString().Split(separator, StringSplitOptions.None)[0], checker);
                checker++;
            }
        }
        //A medida que se complique la app, por ejemplo añadiendo posibilidad de hacer amigos y 
        //de poner notas publicas y privadas y poder editar varias personas el mismo registro, 
        //añadir a PersistedState el valor OutdatedEntity a raiz de comprobar si el campo que habría 
        //que introducir en Nota, "concurrencyTimeStamp", del registro en base de datos es igual que el de la entidad que se está editando,
        //irían aumentando las casuísiticas y en este método se tratarían todas ellas, al menos las de carácter general.
        public ActionResult SimpleJSONFeedback(PersistedState ps)
        {
            return Json(new { persState = ps == PersistedState.OK ? "OK" : "KO" });
        }
    }
}