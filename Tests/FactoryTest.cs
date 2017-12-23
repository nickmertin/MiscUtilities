using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiscUtilities;

namespace Tests
{
    [TestClass]
    public class FactoryTest
    {
        [CustomParser(typeof(ParserType))]
        private class ParamType
        {
            public string Value { get; set; }

            private class ParserType : ICustomParser
            {
                public object Parse(string arg, Type targetType)
                {
                    return new ParamType { Value = arg };
                }
            }
        }

        private enum EnumType { A, B, C }

        private class TargetType
        {
            public TargetType(int _0, string _1, ParamType _2, EnumType _3, [CustomParser(typeof(ParserType))] object _4)
            {
                Primitive = _0;
                String = _1;
                TypeCustomParser = _2;
                Enumeration = _3;
                ParamCustomParser = _4;
            }

            public int Primitive { get; private set; }

            public string String { get; private set; }

            public ParamType TypeCustomParser { get; private set; }

            public EnumType Enumeration { get; private set; }

            public object ParamCustomParser { get; private set; }

            private class ParserType : ICustomParser
            {
                public object Parse(string arg, Type targetType)
                {
                    return arg;
                }
            }
        }

        [TestMethod]
        public void TestParsing()
        {
            TargetType value = Factory.InvokeCtor<TargetType>("12", "Hello", "Test", "A", "Test2");
            Assert.AreEqual(12, value.Primitive);
            Assert.AreEqual("Hello", value.String);
            Assert.AreEqual("Test", value.TypeCustomParser.Value);
            Assert.AreEqual(EnumType.A, value.Enumeration);
            Assert.AreEqual("Test2", value.ParamCustomParser);
        }
    }
}
