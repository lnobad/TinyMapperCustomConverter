using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Nelibur.ObjectMapper;

namespace Assets
{
    public class DictExample
    {
        public DictExample()
        {
            // It does not work
            //TypeDescriptor.AddAttributes(typeof(SourceClass), new TypeConverterAttribute(typeof(SourceClassConverter)));
            TinyMapper.Bind<SourceClass, TargetClass>();

            var source = new SourceClass
            {
                S1 = "q",
                S2 = "w",
                Number = 10,
                Dictionary = new Dictionary<string, string> { { "key", "Value" } }
            };

            var result = TinyMapper.Map<TargetClass>(source);
            Console.WriteLine(result.Dictionary.Count);
            Console.WriteLine(result.str);
        }
    }
    // it works
    [TypeConverter(typeof(SourceClassConverter))]
    public class SourceClass
    {
        public int Number { get; set; }
        public string S1 { get; set; }
        public string S2 { get; set; }
        public Dictionary<string, string> Dictionary { get; set; }
    }

    public class TargetClass
    {
        public int number { get; set; }
        public string str { get; set; }
        public Dictionary<string, string> Dictionary { get; set; }
    }

    public sealed class SourceClassConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(TargetClass);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var concreteValue = (SourceClass)value;
            var result = new TargetClass
            {
                str = string.Format("{0}:{1}", concreteValue.S1, concreteValue.S2),
                Dictionary = concreteValue.Dictionary
                
            };
            return result;
        }
    }
}