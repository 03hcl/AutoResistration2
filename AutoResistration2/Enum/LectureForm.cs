namespace Kntacooh.AutoCourseRegistration
{
    /// <summary>
    /// 講義形態を表します。
    /// </summary>
    public enum LectureForm
    {
        /// <summary>
        /// 指定なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 講義
        /// </summary>
        Lecture = 1,

        /// <summary>
        /// 演習
        /// </summary>
        Exercise = 2,

        /// <summary>
        /// 実験
        /// </summary>
        Experiment = 3,

        /// <summary>
        /// ゼミナール
        /// </summary>
        Seminer = 4,

        /// <summary>
        /// 実技
        /// </summary>
        Practice = 5,

        /// <summary>
        /// 実習
        /// </summary>
        Training = 6,

        /// <summary>
        /// 講義その他
        /// </summary>
        LectureAndOthers = 7,

        /// <summary>
        /// その他
        /// </summary>
        Others = 99,
    }
}
