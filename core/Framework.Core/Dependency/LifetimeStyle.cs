﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Core.Dependency
{
    /// <summary>
    /// 表示依赖注入的对象生命周期
    /// </summary>
    public enum LifetimeStyle
    {
        /// <summary>
        /// 实时模式，每次获取都创建不同对象
        /// Autofac中的 InstancePerDependency
        /// </summary>
        Transient,

        /// <summary>
        /// 局部模式，同一生命周期获得相同对象，不同生命周期获得不同对象（PerRequest）
        /// Autofac中的 InstancePerLifetimeScope
        /// </summary>
        Scoped,

        /// <summary>
        /// 单例模式，在第一次获取实例时创建，之后都获得相同对象
        /// SingleInstance
        /// </summary>
        Singleton
    }
}
