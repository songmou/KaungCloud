using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kaung.Cloud.Gateway
{
    public static class ConsulBuilderExtensions
    {
        // 服务注册
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IApplicationLifetime lifetime, IConfiguration Configuration)
        {
            var serviceScheme = Configuration["Service:Scheme"] ?? "http";
            var serviceName = Configuration["Service:Name"] ?? "";
            var serviceIP = Configuration["Service:IP"] ?? "localhost";
            var servicePort = Configuration["Service:Port"] ?? "5000";
            var serviceHealth = Configuration["Service:HealthHttp"] ?? "health";

            var consulIP = Configuration["Consul:IP"] ?? "localhost";
            var consulPort = Configuration["Consul:Port"] ?? "8500";

            //请求注册的 Consul 地址(非服务地址)
            var consulClient = new ConsulClient(x => x.Address = new Uri($"http://{consulIP}:{consulPort}"));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"{serviceScheme}://{serviceIP}:{servicePort}/{serviceHealth}",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = serviceName.ToLower() + "_" + servicePort,
                Name = serviceName,
                Address = serviceIP,
                Port = int.Parse(servicePort),
                Tags = new[] { $"urlprefix-/" + serviceName.ToLower() }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };
            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            //lifetime.ApplicationStopping.Register(() =>
            //{
            //    consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            //});
            return app;
        }
    }
}
