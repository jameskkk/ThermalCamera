using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThermalCameraNet
{
    public class LineUnit
    {
        [Serializable]
        public class LineNotifyInfo
        {
            public string floorInfo { get; set; }
            public string imageAraayBase64 { get; set; }
            public string imageLink { get; set; }
            public string lineToken { get; set; }
            public string message { get; set; }
            public string moduleInfo { get; set; }
        }

        public static void PushLineNotify(Image image)
        {
            string lineURL = Properties.Settings.Default.LineNotifyURL;
            var task = Task.Run(() =>
            {
                LogUnit.Log.Info("PushLineNotify() Task Thread Id " + Thread.CurrentThread.ManagedThreadId.ToString());
                LogUnit.Log.Info("PushLineNotify() LineNotifyURL: " + Properties.Settings.Default.LineNotifyURL);
                string imageBase64 = ImageUnit.ImageToBase64(image, System.Drawing.Imaging.ImageFormat.Png);
                //LogUnit.Log.Info("PushLineNotify() imageBase64.Length = " + imageBase64.Length);
                UpdateLineNotifyAsync(imageBase64, lineURL);
                //LogUnit.Log.Info("PushLineNotify() end...");
            });
        }

        public static void UpdateLineNotifyAsync(string imageBase64, string serverUrl)
        {
            try
            {
                //LogUnit.Log.Info("UpdateLineNotifyAsync() Enter...");
                WebServiceUnit client = new WebServiceUnit();
                client.EndPoint = serverUrl;
                client.Method = EnumHttpVerb.POST;

                LineNotifyInfo lineNotifyInfo = new LineNotifyInfo();
                lineNotifyInfo.floorInfo = "Loddy";
                lineNotifyInfo.imageAraayBase64 = imageBase64;
                lineNotifyInfo.imageLink = "empty";
                lineNotifyInfo.lineToken = Properties.Settings.Default.LineToken;
                lineNotifyInfo.message = "Alarm temperature overflow!";
                lineNotifyInfo.moduleInfo = "Thermal Camera";
                string jsonText = JsonConvert.SerializeObject(lineNotifyInfo);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                client.ContentType = "application/json";
                client.PostData = jsonText;

                //LogUnit.Log.Info("UpdateLineNotifyAsync() PostData: " + client.PostData);
                var result = client.HttpRequest(headers);
                LogUnit.Log.Info("UpdateLineNotifyAsync() result = " + result);
                //LogUnit.Log.Info("UpdateLineNotifyAsync() end!");
            }
            catch (Exception ex)
            {
                LogUnit.Log.Error("UpdateLineNotifyAsync() Exception: " + ex.Message);
            }
        }
    }
}
