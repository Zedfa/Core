using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Serialization;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;

namespace Core.Serialization.ObjectProxy
{
    public class CSharpCodeProviderForProxyOjectSerialization
    {
        public IObjectProxy Compile()
        {
            var code = GenerateStringCSharpCodeForCompile();
            var provider = new CSharpCodeProvider(
            new Dictionary<String, String> { { "CompilerVersion", "v4.0" } });
            // CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;
            // parameters.OutputAssembly = "OutputAssembly.dll";
            //parameters.ReferencedAssemblies.Add("System.dll");
            //parameters.ReferencedAssemblies.Add("WpfApp1.exe");
            // Reference to System.Drawing library
            System.AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(ass =>
            {
                try
                {
                    var filename = Path.GetFileName(ass.Location);
                    Assembly.LoadFrom(filename);
                    parameters.ReferencedAssemblies.Add(Path.GetFileName(ass.Location));
                }
                catch (Exception ex)
                {

                }
            });
            // True - memory generation, false - external file generation
            parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            parameters.GenerateExecutable = false;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }

            Assembly assembly = results.CompiledAssembly;
            var InstanceOfObjectProxy = Activator.CreateInstance(assembly.GetTypes().FirstOrDefault(type => typeof(IObjectProxy).IsAssignableFrom(type))) as IObjectProxy;
            return InstanceOfObjectProxy;
        }
        public CSharpCodeProviderForProxyOjectSerialization(Type type)
        {
            if (type.IsSimple())
                throw new ArgumentException();
            Type = type;
            ObjectMetaData = ObjectMetaData.GetEntityMetaData(type);
        }
        string GenerateStringOfType(Type type)
        {
            if (type.IsGenericType)
            {
                var stringType = type.Namespace + "." + GenerateStringForDeclaringType(type.DeclaringType) + type.Name.Split('`')[0] + "<";
                foreach (var t in type.GenericTypeArguments)
                {
                    stringType += GenerateStringOfType(t);
                    stringType += ",";
                }
                //for ignoring last ',' in string ...
                stringType = stringType.Substring(0, stringType.Count() - 1);
                stringType += ">";
                return stringType;
            }
            else
                return type.Namespace + "." + GenerateStringForDeclaringType(type.DeclaringType) + type.Name;
        }
        private string GenerateStringForDeclaringType(Type declaringType)
        {
            if (declaringType == null)
                return string.Empty;
            return declaringType.Name + "." + GenerateStringForDeclaringType(declaringType.DeclaringType);
        }
        public Type Type { get; set; }
        public ObjectMetaData ObjectMetaData { get; private set; }
        public string GenerateStringCSharpCodeForCompile()
        {
            var nameSpace = "a" + Guid.NewGuid().ToString().Replace("-", "");
            var nameSpaceBlock = $@"
namespace {nameSpace}
{{
";
            var properyClassesSourceCode = "";
            var ListPropertyClassName = new List<string>();
            var typeClassName = "ClassFor" + Type.Name.Split('`')[0] + "Type";
            var typeClassstr = GenerateStringOfType(Type);
            for (var i = 0; i < ObjectMetaData.WritablePropertyList.Count; i++)
            {
                var propertyTypeString = GenerateStringOfType(ObjectMetaData.WritablePropertyList[i].PropertyType);
                var propertyClassName = "ClassFor" + ObjectMetaData.WritablePropertyList[i].Name + "Property";
                ListPropertyClassName.Add(propertyClassName);

                properyClassesSourceCode +=

                $@"public class {propertyClassName} : Core.Serialization.ObjectProxy.IPropertyProxy
    {{

        public System.Type Type {{ get {{ return typeof({typeClassstr}); }} }}
        public System.Type PropertyType {{ get {{ return typeof({propertyTypeString}); }} }}
        public string ProperyName {{ get {{ return  ""{ObjectMetaData.WritablePropertyList[i].Name}""; }} }}
        public Core.Serialization.ObjectProxy.IPropertyProxy Copy()
            {{
                    return new {propertyClassName}();
            }}
        public object GetProperty(object obj)
        {{
            return (({typeClassstr})obj).{ObjectMetaData.WritablePropertyList[i].Name};
        }}
        public void SetProperty(object obj,object value)
        {{
            var unboxedObj = ({typeClassstr})obj;
            var unboxedValue = ({propertyTypeString})value;
            unboxedObj.{ObjectMetaData.WritablePropertyList[i].Name} = unboxedValue;
        }}
    }}";
                properyClassesSourceCode += Environment.NewLine;
            }

            var startPartOfObjectProxyClass =
             $@"public class {typeClassName} : Core.Serialization.ObjectProxy.IObjectProxy
    {{
        public System.Type Type {{ get {{ return typeof({typeClassstr}); }} }}

        public  {typeClassName}()
        {{
               ProxyPropertyList = new System.Collections.Generic.List<Core.Serialization.ObjectProxy.IPropertyProxy>();
";
            var proxyPropertyListPart = "";
            for (var i = 0; i < ObjectMetaData.WritablePropertyList.Count; i++)
            {
                proxyPropertyListPart += $@"ProxyPropertyList.Add(new {ListPropertyClassName[i]}());";
                proxyPropertyListPart += Environment.NewLine;
            }

            var endPartOfObjectProxyClass = $@"}}
        public System.Collections.Generic.List<Core.Serialization.ObjectProxy.IPropertyProxy> ProxyPropertyList
        {{ get; private set; }}

        public Core.Serialization.ObjectProxy.IObjectProxy Copy()
        {{
            return new {typeClassName}();
        }}
        public object CreateObject()
        {{
            var classCopy = new {typeClassstr}();
            return classCopy;
            
        }}
    }}
}}
";
            var sourceCode = nameSpaceBlock + properyClassesSourceCode + startPartOfObjectProxyClass + proxyPropertyListPart + endPartOfObjectProxyClass;
            return sourceCode;
        }
    }
}
