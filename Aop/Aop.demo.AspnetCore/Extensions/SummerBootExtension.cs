using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aop.demo.AspnetCore.Attritbutes;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aop.demo.AspnetCore.Extensions
{
    public static class SummerBootExtension
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbTransient<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, interceptorTypes);
        }

        /// <summary>
        /// 请求级别
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbScoped<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped, interceptorTypes);
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbSingleton<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, interceptorTypes);
        }

        public static IServiceCollection AddSbService(this IServiceCollection services, Type serviceType, Type implementationType,
            ServiceLifetime lifetime, params Type[] interceptorTypes)
        {
            services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));

            object Factory(IServiceProvider provider)
            {
                var target = provider.GetService(implementationType);
                var properties = implementationType.GetTypeInfo().DeclaredProperties;

                foreach (var info in properties)
                {
                    //属性注入
                    if (info.GetCustomAttribute<AutoWiredAttribute>() != null)
                    {
                        var propertyType = info.PropertyType;
                        var impl = provider.GetService(propertyType);
                        if (impl != null)
                        {
                            info.SetValue(target, impl);
                        }
                    }

                    //配置值注入
                    if (info.GetCustomAttribute<ValueAttribute>() is ValueAttribute valueAttribute)
                    {
                        var value = valueAttribute.Value;
                        if (provider.GetService(typeof(IConfiguration)) is IConfiguration configService)
                        {
                            var pathValue = configService.GetSection(value).Value;
                            if (pathValue != null)
                            {
                                var pathV = Convert.ChangeType(pathValue, info.PropertyType);
                                info.SetValue(target, pathV);
                            }
                        }

                    }
                }

                var interceptors = interceptorTypes.ToList()
                    .ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor);

                var proxy = new ProxyGenerator().CreateInterfaceProxyWithTarget(serviceType, target, interceptors.ToArray());

                return proxy;
            };

            var serviceDescriptor = new ServiceDescriptor(serviceType, Factory, lifetime);
            services.Add(serviceDescriptor);

            return services;
        }

        /// <summary>
        /// 瞬时
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbTransient<TService>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), ServiceLifetime.Transient, interceptorTypes);
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbScoped<TService>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), ServiceLifetime.Scoped, interceptorTypes);
        }

        /// <summary>
        /// 单例
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="services"></param>
        /// <param name="interceptorTypes"></param>
        /// <returns></returns>
        public static IServiceCollection AddSbSingleton<TService>(this IServiceCollection services, params Type[] interceptorTypes)
        {
            return services.AddSbService(typeof(TService), ServiceLifetime.Singleton, interceptorTypes);
        }

        public static IServiceCollection AddSbService(this IServiceCollection services, Type serviceType,
            ServiceLifetime lifetime, params Type[] interceptorTypes)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));
            if (serviceType == (Type)null)
                throw new ArgumentNullException(nameof(serviceType));

            object Factory(IServiceProvider provider)
            {
                var interceptors = interceptorTypes.ToList()
                    .ConvertAll(interceptorType => provider.GetService(interceptorType) as IInterceptor);

                var proxy = new ProxyGenerator().CreateClassProxy(serviceType, interceptors.ToArray());

                var properties = serviceType.GetTypeInfo().DeclaredProperties;

                foreach (PropertyInfo info in properties)
                {
                    //属性注入
                    if (info.GetCustomAttribute<AutoWiredAttribute>() != null)
                    {
                        var propertyType = info.PropertyType;
                        var impl = provider.GetService(propertyType);
                        if (impl != null)
                        {
                            info.SetValue(proxy, impl);
                        }
                    }

                    //配置值注入
                    if (info.GetCustomAttribute<ValueAttribute>() is ValueAttribute valueAttribute)
                    {
                        var value = valueAttribute.Value;
                        if (provider.GetService(typeof(IConfiguration)) is IConfiguration configService)
                        {
                            var pathValue = configService.GetSection(value).Value;
                            if (pathValue != null)
                            {
                                var pathV = Convert.ChangeType(pathValue, info.PropertyType);
                                info.SetValue(proxy, pathV);
                            }
                        }
                    }
                }

                return proxy;
            };

            var serviceDescriptor = new ServiceDescriptor(serviceType, Factory, lifetime);
            services.Add(serviceDescriptor);

            return services;
        }

        /// <summary>
        /// 添加summer boot扩展
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMvcBuilder AddSb(this IMvcBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            ControllerFeature feature = new ControllerFeature();
            builder.PartManager.PopulateFeature<ControllerFeature>(feature);
            foreach (Type type in feature.Controllers.Select<TypeInfo, Type>((Func<TypeInfo, Type>)(c => c.AsType())))
                builder.Services.TryAddTransient(type, type);
            builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, SbControllerActivator>());

            return builder;
        }
    }
}
