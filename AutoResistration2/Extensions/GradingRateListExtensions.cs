namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// <see cref="GradingRate"/> のリストに対する拡張メソッドを定義します。
    /// </summary>
    public static class GradingRateListExtensions
    {
        /// <summary>
        /// 文字列からシラバスの評価の割合を表すデータの詳細を検索して <see cref="ICollection&lt;GradingRate&gt;"/> に追加します。
        /// </summary>
        /// <param name="details"> 追加を行う <see cref="ICollection&lt;GradingRate&gt;"/> 。 </param>
        /// <param name="text"> 詳細を検索する文字列。 </param>
        /// <param name="gradingRate"> 日本語の項目名とその他を区別する正規表現。 </param>
        /// <param name="gradingRate2"> 英語の項目名と割合の値を区別する正規表現。 </param>
        public static void AddDetails(this ICollection<GradingRate> details, string text, Regex gradingRate, Regex gradingRate2)
        {
            if (gradingRate == null)
            {
                throw new ArgumentNullException(nameof(gradingRate), "正規表現が指定されていません。");
            }
            else if (gradingRate2 == null)
            {
                throw new ArgumentNullException(nameof(gradingRate2), "正規表現が指定されていません。");
            }

            for (var rateMatch = gradingRate.Match(text);
                rateMatch.Success;
                rateMatch = rateMatch.NextMatch())
            {
                string keyJP = rateMatch.Groups["jp"].Value.Trim();

                for (var match = gradingRate2.Match(rateMatch.Groups["en"].Value.Trim());
                    match.Success;
                    match = match.NextMatch())
                {
                    ////string keyEN = match.Groups["en"].Value.Trim();
                    string keyEN = match.Groups["en"].Value.Trim(SyllabusRegex.DelimiterAndWhiteSpaceCharacters);
                    decimal? percentage = MainWindow.TryParseNullableDecimal(SyllabusRegex.ConvertAscii(match.Groups["percentage"].Value));

                    if ((!string.IsNullOrEmpty(keyJP) || !string.IsNullOrEmpty(keyEN)) && percentage.HasValue)
                    {
                        details.AddOrFix(new GradingRate(keyJP, string.IsNullOrEmpty(keyEN) ? null : keyEN, percentage));
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="ICollection&lt;GradingRate&gt;"/> に項目の追加を試みます。
        /// 日本語か英語のいずれかの項目名が同じで、割合を表す値も同じか未設定である要素が存在する場合は、
        /// 追加を行わないか、その要素を削除して新しい要素を追加します。
        /// </summary>
        /// <param name="collection"> 追加を試みる <see cref="ICollection&lt;GradingRate&gt;"/> 。 </param>
        /// <param name="item"> 割合を表す値。 </param>
        /// <returns> 単純に要素を追加した場合は 1 、要素を追加しなかった場合は 0 、要素を削除して追加した場合は -1 。 </returns>
        public static int AddOrFix(this ICollection<GradingRate> collection, GradingRate item)
        {
            if (collection == null || item == null)
            {
                throw new ArgumentNullException(nameof(collection), "引数が null です。");
            }

            if (item.GradingType == GradingType.Sum)
            {
                return 0;
            }

            var duplicated = collection.FirstOrDefault(
                r => item.GradingType == r.GradingType && item.GradingType != GradingType.Others && (item.Percentage == r.Percentage || !item.Percentage.HasValue || !r.Percentage.HasValue));

            if (duplicated == null
                || (item.Percentage != duplicated.Percentage && item.Percentage.HasValue && duplicated.Percentage.HasValue)
                || (item.JPKey != duplicated.JPKey && !string.IsNullOrEmpty(item.JPKey) && !string.IsNullOrEmpty(duplicated.JPKey)))
            {
                collection.Add(item);
                return 1;
            }

            string keyJP;
            string keyEN;
            decimal? percentage;

            if (!string.IsNullOrEmpty(duplicated.JPKey))
            {
                keyJP = duplicated.JPKey;
            }
            else
            {
                keyJP = item.JPKey;
            }

            if (!string.IsNullOrEmpty(duplicated.ENKey))
            {
                keyEN = duplicated.ENKey;
            }
            else
            {
                keyEN = item.ENKey;
            }

            if (duplicated.Percentage.HasValue)
            {
                percentage = duplicated.Percentage;
            }
            else
            {
                percentage = item.Percentage;
            }

            if (keyJP == duplicated.JPKey && keyEN == duplicated.ENKey && percentage == duplicated.Percentage)
            {
                if (string.IsNullOrEmpty(keyJP))
                {
                    if (string.IsNullOrEmpty(keyEN) || (item.ENKey != duplicated.ENKey && !string.IsNullOrEmpty(item.ENKey)))
                    {
                        collection.Add(item);
                        return 1;
                    }
                }

                return 0;
            }

            if (string.IsNullOrEmpty(duplicated.JPKey) && string.IsNullOrEmpty(item.JPKey))
            {
                collection.Add(item);
                return 1;
            }

            ////var index = details.IndexOf(duplicated);
            ////details[index] = new GradingRate(keyJP, keyEN, percentage);
            collection.Remove(duplicated);
            collection.Add(new GradingRate(keyJP, keyEN, percentage, item.GradingType));
            return -1;
        }
    }
}
