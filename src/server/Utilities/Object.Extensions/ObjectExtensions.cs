
using System.Linq;

namespace System
{
    public static class ObjectExtensions
    {
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
        
        public static string ToCSharpName<T>(this T obj)
        {
            return ToCSharpName(obj.GetType());
        }

        public static string ToCSharpName(this Type type)
        {
            var name = "";
            BuildNameFromGenericType(type);
            return name;

            void BuildNameFromGenericType(Type t)
            {
                name += t.Name;

                if (!t.GenericTypeArguments.Any())
                    return;
                var idx = name.LastIndexOf('`');
                name = name.Substring(0, idx) + "<";
                BuildNameFromGenericType(t.GenericTypeArguments.First());
                name += '>';
            }
        }
    }
}
