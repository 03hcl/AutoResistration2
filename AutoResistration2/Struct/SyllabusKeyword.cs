namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// シラバスの日本語と英語のキーワードのペアを格納します。
    /// </summary>
    public struct SyllabusKeyword
    {
        /// <summary>
        /// <see cref="SyllabusKeyword"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="jp"> 日本語のキーワード。 </param>
        /// <param name="en"> 英語のキーワード。 </param>
        public SyllabusKeyword(string jp, string en)
        {
            this.JP = jp;
            this.EN = en;
        }

        /// <summary>
        /// 日本語のキーワードを取得します。
        /// </summary>
        public string JP { get; }

        /// <summary>
        /// 英語のキーワードを取得します。
        /// </summary>
        public string EN { get; }

        /// <summary>
        /// ２つのオペランドを比較して、等しいかどうかを判断します。
        /// </summary>
        /// <param name="key1"> 左辺のオペランド。 </param>
        /// <param name="key2"> 右辺のオペランド。 </param>
        /// <returns> オペランドの値が等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public static bool operator ==(SyllabusKeyword key1, SyllabusKeyword key2)
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
        public static bool operator !=(SyllabusKeyword key1, SyllabusKeyword key2) => !(key1 == key2);

        /// <summary>
        /// 指定のオブジェクトが、現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other"> このオブジェクトと比較するオブジェクト。 </param>
        /// <returns> 指定したオブジェクトが現在のオブジェクトと等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public bool Equals(SyllabusKeyword other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.JP == other.JP && this.EN == other.EN;
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

            return this.Equals((SyllabusKeyword)obj);
        }

        /// <summary>
        /// このインスタンスの値のハッシュ コードを返します。
        /// </summary>
        /// <returns> 現在のオブジェクトのハッシュ コード。 </returns>
        public override int GetHashCode() => this.JP.GetHashCode() ^ this.EN.GetHashCode();
    }
}
