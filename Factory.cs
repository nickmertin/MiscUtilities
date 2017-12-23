using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MiscUtilities
{
    public static class Factory
    {
        public static T InvokeCtor<T>(params string[] args)
        {
            ConstructorInfo ctor = typeof(T).GetConstructors().Where(c => c.GetParameters().Length == args.Length).First();
            return (T)ctor.Invoke(ctor.GetParameters().Zip(args, (param, arg) =>
            {
                foreach (var attr in param.CustomAttributes.Concat(param.ParameterType.CustomAttributes))
                    if (attr.AttributeType == typeof(CustomParserAttribute))
                        return (Activator.CreateInstance(attr.ConstructorArguments[0].Value as Type) as ICustomParser).Parse(arg, param.ParameterType);
                if (param.ParameterType == typeof(string))
                    return arg;
                if (param.ParameterType.IsPrimitive)
                    return param.ParameterType.GetMethod("Parse", new[] { typeof(string) }).Invoke(null, new[] { arg });
                if (param.ParameterType.IsEnum)
                    return Enum.Parse(param.ParameterType, arg);
                throw new InvalidOperationException($"No parser is available for parameter '{param.Name}' of type '{param.ParameterType.AssemblyQualifiedName}'");
            }).ToArray());
        }
    }
    
    public interface ICustomParser
    {
        object Parse(string arg, Type targetType);
    }

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct | AttributeTargets.Delegate, Inherited = false, AllowMultiple = false)]
    public sealed class CustomParserAttribute : Attribute
    {
        public CustomParserAttribute(Type parserType) { }
    }
}
