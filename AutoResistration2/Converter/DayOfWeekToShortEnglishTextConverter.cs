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
    /// <see cref="DayOfWeek"/> を曜日を表す短縮英語に変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(DayOfWeek), typeof(string))]
    public class DayOfWeekToShortEnglishTextConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="DayOfWeek"/> を曜日を表す短縮英語に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static string Convert(DayOfWeek value)
        {
            switch (value)
            {
                case DayOfWeek.None:
                    return string.Empty;
                case DayOfWeek.Monday:
                    return "Mon";
                case DayOfWeek.Tuesday:
                    return "Tue";
                case DayOfWeek.Wednesday:
                    return "Wed";
                case DayOfWeek.Thursday:
                    return "Thu";
                case DayOfWeek.Friday:
                    return "Fri";
                case DayOfWeek.Saturday:
                    return "Sat";
                case DayOfWeek.Sunday:
                    return "Sun";
                case DayOfWeek.Intensive:
                    return "Intensive";
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// 曜日を表す短縮英語を <see cref="DayOfWeek"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static DayOfWeek ConvertBack(string value)
        {
            switch (value)
            {
                case "":
                    return DayOfWeek.None;
                case "Mon":
                    return DayOfWeek.Monday;
                case "Tue":
                    return DayOfWeek.Tuesday;
                case "Wed":
                    return DayOfWeek.Wednesday;
                case "Thu":
                    return DayOfWeek.Thursday;
                case "Fri":
                    return DayOfWeek.Friday;
                case "Sat":
                    return DayOfWeek.Saturday;
                case "Sun":
                    return DayOfWeek.Sunday;
                case "Intensive":
                    return DayOfWeek.Intensive;
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="DayOfWeek"/> を曜日を表す短縮英語に変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DayOfWeek origin;

            try
            {
                origin = (DayOfWeek)value;
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
        /// 曜日を表す短縮英語を <see cref="DayOfWeek"/> に変換します。
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
