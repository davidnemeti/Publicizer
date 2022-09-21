﻿using Publicizer;
using NamespaceForTypeWithPrivateMembers;

namespace OuterNamespace
{
    namespace NamespaceForForwarderType
    {
        [Publicize(typeof(TypeWithPrivateMembers), GenerationKind.Instance)]
        public partial class ForwarderType
        {
        }

        [Publicize(typeof(TypeWithPrivateMembers), GenerationKind.Static)]
        public static partial class StaticForwarderType
        {
        }
    }
}
