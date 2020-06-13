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
    /// <see cref="DayOfWeek"/> を曜日を表す短縮漢字に変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(DayOfWeek), typeof(string))]
    public class DayOfWeekToShortTextConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="DayOfWeek"/> を曜日を表す短縮漢字に変換します。
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
                    return "月";
                case DayOfWeek.Tuesday:
                    return "火";
                case DayOfWeek.Wednesday:
                    return "水";
                case DayOfWeek.Thursday:
                    return "木";
                case DayOfWeek.Friday:
                    return "金";
                case DayOfWeek.Saturday:
                    return "土";
                case DayOfWeek.Sunday:
                    return "日";
                case DayOfWeek.Intensive:
                    return "集中講義";
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// 曜日を表す短縮漢字を <see cref="DayOfWeek"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static DayOfWeek ConvertBack(string value)
        {
            switch (value)
            {
                case "":
                    return DayOfWeek.None;
                case "月":
                    return DayOfWeek.Monday;
                case "火":
                    return DayOfWeek.Tuesday;
                case "水":
                    return DayOfWeek.Wednesday;
                case "木":
                    return DayOfWeek.Thursday;
                case "金":
                    return DayOfWeek.Friday;
                case "土":
                    return DayOfWeek.Saturday;
                case "日":
                    return DayOfWeek.Sunday;
                case "集中講義":
                    return DayOfWeek.Intensive;
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="DayOfWeek"/> を曜日を表す短縮漢字に変換します。
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
        /// 曜日を表す短縮漢字を <see cref="DayOfWeek"/> に変換します。
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
