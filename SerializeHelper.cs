using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;


#if NET20
namespace System.Runtime.CompilerServices
{
  [AttributeUsageAttribute( AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method )]
  public class ExtensionAttribute : Attribute
  {
  }
}
#endif

namespace Helpers
{
    internal static class SerializeHelper
    {
        internal static T LoadOrDefault<T>(string fileName, T defaultValue)
        {
            T value = default(T);
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            if (File.Exists(fileName))
                try { value = SerializeHelper.Load<T>(fileName); }
                catch { }
            if (value == null)
            {
                value = defaultValue;
                SerializeHelper.Save(fileName, value);
            }
            return value;
        }

        internal static T LoadOrDefault<T>(Stream stream, T defaultValue)
        {
            T value = default(T);
            try { value = SerializeHelper.Load<T>(stream); }
            catch { }
            if (value == null)
            {
                value = defaultValue;
                SerializeHelper.Save(stream, value);
            }
            return value;
        }

        internal static T Load<T>(string fileName)
        {
            using (Stream reader = File.Open(Environment.ExpandEnvironmentVariables(fileName), FileMode.Open))
            {
                T value = Load<T>(reader);
                reader.Close();
                return value;
            }
        }

        internal static T Load<T>(Stream fromStream)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            T value = (T)xs.Deserialize(fromStream);
            return value;
        }

        internal static void Save<T>(string fileName, T value)
        {
            using (Stream writer = File.Create(Environment.ExpandEnvironmentVariables(fileName)))
            {
                Save<T>(writer, value);
                writer.Close();
            }
        }

        internal static void Save<T>(Stream stream, T value)
        {
            var xs = new XmlSerializer(typeof(T));
            xs.Serialize(stream, value);
            stream.Flush();
        }

        internal static string ToString<T>(T value)
        //internal static string ToString(object value)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            //XmlSerializer xs = new XmlSerializer(value.GetType());
            StringWriter writer = new StringWriter();
            xs.Serialize(writer, value);
            return writer.ToString();
        }

        internal static T FromString<T>(string value)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(value);
            return (T)xs.Deserialize(reader);
        }

        public static T ClonePublicProps<T>(this T fromObj)
        {
            Type t = fromObj.GetType();
            ConstructorInfo ctor = t.GetConstructor(new Type[] { });
            T res = (T)ctor.Invoke(null);
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (PropertyInfo p in props)
                if (p.CanWrite)
                    p.SetValue(res, p.GetValue(fromObj, null), null);
            return res;
        }


        public static T CopyPublicPropsFrom<T>(this T toObj, T fromObj)
        {
            Type t = toObj.GetType();
            PropertyInfo[] props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);
            foreach (PropertyInfo p in props)
                if (p.CanRead && p.CanWrite)
                    p.SetValue(toObj, p.GetValue(fromObj, null), null);
            return toObj;
        }

        public static T SetVal<T>(this T obj, Action<T> actSet)
        {
            actSet(obj);
            return obj;
        }

        public static string GetFullMessage(this Exception ex)
        {
            if (ex == null)
                return string.Empty;

            var sb = new StringBuilder();

            sb.AppendLine(ex.Message);

            var aex = ex as AggregateException;
            if (aex != null)
            {
                aex.InnerExceptions.ToList().ForEach(iex => sb.AppendLine("  " + iex.GetFullMessage()));
            }

            if (ex.InnerException != null)
                sb.AppendLine(ex.InnerException.GetFullMessage());

            return sb.ToString();
        }

    }

    internal static class StringHelper
    {
        /// <summary>
        /// Не только добавляет символы до длины totalWidth, но и сокращает до этой длины
        /// </summary>
        /// <param name="str">Целевая строка</param>
        /// <param name="totalWidth">Целевая длина</param>
        /// <param name="fillChar">Добавляемые символы</param>
        /// <returns></returns>
        internal static string PadLeft(String str, int totalWidth, char paddingChar)
        {
            if (str.Length > totalWidth)
                return str.Remove(totalWidth);
            else
              if (str.Length < totalWidth)
                return str.PadLeft(totalWidth, paddingChar);
            else
                return str;
        }

    }
}
