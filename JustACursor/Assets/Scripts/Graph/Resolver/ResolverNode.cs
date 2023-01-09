using System.Collections.Generic;
using Bosses;
using Bosses.Dependencies;

namespace Graph.Resolver
{
    [NodeWidth(350)]
    public class ResolverNode : Resolver<Boss> {
        
        //[Output(dynamicPortList = true, connectionType = ConnectionType.Override)] public List<ResolvedPattern> Choices;
    }
}