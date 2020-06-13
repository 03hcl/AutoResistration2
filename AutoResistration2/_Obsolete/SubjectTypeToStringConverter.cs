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
    /// <see cref="SubjectType"/> を <see cref="string"/> に変換するコンバーターです。
    /// </summary>
    [System.Obsolete("現在は使いません。")]
    [ValueConversion(typeof(SubjectType), typeof(string))]
    public class SubjectTypeToStringConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="SubjectType"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static string Convert(SubjectType value)
        {
            switch ((SubjectType)value)
            {
                case SubjectType.None:
                    return string.Empty;
                case SubjectType.LiberalArts:
                    return "共通教育科目";
                ////case SubjectType.GS:
                ////    return "共通教育科目(GS科目)";
                case SubjectType.Specialized:
                    return "専門科目等";
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="string"/> を <see cref="SubjectType"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static SubjectType ConvertBack(string value)
        {
            switch ((string)value)
            {
                case "":
                    return SubjectType.None;
                case "共通教育科目":
                    return SubjectType.LiberalArts;
                ////case "共通教育科目(GS科目)":
                ////    return SubjectType.GS;
                case "専門科目等":
                    return SubjectType.Specialized;
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="SubjectType"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SubjectType origin;

            try
            {
                origin = (SubjectType)value;
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
        /// <see cref="string"/> を <see cref="SubjectType"/> に変換します。
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
