using Autofac;
using Framework.Core.Logging;

namespace Framework.Util.Log4net
{
    /// <summary>
    /// 服务映射信息集合扩展操作
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Log4Net日志功能相关映射信息
        /// </summary>
        public static void AddLog4NetServices(this ContainerBuilder builder)
        {
            builder.RegisterType<Log4NetLogging>().As<ILogger>().SingleInstance();
        }
    }
}
