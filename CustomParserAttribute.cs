using System;
using System.Collections.Generic;
using System.Text;

namespace MiscUtilities
{
    public delegate object CustomParser(string arg);

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class CustomParserAttribute : Attribute
    {
        public CustomParserAttribute(CustomParser parser)
        {
            Parser = parser;
        }

        public CustomParser Parser { get; private set; }
    }
}
