using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage; 

namespace AcFunVideo 
{
    public class Config
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="roaming">是否属于漫游数据</param>
        /// <returns></returns>
        public static T GetValue<T>(string name, T value,bool roaming=true)
        {
            object item = roaming
                ? ApplicationData.Current.RoamingSettings.Values[name]
                : ApplicationData.Current.LocalSettings.Values[name];
            if (item == null)
            {
                SetValue(name, value,roaming);
                item = value;
            }
            return (T) item;
        }

        public static void SetValue(string name, object value, bool roaming = true)
        {
            if (roaming)
                ApplicationData.Current.RoamingSettings.Values[name] = value;
            else
                ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        public static void DeleteAll(bool roaming = true)
        {
            if (roaming)
                ApplicationData.Current.RoamingSettings.Values.Clear();
            else 
                ApplicationData.Current.LocalSettings.Values.Clear();
        }

        //public static AuthorizeToken GetAuthorizeToken()
        //{
        //    AuthorizeToken authorize=new AuthorizeToken();
        //    authorize.Expires = GetValue("User.Expires", 0L);
        //    authorize.MobileCheck = GetValue("User.MobileCheck", 0);
        //    authorize.Token = GetValue("User.Token", String.Empty);
        //    authorize.UserGroupLevel = GetValue("User.UserGroupLevel", 0);
        //    authorize.UserId = GetValue("User.UserId", 0);
        //    authorize.UserImg = GetValue("User.UserImg", String.Empty);
        //    authorize.UserName = GetValue("User.UserName", string.Empty);
        //    return authorize;
        //}

        //public static void SetAuthorizeToken(AuthorizeToken authorize)
        //{
        //    SetValue("User.Expires",authorize.Expires);
        //    SetValue("User.MobileCheck",authorize.MobileCheck);
        //    SetValue("User.Token",authorize.Token);
        //    SetValue("User.UserGroupLevel",authorize.UserGroupLevel);
        //    SetValue("User.UserId",authorize.UserId);
        //    SetValue("User.UserImg",authorize.UserImg);
        //    SetValue("User.UserName",authorize.UserName);
        //}
    }

    public class ConfigSetting
    {
        public const string Device = "Device";
        public const string Setting = "Setting";
        public const string FixedScreen = Setting + ".FixedScreen";
        public const string FlashScreenFileName = "flashScreen.json";
        public const string Uid = "User.Uid";
        public const string Token = "User.Token";
        public const string PushServiceUrl = "PushService.Url";
        public const string PushServiceExpiration = "PushService.Expiration";
        public const string ScreenCheckTime = Setting + ".ScreenCheckTime";
        public const string DeviceUUID = Device + ".UUID";
    }

    public class StaticConfig
    {
        public static bool UseLockHomeTab = true;
    }
}
