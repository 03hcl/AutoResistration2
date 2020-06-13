namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// ConfigWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConfigWindow : Window
    {
        #region 依存関係プロパティ

        /// <summary>
        /// <see cref="Interval"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register(nameof(Interval), typeof(double), typeof(ConfigWindow), new PropertyMetadata(default(double)));

        /// <summary>
        /// <see cref="IsHidenUserId"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsShownUserIdProperty =
            DependencyProperty.Register(nameof(IsHidenUserId), typeof(bool), typeof(ConfigWindow), new PropertyMetadata(new PropertyChangedCallback(OnIsHidenUserIDPropertyChanged)));

        /// <summary>
        /// <see cref="CourseHeight"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CourseHeightProperty =
            DependencyProperty.Register(nameof(CourseHeight), typeof(double), typeof(ConfigWindow), new PropertyMetadata(default(double)));

        #endregion

        #region コンストラクタ

        /// <summary>
        /// <see cref="ConfigWindow"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="main"> メインウィンドウ。 </param>
        public ConfigWindow(MainWindow main)
        {
            this.InitializeComponent();
            this.DataContext = this;

            this.MainWindow = main;
            this.Interval = this.DefaultInterval;
            this.CourseHeight = this.DefaultCourseHeight;
            this.BackupFilePath.Text = this.DefaultBackupFilePath;

            this.ChangeStatusLogOnUserText();

            this.IsHidenUserId = true;

            this.SyllabusGakuikiList = new List<SyllabusComboBoxItem>();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// メインウィンドウを取得します。
        /// </summary>
        public MainWindow MainWindow { get; }

        /// <summary>
        /// 金沢大学IDの表示が隠されているかどうかを取得または設定します。
        /// </summary>
        public bool IsHidenUserId
        {
            get { return (bool)this.GetValue(IsShownUserIdProperty); }
            set { this.SetValue(IsShownUserIdProperty, value); }
        }

        /// <summary>
        /// インターバルを取得または設定します。
        /// </summary>
        public double Interval
        {
            get { return (double)this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// ウィンドウに入力されている金沢大学IDを取得します。
        /// </summary>
        public string TextUserId
        {
            get { return this.IsHidenUserId ? this.UnvisibleUserID.Password : this.UserID.Text; }
        }

        /// <summary>
        /// ログインしているユーザーの金沢大学IDを取得または設定します。
        /// </summary>
        public string LogOnUserId { get; set; } = string.Empty;

        /// <summary>
        /// ログの最大表示行数を取得または設定します。
        /// </summary>
        public int LogMaxRow { get; set; } = 255;

        /// <summary>
        /// 既定の行高を取得または設定します。
        /// </summary>
        public double CourseHeight
        {
            get { return (double)this.GetValue(CourseHeightProperty); }
            set { this.SetValue(CourseHeightProperty, value); }
        }

        /// <summary>
        /// シラバスの "学域/学部等" の選択項目名と、それに対応するIDのリストを取得または設定します。
        /// </summary>
        public IList<SyllabusComboBoxItem> SyllabusGakuikiList { get; private set; }
        ////public Dictionary<string, string> SyllabusGakuikiList { get; private set; }

        /// <summary>
        /// シラバスの "学類/学科等" の選択項目名と、それに対応するIDのリストを取得または設定します。
        /// </summary>
        public IList<SyllabusComboBoxItem> SyllabusGakuruiList { get; private set; }
        ////public Dictionary<string, string> SyllabusGakuruiList { get; private set; }

        /// <summary>
        /// シラバスの "対象学生" の "学年" 選択項目名と、それに対応するIDのリストを取得または設定します。
        /// </summary>
        public IList<SyllabusComboBoxItem> SyllabusStudentYearList { get; private set; }
        ////public Dictionary<string, string> SyllabusStudentYearList { get; private set; }

        #endregion

        #region プロパティ(private)
        
        /// <summary>
        /// インターバルの既定値を取得します。
        /// </summary>
        private double DefaultInterval { get; } = 15;

        /// <summary>
        /// 既定の行高の規定値を取得します。
        /// </summary>
        private double DefaultCourseHeight { get; } = 20;

        /// <summary>
        /// <see cref="MainWindow.Courses"/> シリアライズ時の既定のファイルのパスを取得します。
        /// </summary>
        private string DefaultBackupFilePath { get; } = "courses.xml";
        
        #endregion

        #region メソッド

        /// <summary>
        /// ログイン状態を表示するステータスバーを変更します。
        /// </summary>
        public void ChangeStatusLogOnUserText()
        {
            if (string.IsNullOrEmpty(this.LogOnUserId))
            {
                this.MainWindow.StatusLoginUser.Content = "ログアウト中";
            }
            else
            {
                this.MainWindow.StatusLoginUser.Content = "ログイン中" + (this.IsHidenUserId ? string.Empty : "/" + this.LogOnUserId);
            }
        }

        /// <summary>
        /// ログイン処理
        /// </summary>
        /// <returns> <see cref="Task"/>. </returns>
        public async Task LogOn()
        {
            if (this.LogOnUserId == this.TextUserId)
            {
                return;
            }

            this.MainWindow.StatusLoginUser.Content = "ログイン処理中...";

            if (await this.MainWindow.StudentInformationService.Login(this.TextUserId, this.UserPassword.Password))
            {
                this.MainWindow.AddLogString("ログインに成功しました。");
                this.LogOnUserId = this.TextUserId;
            }
            else
            {
                this.MainWindow.AddLogString("ログインに失敗しました。");
            }

            #region

            ////HttpResponseMessage loginResponse;

            ////try
            ////{
            ////    loginResponse = await this.MainWindow.HttpClient.GetAsync("/Portal/login.php?next=");
            ////    loginResponse.EnsureSuccessStatusCode();
            ////}
            ////catch
            ////{
            ////    this.MainWindow.AddLogString("ログインに失敗しました。");

            ////    this.ChangeStatusLogOnUserText();
            ////    return;
            ////}

            ////try
            ////{
            ////    var response = await this.MainWindow.HttpClient.PostAsync(
            ////        "https://ku-sso.cis.kanazawa-u.ac.jp/idp/Authn/UserPassword",
            ////        new FormUrlEncodedContent(new Dictionary<string, string>
            ////        {
            ////            { "j_username", this.TextUserId },
            ////            { "j_password", this.UserPassword.Password },
            ////        }));
            ////    response.EnsureSuccessStatusCode();

            ////    loginResponse = response;
            ////}
            ////catch
            ////{
            ////    try
            ////    {
            ////        var jsessionidRegex = new Regex("<form[^<]*action=\"(?<action>[^\"]*jsessionid[^\"]*)\"[^<]*>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            ////        var url = "https://ku-sso.cis.kanazawa-u.ac.jp" + jsessionidRegex.Match(await loginResponse.Content.ReadAsStringAsync()).Groups["action"].Value;
            ////        var response = await this.MainWindow.HttpClient.PostAsync(
            ////            url,
            ////            new FormUrlEncodedContent(new Dictionary<string, string>
            ////            {
            ////                { "shib_idp_ls_exception.shib_idp_session_ss" , "" },
            ////                { "shib_idp_ls_success.shib_idp_session_ss" , "false" },
            ////                { "shib_idp_ls_value.shib_idp_session_ss" , "" },
            ////                { "shib_idp_ls_exception.shib_idp_persistent_ss" , "" },
            ////                { "shib_idp_ls_success.shib_idp_persistent_ss" , "false" },
            ////                { "shib_idp_ls_value.shib_idp_persistent_ss" , "" },
            ////                { "shib_idp_ls_supported" , "" },
            ////                { "_eventId_proceed" , "" },
            ////            }));
            ////        response.EnsureSuccessStatusCode();

            ////        response = await this.MainWindow.HttpClient.PostAsync(
            ////            response.RequestMessage.RequestUri,
            ////            new FormUrlEncodedContent(new Dictionary<string, string>
            ////            {
            ////                { "j_username", this.TextUserId },
            ////                { "pswd-text", "" },
            ////                { "j_password", this.UserPassword.Password },
            ////                { "_eventId_proceed", "" },
            ////            }));
            ////        response.EnsureSuccessStatusCode();

            ////        loginResponse = response;
            ////    }
            ////    catch
            ////    {
            ////        this.MainWindow.AddLogString("ログインに失敗しました。");

            ////        this.ChangeStatusLogOnUserText();
            ////        return;
            ////    }
            ////}

            ////try
            ////{
            ////    var content = MainWindow.MakeSaml2Content(await loginResponse.Content.ReadAsStringAsync());
            ////    var response = await this.MainWindow.HttpClient.PostAsync(
            ////        "https://acanthus.cis.kanazawa-u.ac.jp/Shibboleth.sso/SAML2/POST",
            ////        content);
            ////    response.EnsureSuccessStatusCode();

            ////    response = await this.MainWindow.HttpClient.GetAsync("https://acanthus.cis.kanazawa-u.ac.jp/Portal/u053/spmode/registrationLink.php");
            ////    response.EnsureSuccessStatusCode();

            ////    response = await this.MainWindow.HttpClient.PostAsync(
            ////        "https://risyu.kanazawa-u.ac.jp/Shibboleth.sso/SAML2/POST",
            ////        MainWindow.MakeSaml2Content(await response.Content.ReadAsStringAsync()));
            ////    response.EnsureSuccessStatusCode();

            ////    this.MainWindow.AddLogString("ログインに成功しました。");
            ////    this.LogOnUserId = this.TextUserId;
            ////}
            ////catch
            ////{
            ////    this.MainWindow.AddLogString("ログインに失敗しました。");
            ////}

            #endregion

            this.ChangeStatusLogOnUserText();

            ////var html = await response.Content.ReadAsStringAsync();
            ////var url = response.RequestMessage.RequestUri.AbsoluteUri;
        }

        /// <summary>
        /// ログアウト処理
        /// </summary>
        /// <returns> <see cref="Task"/>. </returns>
        public async Task LogOff()
        {
            if (this.LogOnUserId == string.Empty)
            {
                return;
            }

            if (await this.MainWindow.StudentInformationService.Logout())
            {
                this.MainWindow.AddLogString("ログインに成功しました。");
                this.LogOnUserId = this.TextUserId;
            }
            else
            {
                this.MainWindow.AddLogString("ログインに失敗しました。");
            }

            this.LogOnUserId = string.Empty;
            this.ChangeStatusLogOnUserText();

            ////try
            ////{
            ////    this.MainWindow.StatusLoginUser.Content = "ログアウト処理中...";

            ////    var response = await this.MainWindow.HttpClient.GetAsync("/Portal/logout.php");
            ////    response.EnsureSuccessStatusCode();

            ////    this.MainWindow.AddLogString("ログアウトに成功しました。");
            ////    this.LogOnUserId = string.Empty;
            ////}
            ////catch
            ////{
            ////    this.MainWindow.AddLogString("ログアウトに失敗しました。");
            ////}

            ////this.ChangeStatusLogOnUserText();
            ////this.MainWindow.RecreateHttpClient();
        }

        /// <summary>
        /// <see cref="IsHidenUserId"/> プロパティの有効値が変更されたときにプロパティ システムによって必ず呼び出されるハンドラー実装への参照です。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnIsHidenUserIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (ConfigWindow)d;
            string oldID;

            if ((bool)e.OldValue)
            {
                oldID = obj.UnvisibleUserID.Password;
                obj.UnvisibleUserID.Visibility = Visibility.Collapsed;
                obj.UnvisibleUserID.Password = string.Empty;
            }
            else
            {
                oldID = obj.UserID.Text;
                obj.UserID.Visibility = Visibility.Collapsed;
                obj.UserID.Text = string.Empty;
            }

            if ((bool)e.NewValue)
            {
                obj.UnvisibleUserID.Password = oldID;
                obj.UnvisibleUserID.Visibility = Visibility.Visible;
            }
            else
            {
                obj.UserID.Text = oldID;
                obj.UserID.Visibility = Visibility.Visible;
            }

            obj.ChangeStatusLogOnUserText();
        }

        /// <summary>
        /// ウィンドウ終了のキャンセル
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// ログイン
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void LogOnButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LogOn();
        }

        /// <summary>
        /// ログアウト
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            await this.LogOff();
        }

        /// <summary>
        /// 更新処理、追加の反映
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await this.MainWindow.RegisterCourses();
            ////await this.MainWindow.CancelRegistrationCourses();
        }

        /// <summary>
        /// シラバスの学域選択変更
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private async void GakuikiComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (var index = 0; index < this.MainWindow.SyllabusPostData.Count; index++)
            {
                if (this.MainWindow.SyllabusPostData[index].Key == this.MainWindow.GakuikiFormKeyName)
                {
                    MainWindow.UpdatePostData(this.MainWindow.SyllabusPostData, index, (string)this.GakuikiComboBox.SelectedValue);
                    ////index--;
                    break;
                }
            }

            var response = await this.MainWindow.SyllabusHttpClient.PostAsync(
                "http://sab.adm.kanazawa-u.ac.jp/search/jyoken/jyoken.asp",
                MainWindow.CreatePresentPostDataStringContent(this.MainWindow.SyllabusPostData));

            if (!response.IsSuccessStatusCode)
            {
                this.GakuruiComboBox.SelectedIndex = -1;
                this.SyllabusGakuruiList = null;
                this.GakuruiComboBox.IsEnabled = false;

                this.StudentYearComboBox.SelectedIndex = -1;
                this.SyllabusStudentYearList = null;
                this.StudentYearComboBox.IsEnabled = false;
                return;
            }

            var html = await this.MainWindow.ReadHtmlAsShiftJis(response);

            ////this.SyllabusGakuruiList = this.MainWindow.GetSelectOptionList(html, this.MainWindow.GakuruiFormKeyName);
            this.SyllabusGakuruiList = new List<SyllabusComboBoxItem>();

            foreach (var option in this.MainWindow.GetSelectOptionList(html, this.MainWindow.GakuruiFormKeyName))
            {
                this.SyllabusGakuruiList.Add(option);
            }

            this.GakuruiComboBox.DataContext = this.SyllabusGakuruiList;
            this.GakuruiComboBox.IsEnabled = true;

            ////this.SyllabusStudentYearList = this.MainWindow.GetSelectOptionList(html, this.MainWindow.StudentYearFormKeyName);
            this.SyllabusStudentYearList = new List<SyllabusComboBoxItem>();

            foreach (var option in this.MainWindow.GetSelectOptionList(html, this.MainWindow.StudentYearFormKeyName))
            {
                this.SyllabusStudentYearList.Add(option);
            }

            this.StudentYearComboBox.DataContext = this.SyllabusStudentYearList;
            this.StudentYearComboBox.IsEnabled = true;
        }

        /////// <summary>
        /////// 項目名選択時の行高変化
        /////// </summary>
        /////// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /////// <param name="e"> イベントオブジェクト。 </param>
        ////private void SelectedCourse(object sender, RoutedEventArgs e)
        ////{
        ////    var item = sender as ListViewItem;
        ////    item.Height = double.NaN;
        ////}

        /////// <summary>
        /////// 項目名選択解除時の行高変化
        /////// </summary>
        /////// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /////// <param name="e"> イベントオブジェクト。 </param>
        ////private void UnselectedCourse(object sender, RoutedEventArgs e)
        ////{
        ////    (sender as ListViewItem).SetBinding(ListViewItem.HeightProperty, new Binding(nameof(ConfigWindow.CourseHeight)) { Source = this });
        ////}

        #endregion

        /// <summary>
        /// 列の表示
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void ColumnVisibled(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as GridViewColumn).Width = 0;
        }

        /// <summary>
        /// 列の非表示
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void ColumnUnvisibled(object sender, RoutedEventArgs e)
        {
            ((sender as CheckBox).DataContext as GridViewColumn).Width = double.NaN;
        }
    }
}
