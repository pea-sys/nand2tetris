using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JackCompiler
{
    public class StringAttibute : Attribute
    {
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringAttibute(string value)
        {
            this.StringValue = value;
        }
    }

    public static class CommonAttribute
    {

        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the StringValue attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetStringValue(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            System.Reflection.FieldInfo fieldInfo = type.GetField(value.ToString());

            //範囲外の値チェック
            if (fieldInfo == null) return null;

            StringAttibute[] attribs = fieldInfo.GetCustomAttributes(typeof(StringAttibute), false) as StringAttibute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;

        }
    }
}
