namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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
    /// DetailWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class DetailWindow : Window
    {
        #region コンストラクタ

        /// <summary>
        /// <see cref="ConfigWindow"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="main"> メインウィンドウ。 </param>
        public DetailWindow(MainWindow main)
        {
            this.InitializeComponent();

            this.MainWindow = main;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// メインウィンドウを取得します。
        /// </summary>
        public MainWindow MainWindow { get; }

        #endregion

        #region メソッド

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

        #endregion
    }
}
