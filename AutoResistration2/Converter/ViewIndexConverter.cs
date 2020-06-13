namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="ListViewItem"/> を対応するインデックスに変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(ListViewItem), typeof(int))]
    public class ViewIndexConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="ListViewItem"/> を対応するインデックスに変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static int Convert(ListViewItem value)
            => (ItemsControl.ItemsControlFromItemContainer(value) as ListView).ItemContainerGenerator.IndexFromContainer(value) + 1;        

        /// <summary>
        /// <see cref="ListViewItem"/> を対応するインデックスに変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ListViewItem origin;

            try
            {
                origin = (ListViewItem)value;
            }
            catch (NullReferenceException)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }
            catch (InvalidCastException)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }

            try
            {
                return Convert(origin);
            }
            catch (ArgumentException)
            {
                return System.Windows.DependencyProperty.UnsetValue;
            }
        }

        /// <summary>
        /// インデックスを対応する <see cref="ListViewItem"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ターゲットによって生成される値。 </param>
        /// <param name="targetType"> 変換後の型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Windows.DependencyProperty.UnsetValue;
        }
    }
}
