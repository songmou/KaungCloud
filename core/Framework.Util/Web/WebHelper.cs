﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Framework.Util.Web
{
    /// <summary>
    /// Web帮助类
    /// </summary>
    public class WebHelper
    {
        //meta正则表达式
        private static Regex _metaregex = new Regex("<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        #region 编码

        /// <summary>
        /// HTML解码
        /// </summary>
        /// <returns></returns>
        public static string HtmlDecode(string s)
        {
            return HttpUtility.HtmlDecode(s);
        }

        /// <summary>
        /// HTML编码
        /// </summary>
        /// <returns></returns>
        public static string HtmlEncode(string s)
        {
            return HttpUtility.HtmlEncode(s);
        }

        /// <summary>
        /// URL解码
        /// </summary>
        /// <returns></returns>
        public static string UrlDecode(string s)
        {
            return HttpUtility.UrlDecode(s);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <returns></returns>
        public static string UrlEncode(string s)
        {
            return HttpUtility.UrlEncode(s);
        }

        #endregion
        
        #region Http

        /// <summary>
        /// 获得参数列表
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static NameValueCollection GetParmList(string data)
        {
            NameValueCollection parmList = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(data))
            {
                int length = data.Length;
                for (int i = 0; i < length; i++)
                {
                    int startIndex = i;
                    int endIndex = -1;
                    while (i < length)
                    {
                        char c = data[i];
                        if (c == '=')
                        {
                            if (endIndex < 0)
                                endIndex = i;
                        }
                        else if (c == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value;
                    if (endIndex >= 0)
                    {
                        key = data.Substring(startIndex, endIndex - startIndex);
                        value = data.Substring(endIndex + 1, (i - endIndex) - 1);
                    }
                    else
                    {
                        key = data.Substring(startIndex, i - startIndex);
                        value = string.Empty;
                    }
                    parmList[key] = value;
                    if ((i == (length - 1)) && (data[i] == '&'))
                        parmList[key] = string.Empty;
                }
            }
            return parmList;
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">发送数据</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string postData)
        {
            return GetRequestData(url, "post", postData);
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData)
        {
            return GetRequestData(url, method, postData, Encoding.UTF8);
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData, Encoding encoding)
        {
            return GetRequestData(url, method, postData, encoding, 20000);
        }

        /// <summary>
        /// 获得http请求数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="method">请求方式</param>
        /// <param name="postData">发送数据</param>
        /// <param name="encoding">编码</param>
        /// <param name="timeout">超时值</param>
        /// <returns></returns>
        public static string GetRequestData(string url, string method, string postData, Encoding encoding, int timeout)
        {
            if (!(url.Contains("http://") || url.Contains("https://")))
                url = "http://" + url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.Trim().ToLower();
            request.Timeout = timeout;
            request.AllowAutoRedirect = true;
            request.ContentType = "text/html";
            request.Accept = "text/html, application/xhtml+xml, */*,zh-CN";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);

            try
            {
                if (!string.IsNullOrEmpty(postData) && request.Method.ToLower() == "post")
                {
                    byte[] buffer = encoding.GetBytes(postData);
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (encoding == null)
                    {
                        MemoryStream stream = new MemoryStream();
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                            new GZipStream(response.GetResponseStream(), CompressionMode.Decompress).CopyTo(stream, 10240);
                        else
                            response.GetResponseStream().CopyTo(stream, 10240);

                        byte[] RawResponse = stream.ToArray();
                        string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                        Match meta = _metaregex.Match(temp);
                        string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                        charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                        if (charter.Length > 0)
                        {
                            charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                            encoding = Encoding.GetEncoding(charter);
                        }
                        else
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                        return encoding.GetString(RawResponse);
                    }
                    else
                    {
                        StreamReader reader = null;
                        if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                        {
                            using (reader = new StreamReader(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress), encoding))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            using (reader = new StreamReader(response.GetResponseStream(), encoding))
                            {
                                try
                                {
                                    return reader.ReadToEnd();
                                }
                                catch (Exception ex)
                                {
                                    return "close:"+ex.Message;
                                }

                            }
                        }
                    }
                }

            }
            catch (WebException ex)
            {
                return "error:" + ex.Message;
            }
        }

        #endregion
        
    }
}
