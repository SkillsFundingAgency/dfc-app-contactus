using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DFC.App.ContactUs.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescriptionX<T>(this T e)
            where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                var values = (int[])System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var enumName = type.GetEnumName(val);

                        if (!string.IsNullOrWhiteSpace(enumName))
                        {
                            var memInfo = type.GetMember(enumName);
                            var descriptionAttribute = memInfo[0]
                                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                                .FirstOrDefault() as DescriptionAttribute;

                            if (descriptionAttribute != null)
                            {
                                return descriptionAttribute.Description;
                            }
                        }
                    }
                }
            }

            return string.Empty;
        }

        public static string GetDescription<T>(this T e)
            where T : IConvertible
        {
            Type genericEnumType = e.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember($"{e}");
            if (memberInfo != null && memberInfo.Length > 0)
            {
                var attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (attribs != null && attribs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attribs.ElementAt(0)).Description;
                }
            }

            return $"{e}";
        }
    }
}
