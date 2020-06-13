namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Controls;

    /// <summary>
    /// フィルターのアイテムを表します。
    /// </summary>
    public class FilterItem
    {
        /// <summary>
        /// <see cref="FilterItem"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="checkBox"> チェックボックス。 </param>
        public FilterItem(CheckBox checkBox)
        {
            this.CheckBox = checkBox;
            this.Label = null;
        }

        /// <summary>
        /// <see cref="FilterItem"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="checkBox"> チェックボックス。 </param>
        /// <param name="label"> ラベル。 </param>
        public FilterItem(CheckBox checkBox, Label label)
        {
            this.CheckBox = checkBox;
            this.Label = label;
        }

        /// <summary>
        /// チェックボックスを取得します。
        /// </summary>
        public CheckBox CheckBox { get; }

        /// <summary>
        /// ラベルを取得します。
        /// </summary>
        public Label Label { get; }
    }
}
