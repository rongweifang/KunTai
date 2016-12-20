using System;
using System.Collections.Generic;
using System.Web;
using System.Reflection;

namespace Plupload.Web.Common
{
    /// <summary>
    /// 枚举描述属性，使用EnumDescriptionAttribute以透明获取的枚举值描述信息。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumDescriptionAttribute : Attribute
    {
        #region Fields...

        /// <summary>
        /// 初始化枚举值描述文本缓存。
        /// </summary>
        private static Dictionary<string, string> dic = new Dictionary<string, string>();

        #endregion

        #region Properties...

        /// <summary>
        /// 获取或设置枚举值描述文本。
        /// </summary>
        public string Description { get; set; }

        #endregion

        #region Methods...

        /// <summary>
        /// 获取指定枚举值的描述文本。
        /// </summary>
        /// <param name="enumValue">指定枚举值。</param>
        /// <returns>指定枚举值的描述文本。</returns>
        public virtual string GetDescription(object enumValue)
        {
            if (enumValue != null)
            {
                return Description ?? enumValue.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 获取指定枚举值的描述文本。
        /// </summary>
        /// <param name="enumValue">指定枚举值。</param>
        /// <returns>指定枚举值的描述文本。</returns>
        public static string GetDescription(Enum enumValue)
        {
            //获取指定枚举值的枚举类型。
            Type type = enumValue.GetType();

            string key = GetCacheKey(type, enumValue.ToString());
            //如果缓存存在，直接返回缓存的枚举值描述文本。
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }

            FieldInfo fieldInfo = type.GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                //获得枚举中各个字段的定义数组
                var atts = (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
                if (atts.Length > 0)
                {
                    dic[key] = atts[0].Description;
                    return atts[0].Description;
                }
            }
            return enumValue.ToString();

        }

        /// <summary>
        /// 以得到指定枚举类型的所有枚举值的由EnumDescriptionAttribute或其继承类标注的描述信息
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="enumIntValue"></param>
        /// <returns></returns>
        public static string GetDescription(Type enumType, int enumIntValue)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            Dictionary<int, string> descs = EnumDescriptionAttribute.GetDescriptions(enumType);
            Dictionary<int, string>.Enumerator en = descs.GetEnumerator();
            while (en.MoveNext())
            {
                if ((enumIntValue & en.Current.Key) == en.Current.Key)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(en.Current.Value);
                    }
                    else
                    {
                        sb.Append(',');
                        sb.Append(en.Current.Value);
                    }
                }
            }

            return sb.ToString();
        }

        public static Dictionary<int, string> GetDescriptions(Type enumType)
        {
            Dictionary<int, string> descs = new Dictionary<int, string>();

            if (enumType != null && enumType.IsEnum)
            {
                FieldInfo[] fields = enumType.GetFields();

                for (int i = 1; i < fields.Length; ++i)
                {
                    object fieldValue = Enum.Parse(enumType, fields[i].Name);
                    object[] attrs = fields[i].GetCustomAttributes(true);
                    bool findAttr = false;
                    foreach (object attr in attrs)
                    {
                        if (typeof(EnumDescriptionAttribute).IsAssignableFrom(attr.GetType()))
                        {
                            descs.Add((int)fieldValue, ((EnumDescriptionAttribute)attr).GetDescription(fieldValue));
                            findAttr = true;
                            break;
                        }
                    }
                    if (!findAttr)
                    {
                        descs.Add((int)fieldValue, fieldValue.ToString());
                    }
                }
            }

            return descs;
        }

        #region Private Methods...

        /// <summary>
        /// 获取指定枚举值描述文本缓存键。
        /// </summary>
        /// <param name="type">指定枚举类型。</param>
        /// <param name="enumStrValue">指定枚举值字符串。</param>
        /// <returns>指定枚举值描述文本缓存键。</returns>
        private static string GetCacheKey(Type type, string enumStrValue)
        {
            return type.ToString() + "_" + enumStrValue;
        }

        #endregion

        #endregion
    }
}