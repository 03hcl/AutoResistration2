namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// シラバスの情報取得時に使われる正規表現を格納します。
    /// </summary>
    /// <remarks> ※初期化時にメンバーの順序が影響します。注意してください。 </remarks>
    public static class SyllabusRegex
    {
        #region ConvertingPairs

        /// <summary>
        /// 全角から変換される半角に対応する数字のペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingNumberPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { '０', '0' },
            { '１', '1' },
            { '２', '2' },
            { '３', '3' },
            { '４', '4' },
            { '５', '5' },
            { '６', '6' },
            { '７', '7' },
            { '８', '8' },
            { '９', '9' },
        });

        /// <summary>
        /// 全角から変換される半角に対応するアルファベットのペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingAlphabetPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { 'Ａ', 'A' },
            { 'Ｂ', 'B' },
            { 'Ｃ', 'C' },
            { 'Ｄ', 'D' },
            { 'Ｅ', 'E' },
            { 'Ｆ', 'F' },
            { 'Ｇ', 'G' },
            { 'Ｈ', 'H' },
            { 'Ｉ', 'I' },
            { 'Ｊ', 'J' },
            { 'Ｋ', 'K' },
            { 'Ｌ', 'L' },
            { 'Ｍ', 'M' },
            { 'Ｎ', 'N' },
            { 'Ｏ', 'O' },
            { 'Ｐ', 'P' },
            { 'Ｑ', 'Q' },
            { 'Ｒ', 'R' },
            { 'Ｓ', 'S' },
            { 'Ｔ', 'T' },
            { 'Ｕ', 'U' },
            { 'Ｖ', 'V' },
            { 'Ｗ', 'W' },
            { 'Ｘ', 'X' },
            { 'Ｙ', 'Y' },
            { 'Ｚ', 'Z' },
            { 'ａ', 'a' },
            { 'ｂ', 'b' },
            { 'ｃ', 'c' },
            { 'ｄ', 'd' },
            { 'ｅ', 'e' },
            { 'ｆ', 'f' },
            { 'ｇ', 'g' },
            { 'ｈ', 'h' },
            { 'ｉ', 'i' },
            { 'ｊ', 'j' },
            { 'ｋ', 'k' },
            { 'ｌ', 'l' },
            { 'ｍ', 'm' },
            { 'ｎ', 'n' },
            { 'ｏ', 'o' },
            { 'ｐ', 'p' },
            { 'ｑ', 'q' },
            { 'ｒ', 'r' },
            { 'ｓ', 's' },
            { 'ｔ', 't' },
            { 'ｕ', 'u' },
            { 'ｖ', 'v' },
            { 'ｗ', 'w' },
            { 'ｘ', 'x' },
            { 'ｙ', 'y' },
            { 'ｚ', 'z' },
        });

        /// <summary>
        /// 全角から変換される半角に対応する(空白・括弧・キーワードの区切り文字に使用される記号を除く)記号のペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingSymbolPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { '！', '!' },
            { '“', '"' },
            { '”', '"' },
            { '＃', '#' },
            { '＄', '$' },
            { '％', '%' },
            { '＆', '&' },
            { '‘', '\'' },
            { '’', '\'' },
            { '＊', '*' },
            { '＋', '+' },
            { '－', '-' },
            { '＝', '=' },
            { '？', '?' },
            { '＠', '@' },
            { '＾', '^' },
            { '＿', '_' },
            { '｀', '`' },
            { '｜', '|' },
            { '￤', '|' },
            { '～', '~' },
        });

        /// <summary>
        /// 全角から変換される半角に対応する括弧のペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingParenthesisPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { '（', '(' },
            { '）', ')' },
            { '＜', '<' },
            { '＞', '>' },
            { '［', '[' },
            { '］', ']' },
            { '｛', '{' },
            { '｝', '}' },
        });

        /// <summary>
        /// 全角から変換される半角に対応するキーワードの区切り文字に使用される記号のペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingDelimiterPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { '，', ',' },
            { '、', ',' },
            { '．', '.' },
            { '。', '.' },
            { '／', '/' },
            { '：', ':' },
            { '；', ';' },
            { '＼', '\\' },
        });

        /// <summary>
        /// 全角から変換される半角に対応する空白のペアの一覧を表します。
        /// </summary>
        private static readonly ReadOnlyDictionary<char, char> ConvertingWhiteSpacePairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        {
            { '　', ' ' },
            { ' ', ' ' },
            { ' ', ' ' },
            { ' ', ' ' },
            { ' ', ' ' },
            { '​', ' ' },
        });

        #endregion
        
        #region 静的フィールド

        /////// <summary>
        /////// 全角から変換される半角に対応する英数字のペアの一覧を表します。
        /////// </summary>
        ////private static readonly ReadOnlyDictionary<char, char> ConvertingCharacterPairs = new ReadOnlyDictionary<char, char>(new Dictionary<char, char>()
        ////{
        ////    { '０', '0' },
        ////    { '１', '1' },
        ////    { '２', '2' },
        ////    { '３', '3' },
        ////    { '４', '4' },
        ////    { '５', '5' },
        ////    { '６', '6' },
        ////    { '７', '7' },
        ////    { '８', '8' },
        ////    { '９', '9' },
        ////    { 'Ａ', 'A' },
        ////    { 'Ｂ', 'B' },
        ////    { 'Ｃ', 'C' },
        ////    { 'Ｄ', 'D' },
        ////    { 'Ｅ', 'E' },
        ////    { 'Ｆ', 'F' },
        ////    { 'Ｇ', 'G' },
        ////    { 'Ｈ', 'H' },
        ////    { 'Ｉ', 'I' },
        ////    { 'Ｊ', 'J' },
        ////    { 'Ｋ', 'K' },
        ////    { 'Ｌ', 'L' },
        ////    { 'Ｍ', 'M' },
        ////    { 'Ｎ', 'N' },
        ////    { 'Ｏ', 'O' },
        ////    { 'Ｐ', 'P' },
        ////    { 'Ｑ', 'Q' },
        ////    { 'Ｒ', 'R' },
        ////    { 'Ｓ', 'S' },
        ////    { 'Ｔ', 'T' },
        ////    { 'Ｕ', 'U' },
        ////    { 'Ｖ', 'V' },
        ////    { 'Ｗ', 'W' },
        ////    { 'Ｘ', 'X' },
        ////    { 'Ｙ', 'Y' },
        ////    { 'Ｚ', 'Z' },
        ////    { 'ａ', 'a' },
        ////    { 'ｂ', 'b' },
        ////    { 'ｃ', 'c' },
        ////    { 'ｄ', 'd' },
        ////    { 'ｅ', 'e' },
        ////    { 'ｆ', 'f' },
        ////    { 'ｇ', 'g' },
        ////    { 'ｈ', 'h' },
        ////    { 'ｉ', 'i' },
        ////    { 'ｊ', 'j' },
        ////    { 'ｋ', 'k' },
        ////    { 'ｌ', 'l' },
        ////    { 'ｍ', 'm' },
        ////    { 'ｎ', 'n' },
        ////    { 'ｏ', 'o' },
        ////    { 'ｐ', 'p' },
        ////    { 'ｑ', 'q' },
        ////    { 'ｒ', 'r' },
        ////    { 'ｓ', 's' },
        ////    { 'ｔ', 't' },
        ////    { 'ｕ', 'u' },
        ////    { 'ｖ', 'v' },
        ////    { 'ｗ', 'w' },
        ////    { 'ｘ', 'x' },
        ////    { 'ｙ', 'y' },
        ////    { 'ｚ', 'z' },
        ////    { '！', '!' },
        ////    { '？', '?' },
        ////    { '－', '-' },
        ////    { '～', '~' },
        ////    { '＿', '_' },
        ////    { '‘', '\'' },
        ////    { '’', '\'' },
        ////    { '“', '"' },
        ////    { '”', '"' },
        ////    { '＃', '#' },
        ////    { '＄', '$' },
        ////    { '％', '%' },
        ////    { '＆', '&' },
        ////});

        ////A-Za-z0-9!?\\-~_'"Ａ-Ｚａ-ｚ０-９！？－～＿‘’“”
        ////{ '', '' },

        /// <summary>
        /// 英語のキーワードに使われる半角・全角の英数字と記号を表す文字列です。
        /// </summary>
        private static readonly string A = AlphanumericRegexString();

        /// <summary>
        /// 区切り文字を表す文字列です。
        /// </summary>
        private static readonly string D = CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ConvertingDelimiterPairs);
        ////private static readonly string D = ",、，.。．:：;；/／\\\\＼";

        /// <summary>
        /// 空白を表す文字列です。
        /// </summary>
        private static readonly string W = WhiteSpaceRegexString();
        ////private static readonly string W = "\\s　";

        /// <summary>
        /// 左括弧を表す文字列です。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "絶対的な定数ではないため const は不適切だから。")]
        private static readonly string L = "<\\(\\[\\{＜（｛［〔「『【〈《";

        /// <summary>
        /// 右括弧を表す文字列です。
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "絶対的な定数ではないため const は不適切だから。")]
        private static readonly string R = ">\\)\\]\\}＞）｝］〕」』】〉》";

        /// <summary>
        /// <see cref="A"/> と <see cref="W"/> を表す文字列です。
        /// </summary>
        private static readonly string AW = A + W;

        /// <summary>
        /// <see cref="D"/> と <see cref="W"/> を表す文字列です。
        /// </summary>
        private static readonly string DW = D + W;

        /// <summary>
        /// <see cref="L"/> と <see cref="R"/> を表す文字列です。
        /// </summary>
        private static readonly string LR = L + R;

        /// <summary>
        /// <see cref="AW"/> と <see cref="LR"/> を表す文字列です。
        /// </summary>
        private static readonly string AWLR = AW + LR;

        /// <summary>
        /// <see cref="DW"/> と <see cref="LR"/> を表す文字列です。
        /// </summary>
        private static readonly string DWLR = DW + LR;

        /////// <summary>
        /////// (シラバスのキーワードに使われる) 全角と半角を合わせた英数字の一覧を表す文字列を取得します。
        /////// </summary>
        ////private static string AlphanumericRegexString { get; }
        ////    ////= new string(Enumerable.Concat(ConvertingCharacters.Keys, ConvertingCharacters.Values).ToArray());

        /////// <summary>
        /////// (シラバスのキーワードに使われる) 区切り文字の一覧を表す文字列を取得します。
        /////// </summary>
        ////private static string DelimiterRegexString { get; }

        /////// <summary>
        /////// (シラバスのキーワードに使われる) スペースの一覧を表す文字列を取得します。
        /////// </summary>
        ////private static string WhiteSpaceRegexString { get; }

        /////// <summary>
        /////// (シラバスのキーワードに使われる) 左括弧の一覧を表す文字列を取得します。
        /////// </summary>
        ////private static string LeftParenthesisRegexString { get; }

        /////// <summary>
        /////// (シラバスのキーワードに使われる) 右括弧の一覧を表す文字列を取得します。
        /////// </summary>
        ////private static string RightParenthesisRegexString { get; }

        /// <summary>
        /// 括弧を表す文字列です。
        /// </summary>
        private static readonly string P = ParenthesisRegexString();

        /// <summary>
        /// ASCII文字を表す文字列です。
        /// </summary>
        private static readonly string ASCII = A + P + DW;
        
        /// <summary>
        /// 数字を表す文字列です。
        /// </summary>
        private static readonly string N = CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ConvertingNumberPairs);

        /// <summary>
        /// <see cref="N"/> と <see cref="D"/> を表す文字列です。
        /// </summary>
        private static readonly string ND = N + D;

        #endregion

        #region プロパティ
        
        /// <summary>
        /// 検索画面のコンボボックスの項目名を表す正規表現を取得します。
        /// </summary>
        public static Regex ComboBoxItem { get; } = new Regex("^(?<jpValue>.*?)\\s+(?<enValue>[" + ASCII + "]*)$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// シラバスの授業科目名を表す正規表現を取得します。
        /// </summary>
        public static Regex Title { get; } = new Regex("^(?<jp>.*?)[［\\[](?<en>.*?)[］\\]]", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// シラバスの担当教員名を表す正規表現を取得します。
        /// </summary>
        public static Regex Instructor { get; } = new Regex("(?<jp>.*?)[［\\[](?<en>.*?)[］\\]](、|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 科目の種類またはシラバスの詳細分類を表す正規表現を取得します。
        /// </summary>
        public static Regex SubjectTypeAndSyllabusSchoolOrDivision { get; } = new Regex("^(?<jp>.*?)<font[^>]*font-en[^>]*>(?<en>.*?)</font>.*?$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// シラバスのキーワードで英語タイプを表す基本の正規表現を取得します。
        /// </summary>
        public static Regex MultilineKeywordsEN { get; } = new Regex(
            "^[" + DW + "]*(?<body>([" + AW + "]+?([" + L + "]+[^" + R + "]*?[" + R + "]+[" + AW + "]*)*[" + D + "]+)*[" + AW + "]+?([" + L + "]+[^" + R + "]*?[" + R + "]+[" + AW + "]*)*)[" + DW + "]*$");
        ////"^[dw]*(?<body>([aw]+?([l]+[^r]*?[r]+[aw]*)*[d]+)*[aw]+?([l]+[^r]*?[r]+[aw]*)*)[dw]*$"

        ////public static Regex MultilineKeywordsEN { get; } = new Regex(
        ////    "^[" + DW + "]*(?<body>(([" + AW + "]*?|[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)[" + D + "]+)*([" + AW + "]*?|[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*))[" + DW + "]*$");
        ////////"^[dw]*(?<body>(([aw]*?|[aw]+?[l]+[^r]*?[r]+[w]*)[d]+)*([aw]*?|[aw]+?[l]+[^r]*?[r]+[w]*))[dw]*$"

        ////public static Regex MultilineKeywordsEN { get; } = new Regex(
        ////            "^[" + DW + "]*(?<body>([" + AWLR + "]*?[" + D + "]+)*[" + AWLR + "]*?)[" + DW + "]*$");
        ////////"^[dw]*(?<body>([awlr]*?[d]+)*[awlr]*?)[dw]*$"

        /// <summary>
        /// シラバスのキーワードで日本語タイプを表す基本の正規表現を取得します。
        /// </summary>
        public static Regex MultilineKeywordsJP { get; } = new Regex(
            "^[" + D + "]*(?<body>([^" + D + "]*?[" + D + "]+)*[^" + D + "]*?)[" + DW + "]*$");
        ////"^[d]*(?<body>([^d]*?[d]+)*[^d]*?)[dw]*$"

        /// <summary>
        /// シラバスのキーワードで単一行のタイプ1を表す正規表現を取得します。
        /// </summary>
        public static Regex SinglelineKeywords1 { get; } = new Regex(
                    "^[" + DW + "]*[" + L + "\"]?(?<jpBody>([^" + DWLR + "]*?[" + DW + "]+)*[^" + DWLR + "]*?[" + DW + "]*)[" + R + "\"]?[" + L + "\"][" + DW + "]*(?<enBody>([" + AW + "]*?[" + D + "]+)*[" + AW + "]*?[" + D + "]*)[" + R + "\"][" + DW + "]*$");
        ////"^[dw]*[l\"]?(?<jpBody>([^dwlr]*?[dw]+)*[^dwlr]*?[dw]*)[r\"]?[l\"][dw]*(?<enBody>([aw]*?[d]+)*[aw]*?[d]*)[r\"][dw]*$"

        /// <summary>
        /// シラバスのキーワードで単一行のタイプ2を表す正規表現を取得します。
        /// </summary>
        public static Regex SinglelineKeywords2 { get; } = new Regex(
                    "^[" + DW + "+]*(?<body>(([^" + D + "]*?[" + D + "]+[" + A + "]+[" + AW + "]*|[" + AW + "]*?)[" + D + "]+[" + W + "]+)*([^" + D + "]*?[" + D + "]+[" + A + "]+[" + AW + "]*|[" + AW + "]*?))[" + DW + "]*$");
        ////"^[dw]*(?<body>(([^d]*?[d]+[a]+[aw]*|[aw]*?)[d]+[w]+)*([^d]*?[d]+[a]+[aw]*|[aw]*?))[dw]*$"

        /// <summary>
        /// シラバスのキーワードで単一行のタイプ3を表す正規表現を取得します。
        /// </summary>
        public static Regex SinglelineKeywords3 { get; } = new Regex(
            "^[" + DW + "]*(?<jpBody>([^" + D + "]*?[" + D + "]+)*?[^" + D + "]*?)[" + DW + "]+(?<enBody>(([" + AW + "]*?|[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)[" + D + "]+)*([" + AW + "]*?|[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*))[" + DW + "]*$");
        ////"^[dw]*(?<jpBody>([^d]*?[d]+)*?[^d]*?)[dw]+(?<enBody>(([aw]*?|[aw]+?[l]+[^r]*?[r]+[w]*)[d]+)*([aw]*?|[aw]+?[l]+[^r]*?[r]+[w]*))[dw]*$"

        ////public static Regex SinglelineKeywords3 { get; } = new Regex(
        ////    "^[" + DW + "]*(?<jpBody>([^" + D + "]*?[" + D + "]+)*?[^" + D + "]*?)[" + DW + "]+(?<enBody>([" + AW + "]*?[" + D + "]+)*[" + AW + "]*?)[" + DW + "]*$");
        ////////"^[dw]*(?<jpBody>([^d]*?[d]+)*?[^d]*?)[dw]+(?<enBody>(([aw]*?)[d]+)*[aw]*?)[dw]*$"

        ////public static Regex SinglelineKeywords3 { get; } = new Regex(
        ////            "^[" + DW + "]*(?<jpBody>([^" + D + "]*?[" + D + "]+)*[^" + D + "]*?[" + D + "]*)[" + W + "]+(?<enBody>([" + AW + "]*?[" + D + "]+)*[" + AW + "]*?)[" + DW + "]*$");
        ////////"^[dw]*(?<jpBody>([^d]*?[d]+)*[^d]*?[d]*)[w]+(?<enBody>(([aw]*?)[d]+)*[aw]*?)[dw]*$"

        /// <summary>
        /// シラバスの日英混合のキーワードを表す正規表現を取得します。
        /// </summary>
        public static Regex KeywordMixed { get; } = new Regex("^(?<jp>[^" + LR + "]*?)[" + L + "]+(?<en>[" + AW + "]*?)[" + R + "]+$");
        ////"^(?<jp>[^lr]*?)[l]+(?<en>[aw]*?)[r]+$"

        /// <summary>
        /// キーワード自体を表す日本語の文字列の正規表現を取得します。
        /// </summary>
        public static Regex KeywordOwnStringJP { get; } = new Regex("キーワー[ドズ]", RegexOptions.IgnoreCase);

        /// <summary>
        /// キーワード自体を表す英語の文字列の正規表現を取得します。
        /// </summary>
        public static Regex KeywordOwnStringEN { get; } = new Regex("key\\s*?words?", RegexOptions.IgnoreCase);

        /// <summary>
        /// 改行の正規表現を取得します。
        /// </summary>
        public static Regex NewLine { get; } = new Regex("(?!" + Environment.NewLine + "|\n)(?<line>.+?)(" + Environment.NewLine + "|\n|$)", RegexOptions.Singleline);

        /// <summary>
        /// trタグを表す正規表現を取得します。
        /// </summary>
        public static Regex TableRow { get; } = new Regex("<tr[^<]*>(.*?)</tr>\\s*(?=<tr|</table|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////public static Regex TableRowRegex { get; } = new Regex("<tr[^<]*>(((?!<tr).)*?)</tr>\\s*(?=<tr|</table|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////public static Regex TableRowRegex { get; } = new Regex("<tr[^<]*>(.*?)</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// tdタグを表す正規表現を取得します。
        /// </summary>
        public static Regex TableData { get; } = new Regex("<td[^<]*>(.*?)</td>\\s*(?=<td|</tr|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////public static Regex TableDataRegex { get; } = new Regex("<td[^<]*>(((?!<td).)*?)</td>\\s*(?=<td|</tr|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////public static Regex TableDataRegex { get; } = new Regex("<td[^<]*>(.*?)</td>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 単一行形式の評価の割合を表す正規表現を取得します。
        /// </summary>
        public static Regex SinglelineGradingRateString { get; }
            = new Regex("^\\s*<td[^<]*>.*?<table[^<]*>.*?<tr[^<]*>(.*?)\\s*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            ////= new Regex("^\\s*<td[^<]*>.*?<table[^<]*>.*?<tr[^<]*>(.*?)</tr>.*?</table>.*?</td>\\s*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 評価の割合の日本語の項目名を表す正規表現を取得します。
        /// </summary>
        public static Regex GradingRateKey { get; } = new Regex("^(?<jp>.*?)[" + DW + "]+(?<en>[" + ASCII + "]*)$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 評価の割合の英語の項目名を表す正規表現を取得します。
        /// </summary>
        public static Regex GradingRateENKey { get; } = new Regex("^[" + ASCII + "]*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 評価の割合の値を表す正規表現を取得します。
        /// </summary>
        public static Regex GradingRatePercentage { get; } = new Regex("^約?([" + ND + "]*)[" + W + "]*%$", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 評価の割合において、日本語の項目名とその他を区別する正規表現を取得します。
        /// </summary>
        public static Regex GradingRate { get; }
            = new Regex("(^|[" + DW + "]+)・?(?<jp>.*?)(?<en>[" + A + DWLR + "約]+?[" + W + "]*[%％])", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(^|[dw]+)・?(?<jp>.*?)(?<en>[adwlr約]+?[w]*[%％])

        /// <summary>
        /// 評価の割合において、英語の項目名と割合の値を区別する正規表現を取得します。
        /// </summary>
        public static Regex GradingRate2 { get; }
            = new Regex("(?<en>[" + DW + "]*[" + AWLR + "]*?)[" + D + "]*[" + L + "]*[" + W + "約]*(?<percentage>[" + ND + "]+)[" + W + "]*[%％点]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(?<en>[dw]*[awlr]*?)[dw]*[l]*[w約]*(?<percentage>[nd]+)[w]*[%％点]

        ////public static Regex GradingRateRegex2 { get; }
        ////    = new Regex("(?<en>[" + DW + "]*[" + AWLR + "]*?)[" + D + "]*[" + L + "]*[" + W + "]*(?<percentage>[" + N + D + "]+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(?<en>[dw]*[awlr]*?)[dw]*[l]*[w]*(?<percentage>[nd]+)

        ////public static Regex GradingRateRegex2 { get; }
        ////    = new Regex("(?<en>[" + DW + "]*[" + AWLR + "]*?)[" + DW + "]*(?<percentage>[" + N + D + "]+)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(?<en>[dw]*[awlr]*?)[dw]*(?<percentage>[nd]+)

        ////public static Regex GradingRateRegex { get; }
        ////    = new Regex("(^|[" + DW + "]+)・?(?<jp>.*?)(?<en>[" + DW + "]*[" + AWLR + "]*)?[" + DW + "]*(?<percentage>[" + N + D + "]+)[" + W + "]*[%点]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(^|[dw]+)・?(?<jp>.*?)(?<en>[dw]*[awlr]*)?[dw]*(?<percentage>[nd]+)[w]*[%点]

        ////    = new Regex("(^|[" + DW + "]+)・?(?<jp>((?!\\n).)*?)(?<en>[" + DW + "]*[" + AWLR + "]*)?[" + DW + "]*(?<percentage>[" + N + D + "]+)[" + W + "]*[%点]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(^|[dw]+)・?(?<jp>((?!\\n).)*?)(?<en>[dw]*[awlr]*)?[dw]*(?<percentage>[nd]+)[w]*[%点]

        /// <summary>
        /// 行全体に当てはまる評価の割合において、日本語の項目名とその他を区別する正規表現を取得します。
        /// </summary>
        public static Regex SinglelineGradingRate { get; }
            = new Regex("^・?(?<jp>.*?)(?<en>((?![" + R + "]+?)[" + A + DWLR + "約]+?)[" + W + "]*([%％]|点([" + DW + "]*[" + L + "\"]+[^" + R + "]*?[" + R + "\"]+)?))[" + DW + "]*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////^・?(?<jp>.*?)(?<en>((?![r]+?)[adwlr約]+?)[w]*([%％]|点([dw]*[l\"]+[^r]*?[r\"]+)?))[dw]*$

        ////public static Regex SinglelineGradingRate { get; }
        ////    = new Regex("^・?(?<jp>.*?)(?<en>[" + A + DWLR + "約]+?[" + W + "]*([%％]|点([" + DW + "]*[" + L + "\"]+[^" + R + "]*?[" + R + "\"]+)?))[" + DW + "]*$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////////^・?(?<jp>.*?)(?<en>[adwlr約]+?[w]*([%％]|点([dw]*[l\"]+[^r]*?[r\"]+)?))[dw]*$

        /// <summary>
        /// 行全体に当てはまる評価の割合において、英語の項目名と割合の値を区別する正規表現を取得します。
        /// </summary>
        public static Regex SinglelineGradingRate2 { get; }
            = new Regex("^・?(?<en>[" + A + DWLR + "]*?)[" + DW + "]*[" + W + "約]*(?<percentage>[" + ND + "]+)[" + W + "]*([%％]|点([" + DW + "]*[" + L + "\"]+[^" + R + "]*?[" + R + "\"]+)?)$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////^・?(?<en>[adwlr]*?)[dw]*[w約]*(?<percentage>[nd]+)[w]*([%％]|点([dw]*[l\"]+[^r]*?[r\"]+)?)$

        ////public static Regex SinglelineGradingRate2 { get; }
        ////    = new Regex("(?<en>[" + DW + "]*[" + AWLR + "]*)[" + DW + "]*[" + L + "]*[" + W + "約]*(?<percentage>[" + ND + "]+)[" + W + "]*([%％]|点([" + DW + "]*[" + L + "\"]+[^" + R + "]*?[" + R + "\"]+)?)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////////(?<en>[dw]*[awlr]*)[dw]*[l]*[w約]*(?<percentage>[nd]+)[w]*([%％]|点([dw]*[l\"]+[^r]*?[r\"]+)?)

        /// <summary>
        /// 行全体に当てはまる評価の割合のうち、特殊パターン1を表す正規表現を取得します。
        /// </summary>
        public static Regex SpecialSinglelineGradingRate { get; } = new Regex("^([" + ND + "]+[" + W + "]*[%％][" + AWLR + "]*[" + DW + "]+)*[" + ND + "]+[" + W + "]*[%％][" + AWLR + "]*?$", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////^([nd]+[w]*[%％][awlr]*[dw]+)*[nd]+[w]*[%％][awlr]*?$

        /// <summary>
        /// 行全体に当てはまる評価の割合のうち、特殊パターン1の英語の項目名と割合の値を区別する正規表現を取得します。
        /// </summary>
        public static Regex SpecialSinglelineGradingRate2 { get; } = new Regex("(?<percentage>[" + ND + "]+)+[" + W + "]*[%％](?<en>[" + AWLR + "]*)([" + DW + "]+|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        ////(?<percentage>[nd]+)+[w]*[%％](?<en>[awlr]*)([dw]+|$)

        /// <summary>
        /// 区切り文字と空白文字の一覧を表す <see cref="char"/> 配列を取得します。
        /// </summary>
        public static char[] DelimiterAndWhiteSpaceCharacters { get; } = (D + CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ConvertingWhiteSpacePairs)).ToCharArray();

        /// <summary>
        /// 左括弧の一覧とダブルクォーテーションを表す <see cref="char"/> を列挙します。
        /// </summary>
        public static char[] LeftParenthesisAndDoubleQuoteCharacters { get; } = (L + "\"").ToArray();

        /// <summary>
        /// 右括弧の一覧とダブルクォーテーションを表す <see cref="char"/> を列挙します。
        /// </summary>
        public static char[] RightParenthesisAndDoubleQuoteCharacters { get; } = (R + "\"").ToArray();

        #endregion

        #region プロパティ(private)

        /// <summary>
        /// 全角から変換される半角に対応するキーワードに含まれる英数字のペアの一覧を取得します。
        /// </summary>
        private static ReadOnlyDictionary<char, char> ConvertingKeywordCharacters { get; }
            = new ReadOnlyDictionary<char, char>(ConvertingNumberPairs.Concat(ConvertingAlphabetPairs).Concat(ConvertingSymbolPairs).ToDictionary(c => c.Key, c => c.Value));

        /// <summary>
        /// 全角から変換される半角に対応するASCII文字のペアの一覧を取得します。
        /// </summary>
        private static ReadOnlyDictionary<char, char> ConvertingAsciiCharacters { get; } = new ReadOnlyDictionary<char, char>(
            ConvertingNumberPairs
            .Concat(ConvertingAlphabetPairs)
            .Concat(ConvertingSymbolPairs)
            .Concat(ConvertingParenthesisPairs)
            .Concat(ConvertingDelimiterPairs)
            .Concat(ConvertingWhiteSpacePairs)
            .ToDictionary(c => c.Key, c => c.Value));

        /// <summary>
        /// シラバスのキーワードで日本語タイプ1を表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword1JP { get; } = new Regex("(?<keyword>[^" + D + "]*?)([" + DW + "]+|$)");
        ////"(?<keyword>[^d]*?)([dw]+|$)"

        /// <summary>
        /// シラバスのキーワードで英語タイプ1を表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword1EN { get; } = new Regex("(?<keyword>[" + AW + "]*?)([" + D + "]+|$)");
        ////"(?<keyword>[aw]*?)([d]+|$)"

        /// <summary>
        /// シラバスのキーワードでタイプ2を表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword2 { get; } = new Regex(
            "((?<jpKeyword>[^" + D + "]*?)[" + D + "]+(?<enKeyword>[" + A + "]+[" + AWLR + "]*)|(?<keyword>[" + AWLR + "]*?))([" + D + "]+[" + W + "]+|$)");
        ////"((?<jpKeyword>[^d]*?)[d]+(?<enKeyword>[a]+[awlr]*)|(?<keyword>[awlr]*?))([d]+[w]+|$)"

        /// <summary>
        /// シラバスのキーワードで日本語タイプを表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword3JP { get; } = new Regex("(?<keyword>[^" + D + "]+?)([" + DW + "]+|$)");
        ////"(?<keyword>[^d]+?)([dw]+|$)" // Keyword1JPと同じ?

        ////private static Regex Keyword3JP { get; } = new Regex("(?<keyword>[^" + D + "]*?|[^" + D + "d]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)([" + D + "]+|$)");
        ////////"(?<keyword>[^d]*?|[^d]+?[l]+[^r]*?[r]+[w]*)([d]+|$)"

        /// <summary>
        /// シラバスのキーワードで英語タイプを表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword3EN { get; } = new Regex("(?<keyword>[" + AW + "]+?([" + L + "]+[^" + R + "]*?[" + R + "]+[" + AW + "]*)*)([" + D + "]+|$)");
        ////"(?<keyword>[aw]+?([l]+[^r]*?[r]+[aw]*)*)([d]+|$)"

        ////private static Regex Keyword3EN { get; } = new Regex("(?<keyword>[" + AWLR + "]+?)([" + D + "]+|$)");
        ////////"(?<keyword>[awlr]+?)([d]+|$)"

        ////private static Regex Keyword3EN { get; } = new Regex("(?<keyword>[" + AW + "]*?|[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)([" + D + "]+|$)");
        ////////"(?<keyword>[aw]*?|[aw]+?[l]+[^r]*?[r]+[w]*)([d]+|$)"

        /// <summary>
        /// シラバスのキーワードで括弧による注釈のついた日本語タイプを表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword3JPWithAnnot { get; } = new Regex("(?<keyword>[^" + D + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)([" + DW + "]+|$)");
        ////"(?<keyword>[^d]+?[l]+[^r]*?[r]+[w]*)([dw]+|$)"

        /// <summary>
        /// シラバスのキーワードで括弧による注釈のついた英語タイプを表す正規表現を取得します。
        /// </summary>
        private static Regex Keyword3ENWithAnnot { get; } = new Regex("(?<keyword>[" + AW + "]+?[" + L + "]+[^" + R + "]*?[" + R + "]+[" + W + "]*)([" + D + "]+|$)");
        ////"(?<keyword>[aw]+?[l]+[^r]*?[r]+[w]*)([d]+|$)"

        #endregion

        #region メソッド

        /// <summary>
        /// キーワードに含まれる英数字を全角から半角に変換します。
        /// </summary>
        /// <param name="origin"> 元の文字列。 </param>
        /// <returns> 変換後の文字列。 </returns>
        public static string ConvertKeyword(string origin) => new string(origin.Select(c => ConvertingKeywordCharacters.ContainsKey(c) ? ConvertingKeywordCharacters[c] : c).ToArray());

        /// <summary>
        /// ASCII文字で表すことのできる文字を全角から半角に変換します。
        /// </summary>
        /// <param name="origin"> 元の文字列。 </param>
        /// <returns> 変換後の文字列。 </returns>
        public static string ConvertAscii(string origin) => new string(origin.Select(c => ConvertingAsciiCharacters.ContainsKey(c) ? ConvertingAsciiCharacters[c] : c).ToArray());

        /// <summary>
        /// シラバスのキーワードで日本語タイプ1に一致する文字列を取得します。
        /// </summary>
        /// <param name="input"> 一致する対象を検索する文字列。 </param>
        /// <returns> 一致する文字列。</returns>
        public static IEnumerable<string> MatchKeyword1JP(string input)
        {
            for (var match = Keyword1JP.Match(input);
                match.Success;
                match = match.NextMatch())
            {
                yield return match.Groups["keyword"].Value;
            }
        }

        /// <summary>
        /// シラバスのキーワードで英語タイプ1に一致する文字列を取得します。
        /// </summary>
        /// <param name="input"> 一致する対象を検索する文字列。 </param>
        /// <returns> 一致する文字列。</returns>
        public static IEnumerable<string> MatchKeyword1EN(string input)
        {
            for (var match = Keyword1EN.Match(input);
                match.Success;
                match = match.NextMatch())
            {
                yield return match.Groups["keyword"].Value;
            }
        }

        /// <summary>
        /// シラバスのキーワードでタイプ2に一致する文字列を取得します。
        /// </summary>
        /// <param name="input"> 一致する対象を検索する文字列。 </param>
        /// <returns> 一致する文字列。</returns>
        public static IEnumerable<SyllabusKeyword> MatchKeyword2(string input)
        {
            for (var match = Keyword2.Match(input);
                match.Success;
                match = match.NextMatch())
            {
                ////if (string.IsNullOrEmpty(match.Groups["keyword"].Value))
                if (match.Groups["keyword"].Success)
                {
                    yield return new SyllabusKeyword(match.Groups["keyword"].Value, match.Groups["keyword"].Value);
                    ////yield return Tuple.Create(match.Groups["keyword"].Value, match.Groups["keyword"].Value);
                    ////jp.Add(match.Groups["keyword"].Value);
                    ////en.Add(match.Groups["keyword"].Value);
                }
                else
                {
                    yield return new SyllabusKeyword(match.Groups["jpKeyword"].Value, match.Groups["enKeyword"].Value);
                    ////yield return Tuple.Create(match.Groups["jpKeyword"].Value, match.Groups["enKeyword"].Value);
                    ////    jp.Add(match.Groups["jpKeyword"].Value);
                    ////    en.Add(match.Groups["enKeyword"].Value);
                }
            }
        }

        /// <summary>
        /// シラバスのキーワードで日本語タイプ3に一致する文字列を取得します。
        /// </summary>
        /// <param name="input"> 一致する対象を検索する文字列。 </param>
        /// <returns> 一致する文字列。</returns>
        public static IEnumerable<string> MatchKeyword3JP(string input)
        {
            var match = Keyword3JP.Match(input);

            var annotMatch = Keyword3JPWithAnnot.Match(input, match.Index);

            while (match.Success)
            {
                if (annotMatch.Success && annotMatch.Index == match.Index)
                {
                    yield return annotMatch.Groups["keyword"].Value;
                    match = Keyword3JP.Match(input, match.Index + annotMatch.Length);
                    annotMatch = annotMatch.NextMatch();
                }
                else
                {
                    yield return match.Groups["keyword"].Value;
                    match = match.NextMatch();
                }
            }
        }

        /// <summary>
        /// シラバスのキーワードで英語タイプ3に一致する文字列を取得します。
        /// </summary>
        /// <param name="input"> 一致する対象を検索する文字列。 </param>
        /// <returns> 一致する文字列。</returns>
        public static IEnumerable<string> MatchKeyword3EN(string input)
        {
            var match = Keyword3EN.Match(input);

            var annotMatch = Keyword3ENWithAnnot.Match(input, match.Index);

            while (match.Success)
            {
                if (annotMatch.Success && annotMatch.Index == match.Index)
                {
                    yield return annotMatch.Groups["keyword"].Value;
                    match = Keyword3EN.Match(input, match.Index + annotMatch.Length);
                    annotMatch = annotMatch.NextMatch();
                }
                else
                {
                    yield return match.Groups["keyword"].Value;
                    match = match.NextMatch();
                }
            }
        }

        #endregion

        #region メソッド(private)

        /// <summary>
        /// <see cref="ReadOnlyDictionary{char, char}"/> から、その Key と Value に含む全ての <see cref="char"/> を列挙した文字列を作成します。
        /// </summary>
        /// <param name="dictionary"> <see cref="ReadOnlyDictionary{char, char}"/> . </param>
        /// <returns> 作成された文字列。 </returns>
        private static string CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ReadOnlyDictionary<char, char> dictionary)
        {
            var builder = new StringBuilder();

            foreach (var c in dictionary.Values.Distinct())
            {
                builder.Append(c);
            }

            foreach (var c in dictionary.Keys)
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        /// <summary>
        /// <see cref="A"/> を初期化します。
        /// </summary>
        /// <returns> <see cref="A"/>. </returns>
        private static string AlphanumericRegexString()
        {
            var builder = new StringBuilder();

            builder.Append(CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ConvertingNumberPairs));
            builder.Append(CreateCharacterListStringFromReadOnlyDictionaryKeyAndValue(ConvertingAlphabetPairs));

            foreach (var c in ConvertingSymbolPairs.Values.Distinct())
            {
                if (c == '-')
                {
                    builder.Append('\\');
                }

                builder.Append(c);
            }

            foreach (var c in ConvertingSymbolPairs.Keys)
            {
                builder.Append(c);
            }

            builder.Append('・');

            return builder.ToString();
        }

        /// <summary>
        /// <see cref="W"/> を初期化します。
        /// </summary>
        /// <returns> <see cref="W"/>. </returns>
        private static string WhiteSpaceRegexString()
        {
            var builder = new StringBuilder("\\s");

            foreach (var c in ConvertingWhiteSpacePairs.Keys)
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        /// <summary>
        /// <see cref="P"/> を初期化します。
        /// </summary>
        /// <returns> <see cref="P"/>. </returns>
        private static string ParenthesisRegexString()
        {
            var builder = new StringBuilder("\\s");

            foreach (var c in ConvertingParenthesisPairs.Values.Distinct())
            {
                if (c == '[' || c == ']')
                {
                    builder.Append('\\');
                }

                builder.Append(c);
            }

            foreach (var c in ConvertingParenthesisPairs.Keys)
            {
                builder.Append(c);
            }

            return builder.ToString();
        }

        #endregion
    }
}
