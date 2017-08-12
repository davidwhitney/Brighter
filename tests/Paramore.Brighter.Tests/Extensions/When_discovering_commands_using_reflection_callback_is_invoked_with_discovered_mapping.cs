using System.Linq;
using FluentAssertions;
using Paramore.Brighter.Extensions;
using Paramore.Brighter.Tests.Extensions.TestDoubles;
using Xunit;

namespace Paramore.Brighter.Tests.Extensions
{
    public class SubscriberRegistryExtensionsTests
    {
        [Fact]
        public void When_discovering_commands_using_reflection_callback_is_invoked_with_discovered_mapping()
        {
            var subscriberRegistry = new SubscriberRegistry();
            var discoveredHandlers = new [] {typeof(ADiscoveredHandlerThatUsesTheBaseImplementation) };

            SubscriberRegistryExtensions.DiscoveredPair callbackPair = null;
            subscriberRegistry.RegisterCommandHandlers(discoveredHandlers, onDiscovery: pair =>
            {
                callbackPair = pair;
            });

            callbackPair.Command.ShouldBeEquivalentTo(typeof(ADiscoveredHandlerThatUsesTheBaseImplementationCommand));
            callbackPair.CommandHandler.ShouldBeEquivalentTo(typeof(ADiscoveredHandlerThatUsesTheBaseImplementation));

            subscriberRegistry.ToList()[0].Key.ShouldBeEquivalentTo(typeof(ADiscoveredHandlerThatUsesTheBaseImplementationCommand));
            subscriberRegistry.ToList()[0].Value[0].ShouldBeEquivalentTo(typeof(ADiscoveredHandlerThatUsesTheBaseImplementation));
        }
    }
}
