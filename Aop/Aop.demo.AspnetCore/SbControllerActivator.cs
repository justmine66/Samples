using System;
using System.Reflection;
using Aop.demo.AspnetCore.Attritbutes;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Aop.demo.AspnetCore
{
    public class SbControllerActivator : IControllerActivator
    {
        /// <inheritdoc />
        public object Create(ControllerContext actionContext)
        {
            if (actionContext == null)
                throw new ArgumentNullException(nameof(actionContext));

            var serviceType = actionContext.ActionDescriptor.ControllerTypeInfo.AsType();

            var target = actionContext.HttpContext.RequestServices.GetRequiredService(serviceType);

            var properties = serviceType.GetTypeInfo().DeclaredProperties;
            var proxy = new ProxyGenerator().CreateClassProxyWithTarget(serviceType, target);

            foreach (var info in properties)
            {
                //属性注入
                if (info.GetCustomAttribute<AutoWiredAttribute>() != null)
                {
                    var propertyType = info.PropertyType;
                    var impl = actionContext.HttpContext.RequestServices.GetService(propertyType);
                    if (impl != null)
                    {
                        info.SetValue(proxy, impl);
                    }
                }

                //配置值注入
                if (info.GetCustomAttribute<ValueAttribute>() is ValueAttribute valueAttribute)
                {
                    var value = valueAttribute.Value;
                    if (actionContext.HttpContext.RequestServices.GetService(typeof(IConfiguration)) is IConfiguration configService)
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
        }

        /// <inheritdoc />
        public virtual void Release(ControllerContext context, object controller)
        {
        }
    }
}
