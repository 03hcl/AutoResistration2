namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// 評価の割合を表します。
    /// </summary>
    public class GradingRate : DependencyObject
    {
        #region 依存関係プロパティ

        /// <summary>
        /// <see cref="JPKey"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPKeyProperty =
            DependencyProperty.Register(nameof(JPKey), typeof(string), typeof(GradingRate), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="ENKey"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENKeyProperty =
            DependencyProperty.Register(nameof(ENKey), typeof(string), typeof(GradingRate), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="Percentage"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register(nameof(Percentage), typeof(decimal?), typeof(GradingRate), new PropertyMetadata(default(decimal?)));

        /// <summary>
        /// <see cref="GradingType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty GradingTypeProperty =
            DependencyProperty.Register(nameof(GradingType), typeof(GradingType), typeof(GradingRate), new PropertyMetadata(default(GradingType)));

        #endregion

        #region コンストラクタ

        /// <summary>
        /// <see cref="GradingRate"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keyJP"> 日本語の項目名。 </param>
        /// <param name="keyEN"> 英語の項目名。 </param>
        /// <param name="percentage"> 割合を表す値。 </param>
        public GradingRate(string keyJP, string keyEN, decimal? percentage)
        {
            this.JPKey = keyJP;
            this.ENKey = keyEN;
            this.Percentage = percentage;

            this.GradingType = GetGradingType(keyJP, keyEN);
        }

        /// <summary>
        /// <see cref="GradingRate"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="keyJP"> 日本語の項目名。 </param>
        /// <param name="keyEN"> 英語の項目名。 </param>
        /// <param name="percentage"> 割合を表す値。 </param>
        /// <param name="gradingType"> 評価の種類。 </param>
        public GradingRate(string keyJP, string keyEN, decimal? percentage, GradingType gradingType)
        {
            this.JPKey = keyJP;
            this.ENKey = keyEN;
            this.Percentage = percentage;

            this.GradingType = gradingType;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 日本語の項目名を取得します。
        /// </summary>
        public string JPKey
        {
            get { return (string)this.GetValue(JPKeyProperty); }
            private set { this.SetValue(JPKeyProperty, value); }
        }

        /// <summary>
        /// 英語の項目名を取得します。
        /// </summary>
        public string ENKey
        {
            get { return (string)this.GetValue(ENKeyProperty); }
            private set { this.SetValue(ENKeyProperty, value); }
        }

        /// <summary>
        /// 割合を表す値を取得します。
        /// </summary>
        public decimal? Percentage
        {
            get { return (decimal?)this.GetValue(PercentageProperty); }
            private set { this.SetValue(PercentageProperty, value); }
        }

        /// <summary>
        /// 評価の種類を取得します。
        /// </summary>
        public GradingType GradingType
        {
            get { return (GradingType)this.GetValue(GradingTypeProperty); }
            private set { this.SetValue(GradingTypeProperty, value); }
        }

        #endregion

        #region 静的メンバー・静的メソッド

        /// <summary>
        /// 評価の割合の項目名比較時に使われる <see cref="CompareInfo"/> を取得します。
        /// </summary>
        private static CompareInfo CompareInfo { get; } = CultureInfo.InvariantCulture.CompareInfo;

        /// <summary>
        /// 指定した日本語の文字列が与えられた文字列内に存在するかどうかを示す値を返します。
        /// </summary>
        /// <param name="source"> 検索対象の文字列。 </param>
        /// <param name="value"> <see cref="source"/> 内で検索する文字列。 </param>
        /// <returns> <see cref="source"/> 全体内で <see cref="value"/> が見つかった場合は <see cref="true"/> 、それ以外の場合は <see cref="false"/> 。 </returns>
        private static bool ContainsJP(string source, string value)
            => !string.IsNullOrEmpty(source) && CompareInfo.IndexOf(source, value, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) >= 0;

        /// <summary>
        /// 指定した英語の文字列が与えられた文字列内に存在するかどうかを示す値を返します。
        /// </summary>
        /// <param name="source"> 検索対象の文字列。 </param>
        /// <param name="value"> <see cref="source"/> 内で検索する文字列。 </param>
        /// <returns> <see cref="source"/> 全体内で <see cref="value"/> が見つかった場合は <see cref="true"/> 、それ以外の場合は <see cref="false"/> 。 </returns>
        private static bool ContainsEN(string source, string value)
            => !string.IsNullOrEmpty(source) && CompareInfo.IndexOf(source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreSymbols) >= 0;

        /// <summary>
        /// 日本語と英語の項目名から対応する評価の種類を取得します。
        /// </summary>
        /// <param name="keyJP"> 日本語の項目名。 </param>
        /// <param name="keyEN"> 英語の項目名。 </param>
        /// <returns> 評価の種類。 </returns>
        private static GradingType GetGradingType(string keyJP, string keyEN)
        {
            if (ContainsJP(keyJP, "出席") || ContainsEN(keyEN, "attendance") || ContainsEN(keyJP, "attendance"))
            {
                return GradingType.Attendance;
            }
            else if (ContainsJP(keyJP, "態度") || ContainsJP(keyJP, "平常") || ContainsJP(keyJP, "意欲") || ContainsJP(keyJP, "取組") || ContainsJP(keyJP, "取り組み")
                || ContainsEN(keyEN, "attitude") || ContainsEN(keyJP, "attitude")
                || ContainsEN(keyEN, "react") || ContainsEN(keyJP, "react")
                || ContainsEN(keyEN, "participation") || ContainsEN(keyJP, "participation"))
            {
                return GradingType.Attitude;
            }
            else if (ContainsJP(keyJP, "論文") || ContainsEN(keyEN, "thesis") || ContainsEN(keyJP, "thesis"))
            {
                return GradingType.Thesis;
            }
            else if (ContainsJP(keyJP, "レポート") || ContainsEN(keyEN, "report") || ContainsEN(keyJP, "report"))
            {
                return GradingType.Report;
            }
            else if (ContainsJP(keyJP, "課題") || ContainsJP(keyJP, "宿題") || ContainsJP(keyJP, "提出")
                || ContainsEN(keyEN, "homework") || ContainsEN(keyJP, "homework")
                || ContainsEN(keyEN, "assignment") || ContainsEN(keyJP, "assignment"))
            {
                return GradingType.Homework;
            }
            else if (ContainsJP(keyJP, "小テスト") || ContainsEN(keyEN, "mini") || ContainsEN(keyJP, "mini"))
            {
                return GradingType.MiniExam;
            }
            else if (ContainsJP(keyJP, "中間") || ContainsEN(keyEN, "midterm") || ContainsEN(keyJP, "midterm"))
            {
                return GradingType.MidtermExam;
            }
            else if (ContainsJP(keyJP, "期末") || ContainsEN(keyEN, "final") || ContainsEN(keyJP, "final"))
            {
                return GradingType.FinalExam;
            }
            else if (ContainsJP(keyJP, "ペーパー") || ContainsEN(keyEN, "paper") || ContainsEN(keyJP, "paper"))
            {
                return GradingType.MiniExam;
            }
            else if (ContainsJP(keyJP, "定期") || ContainsJP(keyJP, "試験") || ContainsJP(keyJP, "テスト")
                || ContainsEN(keyEN, "exam") || ContainsEN(keyJP, "exam")
                || ContainsEN(keyEN, "test") || ContainsEN(keyJP, "test"))
            {
                return GradingType.FinalExam;
            }
            else if (ContainsJP(keyJP, "発表") || ContainsJP(keyJP, "プレゼン") || ContainsEN(keyEN, "presentation") || ContainsEN(keyJP, "presentation"))
            {
                return GradingType.Presentation;
            }
            else if (ContainsJP(keyJP, "演習") || ContainsJP(keyJP, "実習") || ContainsJP(keyJP, "実技") || ContainsEN(keyEN, "exercise") || ContainsEN(keyJP, "exercise"))
            {
                return GradingType.Exercise;
            }
            else if (ContainsJP(keyJP, "実験") || ContainsEN(keyEN, "experiment") || ContainsEN(keyJP, "experiment"))
            {
                return GradingType.Experiment;
            }
            else if (ContainsJP(keyJP, "授業"))
            {
                return GradingType.Attitude;
            }
            else if ((ContainsJP(keyJP, "合計") && keyJP.Length == "合計".Length)
                || (ContainsJP(keyJP, "計") && keyJP.Length == "計".Length)
                || (ContainsJP(keyEN, "sum") && keyEN.Length == "sum".Length)
                || (ContainsJP(keyJP, "sum") && keyJP.Length == "sum".Length))
            {
                return GradingType.Sum;
            }

            return GradingType.Others;
        }

        #endregion
    }
}
