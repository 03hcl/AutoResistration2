namespace Kntacooh.AutoCourseRegistration
{
    /// <summary>
    /// 曜日を表す列挙体です。
    /// </summary>
    public enum DayOfWeek
    {
        /// <summary>
        /// (設定なし)
        /// </summary>
        None = 0,

        /// <summary>
        /// 月曜
        /// </summary>
        Monday = 1,

        /// <summary>
        /// 火曜
        /// </summary>
        Tuesday = 2,

        /// <summary>
        /// 水曜
        /// </summary>
        Wednesday = 3,

        /// <summary>
        /// 木曜
        /// </summary>
        Thursday = 4,

        /// <summary>
        /// 金曜
        /// </summary>
        Friday = 5,

        /// <summary>
        /// 土曜
        /// </summary>
        Saturday = 6,

        /// <summary>
        /// 日曜
        /// </summary>
        Sunday = 7,

        /// <summary>
        /// 集中講義
        /// </summary>
        Intensive = 9,
    }
}
