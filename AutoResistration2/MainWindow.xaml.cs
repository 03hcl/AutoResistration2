namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using System.Xml;
    using System.Xml.Serialization;

    using AcanthusPortalLibrary;

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 依存関係プロパティ

        /// <summary>
        /// <see cref="LogText"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty LogTextProperty =
            DependencyProperty.Register(nameof(LogText), typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        #endregion

        #region コンストラクタ

        /// <summary>
        /// <see cref="MainWindow"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        [SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "コンストラクタでのインスタンス生成であるため。")]
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.Config = new ConfigWindow(this);
            this.Detail = new DetailWindow(this);
            this.Filter = new FilterWindow(this);

            ////this.RecreateHttpClient();

            this.StudentInformationService = new StudentInformationService();

            this.LogText = string.Empty;
            ////this.LogWriter = new StreamWriter("log_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            this.CourseListView.ItemContainerStyle.Setters.Add(new Setter(
                ListViewItem.HeightProperty,
                new Binding(nameof(ConfigWindow.CourseHeight)) { Source = this.Config }));

            this.View = CollectionViewSource.GetDefaultView(this.Courses);

            this.SyllabusHttpClient = new HttpClient() { BaseAddress = new Uri("http://sab.adm.kanazawa-u.ac.jp/") };
            this.SyllabusHttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");
            this.SyllabusHttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            this.SyllabusHttpClient.DefaultRequestHeaders.Add("Accept-Language", "ja,en-US;q=0.8,en;q=0.6");

            this.ResetAutoStatus();

            this.DispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal);

            this.DispatcherTimer.Tick += new EventHandler(RegisterCoursesAuto);

            this.RunsRegisterCoursesAuto = false;

            #region テストデータ
            {
                ////this.AddCourseProperty(new Course()
                ////{
                ////    RegisterNumber = null,
                ////    SubjectType = SubjectType.LiberalArts,
                ////    Id = "00000.000",
                ////    Title = "テストデータ(科目名)",
                ////    Instructor = "テストデータ(教員)",
                ////    DayOfWeek = DayOfWeek.Friday,
                ////    Period = 1,
                ////    UnitType = UnitType.Semester,
                ////    Credit = 0.5M,
                ////    MaxSize = 30,
                ////    RegisteredSize = 31,
                ////    IsDoneLottery = true,
                ////    IsToAdd = false,
                ////    IsToDelete = false,
                ////});

                ////this.AddCourseProperty(new Course()
                ////{
                ////    RegisterNumber = null,
                ////    SubjectType = SubjectType.LiberalArts,
                ////    Id = "00000.001",
                ////    Title = "テストデータ2(科目名)",
                ////    Instructor = "テストデータ2(教員)",
                ////    DayOfWeek = DayOfWeek.Friday,
                ////    Period = 1,
                ////    UnitType = UnitType.Semester,
                ////    Credit = 0.5M,
                ////    MaxSize = 30,
                ////    RegisteredSize = 31,
                ////    IsDoneLottery = true,
                ////    IsToAdd = false,
                ////    IsToDelete = false,
                ////});
            }
            #endregion
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 設定ウィンドウを取得または設定します。
        /// </summary>
        public ConfigWindow Config { get; set; }

        /// <summary>
        /// 詳細ウィンドウを取得または設定します。
        /// </summary>
        public DetailWindow Detail { get; set; }

        /// <summary>
        /// 詳細ウィンドウを取得または設定します。
        /// </summary>
        public FilterWindow Filter { get; set; }

        /// <summary>
        /// ログ表示用のテキストを取得または設定します。
        /// </summary>
        public string LogText
        {
            get { return (string)this.GetValue(LogTextProperty); }
            set { this.SetValue(LogTextProperty, value); }
        }

        /// <summary>
        /// ログ出力するファイルの書き込み用ストリームを取得または設定します。
        /// </summary>
        public StreamWriter LogWriter { get; set; }

        /// <summary>
        /// 科目一覧を取得または設定します。
        /// </summary>
        public ObservableCollection<Course> Courses { get; private set; } = new ObservableCollection<Course>();

        /////// <summary>
        /////// HTTPリクエストのためのクライアントを取得または設定します。
        /////// </summary>
        ////public HttpClient HttpClient { get; set; }

        /////// <summary>
        /////// HttpClientHandlerを取得または設定します。
        /////// </summary>
        ////public HttpClientHandler HttpClientHandler { get; set; }

        /// <summary>
        /// 学生情報サービス上の情報を扱うインスタンスを取得または設定します。
        /// </summary>
        public StudentInformationService StudentInformationService { get; set; }

        /// <summary>
        /// シラバス用のHTTPクライアントを取得または設定します。
        /// </summary>
        public HttpClient SyllabusHttpClient { get; set; }

        /// <summary>
        /// シラバス用のPOSTデータの中身を表す <see cref="Dictionary{string, string}"/> を取得または設定します。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "List<KeyValuePair<string, string>> は正当なデザインである。")]
        public IList<KeyValuePair<string, string>> SyllabusPostData { get; private set; }

        /// <summary> フィルターされた結果の <see cref="ICollectionView"/> 。 </summary>
        /// <remarks> これをDataGridなどのソースにバインディングする。 </remarks>
        public ICollectionView View { get; private set; }

        /// <summary>
        /// "学域/学部等" の選択項目のキー名を取得します。
        /// </summary>
        public string GakuikiFormKeyName { get; } = "val_gakubu_id";

        /// <summary>
        /// "学類/学科等" の選択項目のキー名を取得します。
        /// </summary>
        public string GakuruiFormKeyName { get; } = "val_gakka_id";

        /// <summary>
        /// "対象学生" の "学年" の選択項目のキー名を取得します。
        /// </summary>
        public string StudentYearFormKeyName { get; } = "val_student";

        #endregion

        #region プロパティ(private)

        /// <summary>
        /// 登録の結果を表す正規表現を取得します。
        /// </summary>
        private static Regex RegistrationResultRegex { get; } 
            = new Regex("<td[^<]*bgcolor=['\"](?<bgcolor>.*?)['\"]\\s*[^<]*>(?<text>.*?)</td>\\s*(?=<td|</tr|$)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 履修登録の追加を表す時にPOSTデータで送られる <see cref="KeyValuePair{string, string}"/> を取得します。
        /// </summary>
        private static KeyValuePair<string, string> AddContent { get; } = new KeyValuePair<string, string>("add", "追加");

        /// <summary>
        /// "専門科目等"を表す内部表現の文字列を取得します。
        /// </summary>
        private static string SpecializedSubjectText { get; } = "[専門科目]";

        /// <summary>
        /// 改行タグを表す正規表現を取得します。
        /// </summary>
        private static Regex NewLineTagRegex { get; } = new Regex("<\\s*br\\s*/?\\s*>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// HTMLタグを表す正規表現を取得します。
        /// </summary>
        private static Regex TagRegex { get; } = new Regex("<[^>]*>");

        /// <summary>
        /// SAML2を表す正規表現を取得します。
        /// </summary>
        private static Regex Saml2Regex { get; } = new Regex("<input[^<]*name=\"(?<name>[^\"]*)\"[^<]*value=\"(?<value>[^\"]*)\"[^<]*>");

        /// <summary>
        /// 図書館検索ページへのリンクを表すテキストを取得します。
        /// </summary>
        private static string SyllabusLibraryLinkText { get; } = WebUtility.HtmlDecode("●図書館検索ページへのリンク&nbsp;/&nbsp;The search page on the website of Kanazawa Univ. Library");

        /// <summary>
        /// シフトJISの文字エンコーディングを取得します。
        /// </summary>
        private static Encoding ShiftJisEncoding { get; } = Encoding.GetEncoding("shift_jis");

        /// <summary>
        /// 評価の方法のテンプレートを取得します。
        /// </summary>
        private static string GradingMethodTemplate { get; }
            = @"<table width='100%' border='0' cellspacing='1' cellpadding='3' bgcolor='#ED910B'>
	<tr>
		<td bgcolor='#F8EEB5'>
			<table width='100%' border='0' cellspacing='0' cellpadding='1'>
				<tr>
					<td width='20%' align='left'><font size='2'>※成績評価<br>　Performance rating：</font></td>
					<td width='80%' align='left'>
						<font size='2'>次項の項目及び割合で総合評価し、次のとおり判定する。</font>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td align='left'>
						<font size='2'>
「Ｓ（達成度90％～100％）」、「Ａ（同80％～90％未満）」、<br>
「Ｂ（同70％～80％未満）」、「Ｃ（同60％～70％未満）」を合格とし、<br>
「不可（同60％未満）」を不合格とする。（標準評価方法）
						</font>
					</td>
				</tr>
				<tr>
					<td width='15%'>&nbsp;</td>
					<td width='80%' align='left'>
						<font size='2'>Grade will be decided holistically as below, based on the following terms/rates.</font>
					</td>
				</tr>
				<tr>
					<td>&nbsp;</td>
					<td align='left'>
						<font size='2'>
「Ｓ（Academic achievement 90％～100％）」,「Ａ（over80%, less than90%）」, <br>
「Ｂ（over 70％, less than80％）」, and「Ｃ（over60％, less than70％）」 are indicators of passing, <br>
「不可（less than 60％）」is an indicator of failure.（Standard rating method）<br>
						</font>
					</td>
				</tr>
			</table>
		</td>
	</tr>
</table>";

        /// <summary>
        /// 不必要なASCII制御文字の一覧を取得します。
        /// </summary>
        private static char[] UnnecessaryAsciiControlCodes { get; } = new char[] { (char)0x0E, (char)0x0F, (char)0x1C, (char)0x1D, (char)0x1E, (char)0x1F, };

        /// <summary>
        /// 現在のログ行数を取得または設定します。
        /// </summary>
        private int LogRows { get; set; } = 0;

        /// <summary>
        /// 最後にクリックされたヘッダーを取得または設定します。
        /// </summary>
        private GridViewColumnHeader LastClickedHeader { get; set; } = null;

        /// <summary>
        /// 最後に指定したソートの方向を取得または設定します。
        /// </summary>
        private ListSortDirection LastSortDirection { get; set; } = ListSortDirection.Ascending;

        /////// <summary>
        /////// 自動実行についての <see cref="CancellationTokenSource"/> 。
        /////// </summary>
        ////private CancellationTokenSource TokenSource { get; set; } = new CancellationTokenSource();

        /// <summary>
        /// 指定間隔で実行するタイマーを取得または設定します。
        /// </summary>
        private DispatcherTimer DispatcherTimer { get; set; }

        /// <summary>
        /// タイマーがスタートした時刻を取得または設定します。
        /// </summary>
        private DateTime StartingDateTimeOfDispatcherTimer { get; set; }

        /// <summary>
        /// 自動実行中かどうかを取得または設定します。
        /// </summary>
        private bool RunsRegisterCoursesAuto { get; set; }

        #endregion

        #region 静的メソッド

        /// <summary>
        /// 文字列を <see cref="int?"/> 型に変換します。
        /// 数値に変換できる場合は <see cref="int"/> 型の数値、変換できない場合は <see cref="null"/> が返されます。
        /// </summary>
        /// <param name="value"> 変換する数値を格納する文字列。 </param>
        /// <returns> 変換結果。 </returns>
        public static int? TryParseNullableInt(string value)
        {
            int result;
            return int.TryParse(value, out result) ? result : default(int?);
        }

        /// <summary>
        /// 文字列を <see cref="decimal?"/> 型に変換します。
        /// 数値に変換できる場合は <see cref="decimal"/> 型の数値、変換できない場合は <see cref="null"/> が返されます。
        /// </summary>
        /// <param name="value"> 変換する数値を格納する文字列。 </param>
        /// <returns> 変換結果。 </returns>
        public static decimal? TryParseNullableDecimal(string value)
        {
            decimal result;
            return decimal.TryParse(value, out result) ? result : default(decimal?);
        }

        /// <summary>
        /// 与えられた文字列を、HTMLデコード、HTMLタグの削除、シフトアウト・シフトインの半角空白への変換、また先頭及び末尾にある空白を削除して、その結果の文字列を取得します。
        /// </summary>
        /// <param name="origin"> 元の文字列。 </param>
        /// <param name="convertsNewLineTag"> 元の文字列の改行を削除して、改行タグを改行に変換するかどうか。 </param>
        /// <returns> 変更後の文字列。 </returns>
        public static string GetHtmlDecodedAndFormattedString(string origin, bool convertsNewLineTag)
        {
            var decoded = WebUtility.HtmlDecode(origin);

            if (convertsNewLineTag)
            {
                decoded = NewLineTagRegex.Replace(decoded.Replace(Environment.NewLine, string.Empty).Replace("\n", string.Empty), Environment.NewLine);
            }

            return ReplaceAsciiControlCode(TagRegex.Replace(decoded, string.Empty)).Trim();
        }

        /// <summary>
        /// SAML2のクッキーに指定するContentを作成します。
        /// </summary>
        /// <param name="saml2Html"> HTMLドキュメント。 </param>
        /// <returns> Content. </returns>
        public static FormUrlEncodedContent MakeSaml2Content(string saml2Html)
        {
            var saml2Dictionary = new Dictionary<string, string>();

            for (var saml2Match = Saml2Regex.Match(saml2Html); saml2Match.Success; saml2Match = saml2Match.NextMatch())
            {
                saml2Dictionary.Add(
                    WebUtility.HtmlDecode(saml2Match.Groups["name"].Value),
                    WebUtility.HtmlDecode(saml2Match.Groups["value"].Value));
            }

            return new FormUrlEncodedContent(saml2Dictionary);
        }

        /// <summary>
        /// 現在のPOSTデータからShift-JISでエンコードされる文字列を生成します。
        /// </summary>
        /// <param name="postData"> シラバス用のPOSTデータの中身。 </param>
        /// <returns> POSTデータを表す文字列。 </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "IList<KeyValuePair<string, string>> は正当なデザインである。")]
        public static StringContent CreatePresentPostDataStringContent(IList<KeyValuePair<string, string>> postData)
        {
            if (postData == null)
            {
                throw new ArgumentNullException(nameof(postData), "引数が null です。");
            }

            var result = new StringBuilder();

            foreach (var p in postData)
            {
                result.Append(p.Key);
                result.Append("=");
                result.Append(p.Value);
                result.Append("&");
            }

            return new StringContent(result.ToString(), ShiftJisEncoding, "application/x-www-form-urlencoded");
        }

        /// <summary>
        /// POSTデータを更新します。
        /// </summary>
        /// <param name="postData"> シラバス用のPOSTデータの中身。 </param>
        /// <param name="oldIndex"> 古いデータの位置を表すインデックス。 </param>
        /// <param name="unencodedNewValue"> エンコードされていない新しい値。 </param>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "IList<KeyValuePair<string, string>> は正当なデザインである。")]
        public static void UpdatePostData(IList<KeyValuePair<string, string>> postData, int oldIndex, string unencodedNewValue)
        {
            if (postData == null)
            {
                throw new ArgumentNullException(nameof(postData), "引数が null です。");
            }

            var keyName = postData[oldIndex].Key;
            postData.RemoveAt(oldIndex);
            ////this.SyllabusPostData.Insert(oldIndex, new KeyValuePair<string, string>(
            ////         keyName,
            ////         HttpUtility.UrlEncode(unencodedNewValue, this.ShiftJisEncoding)));
            postData.Insert(oldIndex, new KeyValuePair<string, string>(keyName, unencodedNewValue));
            ////this.SyllabusPostData.Add(new KeyValuePair<string, string>(keyName, newValue));
        }

        #endregion

        #region メソッド

        /////// <summary>
        /////// <see cref="HttpClient"/> を再び新規に生成します。
        /////// </summary>
        ////[SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Rereate とメソッド名についているので Dispose されていないことがわかるため。")]
        ////public void RecreateHttpClient()
        ////{
        ////    if (this.HttpClientHandler != null)
        ////    {
        ////        this.HttpClientHandler.Dispose();
        ////    }

        ////    if (this.HttpClient != null)
        ////    {
        ////        this.HttpClient.Dispose();
        ////    }

        ////    this.HttpClientHandler = new HttpClientHandler() { UseCookies = true };
        ////    this.HttpClient = new HttpClient(this.HttpClientHandler)
        ////    {
        ////        BaseAddress = new Uri("https://acanthus.cis.kanazawa-u.ac.jp/")
        ////    };

        ////    this.HttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");
        ////    this.HttpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        ////    this.HttpClient.DefaultRequestHeaders.Add("Accept-Language", "ja,en-US;q=0.8,en;q=0.6");
        ////}

        /// <summary>
        /// <see cref="Course"/> を追加します。
        /// </summary>
        /// <param name="course"> 追加する <see cref="Course"/> 。 </param>
        public void AddCourse(Course course)
        {
            try
            {
                this.Courses.Add(course);
            }
            catch (ArgumentException)
            {
                this.View.SortDescriptions.Clear();
                ////this.Courses.Add(course);
            }

            this.UpdateViewCount();
        }

        /// <summary>
        /// 件数表示を更新します。
        /// </summary>
        public void UpdateViewCount()
        {
            this.StatusViewCount.Content = this.CourseListView.Items.Count.ToString(CultureInfo.InvariantCulture) + " / " + this.Courses.Count.ToString(CultureInfo.InvariantCulture) + " 件 表示";
        }

        #region

        /// <summary>
        /// 履修希望科目の反映
        /// </summary>
        /// <returns> 履修希望科目の一覧。 </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<IEnumerable<Course>> は正当なデザインである。")]
        public async Task<IEnumerable<Course>> ApplyRequestalCourseSchedule()
        {
            this.StatusMessage.Content = "履修希望科目を読み込んでいます。";
            var result = new List<Course>();

            var response = await this.StudentInformationService.HttpClient.GetAsync("/portal/StudentApp/Regist/RegistEdit.aspx");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var tdIdStr = "ctl00_phContents_ucRegistEdit_reTable_ttTable_td";

            for (var tdMatch = Regex.Match(
                    await response.Content.ReadAsStringAsync(),
                    "<td[^<]*?id=\"" + tdIdStr +  "(?<dayOfWeek>\\w*?)(?<period>\\d*)\"[^<]*?>(?<body>.*?)</td>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
                tdMatch.Success;
                tdMatch = tdMatch.NextMatch())
            {
                var dayOfWeek = DayOfWeek.None;
                int? period = null;

                try
                {
                    dayOfWeek = DayOfWeekToShortEnglishTextConverter.ConvertBack(tdMatch.Groups["dayOfWeek"].Value);
                }
                catch
                {
                    dayOfWeek = DayOfWeek.None;
                }

                period = TryParseNullableInt(tdMatch.Groups["period"].Value);

                this.UpdateCourse(
                    tdMatch.Groups["body"].Value,
                    tdIdStr + tdMatch.Groups["dayOfWeek"].Value + tdMatch.Groups["period"].Value,
                    dayOfWeek,
                    period);
            }

            ////    this.AddLogString("自分の履修希望科目を読み込みました。");
            ////    this.StatusMessage.Content = string.Empty;

            ////    this.Filter.UpdateFilterItem();

            ////    return result;

            return null;
        }

        protected void UpdateCourse(string html, string trIdBaseStr, DayOfWeek dayOfWeek, int? period)
        {
            ////    var tableRowRegex = new Regex("<tr align=\"middle\".*?>(.*?)</tr>", RegexOptions.Singleline);
            ////    var tableDataRegex = new Regex("<td.*?>(.*?)<(.*?)>", RegexOptions.Singleline);
            ////    var rowidRegex = new Regex("input[^<]*value=\"(?<rowid>[^<]*)\"[^<]*", RegexOptions.Singleline);
            ////    var dayOfWeekRegex = new Regex("([月火水木金土日])(\\d)", RegexOptions.Singleline);
            ////    var quarterRegex = new Regex("\\[Q(\\d)\\]", RegexOptions.Singleline);
            ////    var dayOfWeekConverter = new DayOfWeekToShortTextConverter();

            ////        bool existsAlready;

            ////        #region RegisterNumber & ID & RowID
            ////        {
            ////            if (string.IsNullOrWhiteSpace(tableDataMatch.Groups[1].Value))
            ////            {
            ////                continue;
            ////            }

            ////            var registerNumber = TryParseNullableInt(tableDataMatch.Groups[1].Value);

            ////            this.StatusMessage.Content = "履修希望科目を読み込んでいます。/ No." + tableDataMatch.Groups[1].Value;

            ////            tableDataMatch = tableDataMatch.NextMatch();

            ////            if (string.IsNullOrEmpty(tableDataMatch.Groups[1].Value))
            ////            {
            ////                continue;
            ////            }

            ////            var idMatch = rowidRegex.Match(tableDataMatch.Groups[2].Value);
            ////            string rowId = null;

            ////            if (idMatch.Success)
            ////            {
            ////                rowId = idMatch.Groups["rowid"].Value;
            ////            }

            ////            existsAlready = this.Courses.Any(c => c.Id == tableDataMatch.Groups[1].Value);

            ////            if (existsAlready)
            ////            {
            ////                course = this.Courses.FirstOrDefault(c => c.Id == tableDataMatch.Groups[1].Value);
            ////            }
            ////            else
            ////            {
            ////                course = new Course();

            ////                course.Id = tableDataMatch.Groups[1].Value;
            ////            }

            ////            course.RegisterNumber = registerNumber;
            ////            course.RowId = rowId;
            ////        }
            ////        #endregion

            ////        #region Title
            ////        {
            ////            tableDataMatch = tableDataMatch.NextMatch();

            ////            if (existsAlready && course.Title != tableDataMatch.Groups[1].Value)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Title:" + course.Title + "→" + tableDataMatch.Groups[1].Value + ")");
            ////            }

            ////            course.Title = tableDataMatch.Groups[1].Value;
            ////        }
            ////        #endregion

            ////        #region Instructor
            ////        {
            ////            tableDataMatch = tableDataMatch.NextMatch();

            ////            if (existsAlready && course.Instructor != tableDataMatch.Groups[1].Value)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Instructor:" + course.Instructor + "→" + tableDataMatch.Groups[1].Value + ")");
            ////            }

            ////            course.Instructor = tableDataMatch.Groups[1].Value;
            ////        }
            ////        #endregion

            ////        #region UnitType
            ////        {
            ////            tableDataMatch = tableDataMatch.NextMatch();

            ////            var unitType = UnitType.None;
            ////            var quarterMatch = quarterRegex.Match(tableDataMatch.Groups[1].Value);

            ////            if (quarterMatch.Success)
            ////            {
            ////                if (!string.IsNullOrEmpty(this.Config.QuarterText.Text) && quarterMatch.Groups[1].Value != this.Config.QuarterText.Text)
            ////                {
            ////                    continue;
            ////                }

            ////                unitType = UnitType.Quarter;
            ////            }
            ////            else
            ////            {
            ////                unitType = UnitType.Semester;
            ////            }

            ////            if (existsAlready && course.UnitType.HasValue && course.UnitType != unitType)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", UnitType:" + course.UnitType.Value + "→" + unitType + ")");
            ////            }

            ////            course.UnitType = unitType;
            ////        }
            ////        #endregion

            ////        #region DayOfWeek & Period
            ////        {
            ////            var dayOfWeek = DayOfWeek.None;
            ////            int? period = null;

            ////            var dayOfWeekMatch = dayOfWeekRegex.Match(tableDataMatch.Groups[1].Value);

            ////            if (dayOfWeekMatch.Success)
            ////            {
            ////                try
            ////                {
            ////                    dayOfWeek = DayOfWeekToShortTextConverter.ConvertBack(dayOfWeekMatch.Groups[1].Value);
            ////                }
            ////                catch
            ////                {
            ////                    dayOfWeek = DayOfWeek.None;
            ////                }

            ////                period = TryParseNullableInt(dayOfWeekMatch.Groups[2].Value);
            ////            }
            ////            else
            ////            {
            ////                dayOfWeek = DayOfWeek.Intensive;
            ////                period = 0;
            ////            }

            ////            if (existsAlready && course.DayOfWeek != dayOfWeek)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", DayOfWeek:" + course.DayOfWeek + "→" + dayOfWeek + ")");
            ////            }

            ////            course.DayOfWeek = dayOfWeek;

            ////            if (existsAlready && course.Period != period)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Period:" + course.Period + "→" + period + ")");
            ////            }

            ////            course.Period = period;
            ////        }
            ////        #endregion

            ////        #region Credit
            ////        {
            ////            tableDataMatch = tableDataMatch.NextMatch();
            ////            var credit = TryParseNullableDecimal(tableDataMatch.Groups[1].Value);

            ////            if (existsAlready && course.Credit.HasValue && course.Credit != credit)
            ////            {
            ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Credit:" + course.Credit.Value + "→" + credit + ")");
            ////            }

            ////            course.Credit = credit;
            ////        }
            ////        #endregion

            ////        if (!existsAlready)
            ////        {
            ////            this.AddCourse(course);
            ////        }

            ////        result.Add(course);
            ////    }
        }

        ////public async Task<IEnumerable<Course>> ApplyRequestalCourseSchedule()
        ////{

        ////    this.StatusMessage.Content = "履修希望科目を読み込んでいます。";
        ////    var result = new List<Course>();

        ////    ////string html = await this.HttpClient.GetStringAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/u053/spmode/registrationLink.php");

        ////    ////var response = await this.HttpClient.PostAsync(
        ////    ////    "https://risyu.kanazawa-u.ac.jp/Shibboleth.sso/SAML2/POST",
        ////    ////    this.MakeSaml2Content(html));
        ////    ////response.EnsureSuccessStatusCode();

        ////    ////html = await response.Content.ReadAsStringAsync();

        ////    var response = await this.ReadPortal();

        ////    if (!response.IsSuccessStatusCode)
        ////    {
        ////        return null;
        ////    }

        ////    ////var response = await this.ReadRequestalCourseSchedule();

        ////    response = await this.HttpClient.GetAsync("https://risyu.kanazawa-u.ac.jp/risyu/edit.asp");
        ////    var html = WebUtility.HtmlDecode(await this.ReadHtmlAsShiftJis(response));

        ////    var tableRowRegex = new Regex("<tr align=\"middle\".*?>(.*?)</tr>", RegexOptions.Singleline);
        ////    var tableDataRegex = new Regex("<td.*?>(.*?)<(.*?)>", RegexOptions.Singleline);
        ////    var rowidRegex = new Regex("input[^<]*value=\"(?<rowid>[^<]*)\"[^<]*", RegexOptions.Singleline);
        ////    var dayOfWeekRegex = new Regex("([月火水木金土日])(\\d)", RegexOptions.Singleline);
        ////    var quarterRegex = new Regex("\\[Q(\\d)\\]", RegexOptions.Singleline);
        ////    var dayOfWeekConverter = new DayOfWeekToShortTextConverter();

        ////    ////var tableRowMatch = tableRowRegex.Match(html);
        ////    ////tableRowMatch = tableRowMatch.NextMatch();

        ////    for (var tableRowMatch = tableRowRegex.Match(html);
        ////        tableRowMatch.Success;
        ////        tableRowMatch = tableRowMatch.NextMatch())
        ////    {
        ////        Course course;

        ////        var tableDataMatch = tableDataRegex.Match(tableRowMatch.Groups[1].Value);

        ////        bool existsAlready;

        ////        #region RegisterNumber & ID & RowID
        ////        {
        ////            if (string.IsNullOrWhiteSpace(tableDataMatch.Groups[1].Value))
        ////            {
        ////                continue;
        ////            }

        ////            var registerNumber = TryParseNullableInt(tableDataMatch.Groups[1].Value);

        ////            this.StatusMessage.Content = "履修希望科目を読み込んでいます。/ No." + tableDataMatch.Groups[1].Value;

        ////            tableDataMatch = tableDataMatch.NextMatch();

        ////            if (string.IsNullOrEmpty(tableDataMatch.Groups[1].Value))
        ////            {
        ////                continue;
        ////            }

        ////            var idMatch = rowidRegex.Match(tableDataMatch.Groups[2].Value);
        ////            string rowId = null;

        ////            if (idMatch.Success)
        ////            {
        ////                rowId = idMatch.Groups["rowid"].Value;
        ////            }

        ////            existsAlready = this.Courses.Any(c => c.Id == tableDataMatch.Groups[1].Value);

        ////            if (existsAlready)
        ////            {
        ////                course = this.Courses.FirstOrDefault(c => c.Id == tableDataMatch.Groups[1].Value);
        ////            }
        ////            else
        ////            {
        ////                course = new Course();

        ////                course.Id = tableDataMatch.Groups[1].Value;
        ////            }

        ////            course.RegisterNumber = registerNumber;
        ////            course.RowId = rowId;
        ////        }
        ////        #endregion

        ////        #region Title
        ////        {
        ////            tableDataMatch = tableDataMatch.NextMatch();

        ////            if (existsAlready && course.Title != tableDataMatch.Groups[1].Value)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Title:" + course.Title + "→" + tableDataMatch.Groups[1].Value + ")");
        ////            }

        ////            course.Title = tableDataMatch.Groups[1].Value;
        ////        }
        ////        #endregion

        ////        #region Instructor
        ////        {
        ////            tableDataMatch = tableDataMatch.NextMatch();

        ////            if (existsAlready && course.Instructor != tableDataMatch.Groups[1].Value)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Instructor:" + course.Instructor + "→" + tableDataMatch.Groups[1].Value + ")");
        ////            }

        ////            course.Instructor = tableDataMatch.Groups[1].Value;
        ////        }
        ////        #endregion

        ////        #region UnitType
        ////        {
        ////            tableDataMatch = tableDataMatch.NextMatch();

        ////            var unitType = UnitType.None;
        ////            var quarterMatch = quarterRegex.Match(tableDataMatch.Groups[1].Value);

        ////            if (quarterMatch.Success)
        ////            {
        ////                if (!string.IsNullOrEmpty(this.Config.QuarterText.Text) && quarterMatch.Groups[1].Value != this.Config.QuarterText.Text)
        ////                {
        ////                    continue;
        ////                }

        ////                unitType = UnitType.Quarter;
        ////            }
        ////            else
        ////            {
        ////                unitType = UnitType.Semester;
        ////            }

        ////            if (existsAlready && course.UnitType.HasValue && course.UnitType != unitType)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", UnitType:" + course.UnitType.Value + "→" + unitType + ")");
        ////            }

        ////            course.UnitType = unitType;
        ////        }
        ////        #endregion

        ////        #region DayOfWeek & Period
        ////        {
        ////            var dayOfWeek = DayOfWeek.None;
        ////            int? period = null;

        ////            var dayOfWeekMatch = dayOfWeekRegex.Match(tableDataMatch.Groups[1].Value);

        ////            if (dayOfWeekMatch.Success)
        ////            {
        ////                try
        ////                {
        ////                    dayOfWeek = DayOfWeekToShortTextConverter.ConvertBack(dayOfWeekMatch.Groups[1].Value);
        ////                }
        ////                catch
        ////                {
        ////                    dayOfWeek = DayOfWeek.None;
        ////                }

        ////                period = TryParseNullableInt(dayOfWeekMatch.Groups[2].Value);
        ////            }
        ////            else
        ////            {
        ////                dayOfWeek = DayOfWeek.Intensive;
        ////                period = 0;
        ////            }

        ////            if (existsAlready && course.DayOfWeek != dayOfWeek)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", DayOfWeek:" + course.DayOfWeek + "→" + dayOfWeek + ")");
        ////            }

        ////            course.DayOfWeek = dayOfWeek;

        ////            if (existsAlready && course.Period != period)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Period:" + course.Period + "→" + period + ")");
        ////            }

        ////            course.Period = period;
        ////        }
        ////        #endregion

        ////        #region Credit
        ////        {
        ////            tableDataMatch = tableDataMatch.NextMatch();
        ////            var credit = TryParseNullableDecimal(tableDataMatch.Groups[1].Value);

        ////            if (existsAlready && course.Credit.HasValue && course.Credit != credit)
        ////            {
        ////                this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Credit:" + course.Credit.Value + "→" + credit + ")");
        ////            }

        ////            course.Credit = credit;
        ////        }
        ////        #endregion

        ////        if (!existsAlready)
        ////        {
        ////            this.AddCourse(course);
        ////        }

        ////        result.Add(course);
        ////    }

        ////    this.AddLogString("自分の履修希望科目を読み込みました。");
        ////    this.StatusMessage.Content = string.Empty;

        ////    this.Filter.UpdateFilterItem();

        ////    return result;
        ////}

        /// <summary>
        /// 時間割の読み込みと履修希望への追加
        /// </summary>
        /// <returns> <see cref="Task"/> . </returns>
        public async Task ReadAndAddCourses()
        {

        }
        ////public async Task ReadAndAddCourses()
        ////{
        ////    this.StatusMessage.Content = "時間割を読み込んでいます。";
        ////    var dayOfWeekConverter = new DayOfWeekToShortTextConverter();

        ////    var response = await this.ReadPortal();

        ////    if (!response.IsSuccessStatusCode)
        ////    {
        ////        return;
        ////    }

        ////    response = await this.HttpClient.GetAsync("https://risyu.kanazawa-u.ac.jp/risyu/search_lecture.asp");
        ////    var searchLectureHtml = await this.ReadHtmlAsShiftJis(response);

        ////    var lctdivRegex = new Regex("<input[^<]*name=\"lct_div\"[^<]*value=\"(?<value>[^\"]*)\"[^<]*>", RegexOptions.Singleline);
        ////    var dayOfWeekAndPeriodRegex = new Regex("<input[^<]*value=\"(?<value>[^\"]*)\"[^<]*name=\"(?<name>(?<dayOfWeek>\\d*)-(?<period>\\d*))\"[^<]*>", RegexOptions.Singleline);

        ////    var tableRowRegex = new Regex("<form[^<]*>.*?<tr[^<]*>(?<body>.*?<input[^<]*name=\"lct_cd\"[^<]*value=\"(?<lct_cd>[^<]*)\"[^<]*>.*?)</tr>", RegexOptions.Singleline);
        ////    var tableDataRegex = new Regex("<td.*?>(.*?)</td>", RegexOptions.Singleline);

        ////    for (var lctdivMatch = lctdivRegex.Match(searchLectureHtml);
        ////        lctdivMatch.Success;
        ////        lctdivMatch = lctdivMatch.NextMatch())
        ////    {
        ////        var lctdivContent = new KeyValuePair<string, string>(
        ////            "lct_div", lctdivMatch.Groups["value"].Value);

        ////        string subjectType;
        ////        bool isSpecializedSubject;

        ////        #region subjectType, isSpecializedSubject
        ////        {
        ////            ////var subjectType = (SubjectType)(TryParseNullableInt(lctdivMatch.Groups["value"].Value) ?? 0);

        ////            subjectType = lctdivMatch.Groups["value"].Value;
        ////            var subjectTypeNumber = TryParseNullableInt(subjectType);

        ////            isSpecializedSubject = false;

        ////            if (subjectTypeNumber.HasValue)
        ////            {
        ////                if (subjectTypeNumber == 1)
        ////                {
        ////                    subjectType = "共通教育科目";
        ////                }
        ////                else if (subjectTypeNumber == 2)
        ////                {
        ////                    subjectType = "専門科目等";
        ////                    isSpecializedSubject = true;
        ////                }
        ////            }
        ////        }
        ////        #endregion

        ////        for (var dayOfWeekAndPeriodMatch = dayOfWeekAndPeriodRegex.Match(searchLectureHtml);
        ////            dayOfWeekAndPeriodMatch.Success;
        ////            dayOfWeekAndPeriodMatch = dayOfWeekAndPeriodMatch.NextMatch())
        ////        {
        ////            var dayOfWeekAndPeriodContent = new KeyValuePair<string, string>(
        ////                WebUtility.HtmlDecode(dayOfWeekAndPeriodMatch.Groups["name"].Value),
        ////                WebUtility.HtmlDecode(dayOfWeekAndPeriodMatch.Groups["value"].Value));

        ////            DayOfWeek presentDayOfWeek;
        ////            int? presentPeriod;

        ////            #region presentDayOfWeek, presentPeriod
        ////            {
        ////                if (dayOfWeekAndPeriodMatch.Groups["name"].Value == "9-9")
        ////                {
        ////                    presentDayOfWeek = DayOfWeek.Intensive;
        ////                    presentPeriod = 0;
        ////                }
        ////                else
        ////                {
        ////                    presentDayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeekAndPeriodMatch.Groups["dayOfWeek"].Value);
        ////                    presentPeriod = TryParseNullableInt(dayOfWeekAndPeriodMatch.Groups["period"].Value);
        ////                }

        ////                this.StatusMessage.Content = "時間割を読み込んでいます。/ "
        ////                    + subjectType + " "
        ////                    + DayOfWeekToShortTextConverter.Convert(presentDayOfWeek) + "曜 "
        ////                    + presentPeriod.ToString() + "限";
        ////            }
        ////            #endregion

        ////            response = await this.HttpClient.PostAsync(
        ////                "https://risyu.kanazawa-u.ac.jp/risyu/search_results.asp",
        ////                new FormUrlEncodedContent(new KeyValuePair<string, string>[] { lctdivContent, dayOfWeekAndPeriodContent }));

        ////            var searchResultsHtml = WebUtility.HtmlDecode(await this.ReadHtmlAsShiftJis(response));

        ////            for (var tableRowMatch = tableRowRegex.Match(searchResultsHtml);
        ////                tableRowMatch.Success;
        ////                tableRowMatch = tableRowMatch.NextMatch())
        ////            {
        ////                var tableDataMatch = tableDataRegex.Match(tableRowMatch.Groups["body"].Value);
        ////                tableDataMatch = tableDataMatch.NextMatch();

        ////                Course course;
        ////                bool existsAlready;

        ////                #region ID
        ////                {
        ////                    existsAlready = this.Courses.Any(c => c.Id == tableDataMatch.Groups[1].Value);

        ////                    if (existsAlready)
        ////                    {
        ////                        course = this.Courses.FirstOrDefault(c => c.Id == tableDataMatch.Groups[1].Value);
        ////                    }
        ////                    else
        ////                    {
        ////                        course = new Course();

        ////                        course.Id = tableDataMatch.Groups[1].Value;

        ////                        course.IsToAdd = false;
        ////                        course.IsToDelete = false;
        ////                    }
        ////                }
        ////                #endregion

        ////                #region Title
        ////                {
        ////                    tableDataMatch = tableDataMatch.NextMatch();

        ////                    if (existsAlready && !string.IsNullOrEmpty(course.Title) && course.Title != tableDataMatch.Groups[1].Value)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Title:" + course.Title + "→" + tableDataMatch.Groups[1].Value + ")");
        ////                    }

        ////                    course.Title = tableDataMatch.Groups[1].Value;
        ////                }
        ////                #endregion

        ////                #region Instructor
        ////                {
        ////                    tableDataMatch = tableDataMatch.NextMatch();

        ////                    if (existsAlready && !string.IsNullOrEmpty(course.Instructor) && course.Instructor != tableDataMatch.Groups[1].Value)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Instructor:" + course.Instructor + "→" + tableDataMatch.Groups[1].Value + ")");
        ////                    }

        ////                    course.Instructor = tableDataMatch.Groups[1].Value;
        ////                }
        ////                #endregion

        ////                #region MaxSize
        ////                {
        ////                    tableDataMatch = tableDataMatch.NextMatch();
        ////                    var maxSize = TryParseNullableInt(tableDataMatch.Groups[1].Value);

        ////                    if (existsAlready && course.MaxSize.HasValue && course.MaxSize != maxSize)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", MaxSize:" + course.MaxSize.Value + "→" + maxSize + ")");
        ////                    }

        ////                    course.MaxSize = maxSize;
        ////                }
        ////                #endregion

        ////                #region RegisteredSize
        ////                {
        ////                    tableDataMatch = tableDataMatch.NextMatch();
        ////                    course.RegisteredSize = TryParseNullableInt(tableDataMatch.Groups[1].Value);
        ////                }
        ////                #endregion

        ////                #region IsDoneLottery
        ////                {
        ////                    tableDataMatch = tableDataMatch.NextMatch();
        ////                    var isDoneLottery = tableDataMatch.Groups[1].Value == "○";

        ////                    if (existsAlready && course.IsDoneLottery.HasValue && course.IsDoneLottery != isDoneLottery)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", IsDoneLottery:" + course.IsDoneLottery.Value + "→" + isDoneLottery + ")");
        ////                    }

        ////                    course.IsDoneLottery = isDoneLottery;
        ////                }
        ////                #endregion

        ////                #region DayOfWeek
        ////                {
        ////                    if (existsAlready && course.DayOfWeek != presentDayOfWeek)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", DayOfWeek:" + course.DayOfWeek + "→" + presentDayOfWeek + ")");
        ////                    }

        ////                    course.DayOfWeek = presentDayOfWeek;
        ////                }
        ////                #endregion

        ////                #region Period
        ////                {
        ////                    if (existsAlready && course.Period != presentPeriod)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", Period:" + course.Period + "→" + presentPeriod + ")");
        ////                    }

        ////                    course.Period = presentPeriod;
        ////                }
        ////                #endregion

        ////                #region JpSubjectType
        ////                {
        ////                    if (isSpecializedSubject)
        ////                    {
        ////                        if (string.IsNullOrEmpty(subjectType))
        ////                        {
        ////                            course.RawSubjectType = SpecializedSubjectText;
        ////                        }
        ////                    }
        ////                    else
        ////                    {
        ////                        if (existsAlready
        ////                            && string.IsNullOrEmpty(course.JPSubjectType)
        ////                            && course.JPSubjectType != subjectType
        ////                            && !(course.JPSubjectType == SpecializedSubjectText && !string.IsNullOrEmpty(subjectType)))
        ////                        {
        ////                            this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", JpSubjectType:" + course.JPSubjectType + "→" + subjectType + ")");
        ////                        }

        ////                        course.RawSubjectType = subjectType;
        ////                    }
        ////                }
        ////                #endregion

        ////                #region LctCd
        ////                {
        ////                    if (existsAlready && !string.IsNullOrEmpty(course.Lctcd) && course.Lctcd != tableRowMatch.Groups["lct_cd"].Value)
        ////                    {
        ////                        this.AddLogString("登録情報に変更がありました。(ID:" + course.Id + ", LctCd:" + course.Lctcd + "→" + tableRowMatch.Groups["lct_cd"].Value + ")");
        ////                    }

        ////                    course.Lctcd = tableRowMatch.Groups["lct_cd"].Value;
        ////                }
        ////                #endregion

        ////                if (!existsAlready)
        ////                {
        ////                    this.AddCourse(course);
        ////                }
        ////            }
        ////        }
        ////    }

        ////    this.AddLogString("時間割検索からすべての時間割を読み込みました。");
        ////    this.StatusMessage.Content = string.Empty;

        ////    this.Filter.UpdateFilterItem();
        ////}

        #endregion

        /// <summary>
        /// 追加ボタンがチェックされている科目の追加を試みます。
        /// </summary>
        /// <returns> <see cref="Task"/> . </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<IEnumerable<Course>> は正当なデザインである。")]
        public async Task<IEnumerable<Course>> RegisterCourses() => await this.RegisterCourses(false);

        /// <summary>
        /// 科目の追加を試みます。
        /// </summary>
        /// <param name="isCheckRest"> 登録の残り枠数をチェックするかどうか。 </param>
        /// <returns> <see cref="Task"/> . </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<IEnumerable<Course>> は正当なデザインである。")]
        public async Task<IEnumerable<Course>> RegisterCourses(bool isCheckRest) => await this.RegisterCourses(0, isCheckRest);

        /// <summary>
        /// 科目の追加を試みます。
        /// </summary>
        /// <param name="retry"> リトライ回数。 </param>
        /// <param name="isCheckRest"> 登録の残り枠数をチェックするかどうか。 </param>
        /// <returns> <see cref="Task"/> . </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<IEnumerable<Course>> は正当なデザインである。")]
        public async Task<IEnumerable<Course>> RegisterCourses(int retry, bool isCheckRest)
        {
            if (isCheckRest)
            {
                return await this.RegisterCourses(retry, c => c.IsToAdd && ((c.IsDoneLottery == true && c.RestSize.HasValue) || c.IsDoneLottery != true));
            }
            else
            {
                return await this.RegisterCourses(retry, c => c.IsToAdd);
            }
        }

        /////// <summary>
        /////// 削除ボタンがチェックされている科目の削除を試みます。
        /////// その後に履修希望科目を反映させます。
        /////// </summary>
        /////// <returns> <see cref="Task"/> . </returns>
        ////public async Task CancelRegistrationCourses()
        ////{
        ////    this.StatusMessage.Content = "履修希望科目を削除しています。";

        ////    var saveContent = new Dictionary<string, string>()
        ////    {
        ////        { "save", "編集結果を保存/Update and Save" }
        ////    };

        ////    var coursesToDelete = this.Courses.Where(c => c.IsToDelete && c.Registers);

        ////    foreach (var course in coursesToDelete)
        ////    {
        ////        saveContent.Add("cancel" + course.RegisterNumber.ToString(), "1");
        ////        saveContent.Add("rowid" + course.RegisterNumber.ToString(), course.RowId);
        ////    }

        ////    if (coursesToDelete.Count() > 0)
        ////    {
        ////        var response = await this.HttpClient.PostAsync(
        ////            "https://risyu.kanazawa-u.ac.jp/risyu/save.asp",
        ////            new FormUrlEncodedContent(saveContent));

        ////        ////var html = this.ReadHtmlAsShiftJis(response);
        ////    }

        ////    var registeredCourses = await this.ApplyRequestalCourseSchedule();

        ////    foreach (var deleted in coursesToDelete.Where(d => registeredCourses.Any(c => c == d)))
        ////    {
        ////        deleted.RegisterNumber = null;
        ////        deleted.RowId = null;
        ////        deleted.IsToDelete = false;
        ////    }
        ////}

        /////// <summary>
        /////// 履修登録のポータルページの HTTP 応答メッセージを取得します。
        /////// </summary>
        /////// <returns> ポータルの HTTP 応答メッセージ。 </returns>
        ////public async Task<HttpResponseMessage> ReadPortal() => await this.ReadPortal(0, false);

        /////// <summary>
        /////// 履修登録のポータルページの HTTP 応答メッセージを取得します。
        /////// 取得できない場合は、一定回数まで再ログインを試みます。
        /////// </summary>
        /////// <param name="retry"> 最大リトライ回数。 </param>
        /////// <param name="occursError"> 接続失敗時にエラーを起こすかどうか。 </param>
        /////// <returns> ポータルの HTTP 応答メッセージ。 </returns>
        ////public async Task<HttpResponseMessage> ReadPortal(int retry, bool occursError)
        ////{
        ////    var url = "https://risyu.kanazawa-u.ac.jp/risyu/portal.asp";
        ////    ////var url = "https://risyu.kanazawa-u.ac.jp/risyu/edit.asp";

        ////    for (int count = -1; count < retry; count++)
        ////    {
        ////        try
        ////        {
        ////            var response = await this.HttpClient.GetAsync(url);
        ////            response.EnsureSuccessStatusCode();
        ////            ////var html = WebUtility.HtmlDecode(await this.ReadHtmlAsShiftJis(response));

        ////            if (response.RequestMessage.RequestUri.AbsoluteUri == url)
        ////            {
        ////                return response;
        ////            }
        ////        }
        ////        catch
        ////        {
        ////        }

        ////        await this.Config.LogOff();
        ////        await this.Config.LogOn();
        ////    }

        ////    if (occursError)
        ////    {
        ////        throw new TimeoutException("リトライ回数オーバーによる時間切れ");
        ////    }

        ////    return new HttpResponseMessage(HttpStatusCode.NotFound);
        ////}

        /// <summary>
        /// HTMLドキュメントを shift_jis として読み込みます。
        /// </summary>
        /// <param name="response"> HTTP 応答メッセージ。 </param>
        /// <returns> HTML ドキュメント。 </returns>
        public async Task<string> ReadHtmlAsShiftJis(HttpResponseMessage response)
        {
            try
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var reader = (new StreamReader(stream, ShiftJisEncoding, true)) as TextReader)
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// ログラベルに文字列を追加します。
        /// </summary>
        /// <param name="str"> 追加する文字列。 </param>
        public void AddLogString(string str)
        {
            var text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) + " " + str;

            if (this.LogRows < this.Config.LogMaxRow)
            {
                this.LogRows++;

                if (!string.IsNullOrEmpty(this.LogText))
                {
                    this.LogText += "\n";
                }

                this.LogText += text;
            }
            else
            {
                var index = this.LogText.IndexOf('\n');

                if (index < 0)
                {
                    this.LogText = text;
                }
                else
                {
                    this.LogText = this.LogText.Remove(0, index + 1) + "\n" + text;
                }
            }
        }

        /// <summary>
        /// <see cref="CoursesGridView"/> の列幅をリサイズします。
        /// </summary>
        public void ResizeColumnWidth()
        {
            foreach (var column in this.CoursesGridView.Columns)
            {
                if (double.IsNaN(column.Width))
                {
                    column.Width = 0;
                    column.Width = double.NaN;
                }
            }
        }

        /// <summary>
        /// シラバス検索画面から送信するPOSTデータを表すディクショナリーを作成します。
        /// </summary>
        /// <param name="html"> シラバスのHTMLドキュメント。 </param>
        public void CreateSyllabusPostData(string html)
        {
            this.SyllabusPostData = new List<KeyValuePair<string, string>>();

            var formHtml = new Regex("<form[^>]*?>(?<data>.*?)</form>", RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(html).Groups["data"].Value;

            #region inputタグ(hidden)
            {
                var hiddenInputRegex = new Regex(
                    "<input[^>]*name=\"(?<name>[^\"]*)\"[^>]*value=\"(?<value>[^>]*)\"[^>]*>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);

                for (var hiddenInputMatch = hiddenInputRegex.Match(formHtml);
                    hiddenInputMatch.Success;
                    hiddenInputMatch = hiddenInputMatch.NextMatch())
                {
                    ////this.SyllabusPostData.Add(new KeyValuePair<string, string>(
                    ////    HttpUtility.UrlEncode(hiddenInputMatch.Groups["name"].Value, this.ShiftJisEncoding),
                    ////    HttpUtility.UrlEncode(hiddenInputMatch.Groups["value"].Value, this.ShiftJisEncoding)));
                    this.SyllabusPostData.Add(new KeyValuePair<string, string>(hiddenInputMatch.Groups["name"].Value, hiddenInputMatch.Groups["value"].Value));
                }
            }
            #endregion

            #region selectタグ
            {
                var optionRegex = new Regex(
                    "<select[^>]*name=\"(?<name>[^\"]*)\"[^>]*>.*?<option[^>]*value='(?<value>[^']*)'[^>]*selected>.*?</select>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);

                for (var optionMatch = optionRegex.Match(html);
                    optionMatch.Success;
                    optionMatch = optionMatch.NextMatch())
                {
                    ////this.SyllabusPostData.Add(new KeyValuePair<string, string>(
                    ////    HttpUtility.UrlEncode(optionMatch.Groups["name"].Value, this.ShiftJisEncoding),
                    ////    HttpUtility.UrlEncode(optionMatch.Groups["value"].Value, this.ShiftJisEncoding)));
                    this.SyllabusPostData.Add(new KeyValuePair<string, string>(optionMatch.Groups["name"].Value, optionMatch.Groups["value"].Value));
                }
            }
            #endregion

            #region inputタグ(checked)
            {
                var inputRegex = new Regex(
                    "<input[^>]*name='(?<name>[^']*)'[^>]*value='(?<value>[^']*)'[^>]*checked[^>]*>",
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
                for (var inputMatch = inputRegex.Match(html);
                    inputMatch.Success;
                    inputMatch = inputMatch.NextMatch())
                {
                    ////this.SyllabusPostData.Add(new KeyValuePair<string, string>(
                    ////    HttpUtility.UrlEncode(inputMatch.Groups["name"].Value, this.ShiftJisEncoding),
                    ////    HttpUtility.UrlEncode(inputMatch.Groups["value"].Value, this.ShiftJisEncoding)));
                    this.SyllabusPostData.Add(new KeyValuePair<string, string>(inputMatch.Groups["name"].Value, inputMatch.Groups["value"].Value));
                }
            }
            #endregion
        }

        /// <summary>
        /// シラバス検索画面で指定される名前のキーのオプションの一覧を取得します。
        /// </summary>
        /// <param name="html"> シラバス検索画面のHTMLドキュメント。 </param>
        /// <param name="keyName"> キーの名前。 </param>
        /// <returns> オプションの一覧。 </returns>
        public IEnumerable<SyllabusComboBoxItem> GetSelectOptionList(string html, string keyName)
        {
            var result = new Dictionary<string, string>();

            var gakubuListMatch = new Regex("<select[^>]*name=\"" + keyName + "\"[^>]*>(?<data>.*?)</select>", RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(html);
            var gakubuRegex = new Regex("<option[^>]*value=('?(?<id>[^']*?)(['\\s][^>]*?)?)>(?<text>.*?)(?=(</?option|$))", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var gakubuMatch = gakubuRegex.Match(gakubuListMatch.Groups["data"].Value);
            ////int startat = 0;

            while (gakubuMatch.Success)
            {
                var key = gakubuMatch.Groups["id"].Value;
                var value = gakubuMatch.Groups["text"].Value.Trim();

                var itemValueMatch = SyllabusRegex.ComboBoxItem.Match(value);

                if (itemValueMatch.Success)
                {
                    yield return new SyllabusComboBoxItem(key, itemValueMatch.Groups["jpValue"].Value, itemValueMatch.Groups["enValue"].Value);
                }
                else
                {
                    yield return new SyllabusComboBoxItem(key, value, string.Empty);
                }

                ////yield return new KeyValuePair<string, string>(key, value);

                gakubuMatch = gakubuMatch.NextMatch();
            }
        }

        /// <summary>
        /// シラバスから追加した情報を <see cref="Courses"/> に反映します。
        /// </summary>
        /// <returns> <see cref="Task"/> . </returns>
        public async Task ReadSyllabus()
        {
            this.StatusMessage.Content = "シラバスから情報を読み込んでいます。";

            var postData = new List<KeyValuePair<string, string>>();

            #region POSTデータの修正
            {
                foreach (var p in this.SyllabusPostData)
                {
                    postData.Add(p);
                }

                for (var index = 0; index < postData.Count; index++)
                {
                    if (postData[index].Key == this.GakuikiFormKeyName)
                    {
                        UpdatePostData(postData, index, (string)this.Config.GakuikiComboBox.SelectedValue);
                        ////index--;
                    }
                    else if (postData[index].Key == this.GakuruiFormKeyName)
                    {
                        UpdatePostData(postData, index, (string)this.Config.GakuruiComboBox.SelectedValue);
                        ////index--;
                    }
                    else if (postData[index].Key == this.StudentYearFormKeyName)
                    {
                        UpdatePostData(postData, index, (string)this.Config.StudentYearComboBox.SelectedValue);
                        ////index--;
                    }
                    else if (postData[index].Key == "int_nowpage")
                    {
                        UpdatePostData(postData, index, 1.ToString());
                        ////index--;
                    }
                }

                postData.Add(new KeyValuePair<string, string>("val_kamoku", this.Config.TitlesText.Text));
            }
            #endregion

            var response = await this.SyllabusHttpClient.PostAsync(
                "http://sab.adm.kanazawa-u.ac.jp/search/list/list.asp",
                CreatePresentPostDataStringContent(postData));

            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var html = await this.ReadHtmlAsShiftJis(response);

            long records = 0;

            #region 件数取得
            {
                var recordMatch = new Regex("(\\d*)件見つかりました。", RegexOptions.Singleline | RegexOptions.IgnoreCase).Match(html);
                if (recordMatch.Success)
                {
                    records = long.Parse(recordMatch.Groups[1].Value);
                }
            }
            #endregion

            ////string subjectType = ((SyllabusComboBoxItem)this.Config.GakuikiComboBox.SelectedItem).JPValue;

            #region 正規表現の定義

            var resultRegex = new Regex("<div[^<]*id=\"jyoken\"[^<]*>.*?<center>(.*?)</center>.*?</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var tableRowRegex = new Regex("<tr[^<]*class='list_[^_']*'[^<]*>(.*?)</tr>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var onclickRegex = new Regex("onclick=\"GoDetail\\('([^']*)'\\);\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            ////var schoolOrDivisionRegex = new Regex("<div[^<]*>.*?" + subjectType + ".*?＞(.*?)<", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var subjectTypeAndSchoolOrDivisionRegex = new Regex(
                "<table[^>]*>.*?<tr[^>]*>.*?<td[^>]*>.*?<div[^>]*>(?<subjectType>.*?)(＞(?<schoolOrDivision>.*?))?</div>.*?</td>.*?</tr>.*?</table>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var kamokuTableRegex = new Regex("<table[^<]*id=\"kamoku\"[^<]*>(.*?)</table>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var kamokuTableRowRegex = new Regex("<tr>(.*?)(?=(</?tr|$))", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var kamokuSingleDetailRegex = new Regex(
                "<td[^<]*class=\"detail_title_[^\"]*\"[^<]*>.*?</td>.*?<td[^<]*colspan=\"3\"[^<]*>(.*?)</td>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var cityCollegeRegex = new Regex("<img[^<]*city[^<]*college[^<]*>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var numberingCodeTagRegex = new Regex("<a[^<]*course_numbering_systems[^<]*>.*?</a>");
            var kamokuDoubleDetailRegex = new Regex(
                "<td[^<]*class=\"detail_title_[^\"]*\"[^<]*>.*?</td>.*?<td[^<]*>(.*?)</td>.*?<td[^<]*class=\"detail_title_[^\"]*\"[^<]*>.*?</td>.*?<td[^<]*>(.*?)</td>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var dayOfWeekAndPeriodRegex = new Regex("(?<dayOfWeek>[月火水木金土日])曜・(?<period>\\d)限", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            var topicRegex = new Regex(
                "<div[^<]*id=['\"]titleobi_[^'\"]*['\"][^<]*>(?<title>.*?)</div>.*?<div[^<]*id=['\"]naiyo['\"][^<]*>(?<body>.*?)</div>",
                RegexOptions.Singleline | RegexOptions.IgnoreCase);

            #endregion

            int nowpage = 1;

            while (true)
            {
                var resultHtml = resultRegex.Match(html).Groups[1].Value;

                for (var tableRowMatch = tableRowRegex.Match(resultHtml);
                    tableRowMatch.Success;
                    tableRowMatch = tableRowMatch.NextMatch())
                {
                    Course course = null;
                    bool existsAlready = true;

                    #region 各データに対応するCoursesの要素・シラバスのページがそれぞれ存在するかを確認
                    {
                        var tableDataMatch = SyllabusRegex.TableData.Match(tableRowMatch.Groups[1].Value);

                        this.StatusMessage.Content =
                            "シラバスから情報を読み込んでいます。 ( " + tableDataMatch.Groups[1].Value.Trim() + " / " + records.ToString() + " 件 )";

                        tableDataMatch = tableDataMatch.NextMatch();
                        tableDataMatch = tableDataMatch.NextMatch();
                        tableDataMatch = tableDataMatch.NextMatch();
                        tableDataMatch = tableDataMatch.NextMatch();
                        tableDataMatch = tableDataMatch.NextMatch();

                        tableDataMatch = tableDataMatch.NextMatch();
                        var presentID = tableDataMatch.Groups[1].Value.Trim();

                        if (!this.Courses.Any(c => c.Id == presentID))
                        {
                            if (this.Config.IsReadAllSyllabusData.IsChecked == true)
                            {
                                course = new Course();
                                course.Id = presentID;
                                existsAlready = false;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            course = this.Courses.First(c => c.Id == tableDataMatch.Groups[1].Value.Trim());
                        }

                        tableDataMatch = tableDataMatch.NextMatch();
                        tableDataMatch = tableDataMatch.NextMatch();

                        response = await this.SyllabusHttpClient.PostAsync(
                            "http://sab.adm.kanazawa-u.ac.jp/search/detail/detail.asp",
                            new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                            {
                            new KeyValuePair<string, string>("key_number", onclickRegex.Match(tableDataMatch.Groups[1].Value).Groups[1].Value)
                            }));

                        if (!response.IsSuccessStatusCode)
                        {
                            continue;
                        }
                    }
                    #endregion

                    ////course.SubjectType = subjectType;

                    html = await this.ReadHtmlAsShiftJis(response);

                    var kamokuTableText = kamokuTableRegex.Match(html).Groups[1].Value;

                    #region 科目の詳細ページからの情報取得
                    {
                        #region SubjectType, SyllabusSchoolOrDivision
                        {
                            ////course.SyllabusSchoolOrDivision = MainWindow.GetHtmlDecodedAndFormattedString(schoolOrDivisionRegex.Match(html).Groups[1].Value, false);

                            var match = subjectTypeAndSchoolOrDivisionRegex.Match(html);

                            course.RawSubjectType = match.Groups["subjectType"].Value;
                            course.RawSyllabusSchoolOrDivision = match.Groups["schoolOrDivision"].Value;

                            ////course.JpSubjectType = MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["jpSubjectType"].Value, true).Trim();
                            ////course.EnSubjectType = MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["enSubjectType"].Value, true).Trim();
                            ////course.JpSyllabusSchoolOrDivision = MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["jpSchoolOrDivision"].Value, true).Trim();
                            ////course.EnSyllabusSchoolOrDivision = MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["enSchoolOrDivision"].Value, true).Trim();
                        }
                        #endregion

                        #region kamokuテーブルから
                        {
                            Match kamokuTableRowMatch;
                            Match kamokuTableDataMatch;

                            kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText);
                            kamokuTableDataMatch = kamokuSingleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);
                            course.SyllabusTitle = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, false);
                            course.IsCityCollege = cityCollegeRegex.Match(kamokuTableDataMatch.Groups[1].Value).Success;

                            ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                            kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();

                            if (kamokuTableRowMatch.Value.Contains("過去の同一科目"))
                            {
                                kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                            }

                            kamokuTableDataMatch = kamokuSingleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);
                            course.SyllabusInstructor = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, false);

                            ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                            kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                            kamokuTableDataMatch = kamokuSingleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);
                            course.SyllabusNumberingCode =
                                MainWindow.GetHtmlDecodedAndFormattedString(numberingCodeTagRegex.Replace(kamokuTableDataMatch.Groups[1].Value, string.Empty), false);

                            ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                            kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                            kamokuTableDataMatch = kamokuDoubleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);
                            course.SyllabusCategory = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[2].Value, false);

                            ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                            kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                            kamokuTableDataMatch = kamokuDoubleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);

                            try
                            {
                                course.SyllabusLectureForm = NullableLectureFormToStringConverter.ConvertBack(MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, false));
                            }
                            catch
                            {
                                course.SyllabusLectureForm = null;
                            }

                            course.SyllabusAssignedYear = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[2].Value, false);

                            #region MaxSize, UnitType, OffersOfEachQuarter
                            {
                                ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                                kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                                kamokuTableDataMatch = kamokuDoubleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);

                                int? maxSize = TryParseNullableInt(MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, false));

                                if (existsAlready && course.MaxSize.HasValue && course.MaxSize != maxSize)
                                {
                                    this.AddLogString("シラバスから登録情報の変更を行いました。(ID:" + course.Id + ", MaxSize:" + course.MaxSize.Value + "→" + maxSize + ")");
                                }

                                course.MaxSize = maxSize;

                                string semesterString;
                                var newLineIndex = kamokuTableDataMatch.Groups[2].Value.IndexOf("<br>");

                                if (newLineIndex >= 0)
                                {
                                    semesterString = kamokuTableDataMatch.Groups[2].Value.Substring(0, newLineIndex).Trim();
                                }
                                else
                                {
                                    semesterString = kamokuTableDataMatch.Groups[2].Value.Trim();
                                }

                                UnitType? unitType = UnitType.Quarter;

                                if (semesterString.Contains("通年"))
                                {
                                    course.OffersInQ1 = true;
                                    course.OffersInQ2 = true;
                                    course.OffersInQ3 = true;
                                    course.OffersInQ4 = true;

                                    unitType = UnitType.Year;
                                }
                                else if (semesterString.Contains("前期"))
                                {
                                    course.OffersInQ1 = true;
                                    course.OffersInQ2 = true;
                                    course.OffersInQ3 = false;
                                    course.OffersInQ4 = false;

                                    if (semesterString.Contains("前半"))
                                    {
                                        course.OffersInQ2 = false;
                                    }
                                    else if (semesterString.Contains("後半"))
                                    {
                                        course.OffersInQ1 = false;
                                    }
                                    else
                                    {
                                        unitType = UnitType.Semester;
                                    }
                                }
                                else if (semesterString.Contains("後期"))
                                {
                                    course.OffersInQ1 = false;
                                    course.OffersInQ2 = false;
                                    course.OffersInQ3 = true;
                                    course.OffersInQ4 = true;

                                    if (semesterString.Contains("前半"))
                                    {
                                        course.OffersInQ4 = false;
                                    }
                                    else if (semesterString.Contains("後半"))
                                    {
                                        course.OffersInQ3 = false;
                                    }
                                    else
                                    {
                                        unitType = UnitType.Semester;
                                    }
                                }

                                if (existsAlready && course.UnitType.HasValue && course.UnitType != unitType)
                                {
                                    this.AddLogString("シラバスから登録情報の変更を行いました。(ID:" + course.Id + ", UnitType:" + course.UnitType.Value + "→" + unitType + ")");
                                }

                                course.UnitType = unitType;
                            }
                            #endregion

                            #region DayOfWeek, Period, Credit
                            {
                                ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                                kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                                kamokuTableDataMatch = kamokuDoubleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);

                                var dayOfWeekAndPeriodString = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, false);
                                DayOfWeek dayOfWeek = DayOfWeek.None;
                                int? period = null;

                                if (dayOfWeekAndPeriodString.Contains("集中"))
                                {
                                    dayOfWeek = DayOfWeek.Intensive;
                                    period = 0;
                                }
                                else
                                {
                                    var dayOfWeekAndPeriodMatch = dayOfWeekAndPeriodRegex.Match(dayOfWeekAndPeriodString);

                                    if (dayOfWeekAndPeriodMatch.Success)
                                    {
                                        try
                                        {
                                            dayOfWeek = DayOfWeekToShortTextConverter.ConvertBack(dayOfWeekAndPeriodMatch.Groups["dayOfWeek"].Value);
                                        }
                                        catch
                                        {
                                            dayOfWeek = DayOfWeek.None;
                                        }

                                        period = TryParseNullableInt(dayOfWeekAndPeriodMatch.Groups["period"].Value);
                                    }
                                }

                                if (existsAlready && course.DayOfWeek != dayOfWeek)
                                {
                                    this.AddLogString("シラバスから登録情報の変更を行いました。(ID:" + course.Id + ", DayOfWeek:" + course.DayOfWeek + "→" + dayOfWeek + ")");
                                }

                                course.DayOfWeek = dayOfWeek;

                                if (existsAlready && course.Period != period)
                                {
                                    this.AddLogString("シラバスから登録情報の変更を行いました。(ID:" + course.Id + ", Period:" + course.Period + "→" + period + ")");
                                }

                                course.Period = period;

                                var credit = TryParseNullableDecimal(MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[2].Value, false));

                                if (existsAlready && course.Credit.HasValue && course.Credit != credit)
                                {
                                    this.AddLogString("シラバスから登録情報の変更を行いました。(ID:" + course.Id + ", Credit:" + course.Credit.Value + "→" + credit + ")");
                                }

                                course.Credit = credit;
                            }
                            #endregion

                            ////kamokuTableRowMatch = kamokuTableRowRegex.Match(kamokuTableText, kamokuTableRowMatch.Index + 1);
                            kamokuTableRowMatch = kamokuTableRowMatch.NextMatch();
                            kamokuTableDataMatch = kamokuSingleDetailRegex.Match(kamokuTableRowMatch.Groups[1].Value);
                            ////var keywordsString = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value.Replace("<br>", Environment.NewLine));
                            course.SyllabusKeyword = MainWindow.GetHtmlDecodedAndFormattedString(kamokuTableDataMatch.Groups[1].Value, true);
                        }
                        #endregion

                        #region 本文データから
                        {
                            for (var topicMatch = topicRegex.Match(html);
                                topicMatch.Success;
                                topicMatch = topicMatch.NextMatch())
                            {
                                var title = topicMatch.Groups["title"].Value;

                                if (title.Contains("授業の主題"))
                                {
                                    course.SyllabusTopic = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("授業の目標"))
                                {
                                    course.SyllabusObjective = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("学生の学習目標"))
                                {
                                    course.SyllabusLearningOutcomes = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("授業の概要"))
                                {
                                    course.SyllabusOutline = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("講義スケジュール"))
                                {
                                    course.RawSyllabusSchedule = MainWindow.ReplaceAsciiControlCode(topicMatch.Groups["body"].Value);
                                }
                                else if (title.Contains("評価の方法"))
                                {
                                    course.SyllabusGradingMethod
                                        = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value.Replace(MainWindow.GradingMethodTemplate, string.Empty), true);
                                }
                                else if (title.Contains("評価の割合"))
                                {
                                    course.RawSyllabusGradingRate = MainWindow.ReplaceAsciiControlCode(topicMatch.Groups["body"].Value);
                                }
                                else if (title.Contains("テキスト・教材・参考書等"))
                                {
                                    course.SyllabusTeachingMaterials
                                        = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true).Replace(MainWindow.SyllabusLibraryLinkText, string.Empty).Trim();
                                }
                                else if (title.Contains("その他履修上の注意事項や学習上の助言"))
                                {
                                    course.SyllabusOthers = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("オフィスアワー等（学生からの質問への対応方法等）"))
                                {
                                    course.SyllabusConsultationTime = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("履修条件"))
                                {
                                    course.SyllabusPrerequisites = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("受講者数調整の方法") || title.Contains("適正人数と受講者の調整方法"))
                                {
                                    course.SyllabusMethodForAdjustingClassSize = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("開放科目"))
                                {
                                    course.SyllabusSubjectOfOpen = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("関連科目"))
                                {
                                    course.SyllabusRelatedCourses = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("カリキュラムの中の位置づけ（関連科目、履修条件等）"))
                                {
                                    course.SyllabusRelationsWithTheOtherCoursesInTheCurriculum = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else if (title.Contains("特記事項"))
                                {
                                    course.SyllabusSpecialNote = MainWindow.GetHtmlDecodedAndFormattedString(topicMatch.Groups["body"].Value, true);
                                }
                                else
                                {
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    if (!existsAlready)
                    {
                        this.AddCourse(course);
                    }
                }

                #region 次ページ表示
                {
                    nowpage++;

                    if (nowpage > ((records - 1) / 50) + 1)
                    {
                        break;
                    }

                    for (var index = 0; index < postData.Count; index++)
                    {
                        if (postData[index].Key == "int_nowpage")
                        {
                            UpdatePostData(postData, index, nowpage.ToString());
                            ////index--;
                            break;
                        }
                    }

                    response = await this.SyllabusHttpClient.PostAsync(
                        "http://sab.adm.kanazawa-u.ac.jp/search/list/list.asp",
                        CreatePresentPostDataStringContent(postData));

                    html = await this.ReadHtmlAsShiftJis(response);
                }
                #endregion
            }

            this.AddLogString("シラバスから情報を読み込みました。");
            this.StatusMessage.Content = string.Empty;

            this.Filter.UpdateFilterItem();
        }

        /// <summary>
        /// 科目一覧を指定されたパスに書き出します。
        /// </summary>
        /// <param name="path"> 書き出すファイルのパス。 </param>
        /// <returns> 書き出しが成功したかどうか。 </returns>
        public async Task<bool> SaveCourses(string path)
        {
            if (File.Exists(path))
            {
                if (MessageBox.Show(path + " の内容を上書きしてもいいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                {
                    this.AddLogString("科目データの書き出しを中止しました。");
                    return false;
                }
            }

            this.StatusMessage.Content = "科目データを書き出しています。";

            try
            {
                await this.Dispatcher.BeginInvoke(new Action(() => { MainWindow.SaveCoursesData(path, this.Courses); }));
                ////MainWindow.SaveCoursesData(path, this.Courses);
            }
            catch (Exception e)
            when (e is UnauthorizedAccessException
                || e is ArgumentException
                || e is ArgumentNullException
                || e is DirectoryNotFoundException
                || e is IOException
                || e is PathTooLongException
                || e is System.Security.SecurityException)
            {
                this.AddLogString("科目データの書き出しに失敗しました。");
                this.StatusMessage.Content = string.Empty;
                return false;
            }

            this.AddLogString("科目データを " + path + " に書き出しました。");
            this.StatusMessage.Content = string.Empty;
            return true;
        }

        /// <summary>
        /// 科目一覧を指定されたパスから読み込みます。
        /// </summary>
        /// <param name="path"> 読み込むファイルのパス。 </param>
        /// <returns> 読み込みが成功したかどうか。 </returns>
        public async Task<bool> LoadCourses(string path)
        {
            if (!File.Exists(path))
            {
                this.AddLogString("科目データの読み込みを中止しました。");
                return false;
            }

            if (this.Courses.Count > 0)
            {
                if (MessageBox.Show("現在の科目データを破棄してもいいですか？", "確認", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.Cancel)
                {
                    this.AddLogString("科目データの読み込みを中止しました。");
                    return false;
                }
            }

            this.StatusMessage.Content = "科目データを読み込んでいます。";

            try
            {
                this.Courses.Clear();
                this.UpdateViewCount();

                var count = 0;

                foreach (var c in MainWindow.LoadCoursesData(path))
                {
                    count++;
                    this.StatusMessage.Content = "科目データを読み込んでいます。 ( " + count + " 件目 )";
                    ////this.StatusMessage.Content= "科目データを読み込んでいます。 ( " + count + " / " + /* 全件数 */ + " 件 )";
                    this.AddCourse(await c);
                }
            }
            catch (Exception e)
            when (e is ArgumentException
                || e is ArgumentNullException)
            {
                this.AddLogString("科目データの読み込みに失敗しました。");
                this.StatusMessage.Content = string.Empty;
                return false;
            }

            this.Filter.UpdateFilterItem();

            this.AddLogString("科目データを " + path + " から読み込みました。");
            this.StatusMessage.Content = string.Empty;
            return true;
        }

        #endregion

        #region 静的メソッド(private)

        /// <summary>
        /// 一部のASCII制御文字を変換します。
        /// </summary>
        /// <param name="value"> 変換する文字列。 </param>
        /// <returns> 変換後の文字列。 </returns>
        private static string ReplaceAsciiControlCode(string value)
            => string.Concat(value.Where(c => !UnnecessaryAsciiControlCodes.Any(ascii => ascii == c)));

        /////// <summary>
        /////// 文字列を <see cref="bool?"/> 型に変換します。
        /////// 変換できる場合は <see cref="bool"/> 型の値、変換できない場合は <see cref="null"/> が返されます。
        /////// </summary>
        /////// <param name="value"> 変換する文字列。 </param>
        /////// <returns> 変換結果。 </returns>
        ////private static bool? TryParseNullableBoolean(string value)
        ////{
        ////    bool result;
        ////    return bool.TryParse(value, out result) ? result : default(bool?);
        ////}

            /////// <summary>
            /////// 文字列を指定した列挙型に変換します。
            /////// 変換できる場合は指定した列挙型の値、変換できない場合は <see cref="null"/> が返されます。
            /////// </summary>
            /////// <typeparam name="T"> 指定する列挙型。 </typeparam>
            /////// <param name="value"> 変換する文字列。 </param>
            /////// <returns> 変換結果。 </returns>
            ////private static T? TryParseNullableEnum<T>(string value) where T : struct
            //// {
            ////    T result;
            ////    return Enum.TryParse(value, out result) ? result : default(T?);
            ////}

        /// <summary>
        /// 科目一覧を実際に書き出します。
        /// </summary>
        /// <param name="path"> 書き出すファイルのパス。 </param>
        /// <param name="courses"> 書き出す科目一覧。 </param>
        private static void SaveCoursesData(string path, ObservableCollection<Course> courses)
        {
            var serializer = new XmlSerializer(typeof(ObservableCollection<Course>));

            using (var output = new StreamWriter(path, false, new UTF8Encoding(false)))
            {
                serializer.Serialize(output, courses);

                #region コメントアウト(シリアライズ自力実装:未完成)

                ////using (var writer = XmlWriter.Create(output, new XmlWriterSettings() { OmitXmlDeclaration = false }))
                ////{
                ////    ////serializer.Serialize(writer, courses);

                ////    writer.WriteStartElement("ArrayOfCourse");

                ////    {
                ////        foreach (var course in courses)
                ////        {
                ////            writer.WriteStartElement(nameof(Course));

                ////            #region Course
                ////            {
                ////                writer.WriteStartElement(nameof(Course.RegisterNumber));
                ////                writer.WriteValue(course.RegisterNumber.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.RawSubjectType));
                ////                writer.WriteValue(course.RawSubjectType);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.Id));
                ////                writer.WriteValue(course.Id);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.DayOfWeek));
                ////                writer.WriteValue(course.DayOfWeek.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.Period));
                ////                writer.WriteValue(course.Period.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.UnitType));
                ////                writer.WriteValue(course.UnitType.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.Credit));
                ////                writer.WriteValue(course.Credit.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.MaxSize));
                ////                writer.WriteValue(course.MaxSize.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.RegisteredSize));
                ////                writer.WriteValue(course.RegisteredSize.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.IsDoneLottery));
                ////                writer.WriteValue(course.IsDoneLottery.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.RawSyllabusSchoolOrDivision));
                ////                writer.WriteValue(course.RawSyllabusSchoolOrDivision);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusTitle));
                ////                writer.WriteValue(course.SyllabusTitle);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.IsCityCollege));
                ////                writer.WriteValue(course.IsCityCollege.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusInstructor));
                ////                writer.WriteValue(course.SyllabusInstructor);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusNumberingCode));
                ////                writer.WriteValue(course.SyllabusNumberingCode);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusCategory));
                ////                writer.WriteValue(course.SyllabusCategory);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusLectureForm));
                ////                writer.WriteValue(course.SyllabusLectureForm.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusAssignedYear));
                ////                writer.WriteValue(course.SyllabusAssignedYear);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.OffersInQ1));
                ////                writer.WriteValue(course.OffersInQ1.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.OffersInQ2));
                ////                writer.WriteValue(course.OffersInQ2.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.OffersInQ3));
                ////                writer.WriteValue(course.OffersInQ3.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.OffersInQ4));
                ////                writer.WriteValue(course.OffersInQ4.ToString());
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusKeyword));
                ////                writer.WriteValue(course.SyllabusKeyword);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusTopic));
                ////                writer.WriteValue(course.SyllabusTopic);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusObjective));
                ////                writer.WriteValue(course.SyllabusObjective);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusLearningOutcomes));
                ////                writer.WriteValue(course.SyllabusLearningOutcomes);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusOutline));
                ////                writer.WriteValue(course.SyllabusOutline);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusGradingMethod));
                ////                writer.WriteValue(course.SyllabusGradingMethod);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.RawSyllabusGradingRate));
                ////                writer.WriteValue(course.RawSyllabusGradingRate);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusTeachingMaterials));
                ////                writer.WriteValue(course.SyllabusTeachingMaterials);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusOthers));
                ////                writer.WriteValue(course.SyllabusOthers);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusConsultationTime));
                ////                writer.WriteValue(course.SyllabusConsultationTime);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusMethodForAdjustingClassSize));
                ////                writer.WriteValue(course.SyllabusMethodForAdjustingClassSize);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusSubjectOfOpen));
                ////                writer.WriteValue(course.SyllabusSubjectOfOpen);
                ////                writer.WriteEndElement();

                ////                writer.WriteStartElement(nameof(Course.SyllabusSpecialNote));
                ////                writer.WriteValue(course.SyllabusSpecialNote);
                ////                writer.WriteEndElement();
                ////            }
                ////            #endregion

                ////            writer.WriteEndElement();
                ////        }
                ////    }

                ////    writer.WriteEndElement();
                ////}

                #endregion
            }
        }

        /// <summary>
        /// 科目一覧を実際に読み込みます。
        /// </summary>
        /// <param name="path"> 読み込むファイルのパス。 </param>
        /// <returns> 読み込んだ科目一覧。 </returns>
        private static IEnumerable<Task<Course>> LoadCoursesData(string path)
        {
            ////var serializer = new XmlSerializer(typeof(ObservableCollection<Course>));

            using (var input = new StreamReader(path, new UTF8Encoding(false)))
            {
                ////return (ObservableCollection<Course>)serializer.Deserialize(input);

                using (var reader = XmlReader.Create(input, new XmlReaderSettings() { Async = true, IgnoreComments = true, IgnoreWhitespace = true }))
                {
                    ////return (ObservableCollection<Course>)serializer.Deserialize(reader);

                    reader.Read();

                    reader.ReadStartElement("ArrayOfCourse");

                    while (reader.IsStartElement())
                    {
                        reader.ReadStartElement(nameof(Course));

                        yield return LoadCourse(reader);

                        reader.ReadEndElement();
                    }

                    reader.ReadEndElement();
                }
            }
        }

        /// <summary>
        /// 現在の科目についての情報を読み込みます。
        /// </summary>
        /// <param name="reader"> 使用するXMLリーダー。 </param>
        /// <returns> 科目についての情報。 </returns>
        private static async Task<Course> LoadCourse(XmlReader reader)
        {
            var course = new Course();

            course.RegisterNumber = ReadElementContent<int?>(reader, nameof(Course.RegisterNumber), r => r.ReadContentAsInt());
            course.RawSubjectType = await ReadElementContentAsync(reader, nameof(Course.RawSubjectType), async r => await r.ReadContentAsStringAsync());
            course.RowId = await ReadElementContentAsync(reader, nameof(Course.RowId), async r => await r.ReadContentAsStringAsync());
            course.Lctcd = await ReadElementContentAsync(reader, nameof(Course.Lctcd), async r => await r.ReadContentAsStringAsync());
            course.Id = await ReadElementContentAsync(reader, nameof(Course.Id), async r => await r.ReadContentAsStringAsync());
            course.Title = await ReadElementContentAsync(reader, nameof(Course.Title), async r => await r.ReadContentAsStringAsync());
            course.Instructor = await ReadElementContentAsync(reader, nameof(Course.Instructor), async r => await r.ReadContentAsStringAsync());
            course.DayOfWeek = await ReadElementContentAsync(reader, nameof(Course.DayOfWeek), async r => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), await r.ReadContentAsStringAsync()));
            course.Period = ReadElementContent<int?>(reader, nameof(Course.Period), r => r.ReadContentAsInt());
            course.UnitType = await ReadElementContentAsync<UnitType?>(reader, nameof(Course.UnitType), async r => (UnitType)Enum.Parse(typeof(UnitType), await r.ReadContentAsStringAsync()));
            course.Credit = ReadElementContent<decimal?>(reader, nameof(Course.Credit), r => r.ReadContentAsDecimal());
            course.MaxSize = ReadElementContent<int?>(reader, nameof(Course.MaxSize), r => r.ReadContentAsInt());
            course.RegisteredSize = ReadElementContent<int?>(reader, nameof(Course.RegisteredSize), r => r.ReadContentAsInt());
            course.IsDoneLottery = ReadElementContent<bool?>(reader, nameof(Course.IsDoneLottery), r => r.ReadContentAsBoolean());

            course.RawSyllabusSchoolOrDivision = await ReadElementContentAsync(reader, nameof(Course.RawSyllabusSchoolOrDivision), async r => await r.ReadContentAsStringAsync());
            course.SyllabusTitle = await ReadElementContentAsync(reader, nameof(Course.SyllabusTitle), async r => await r.ReadContentAsStringAsync());
            course.IsCityCollege = ReadElementContent<bool?>(reader, nameof(Course.IsCityCollege), r => r.ReadContentAsBoolean());
            course.SyllabusInstructor = await ReadElementContentAsync(reader, nameof(Course.SyllabusInstructor), async r => await r.ReadContentAsStringAsync());
            course.SyllabusNumberingCode = await ReadElementContentAsync(reader, nameof(Course.SyllabusNumberingCode), async r => await r.ReadContentAsStringAsync());
            course.SyllabusCategory = await ReadElementContentAsync(reader, nameof(Course.SyllabusCategory), async r => await r.ReadContentAsStringAsync());
            course.SyllabusLectureForm = await ReadElementContentAsync<LectureForm?>(reader, nameof(Course.SyllabusLectureForm), async r => (LectureForm)Enum.Parse(typeof(LectureForm), await r.ReadContentAsStringAsync()));
            course.SyllabusAssignedYear = await ReadElementContentAsync(reader, nameof(Course.SyllabusAssignedYear), async r => await r.ReadContentAsStringAsync());
            course.OffersInQ1 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ1), r => r.ReadContentAsBoolean());
            course.OffersInQ2 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ2), r => r.ReadContentAsBoolean());
            course.OffersInQ3 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ3), r => r.ReadContentAsBoolean());
            course.OffersInQ4 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ4), r => r.ReadContentAsBoolean());
            course.SyllabusKeyword = await ReadElementContentAsync(reader, nameof(Course.SyllabusKeyword), async r => await r.ReadContentAsStringAsync());
            course.SyllabusTopic = await ReadElementContentAsync(reader, nameof(Course.SyllabusTopic), async r => await r.ReadContentAsStringAsync());
            course.SyllabusObjective = await ReadElementContentAsync(reader, nameof(Course.SyllabusObjective), async r => await r.ReadContentAsStringAsync());
            course.SyllabusLearningOutcomes = await ReadElementContentAsync(reader, nameof(Course.SyllabusLearningOutcomes), async r => await r.ReadContentAsStringAsync());
            course.SyllabusOutline = await ReadElementContentAsync(reader, nameof(Course.SyllabusOutline), async r => await r.ReadContentAsStringAsync());
            course.RawSyllabusSchedule = await ReadElementContentAsync(reader, nameof(Course.RawSyllabusSchedule), async r => await r.ReadContentAsStringAsync());
            course.SyllabusGradingMethod = await ReadElementContentAsync(reader, nameof(Course.SyllabusGradingMethod), async r => await r.ReadContentAsStringAsync());
            course.RawSyllabusGradingRate = await ReadElementContentAsync(reader, nameof(Course.RawSyllabusGradingRate), async r => await r.ReadContentAsStringAsync());
            course.SyllabusTeachingMaterials = await ReadElementContentAsync(reader, nameof(Course.SyllabusTeachingMaterials), async r => await r.ReadContentAsStringAsync());
            course.SyllabusOthers = await ReadElementContentAsync(reader, nameof(Course.SyllabusOthers), async r => await r.ReadContentAsStringAsync());
            course.SyllabusConsultationTime = await ReadElementContentAsync(reader, nameof(Course.SyllabusConsultationTime), async r => await r.ReadContentAsStringAsync());
            course.SyllabusConsultationTime = await ReadElementContentAsync(reader, nameof(Course.SyllabusConsultationTime), async r => await r.ReadContentAsStringAsync());
            course.SyllabusMethodForAdjustingClassSize = await ReadElementContentAsync(reader, nameof(Course.SyllabusMethodForAdjustingClassSize), async r => await r.ReadContentAsStringAsync());
            course.SyllabusSubjectOfOpen = await ReadElementContentAsync(reader, nameof(Course.SyllabusSubjectOfOpen), async r => await r.ReadContentAsStringAsync());
            course.SyllabusRelatedCourses = await ReadElementContentAsync(reader, nameof(Course.SyllabusRelatedCourses), async r => await r.ReadContentAsStringAsync());
            course.SyllabusRelationsWithTheOtherCoursesInTheCurriculum = await ReadElementContentAsync(reader, nameof(Course.SyllabusRelationsWithTheOtherCoursesInTheCurriculum), async r => await r.ReadContentAsStringAsync());
            course.SyllabusSpecialNote = await ReadElementContentAsync(reader, nameof(Course.SyllabusSpecialNote), async r => await r.ReadContentAsStringAsync());

            #region コメントアウト

            ////course.RegisterNumber = ReadElementContent<int?>(reader, nameof(Course.RegisterNumber));
            ////course.RawSubjectType = ReadElementContent<string>(reader, nameof(Course.RawSubjectType));
            ////course.Id = ReadElementContent<string>(reader, nameof(Course.Id));
            ////course.DayOfWeek = ReadElementContent<DayOfWeek>(reader, nameof(Course.DayOfWeek));
            //////var dayOfWeek = ReadElementContent<string>(reader, nameof(Course.DayOfWeek));
            //////course.DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOfWeek);
            ////course.Period = ReadElementContent<int?>(reader, nameof(Course.Period));
            ////course.UnitType = ReadElementContent<UnitType?>(reader, nameof(Course.UnitType));
            ////////var unitType = ReadElementContent<string>(reader, nameof(Course.UnitType));
            ////////course.UnitType = unitType == default(string) ? default(UnitType?) : (UnitType)Enum.Parse(typeof(UnitType), unitType);
            ////course.Credit = ReadElementContent<decimal?>(reader, nameof(Course.Credit));
            ////course.MaxSize = ReadElementContent<int?>(reader, nameof(Course.MaxSize));
            ////course.RegisteredSize = ReadElementContent<int?>(reader, nameof(Course.RegisteredSize));
            ////course.IsDoneLottery = ReadElementContent<bool?>(reader, nameof(Course.IsDoneLottery));
            ////course.RawSyllabusSchoolOrDivision = ReadElementContent<string>(reader, nameof(Course.RawSyllabusSchoolOrDivision));
            ////course.SyllabusTitle = ReadElementContent<string>(reader, nameof(Course.SyllabusTitle));
            ////course.IsCityCollege = ReadElementContent<bool?>(reader, nameof(Course.IsCityCollege));
            ////course.SyllabusInstructor = ReadElementContent<string>(reader, nameof(Course.SyllabusInstructor));
            ////course.SyllabusNumberingCode = ReadElementContent<string>(reader, nameof(Course.SyllabusNumberingCode));
            ////course.SyllabusCategory = ReadElementContent<string>(reader, nameof(Course.SyllabusCategory));
            ////course.SyllabusLectureForm = ReadElementContent<LectureForm?>(reader, nameof(Course.SyllabusLectureForm));
            ////////var lectureForm = ReadElementContent<string>(reader, nameof(Course.SyllabusLectureForm));
            ////////course.SyllabusLectureForm = lectureForm == default(string) ? default(LectureForm?) : (LectureForm)Enum.Parse(typeof(LectureForm), lectureForm);
            ////course.SyllabusAssignedYear = ReadElementContent<string>(reader, nameof(Course.SyllabusAssignedYear));
            ////course.OffersInQ1 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ1));
            ////course.OffersInQ2 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ2));
            ////course.OffersInQ3 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ3));
            ////course.OffersInQ4 = ReadElementContent<bool?>(reader, nameof(Course.OffersInQ4));
            ////course.SyllabusKeyword = ReadElementContent<string>(reader, nameof(Course.SyllabusKeyword));
            ////course.SyllabusTopic = ReadElementContent<string>(reader, nameof(Course.SyllabusTopic));
            ////course.SyllabusObjective = ReadElementContent<string>(reader, nameof(Course.SyllabusObjective));
            ////course.SyllabusLearningOutcomes = ReadElementContent<string>(reader, nameof(Course.SyllabusLearningOutcomes));
            ////course.SyllabusOutline = ReadElementContent<string>(reader, nameof(Course.SyllabusOutline));
            ////course.SyllabusGradingMethod = ReadElementContent<string>(reader, nameof(Course.SyllabusGradingMethod));
            ////course.RawSyllabusGradingRate = ReadElementContent<string>(reader, nameof(Course.RawSyllabusGradingRate));
            ////course.SyllabusTeachingMaterials = ReadElementContent<string>(reader, nameof(Course.SyllabusTeachingMaterials));
            ////course.SyllabusOthers = ReadElementContent<string>(reader, nameof(Course.SyllabusOthers));
            ////course.SyllabusConsultationTime = ReadElementContent<string>(reader, nameof(Course.SyllabusConsultationTime));
            ////course.SyllabusMethodForAdjustingClassSize = ReadElementContent<string>(reader, nameof(Course.SyllabusMethodForAdjustingClassSize));
            ////course.SyllabusSubjectOfOpen = ReadElementContent<string>(reader, nameof(Course.SyllabusSubjectOfOpen));
            ////course.SyllabusSpecialNote = ReadElementContent<string>(reader, nameof(Course.SyllabusSpecialNote));

            #endregion

            return course;
        }

        /// <summary>
        /// 指定した要素名を持つ現在の要素を指定した型として返します。
        /// </summary>
        /// <typeparam name="T"> 要素の型。 </typeparam>
        /// <param name="reader"> 使用するXMLリーダー。 </param>
        /// <param name="elementName"> 要素名。 </param>
        /// <param name="readFunction"> 要素を読み取る関数。 </param>
        /// <returns> 要素。 </returns>
        private static T ReadElementContent<T>(XmlReader reader, string elementName, Func<XmlReader, T> readFunction)
        {
            var isSkip = reader.IsEmptyElement;

            try
            {
                reader.ReadStartElement(elementName);
            }
            catch (XmlException)
            {
                isSkip = true;
            }

            if (isSkip)
            {
                return default(T);
            }

            var result = readFunction(reader);
            reader.ReadEndElement();

            return result;
        }

        /// <summary>
        /// 指定した要素名を持つ現在の要素を指定した待機可能な型として返します。
        /// </summary>
        /// <typeparam name="T"> 要素の待機可能なな型。 </typeparam>
        /// <param name="reader"> 使用するXMLリーダー。 </param>
        /// <param name="elementName"> 要素名。 </param>
        /// <param name="readFunction"> 要素を読み取る関数。 </param>
        /// <returns> 要素。 </returns>
        private static async Task<T> ReadElementContentAsync<T>(XmlReader reader, string elementName, Func<XmlReader, Task<T>> readFunction)
        {
            var isSkip = reader.IsEmptyElement;

            try
            {
                reader.ReadStartElement(elementName);
            }
            catch (XmlException)
            {
                isSkip = true;
            }

            if (isSkip)
            {
                ////return default(T);
                return await Task.Run(() => default(T));
            }

            ////var result = readFunction(reader);
            var result = await readFunction(reader);
            reader.ReadEndElement();

            return result;
        }

        /////// <summary>
        /////// 指定した要素名を持つ現在の要素を指定した型として返します。
        /////// </summary>
        /////// <typeparam name="T"> 要素の型。 </typeparam>
        /////// <param name="reader"> 使用するXMLリーダー。 </param>
        /////// <param name="elementName"> 要素名。 </param>
        /////// <param name="readFunction"> 要素を読み取る関数。 </param>
        /////// <returns> 要素。 </returns>
        ////private static T ReadElementContent<T>(XmlReader reader, string elementName, Func<XmlReader, T> readFunction)
        ////    => ReadElementContentBase(reader, elementName, readFunction, default(T));

        /////// <summary>
        /////// 指定した要素名を持つ現在の要素を指定した待機可能な型として返します。
        /////// </summary>
        /////// <typeparam name="T"> 要素の待機可能なな型。 </typeparam>
        /////// <param name="reader"> 使用するXMLリーダー。 </param>
        /////// <param name="elementName"> 要素名。 </param>
        /////// <param name="readFunction"> 要素を読み取る関数。 </param>
        /////// <returns> 要素。 </returns>
        ////private static async Task<T> ReadElementContentAsync<T>(XmlReader reader, string elementName, Func<XmlReader, Task<T>> readFunction)
        ////    => await ReadElementContentBase(reader, elementName, readFunction, Task.Run(() => default(T)));

        /////// <summary>
        /////// 指定した要素名を持つ現在の要素を指定した型として返します。
        /////// </summary>
        /////// <typeparam name="T"> 要素の型。 </typeparam>
        /////// <param name="reader"> 使用するXMLリーダー。 </param>
        /////// <param name="elementName"> 要素名。 </param>
        /////// <param name="readFunction"> 要素を読み取る関数。 </param>
        /////// <param name="defaultValue"> 要素の既定値。 </param>
        /////// <returns> 要素。 </returns>
        ////private static T ReadElementContentBase<T>(XmlReader reader, string elementName, Func<XmlReader, T> readFunction, T defaultValue)
        ////{
        ////    var isSkip = reader.IsEmptyElement;

        ////    try
        ////    {
        ////        reader.ReadStartElement(elementName);
        ////    }
        ////    catch (XmlException)
        ////    {
        ////        isSkip = true;
        ////    }

        ////    if (isSkip)
        ////    {
        ////        return defaultValue;
        ////    }

        ////    var result = readFunction(reader);
        ////    ////result = (T)reader.ReadContentAs(typeof(T), null);
        ////    reader.ReadEndElement();

        ////    return result;
        ////}

        #endregion

        #region メソッド(private)
        
        /// <summary>
        /// 科目の追加を試みます。
        /// </summary>
        /// <param name="retry"> リトライ回数。 </param>
        /// <param name="targetFunction"> 追加したい科目をフィルター処理する関数。 </param>
        /// <returns> <see cref="Task"/> . </returns>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Task<IEnumerable<Course>> は正当なデザインである。")]
        private async Task<IEnumerable<Course>> RegisterCourses(int retry, Func<Course, bool> targetFunction)
        {
            var result = new List<Course>();

            foreach (var course in this.Courses.Where(targetFunction))
            {
                if (await this.RegisterCourse(retry, course))
                {
                    result.Add(course);
                }
            }

            return result;
        }

        /// <summary>
        /// 個別の科目の追加を試みます。
        /// </summary>
        /// <param name="retry"> リトライ回数。 </param>
        /// <param name="course"> 追加したい科目。 </param>
        /// <returns> <see cref="Task"/> . </returns>
        private async Task<bool> RegisterCourse(int retry, Course course)
        {
            ////var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("lct_cd", course.Lctcd), MainWindow.AddContent });

            ////for (var count = -1; count < retry; count++)
            ////{
            ////    var response = await this.HttpClient.PostAsync("https://risyu.kanazawa-u.ac.jp/risyu/add_regist_rec.asp", content);

            ////    var recHtml = await this.ReadHtmlAsShiftJis(response);
            ////    var match = MainWindow.RegistrationResultRegex.Match(recHtml);

            ////    if (!match.Success)
            ////    {
            ////        continue;
            ////    }

            ////    if (match.Groups["bgcolor"].Value == "b0e0e6" || match.Groups["bgcolor"].Value == "B0E0E6")
            ////    {
            ////        course.IsToAdd = false;
            ////        return true;
            ////    }
            ////    else if (match.Groups["bgcolor"].Value == "FFCCCC" || match.Groups["bgcolor"].Value == "ffcccc")
            ////    {
            ////        continue;
            ////    }
            ////    else
            ////    {
            ////    }
            ////}

            return false;
        }
        ////private async Task<bool> RegisterCourse(int retry, Course course)
        ////{
        ////    var content = new FormUrlEncodedContent(new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("lct_cd", course.Lctcd), MainWindow.AddContent });

        ////    for (var count = -1; count < retry; count++)
        ////    {
        ////        var response = await this.HttpClient.PostAsync("https://risyu.kanazawa-u.ac.jp/risyu/add_regist_rec.asp", content);

        ////        var recHtml = await this.ReadHtmlAsShiftJis(response);
        ////        var match = MainWindow.RegistrationResultRegex.Match(recHtml);

        ////        if (!match.Success)
        ////        {
        ////            continue;
        ////        }

        ////        if (match.Groups["bgcolor"].Value == "b0e0e6" || match.Groups["bgcolor"].Value == "B0E0E6")
        ////        {
        ////            course.IsToAdd = false;
        ////            return true;
        ////        }
        ////        else if (match.Groups["bgcolor"].Value == "FFCCCC" || match.Groups["bgcolor"].Value == "ffcccc")
        ////        {
        ////            continue;
        ////        }
        ////        else
        ////        {
        ////        }
        ////    }

        ////    return false;
        ////}


        /////// <summary>
        /////// 一定時間ごとに自動で科目の追加を試みます。
        /////// </summary>
        /////// <param name="retry"> リトライ回数。 </param>
        /////// <param name="isReload"> 追加した時に履修希望科目をリロードするかどうか。 </param>
        /////// <returns> <see cref="Task"/> . </returns>
        ////private async Task RegisterCoursesAuto(int retry, bool isReload)
        ////{
        ////    while (true)
        ////    {
        ////        var interval = this.Config.Interval;
        ////        this.StatusInterval.Content = "実行間隔: " + interval + "秒";

        ////        var nextTime = DateTime.Now.AddSeconds(interval);
        ////        this.StatusNext.Content = "次の実行時刻は" + nextTime + "です。";

        ////        var registered = await this.RegisterCourses();

        ////        this.StatusIsRun.Content = "自動監視待機中";

        ////        if (isReload && registered.Count() > 0)
        ////        {
        ////            await this.ApplyRequestalCourseSchedule();
        ////        }

        ////        do
        ////        {
        ////            if (this.TokenSource.IsCancellationRequested)
        ////            {
        ////                return;
        ////            }

        ////            Thread.Sleep(1);
        ////        }
        ////        while (DateTime.Now < nextTime);
        ////    }
        ////}

        /// <summary>
        /// 自動監視状態についてのステータスバーをリセットします。
        /// </summary>
        private void ResetAutoStatus()
        {
            this.StatusIsRun.Content = string.Empty;
            this.StatusInterval.Content = string.Empty;
            this.StatusNext.Content = string.Empty;
        }

        #endregion

        #region メソッド(イベント)

        /// <summary>
        /// 設定表示
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void OpenConfig(object sender, RoutedEventArgs e)
        {
            this.Config.Show();
        }

        /// <summary>
        /// 詳細表示
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void OpenDetail(object sender, RoutedEventArgs e)
        {
            this.Detail.Show();
        }

        /// <summary>
        /// 詳細表示
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void OpenFilter(object sender, RoutedEventArgs e)
        {
            this.Filter.Show();
        }

        /// <summary>
        /// 履修希望科目読み込み
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void ReadSchedule(object sender, RoutedEventArgs e)
        {
            #region コメントアウト

            ////            using (var handler = new HttpClientHandler() { UseCookies = true })
            ////            ////using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://www.yakujihou.org/netmember/html/java_check.html") })
            ////            using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://acanthus.cis.kanazawa-u.ac.jp/") })
            ////            {
            ////                ////client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko");
            ////                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.84 Safari/537.36");
            ////                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*;q=0.8");
            ////                client.DefaultRequestHeaders.Add("Accept-Language", "ja,en-US;q=0.8,en;q=0.6");

            ////                if (this.Config.Interval <= 0)
            ////                {
            ////                    this.Config.Interval = this.Config.DefaultInterval;
            ////                }

            ////                client.Timeout = TimeSpan.FromSeconds(this.Config.Interval);

            ////                ////html = await client.GetStringAsync("https://risyu.kanazawa-u.ac.jp/risyu/edit.asp");

            ////                var response = await client.GetAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/login.php?next=");
            ////                response.EnsureSuccessStatusCode();

            ////                var content = new FormUrlEncodedContent(new Dictionary<string, string>
            ////                    {
            ////                        { "j_username", this.Config.UserID.Text },
            ////                        ////{ "pswd-text", this.Config.UserID.Text },
            ////                        { "j_password", this.Config.UserPassword.Password },
            ////                        ////{ "pswd", this.Config.UserID.Text },
            ////                        ////{ "pswd-text", this.Config.UserID.Text },
            ////                        ////{ "password", this.Config.UserPassword.Text },
            ////                    });

            ////                response = await client.PostAsync("https://ku-sso.cis.kanazawa-u.ac.jp/idp/Authn/UserPassword", content);
            ////                response.EnsureSuccessStatusCode();

            ////                var saml2Html = await response.Content.ReadAsStringAsync();
            ////                var saml2Regex = new Regex("<input[^<]*name=\"(?<name>[^\"]*)\"[^<]*value=\"(?<value>[^\"]*)\"[^<]*>");
            ////                var saml2Dictionary = new Dictionary<string, string>();

            ////                for (var saml2Match = saml2Regex.Match(saml2Html); saml2Match.Success; saml2Match = saml2Match.NextMatch())
            ////                {
            ////                    saml2Dictionary.Add(
            ////                        WebUtility.HtmlDecode(saml2Match.Groups["name"].Value),
            ////                        WebUtility.HtmlDecode(saml2Match.Groups["value"].Value));
            ////                }

            ////                response = await client.PostAsync(
            ////                    "https://acanthus.cis.kanazawa-u.ac.jp/Shibboleth.sso/SAML2/POST",
            ////                    new FormUrlEncodedContent(saml2Dictionary));
            ////                response.EnsureSuccessStatusCode();

            ////                ////await client.PostAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/login.php?next=", content);
            ////                ////await client.PostAsync("https://ku-sso.cis.kanazawa-u.ac.jp/idp/Authn/UserPassword", content);
            ////                ////response = client.PostAsync("https://ku-sso.cis.kanazawa-u.ac.jp/idp/Authn/UserPassword", content).Result;

            ////                ////response = await client.GetAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/u001/index.php");

            ////                ////var response = await client.GetAsync("https://www.yakujihou.org/netmember/html/java_check.html");
            ////                ////response.EnsureSuccessStatusCode();

            ////                ////CookieCollection cookies = handler.CookieContainer.GetCookies(new Uri("https://ku-sso.cis.kanazawa-u.ac.jp/idp/Authn/UserPassword"));
            ////                ////response.EnsureSuccessStatusCode();

            ////                using (var stream = await response.Content.ReadAsStreamAsync())
            ////                ////using (var reader = (new StreamReader(stream, Encoding.GetEncoding("shift_jis"), true)) as TextReader)
            ////                using (var reader = (new StreamReader(stream, Encoding.UTF8, true)) as TextReader)
            ////                {
            ////                    html = await reader.ReadToEndAsync();
            ////                }

            ////                html = await client.GetStringAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/u001/index.php");
            ////            }

            #endregion

            await this.ApplyRequestalCourseSchedule();
            ////this.FormatCoursesGridView();
            ////this.ResizeColumnWidth();

            #region コメントアウト

            ////document.getElementsByName("search").item(index: 0).click();
            ////this.IE.Navigate("javascript:search_window()");

            ////string ScriptString = "javascript:document.getElementById('search').click()";
            ////document.Script.setTimeout(ScriptString, 200);
            ////dynamic d2 = this.IE.Document;
            ////html = d2.Body.innerHTML;

            #endregion
        }

        /// <summary>
        /// 時間割読み込み
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void ReadAllCourses(object sender, RoutedEventArgs e)
        {
            await this.ReadAndAddCourses();
            ////this.ResizeColumnWidth();
        }

        /// <summary>
        /// シラバスから追加情報読み込み
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void ReadFromSyllabus(object sender, RoutedEventArgs e)
        {
            await this.ReadSyllabus();
            ////this.ResizeColumnWidth();
        }

        /// <summary>
        /// 科目一覧の書き出し
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void OutputCourses(object sender, RoutedEventArgs e)
        {
            await this.SaveCourses(this.Config.BackupFilePath.Text);
        }

        /// <summary>
        /// 科目一覧の読み込み
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void InputCourses(object sender, RoutedEventArgs e)
        {
            await this.LoadCourses(this.Config.BackupFilePath.Text);
        }

        /// <summary>
        /// フィルターの更新
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void UpdateFiltering(object sender, RoutedEventArgs e)
        {
            this.Filter.UpdateFilterItem();
            this.View.Refresh();
        }

        /// <summary>
        /// 並び替えのリセット
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void ResetSorting(object sender, RoutedEventArgs e)
        {
            this.View.SortDescriptions.Clear();
            this.View.Refresh();
        }

        /// <summary>
        /// フィルターのリセット
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void ResetFiltering(object sender, RoutedEventArgs e)
        {
            this.Filter.ResetFilter();
            this.View.Refresh();
        }

        /// <summary>
        /// すべての科目データ削除
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void ClearCourses(object sender, RoutedEventArgs e)
        {
            this.Courses.Clear();

            this.Filter.UpdateFilterItem();
        }

        /// <summary>
        /// 自動更新
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void RunAuto(object sender, RoutedEventArgs e)
        {
            if (this.IsRunningAuto.IsChecked)
            {
                this.AddLogString("自動実行を開始しました。");

                this.StatusIsRun.Content = "自動実行中";

                this.DispatcherTimer.Interval = TimeSpan.FromSeconds(this.Config.Interval);
                this.StatusInterval.Content = "実行間隔: " + this.DispatcherTimer.Interval.TotalSeconds + " 秒";

                this.StartingDateTimeOfDispatcherTimer = DateTime.Now;
                this.DispatcherTimer.Start();

                ////this.Dispatcher = new System.Windows.Threading.Dispatcher();
                ////Task.Factory.StartNew(() => this.RegisterCoursesAuto(0, true), this.TokenSource.Token);
            }
            else
            {
                this.DispatcherTimer.Stop();

                ////this.TokenSource.Cancel();

                this.ResetAutoStatus();
                this.AddLogString("自動実行を終了しました。");
            }
        }

        /// <summary>
        /// 自動実行
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void RegisterCoursesAuto(object sender, EventArgs e)
        {
            if (this.RunsRegisterCoursesAuto)
            {
                return;
            }

            ////this.StatusIsRun.Content = "自動実行中";
            ////this.StatusInterval.Content = "実行間隔: " + this.DispatcherTimer.Interval.TotalSeconds + " 秒";

            this.RunsRegisterCoursesAuto = true;

            var registered = await this.RegisterCourses();

            if (registered.Count() > 0)
            {
                await this.ApplyRequestalCourseSchedule();
            }

            ////this.StatusIsRun.Content = "自動実行待機中";

            ////var nextTime = DateTime.Now.AddSeconds(this.DispatcherTimer.Interval.TotalSeconds);
            var nextTime = this.StartingDateTimeOfDispatcherTimer.AddSeconds(
                Math.Ceiling((DateTime.Now - this.StartingDateTimeOfDispatcherTimer).TotalSeconds / this.DispatcherTimer.Interval.TotalSeconds) * this.DispatcherTimer.Interval.TotalSeconds);
            this.StatusNext.Content = "次の実行時刻は " + nextTime.ToLongTimeString() + " です。";

            this.RunsRegisterCoursesAuto = false;
        }

        /// <summary>
        /// 終了
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// ヘッダークリック時の並び替え
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void CourseListViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var header = e.OriginalSource as GridViewColumnHeader;

            if (header == null)
            {
                return;
            }

            if (header.Role != GridViewColumnHeaderRole.Padding)
            {
                ListSortDirection direction;

                if (this.LastClickedHeader == header && this.LastSortDirection == ListSortDirection.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }

                string propertyName;

                try
                {
                    propertyName = (header.Column.DisplayMemberBinding as Binding).Path.Path;
                }
                catch (NullReferenceException)
                {
                    var dataTemplate = header.Column.CellTemplate as DataTemplate;
                    var textBlock = (TextBlock)dataTemplate.LoadContent();
                    var binding = BindingOperations.GetBinding(textBlock, TextBlock.TextProperty);
                    propertyName = binding.Path.Path;
                }

                for (var index = 0; index < this.View.SortDescriptions.Count; index++)
                {
                    if (this.View.SortDescriptions[index].PropertyName == propertyName)
                    {
                        this.View.SortDescriptions.RemoveAt(index);
                        index--;
                    }
                }

                ////this.View.SortDescriptions.Clear();

                var description = new SortDescription(propertyName, direction);

                try
                {
                    this.View.SortDescriptions.Insert(0, description);
                }
                catch (InvalidOperationException)
                {
                    this.View.SortDescriptions.Remove(description);
                }

                ////this.View.SortDescriptions.Add(new SortDescription(propertyName, direction));
                this.View.Refresh();

                this.LastClickedHeader = header;
                this.LastSortDirection = direction;
            }
        }
        
        /// <summary>
        /// 科目選択時の行高変化
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void SelectedCourse(object sender, RoutedEventArgs e)
        {
            var item = sender as ListViewItem;
            item.Height = double.NaN;
            this.Detail.DataContext = item.Content;
        }

        /// <summary>
        /// 科目選択解除時の行高変化
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void UnselectedCourse(object sender, RoutedEventArgs e)
        {            
            (sender as ListViewItem).SetBinding(ListViewItem.HeightProperty, new Binding(nameof(ConfigWindow.CourseHeight)) { Source = this.Config });
        }

        /// <summary>
        /// 開始処理
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ////            this.Config = new ConfigWindow();
            ////            this.Config.Owner = this;

            ////this.View.SortDescriptions.Add(new SortDescription(nameof(Course.ID), ListSortDirection.Ascending));
            ////this.View.SortDescriptions.Add(new SortDescription(nameof(Course.Title), ListSortDirection.Ascending));
            ////this.View.Filter = (o) => true;

            ////var col = this.CoursesGridView.Columns[1];
            ////col.Sort = ListSortDirection.Ascending;

            (this.View as ICollectionViewLiveShaping).IsLiveSorting = true;
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ////this.HttpClient.Dispose();
            ////this.HttpClientHandler.Dispose();

            this.StudentInformationService.Dispose();

            this.SyllabusHttpClient.Dispose();
        }

        /// <summary>
        /// アクティブ時処理
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void Window_Activated(object sender, EventArgs e)
        {
            if (this.Config.Owner == null)
            {
                ////this.Config = new ConfigWindow(this);
                this.Config.Owner = this;

                var response = await this.SyllabusHttpClient.GetAsync(
                    "http://sab.adm.kanazawa-u.ac.jp/search/jyoken/jyoken.asp?kensaku_type1=all&kensaku_type2=gakuikigakubu&nendo=2016");

                if (!response.IsSuccessStatusCode)
                {
                    this.Config.Show();
                    return;
                }

                var html = await this.ReadHtmlAsShiftJis(response);

                this.CreateSyllabusPostData(html);

                ////this.Config.SyllabusGakuikiList = this.GetSelectOptionList(html, this.GakuikiFormKeyName);

                this.Config.SyllabusGakuikiList.Clear();

                foreach (var option in this.GetSelectOptionList(html, this.GakuikiFormKeyName))
                {
                    this.Config.SyllabusGakuikiList.Add(option);
                }

                this.Config.GakuikiComboBox.DataContext = this.Config.SyllabusGakuikiList;

                this.Config.CourseColumnVisiblityListView.SetBinding(
                    ListView.ItemsSourceProperty,
                    new Binding(nameof(GridView.Columns)) { Source = this.CoursesGridView });

                ////this.Config.CourseColumnVisiblityListView.SetBinding(
                ////    ListView.)

                ////foreach (var column in CoursesGridView.Columns)
                ////{
                ////    var visiblity = new CheckBox();
                ////    visiblity.Content = column.Header;
                ////    visiblity.Margin = new Thickness(0, 0, 0, 6);
                ////    visiblity.SetBinding(CheckBox.HeightProperty, new Binding(nameof(ConfigWindow.CourseHeight)) { Source = this.Config });
                ////    this.Config.GridViewColumnVisiblityList.Children.Add(visiblity);
                ////}

                this.Config.Show();
            }

            if (this.Detail.Owner == null)
            {
                this.Detail.Owner = this;
            }

            if (this.Filter.Owner == null)
            {
                this.Filter.Owner = this;
            }
        }

        #endregion
    }
}
