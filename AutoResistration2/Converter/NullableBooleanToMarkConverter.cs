namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="bool?"/> 値を○×を表す <see cref="string"/> に変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(bool?), typeof(string))]
    public class NullableBooleanToMarkConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="bool?"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static string Convert(bool? value)
        {
            if (value.HasValue)
            {
                if (value.Value)
                {
                    return "○";
                }
                else
                {
                    return "×";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// <see cref="string"/> を <see cref="bool?"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static bool? ConvertBack(string value)
        {
            switch (value)
            {
                case "":
                    return null;
                case "○":
                    return true;
                case "×":
                    return false;
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="bool?"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? origin;

            try
            {
                origin = (bool?)value;
            }
            catch (NullReferenceException)
            {
                return DependencyProperty.UnsetValue;
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                return Convert(origin);
            }
            catch (ArgumentException)
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// <see cref="string"/> を <see cref="bool?"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ターゲットによって生成される値。 </param>
        /// <param name="targetType"> 変換後の型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string origin;

            try
            {
                origin = (string)value;
            }
            catch (NullReferenceException)
            {
                return DependencyProperty.UnsetValue;
            }
            catch (InvalidCastException)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                return ConvertBack(origin);
            }
            catch (ArgumentException)
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
