using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Mkh.Data
{
    /// <summary>
    /// 通用扩展方法
    /// </summary>
    internal static class CommonExtensions
    {
        #region ==数据转换扩展==

        /// <summary>
        /// 转换成Byte
        /// </summary>
        /// <param name="s">输入字符串</param>
        /// <returns></returns>
        public static byte ToByte(this string s)
        {
            if (s.IsNull())
                return 0;

            byte.TryParse(s, out byte result);
            return result;
        }

        /// <summary>
        /// 转换成Char
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char ToChar(this string s)
        {
            if (s.IsNull())
                return default;

            char.TryParse(s, out char result);
            return result;
        }

        /// <summary>
        /// 转换成Char
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char ToChar(this int s)
        {
            return (char)s;
        }

        /// <summary>
        /// 转换成short/Int16
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static short ToShort(this string s)
        {
            if (s.IsNull())
                return 0;

            short.TryParse(s, out short result);
            return result;
        }

        /// <summary>
        /// 转换成Int/Int32
        /// </summary>
        /// <param name="s"></param>
        /// <param name="round">是否四舍五入，默认false</param>
        /// <returns></returns>
        public static int ToInt(this object s, bool round = false)
        {
            if (s == null || s == DBNull.Value)
                return 0;

            if (s is bool b)
                return b ? 1 : 0;

            if (int.TryParse(s.ToString(), out int result))
                return result;

            if (s.GetType().IsEnum)
            {
                return (int)s;
            }

            var f = s.ToFloat();
            return round ? Convert.ToInt32(f) : (int)f;
        }

        /// <summary>
        /// 转换成Long/Int64
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static long ToLong(this object s)
        {
            if (s == null || s == DBNull.Value)
                return 0L;

            long.TryParse(s.ToString(), out long result);
            return result;
        }

        /// <summary>
        /// 转换成Float/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static float ToFloat(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value)
                return 0f;

            float.TryParse(s.ToString(), out float result);

            if (decimals == null)
                return result;

            return (float)Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换成Double/Single
        /// </summary>
        /// <param name="s"></param>
        /// <param name="digits">小数位数</param>
        /// <returns></returns>
        public static double ToDouble(this object s, int? digits = null)
        {
            if (s == null || s == DBNull.Value)
                return 0d;

            double.TryParse(s.ToString(), out double result);

            if (digits == null)
                return result;

            return Math.Round(result, digits.Value);
        }

        /// <summary>
        /// 转换成Decimal
        /// </summary>
        /// <param name="s"></param>
        /// <param name="decimals">小数位数</param>
        /// <returns></returns>
        public static decimal ToDecimal(this object s, int? decimals = null)
        {
            if (s == null || s == DBNull.Value) return 0m;

            decimal.TryParse(s.ToString(), out decimal result);

            if (decimals == null)
                return result;

            return Math.Round(result, decimals.Value);
        }

        /// <summary>
        /// 转换成DateTime
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object s)
        {
            if (s == null || s == DBNull.Value)
                return DateTime.MinValue;

            DateTime.TryParse(s.ToString(), out DateTime result);
            return result;
        }

        /// <summary>
        /// 转换成Date
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime ToDate(this object s)
        {
            return s.ToDateTime().Date;
        }

        /// <summary>
        /// 转换成Boolean
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool ToBool(this object s)
        {
            if (s == null) 
                return false;

            s = s.ToString().ToLower();
            if (s.Equals(1) || s.Equals("1") || s.Equals("true") || s.Equals("是") || s.Equals("yes"))
                return true;
            if (s.Equals(0) || s.Equals("0") || s.Equals("false") || s.Equals("否") || s.Equals("no"))
                return false;

            Boolean.TryParse(s.ToString(), out bool result);
            return result;
        }

        /// <summary>
        /// 字符串转Guid
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string s)
        {
            if (s.NotNull() && Guid.TryParse(s, out Guid val))
                return val;

            return Guid.Empty;
        }

        /// <summary>
        /// 泛型转换，转换失败会抛出异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T To<T>(this object s)
        {
            return (T)Convert.ChangeType(s, typeof(T));
        }

        #endregion

        #region ==布尔转换==

        /// <summary>
        /// 布尔值转换为字符串1或者0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToIntString(this bool b)
        {
            return b ? "1" : "0";
        }

        /// <summary>
        /// 布尔值转换为整数1或者0
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int ToInt(this bool b)
        {
            return b ? 1 : 0;
        }

        /// <summary>
        /// 布尔值转换为中文
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToZhCn(this bool b)
        {
            return b ? "是" : "否";
        }

        #endregion

        #region ==字节转换==

        /// <summary>
        /// 转为十六进制
        /// </summary>
        /// <param name="val"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        public static string ToHex(this string val, bool lowerCase = true)
        {
            if (val.IsNull())
                return null;

            var bytes = Encoding.UTF8.GetBytes(val);
            return bytes.ToHex(lowerCase);
        }

        /// <summary>
        /// 转换为16进制
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="lowerCase">是否小写</param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes, bool lowerCase = true)
        {
            if (bytes == null)
                return null;

            var result = new StringBuilder();
            var format = lowerCase ? "x2" : "X2";
            for (var i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString(format));
            }

            return result.ToString();
        }

        /// <summary>
        /// 16进制转字节数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Hex2Bytes(this string s)
        {
            if (s.IsNull())
                return null;
            var bytes = new byte[s.Length / 2];

            for (int x = 0; x < s.Length / 2; x++)
            {
                int i = (Convert.ToInt32(s.Substring(x * 2, 2), 16));
                bytes[x] = (byte)i;
            }

            return bytes;
        }

        /// <summary>
        /// 16进制转字符串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Hex2String(this string val)
        {
            if (val.IsNull())
                return null;

            var bytes = val.Hex2Bytes();
            return Encoding.UTF8.GetString(bytes);
        }

        #endregion

        #region ==类型扩展==

        /// <summary>
        /// 判断属性是否是静态的
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsStatic(this PropertyInfo property) => (property.GetMethod ?? property.SetMethod).IsStatic;

        /// <summary>
        /// 判断指定类型是否实现于该类型
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementType"></param>
        /// <returns></returns>
        public static bool IsImplementType(this Type serviceType, Type implementType)
        {
            //泛型
            if (serviceType.IsGenericType)
            {
                if (serviceType.IsInterface)
                {
                    var interfaces = implementType.GetInterfaces();
                    if (interfaces.Any(m => m.IsGenericType && m.GetGenericTypeDefinition() == serviceType))
                    {
                        return true;
                    }
                }
                else
                {
                    if (implementType.BaseType != null && implementType.BaseType.IsGenericType && implementType.BaseType.GetGenericTypeDefinition() == serviceType)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (serviceType.IsInterface)
                {
                    var interfaces = implementType.GetInterfaces();
                    if (interfaces.Any(m => m == serviceType))
                        return true;
                }
                else
                {
                    if (implementType.BaseType != null && implementType.BaseType == serviceType)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断是否继承自指定的泛型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="generic"></param>
        /// <returns></returns>
        public static bool IsSubclassOfGeneric(this Type type, Type generic)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == cur)
                {
                    return true;
                }
                type = type.BaseType;
            }
            return false;
        }

        /// <summary>
        /// 判断是否可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 判断是否是String类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsString(this Type type)
        {
            return type == TypeConst.String;
        }

        /// <summary>
        /// 判断是否是Byte类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsByte(this Type type)
        {
            return type == TypeConst.Byte;
        }

        /// <summary>
        /// 判断是否是Char类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsChar(this Type type)
        {
            return type == TypeConst.Char;
        }

        /// <summary>
        /// 判断是否是Short类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsShort(this Type type)
        {
            return type == TypeConst.Short;
        }

        /// <summary>
        /// 判断是否是Int类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInt(this Type type)
        {
            return type == TypeConst.Int;
        }

        /// <summary>
        /// 判断是否是Long类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsLong(this Type type)
        {
            return type == TypeConst.Long;
        }

        /// <summary>
        /// 判断是否是Float类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFloat(this Type type)
        {
            return type == TypeConst.Float;
        }

        /// <summary>
        /// 判断是否是Double类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDouble(this Type type)
        {
            return type == TypeConst.Double;
        }

        /// <summary>
        /// 判断是否是Decimal类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDecimal(this Type type)
        {
            return type == TypeConst.Decimal;
        }

        /// <summary>
        /// 判断是否是DateTime类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsDateTime(this Type type)
        {
            return type == TypeConst.DateTime;
        }

        /// <summary>
        /// 判断是否是Guid类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGuid(this Type type)
        {
            return type == TypeConst.Guid;
        }

        /// <summary>
        /// 判断是否是Bool类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBool(this Type type)
        {
            return type == TypeConst.Bool;
        }

        #endregion

        #region ==字符串扩展==

        /// <summary>
        /// 判断字符串是否为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNull(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 判断字符串是否不为Null、空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool NotNull(this string s)
        {
            return !string.IsNullOrWhiteSpace(s);
        }

        /// <summary>
        /// 与字符串进行比较，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualsIgnoreCase(this string s, string value)
        {
            return s.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 匹配字符串结尾，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EndsWithIgnoreCase(this string s, string value)
        {
            return s.EndsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 匹配字符串开头，忽略大小写
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StartsWithIgnoreCase(this string s, string value)
        {
            return s.StartsWith(value, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 首字母转小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToLower(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToLower() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 首字母转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            string str = s.First().ToString().ToUpper() + s.Substring(1);
            return str;
        }

        /// <summary>
        /// 转为Base64，UTF-8格式
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToBase64(this string s)
        {
            return s.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 转为Base64
        /// </summary>
        /// <param name="s"></param>
        /// <param name="encoding">编码</param>
        /// <returns></returns>
        public static string ToBase64(this string s, Encoding encoding)
        {
            if (s.IsNull())
                return string.Empty;

            var bytes = encoding.GetBytes(s);
            return bytes.ToBase64();
        }

        /// <summary>
        /// 转换为Base64
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            return Convert.ToBase64String(bytes);
        }

        #endregion

        #region ==集合扩展==

        /// <summary>
        /// 判断集合不为NULL且元素数不为0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }


        /// <summary>
        /// 判断集合不为NULL且元素数不为0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool NotNullAndEmpty<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        #endregion
    }


    /// <summary>
    /// 类型常量
    /// </summary>
    internal class TypeConst
    {
        /// <summary>
        /// String
        /// </summary>
        public static readonly Type String = typeof(string);

        /// <summary>
        /// Byte
        /// </summary>
        public static readonly Type Byte = typeof(byte);

        /// <summary>
        /// Char
        /// </summary>
        public static readonly Type Char = typeof(char);

        /// <summary>
        /// Short
        /// </summary>
        public static readonly Type Short = typeof(short);

        /// <summary>
        /// Int
        /// </summary>
        public static readonly Type Int = typeof(int);

        /// <summary>
        /// Long
        /// </summary>
        public static readonly Type Long = typeof(long);

        /// <summary>
        /// Float
        /// </summary>
        public static readonly Type Float = typeof(float);

        /// <summary>
        /// Double
        /// </summary>
        public static readonly Type Double = typeof(double);

        /// <summary>
        /// Decimal
        /// </summary>
        public static readonly Type Decimal = typeof(decimal);

        /// <summary>
        /// DateTime
        /// </summary>
        public static readonly Type DateTime = typeof(DateTime);

        /// <summary>
        /// Guid
        /// </summary>
        public static readonly Type Guid = typeof(Guid);

        /// <summary>
        /// Bool
        /// </summary>
        public static readonly Type Bool = typeof(bool);
    }
}
