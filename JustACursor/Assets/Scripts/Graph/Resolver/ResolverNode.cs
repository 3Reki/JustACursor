﻿using System.Collections.Generic;
using Bosses;
using Bosses.Dependencies;

namespace Graph.Resolver
{
    [NodeWidth(500)]
    public class ResolverNode : BaseNode {
        [Input(ShowBackingValue.Never)] public int entry;
        [Output(dynamicPortList = true, connectionType = ConnectionType.Override)] public List<ResolvedPattern<Boss>> Choices;
    }
}