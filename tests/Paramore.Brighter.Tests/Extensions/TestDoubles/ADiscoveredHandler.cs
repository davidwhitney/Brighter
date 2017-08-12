using System;
using System.Collections.Generic;
using System.Text;

namespace Paramore.Brighter.Tests.Extensions.TestDoubles
{
    public class ADiscoveredHandlerThatUsesTheBaseImplementation : RequestHandler<ADiscoveredHandlerThatUsesTheBaseImplementationCommand>
    {
    }

    public class ADiscoveredHandlerThatUsesTheBaseImplementationCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
