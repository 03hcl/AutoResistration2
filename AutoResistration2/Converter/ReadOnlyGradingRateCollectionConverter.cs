namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="ReadOnlyCollection{GradingRate}"/> を <see cref="string"/> に変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(ReadOnlyCollection<GradingRate>), typeof(string))]
    public class ReadOnlyGradingRateCollectionConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="ReadOnlyCollection{GradingRate}"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static string Convert(ReadOnlyCollection<GradingRate> value)
        {
            if (value == null)
            {
                return null;
            }

            return string.Join(
                Environment.NewLine,
                value.Select(
                    g =>
                    {
                        var result = new StringBuilder();

                        if (g.Percentage.HasValue)
                        {
                            result.Append(g.Percentage.Value.ToString(CultureInfo.InvariantCulture) + "%");
                        }

                        result.Append(" / ");

                        if (!string.IsNullOrEmpty(g.JPKey) && !string.IsNullOrEmpty(g.ENKey))
                        {
                            result.Append(g.JPKey);
                            result.Append(" / ");
                            result.Append(g.ENKey);
                        }
                        else
                        {
                            result.Append(g.JPKey);
                            result.Append(g.ENKey);
                        }

                        return result.ToString();
                    }));

            ////return string.Join(
            ////    Environment.NewLine,
            ////    value.Select(
            ////        g =>
            ////        {
            ////            var result = g.JPKey;

            ////            if (g.Percentage.HasValue)
            ////            {
            ////                result += "/" + g.Percentage.Value.ToString(CultureInfo.InvariantCulture) + "%";
            ////            }

            ////            if (!string.IsNullOrEmpty(g.ENKey))
            ////            {
            ////                result += " (" + g.ENKey + ")";
            ////            }

            ////            return result;
            ////        }));
        }

        /// <summary>
        /// <see cref="ReadOnlyCollection{GradingRate}"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlyCollection<GradingRate> origin;

            try
            {
                origin = (ReadOnlyCollection<GradingRate>)value;
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
        /// <see cref="string"/> を <see cref="ReadOnlyCollection{GradingRate}"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ターゲットによって生成される値。 </param>
        /// <param name="targetType"> 変換後の型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
