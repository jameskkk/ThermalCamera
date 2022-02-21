using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using RestSharp;
using System.Diagnostics;

namespace ThermalCameraNet
{
    /// <summary>
    /// Request Type
    /// </summary>
    public enum EnumHttpVerb
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public class WebServiceUnit
    {
        #region Properties
        /// <summary>
        /// End Point Path
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// Request Method
        /// </summary>
        public EnumHttpVerb Method { get; set; }

        /// <summary>
        /// Content Type（1、application/json 2、txt/html）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Post Data (Json is normal)
        /// </summary>
        public string PostData { get; set; }

        private const int m_Timeout = 10 * 1000;

        #endregion

        #region Initialize

        public WebServiceUnit()
        {
            EndPoint = "";
            Method = EnumHttpVerb.GET;
            ContentType = "application/json";
            PostData = "";
        }

        public WebServiceUnit(string endpoint)
        {
            EndPoint = endpoint;
            Method = EnumHttpVerb.GET;
            ContentType = "application/json";
            PostData = "";
        }

        public WebServiceUnit(string endpoint, EnumHttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json";
            PostData = "";
        }

        public WebServiceUnit(string endpoint, EnumHttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json";
            PostData = postData;
        }

        #endregion

        #region Method

        /// <summary>
        /// http Request (without parameters)
        /// </summary>
        /// <returns></returns>
        public string HttpRequest()
        {
            return HttpRequest(null);
        }

        /// <summary>
        /// http Request (with parameter)
        /// </summary>
        /// <param name="parameters">parameters Example：?name=John</param>
        /// <returns></returns>
        public string HttpRequest(Dictionary<string, string> headers)
        {
            var responseValue = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;
            //request.Accept = "application/json";
            request.Timeout = m_Timeout;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    //request.Headers.Add("Apikey", m_xmlUnit.MESAPIKey);
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(PostData) && Method == EnumHttpVerb.POST)
                {
                    var bytes = Encoding.UTF8.GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
                    {
                        var message = string.Format("Request data fail! Return HTTP error code：{0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }
                    else
                    {
                        LogUnit.Log.Info("HttpRequest() response.StatusCode: " + response.StatusCode.ToString());
                    }

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUnit.Log.Error("HttpRequest() Exception: " + ex.Message);
            }

            return responseValue;
        }

        public async Task<string> HttpRequestAsync(Dictionary<string, string> headers)
        {
            var responseValue = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(EndPoint);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;
            request.Timeout = m_Timeout;
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> item in headers)
                {
                    //request.Headers.Add("Apikey", m_xmlUnit.MESAPIKey);
                    request.Headers.Add(item.Key, item.Value);
                }
            }

            try
            {
                //LogUnit.Log.Info("HttpRequestAsync() Start GetRequestStream()...");
                if (!string.IsNullOrEmpty(PostData) && Method == EnumHttpVerb.POST)
                {
                    var bytes = Encoding.UTF8.GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                    }
                }

                //LogUnit.Log.Info("HttpRequestAsync() GetResponseAsync()...");
                using (var webResponse = await request.GetResponseAsync())
                {
                    var response = (HttpWebResponse)webResponse;
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        LogUnit.Log.Error("HttpRequestAsync() Request data fail! Return HTTP error code：" + response.StatusCode);
                        var message = string.Format("Request data fail! Return HTTP error code：{0}", response.StatusCode);
                        throw new ApplicationException(message);
                    }
                    //LogUnit.Log.Info("HttpRequestAsync() HttpStatusCode OK!");

                    using (var responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (var reader = new StreamReader(responseStream))
                            {
                                responseValue = reader.ReadToEnd();
                                //LogUnit.Log.Info("HttpRequestAsync() responseValue: " + responseValue);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUnit.Log.Error("HttpRequestAsync() Exception: " + ex.Message);
            }

            //LogUnit.Log.Info("HttpRequestAsync() End...");
            return responseValue;
        }

        #endregion
    }
}
