using Mono.Cecil;
using Zenos.Core;

namespace Zenos.Stages
{
    class mono_defaults
    {

        static mono_defaults()
        {
            corlib = ModuleDefinition.ReadModule(typeof(object).Assembly.Location);

            object_class = GetCorlibType("Object");
            void_class = GetCorlibType("Void");
            boolean_class = GetCorlibType("Boolean");
            byte_class = GetCorlibType("Byte");
            sbyte_class = GetCorlibType("SByte");
            int16_class = GetCorlibType("Int16");
            uint16_class = GetCorlibType("UInt16");
            int32_class = GetCorlibType("Int32");
            uint32_class = GetCorlibType("UInt32");
            uint_class = GetCorlibType("UIntPtr");
            int_class = GetCorlibType("IntPtr");
            int64_class = GetCorlibType("Int64");
            uint64_class = GetCorlibType("UInt64");
            single_class = GetCorlibType("Single");
            double_class = GetCorlibType("Double");
            char_class = GetCorlibType("Char");
            string_class = GetCorlibType("String");
            enum_class = GetCorlibType("Enum");
            array_class = GetCorlibType("Array");
            delegate_class = GetCorlibType("Delegate");
            multicastdelegate_class = GetCorlibType("MulticastDelegate");
            typed_reference_class = GetCorlibType("TypedReference");
            
        }

        private static TypeDefinition GetCorlibType(string typeName)
        {
            var definition = mono_class_from_name(corlib, "System", typeName);
            Helper.NotNull(definition);
            return definition;
        }


        public static ModuleDefinition corlib { get; private set; }

        public static TypeDefinition object_class { get; private set; }
        public static TypeDefinition void_class { get; private set; }
        public static TypeDefinition boolean_class { get; private set; }
        public static TypeDefinition byte_class { get; private set; }
        public static TypeDefinition sbyte_class { get; private set; }
        public static TypeDefinition int16_class { get; private set; }
        public static TypeDefinition uint16_class { get; private set; }
        public static TypeDefinition int32_class { get; private set; }
        public static TypeDefinition uint32_class { get; private set; }
        public static TypeDefinition uint_class { get; private set; }
        public static TypeDefinition int_class { get; private set; }
        public static TypeDefinition int64_class { get; private set; }
        public static TypeDefinition uint64_class { get; private set; }
        public static TypeDefinition single_class { get; private set; }
        public static TypeDefinition double_class { get; private set; }
        public static TypeDefinition char_class { get; private set; }
        public static TypeDefinition string_class { get; private set; }
        public static TypeDefinition enum_class { get; private set; }
        public static TypeDefinition array_class { get; private set; }
        public static TypeDefinition delegate_class { get; private set; }
        public static TypeDefinition multicastdelegate_class { get; private set; }
        public static TypeDefinition typed_reference_class { get; private set; }


        private static TypeDefinition mono_class_from_name(ModuleDefinition corlib, string namespaceName, string typeName)
        {
            return corlib.GetType(namespaceName, typeName);
        }
    }
}