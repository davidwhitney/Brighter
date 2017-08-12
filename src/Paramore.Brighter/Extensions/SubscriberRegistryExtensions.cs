using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Paramore.Brighter.Extensions
{
    public static class SubscriberRegistryExtensions
    {
        /// <summary>
        /// Discovers and registers commands found in the assembly containing TRootedType
        /// </summary>
        /// <typeparam name="TRootedType">Any type in the assembly you wish to scan</typeparam>
        /// <param name="registry"></param>
        /// <param name="onDiscovery">Callback upon discovery</param>
        /// <returns></returns>
        public static SubscriberRegistry RegisterCommandsFromAssemblyContaining<TRootedType>(
            this SubscriberRegistry registry, Action<DiscoveredPair> onDiscovery = null)
        {
            return RegisterCommandsFromAssemblies(registry, new[] {typeof(TRootedType).GetTypeInfo().Assembly});
        }

        public static SubscriberRegistry RegisterCommandsFromAssemblies(this SubscriberRegistry registry,
            IEnumerable<Assembly> assemblies, Action<DiscoveredPair> onDiscovery = null)
        {
            var commandHandlers = assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.GetTypeInfo().GetInterfaces().Contains(typeof(IHandleRequests)));

            return RegisterCommandHandlers(registry, commandHandlers, onDiscovery);
        }

        public static SubscriberRegistry RegisterCommandHandlers(this SubscriberRegistry registry,
            IEnumerable<Type> commandHandlers, Action<DiscoveredPair> onDiscovery = null)
        {
            onDiscovery = onDiscovery ?? (_ => { });

            foreach (var handlerType in commandHandlers)
            {
                Type commandType = null;
                var handlerTypeInfo = handlerType.GetTypeInfo();

                if (handlerTypeInfo.IsGenericType)
                {
                    commandType = handlerTypeInfo.GetGenericArguments().First();
                }

                if (handlerTypeInfo.BaseType != null)
                {
                    var baseTypeInfo = handlerTypeInfo.BaseType.GetTypeInfo();
                    if (baseTypeInfo.IsGenericType)
                    {
                        commandType = baseTypeInfo.GetGenericArguments().First();
                    }
                }

                if (commandType == null)
                {
                    continue;
                }

                registry.Add(commandType, handlerType);
                onDiscovery(new DiscoveredPair {Command = commandType, CommandHandler = handlerType});
            }

            return registry;
        }

        public class DiscoveredPair
        {
            public Type Command { get; set; }
            public Type CommandHandler { get; set; }
        }
    }

}