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
    /// <see cref="LectureForm"/> を <see cref="string"/> に変換するコンバーターです。
    /// </summary>
    [ValueConversion(typeof(LectureForm?), typeof(string))]
    public class NullableLectureFormToStringConverter : IValueConverter
    {
        /// <summary>
        /// <see cref="LectureForm?"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static string Convert(LectureForm? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case LectureForm.None:
                    return string.Empty;
                case LectureForm.Lecture:
                    return "講義";
                case LectureForm.Exercise:
                    return "演習";
                case LectureForm.Experiment:
                    return "実験";
                case LectureForm.Seminer:
                    return "ゼミナール";
                case LectureForm.Practice:
                    return "実技";
                case LectureForm.Training:
                    return "実習";
                case LectureForm.LectureAndOthers:
                    return "講義その他";
                case LectureForm.Others:
                    return "その他";
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="string"/> を <see cref="LectureForm?"/> に変換します。
        /// </summary>
        /// <param name="value"> 変換前の値。 </param>
        /// <returns> 変換された値。 </returns>
        public static LectureForm? ConvertBack(string value)
        {
            switch (value)
            {
                case null:
                    return null;
                case "":
                    return LectureForm.None;
                case "講義":
                    return LectureForm.Lecture;
                case "演習":
                    return LectureForm.Exercise;
                case "実験":
                case "実験・演習":
                    return LectureForm.Experiment;
                case "ゼミナール":
                    return LectureForm.Seminer;
                case "実技":
                    return LectureForm.Practice;
                case "実習":
                    return LectureForm.Training;
                case "講義その他":
                    return LectureForm.LectureAndOthers;
                case "その他":
                    return LectureForm.Others;
                default:
                    throw new ArgumentException("変換前が不正な値であるため、変換できません。");
            }
        }

        /// <summary>
        /// <see cref="LectureForm?"/> を <see cref="string"/> に変換します。
        /// </summary>
        /// <param name="value"> バインディング ソースによって生成された値。 </param>
        /// <param name="targetType"> バインディング ターゲット プロパティの型。 </param>
        /// <param name="parameter"> 使用するコンバーター パラメーター。 </param>
        /// <param name="culture"> コンバーターで使用するカルチャ。 </param>
        /// <returns> 変換された値。メソッドが <see cref="null"/> を返す場合は、有効な <see cref="null"/> 値が使用されています。 </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LectureForm? origin;

            try
            {
                origin = (LectureForm?)value;
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
        /// <see cref="string"/> を <see cref="LectureForm?"/> に変換します。
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