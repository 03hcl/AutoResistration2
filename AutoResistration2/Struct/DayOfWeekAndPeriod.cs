namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 曜日と時限を格納します。
    /// </summary>
    public struct DayOfWeekAndPeriod : IEquatable<DayOfWeekAndPeriod>
    {
        /// <summary>
        /// <see cref="DayOfWeekAndPeriod"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="dayOfWeek"> 曜日。 </param>
        /// <param name="period"> 時限。 </param>
        public DayOfWeekAndPeriod(DayOfWeek dayOfWeek, int? period)
        {
            this.DayOfWeek = dayOfWeek;
            this.Period = period;
        }

        /// <summary>
        /// 曜日を取得します。
        /// </summary>
        public DayOfWeek DayOfWeek { get; }

        /// <summary>
        /// 時限を取得します。
        /// </summary>
        public int? Period { get; }

        /// <summary>
        /// ２つのオペランドを比較して、等しいかどうかを判断します。
        /// </summary>
        /// <param name="key1"> 左辺のオペランド。 </param>
        /// <param name="key2"> 右辺のオペランド。 </param>
        /// <returns> オペランドの値が等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public static bool operator ==(DayOfWeekAndPeriod key1, DayOfWeekAndPeriod key2)
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
        public static bool operator !=(DayOfWeekAndPeriod key1, DayOfWeekAndPeriod key2) => !(key1 == key2);

        /// <summary>
        /// 指定のオブジェクトが、現在のオブジェクトと等しいかどうかを判断します。
        /// </summary>
        /// <param name="other"> このオブジェクトと比較するオブジェクト。 </param>
        /// <returns> 指定したオブジェクトが現在のオブジェクトと等しい場合は <see cref="true"/>。それ以外の場合は <see cref="false"/>。 </returns>
        public bool Equals(DayOfWeekAndPeriod other)
        {
            if (object.ReferenceEquals(other, null))
            {
                return false;
            }

            return this.DayOfWeek == other.DayOfWeek && this.Period == other.Period;
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

            return this.Equals((DayOfWeekAndPeriod)obj);
        }

        /// <summary>
        /// このインスタンスの値のハッシュ コードを返します。
        /// </summary>
        /// <returns> 現在のオブジェクトのハッシュ コード。 </returns>
        public override int GetHashCode() => ((int)this.DayOfWeek << 8) ^ (this.Period.HasValue ? this.Period.Value : 0);
    }
}
