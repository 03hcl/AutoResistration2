namespace Kntacooh.AutoCourseRegistration
{
    /// <summary>
    /// 評価の種類を表す列挙体です。
    /// </summary>
    public enum GradingType
    {
        /// <summary>
        /// 設定なし
        /// </summary>
        None = 0,

        /// <summary>
        /// 出席, Attendance
        /// </summary>
        Attendance,

        /// <summary>
        /// 論文, Thesis
        /// </summary>
        Thesis,

        /// <summary>
        /// レポート, Report
        /// </summary>
        Report,

        /// <summary>
        /// 課題, 宿題, 提出, Homework, Assignment
        /// </summary>
        Homework,

        /// <summary>
        /// 小テスト, Mini / ペーパー, Paper
        /// </summary>
        MiniExam,

        /// <summary>
        /// 中間, Midterm
        /// </summary>
        MidtermExam,

        /// <summary>
        /// 期末, Final / 定期, 試験, テスト, Exam, Test, (Quiz?)
        /// </summary>
        FinalExam,

        /// <summary>
        /// 発表, プレゼン, Presentation
        /// </summary>
        Presentation,

        /// <summary>
        /// 態度, 平常, 意欲, 取組, 取り組み, Attitude, React, Participation / 授業
        /// </summary>
        Attitude,

        /// <summary>
        /// 演習, 実習, 実技, Exercise
        /// </summary>
        Exercise,

        /// <summary>
        /// 実験, Experiment
        /// </summary>
        Experiment,

        /// <summary>
        /// 合計, 計, Sum
        /// </summary>
        Sum,

        /// <summary>
        /// その他
        /// </summary>
        Others,
    }
}