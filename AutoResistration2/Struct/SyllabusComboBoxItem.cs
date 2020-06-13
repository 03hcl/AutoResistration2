namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// シラバスのコンボボックスのアイテムの情報を格納します。
    /// </summary>
    public struct SyllabusComboBoxItem
    {
        /// <summary>
        /// <see cref="SyllabusComboBoxItem"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="key"> キー。 </param>
        /// <param name="valueJP"> 日本語の項目名。 </param>
        /// <param name="valueEN"> 英語の項目名。 </param>
        public SyllabusComboBoxItem(string key, string valueJP, string valueEN)
        {
            this.Key = key;
            this.JPValue = valueJP;
            this.ENValue = valueEN;
        }

        /// <summary>
        /// キーを取得します。
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// 日本語の項目名を取得します。
        /// </summary>
        public string JPValue { get; }

        /// <summary>
        /// 英語の項目名を取得します。
        /// </summary>
        public string ENValue { get; }

        /// <summary>
        /// ２つのオペランドを比較して、等しいかどうかを判断します。
        /// </summary>
        /// <param name="key1"> 左辺のオペランド。 </param>
        /// <param name="key2"> 右辺のオペランド。 </param>
        /// <returns> オペランドの値が等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public static bool operator ==(SyllabusComboBoxItem key1, SyllabusComboBoxItem key2)
        {
            if (object.ReferenceEquals(key1, null))
            {
                return false;
            }

            return key1.Equals(key2);
        }

        /// <summary>
        /// ２つのオペランドを比較して、等しくないかどうかを判断します。
        /// </summary>
        /// <param name="key1"> 左辺のオペランド。 </param>
        /// <param name="key2"> 右辺のオペランド。 </param>
        /// <returns> オペランドの値が等しい場合は <see cref="false"/>。それ以外の場合は <see cref="true"/>。 </returns>
        public static bool operator !=(SyllabusComboBoxItem key1, SyllabusComboBoxItem key2) => !(key1 == key2);

        /// <summary>
        /// 指定のオブジェクトが、現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other"> このオブジェクトと比較するオブジェクト。 </param>
        /// <returns> 指定したオブジェクトが現在のオブジェクトと等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public bool Equals(SyllabusComboBoxItem other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.Key == other.Key && this.JPValue == other.JPValue && this.ENValue == other.ENValue;
        }

        /// <summary>
        /// 指定のオブジェクトが、現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="obj"> 現在のオブジェクトと比較するオブジェクト。 </param>
        /// <returns> 指定したオブジェクトが現在のオブジェクトと等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals((SyllabusComboBoxItem)obj);
        }

        /// <summary>
        /// このインスタンスの値のハッシュ コードを返します。
        /// </summary>
        /// <returns> 現在のオブジェクトのハッシュ コード。 </returns>
        public override int GetHashCode() => this.Key.GetHashCode() ^ this.JPValue.GetHashCode() ^ this.ENValue.GetHashCode();
    }
}
