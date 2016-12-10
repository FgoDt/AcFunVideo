using System;
using System.Diagnostics;
using Windows.ApplicationModel.Store;
using Windows.Networking.PushNotifications;
using Windows.Storage;
using cn.bmob.api;
using cn.bmob.io;
using cn.bmob.tools;

namespace AcFunVideo.Utilites
{
    public static class PushService
    {
        public static string UUID
        {
            get
            {
                var u = ApplicationData.Current.LocalSettings.Values["UUID"];
                if (u == null)
                {
                    var guid = Guid.NewGuid().ToString("N");
                    ApplicationData.Current.LocalSettings.Values["UUID"] = guid;
                    return guid;
                }
                return (string)u;
            }
        }

        private static BmobWindows bmob = new BmobWindows();

        public static void InitBmob(string appkey, string reskey)
        {
            bmob.initialize(appkey, reskey);
            BmobDebug.Register(msg => { Debug.WriteLine(msg); });
        }
        public static void InitPushService(PushNotificationChannel channel,string tableName,Action action)
        {
            BmobQuery query=new BmobQuery();
            query.WhereEqualTo("DeviceId", UUID);
            bmob.Find<Push>(tableName, query, (resp, ex) =>
            {
                if (ex != null || resp.results.Count == 0)
                {
                    Push push = new Push();
                    push.LeftTime = channel.ExpirationTime.UtcDateTime;
                    push.CURL = channel.Uri;
                    push.DeviceId = UUID;
                    push.ProductId = CurrentApp.AppId.ToString("D");
                    bmob.Create(tableName, push, (s, e) =>
                    {
                        action?.Invoke();
                    });
                    return;
                }
                if (resp.results.Count == 1)
                {
                    var push = resp.results[0];
                    push.LeftTime= channel.ExpirationTime.UtcDateTime;
                    push.CURL = channel.Uri;
                    bmob.Update(tableName,push.objectId,push, (s, e) =>
                    {
                        action?.Invoke();
                    });
                }
            });
        } 
    } 
    public class Push : BmobTable
    {
        public DateTime LeftTime { get; set; } 
        public string DeviceId { get; set; }
        public string CURL { get; set; }
        public string ProductId { get; set; }
        public override void readFields(BmobInput input)
        {
            base.readFields(input);
            LeftTime = input.getDate("LeftTime");
            DeviceId = input.Get<string>("DeviceId");
            CURL = input.Get<string>("CURL");
            ProductId = input.Get<string>("ProductId");
        }

        public override void write(BmobOutput output, bool all)
        {
            base.write(output, all);
            output.Put("LeftTime",(BmobDate)LeftTime);
            output.Put("DeviceId", DeviceId);
            output.Put("CURL", CURL);
            output.Put("ProductId", ProductId);
        }
    }
}
