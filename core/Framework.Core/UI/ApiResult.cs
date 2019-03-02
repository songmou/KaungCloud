using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.UI
{
    /// <summary>
    /// api 操作结果 
    /// </summary>
    public class ApiResult
    {
        public static ApiResult ToJson(string content, object data, ApiResultType type = ApiResultType.Info)
        {
            return new ApiResult(content, data, type);
        }

        public static ApiResult Success(string content, object data)
        {
            return ToJson(content, data, ApiResultType.Success);
        }
        public static ApiResult Error(string content, object data)
        {
            return ToJson(content, data, ApiResultType.Error);
        }

        /// <summary>
        /// 初始化一个<see cref="ApiResult"/>类型的新实例
        /// </summary>
        public ApiResult()
            : this(null)
        { }

        /// <summary>
        /// 初始化一个<see cref="ApiResult"/>类型的新实例
        /// </summary>
        public ApiResult(string content, ApiResultType type = ApiResultType.Info, object data = null)
            : this(content, data, type)
        { }

        /// <summary>
        /// 初始化一个<see cref="ApiResult"/>类型的新实例
        /// </summary>
        public ApiResult(string content, object data, ApiResultType type = ApiResultType.Info)
        {
            Type = type.ToString();
            Code = type;
            Content = content;
            Data = data;
        }

        public ApiResultType Code { get; set; }

        /// <summary>
        /// 获取或设置 Ajax操作结果类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 返回数据
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 表示 ajax 操作结果类型的枚举
    /// </summary>
    public enum ApiResultType
    {
        /// <summary>
        /// 警告结果类型
        /// </summary>
        Warning = 190,

        /// <summary>
        /// 消息结果类型
        /// </summary>
        Info = 201,


        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error = 500,

        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success = 200,

        /// <summary>
        /// 用户信息过期（重新登录）
        /// </summary>
        Overdue = 401,
    }
}
