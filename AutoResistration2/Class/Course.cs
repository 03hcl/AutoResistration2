namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Xml.Serialization;

    /// <summary>
    /// 科目についての情報を格納します。
    /// </summary>
    public class Course : DependencyObject
    {
        #region 依存関係プロパティ

        /////// <summary>
        /////// <see cref="RegisterNumber"/> 依存関係プロパティを識別します。
        /////// </summary>
        ////public static readonly DependencyProperty RegisterNumberProperty = DependencyProperty.Register(
        ////    nameof(RegisterNumber),
        ////    typeof(int?),
        ////    typeof(Course),
        ////    new PropertyMetadata(
        ////        default(int?),
        ////        new PropertyChangedCallback(
        ////            (d, e) =>
        ////            {
        ////                var obj = d as Course;
        ////                obj.Registers = obj.RegisterNumber.HasValue;
        ////                obj.NotRegisters = !obj.Registers;
        ////            })));

        /// <summary>
        /// <see cref="RegisterNumber"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RegisterNumberProperty =
            DependencyProperty.Register(nameof(RegisterNumber), typeof(int?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRegisterNumberChanged)));

        /// <summary>
        /// <see cref="Registers"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RegistersProperty =
            DependencyProperty.Register(nameof(Registers), typeof(bool), typeof(Course), new PropertyMetadata(default(bool)));

        /// <summary>
        /// <see cref="NotRegisters"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty NotRegistersProperty =
            DependencyProperty.Register(nameof(NotRegisters), typeof(bool), typeof(Course), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="RawSubjectType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RawSubjectTypeProperty =
            DependencyProperty.Register(nameof(RawSubjectType), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRawSubjectTypeChanged)));

        /// <summary>
        /// <see cref="KugsOfGS"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty KugsOfGSProperty =
            DependencyProperty.Register(nameof(KugsOfGS), typeof(int?), typeof(Course), new PropertyMetadata(default(int?)));

        /// <summary>
        /// <see cref="SignOfGS"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SignOfGSProperty =
            DependencyProperty.Register(nameof(SignOfGS), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SignDetailOfGS"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SignDetailOfGSProperty =
            DependencyProperty.Register(nameof(SignDetailOfGS), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="RowId"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RowIdProperty =
            DependencyProperty.Register(nameof(RowId), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="Lctcd"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty LctcdProperty =
            DependencyProperty.Register(nameof(Lctcd), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="Id"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.Register(nameof(Id), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnIdChanged)));

        /// <summary>
        /// <see cref="Title"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="Instructor"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty InstructorProperty =
            DependencyProperty.Register(nameof(Instructor), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="DayOfWeek"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty DayOfWeekProperty =
            DependencyProperty.Register(nameof(DayOfWeek), typeof(DayOfWeek), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnDayOfWeekChanged)));

        /// <summary>
        /// <see cref="Period"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register(nameof(Period), typeof(int?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnPeriodChanged)));

        /// <summary>
        /// <see cref="DayOfWeekAndPeriod"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty DayOfWeekAndPeriodProperty =
            DependencyProperty.Register(nameof(DayOfWeekAndPeriod), typeof(DayOfWeekAndPeriod), typeof(Course), new PropertyMetadata(default(DayOfWeekAndPeriod)));

        /// <summary>
        /// <see cref="UnitType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty UnitTypeProperty =
            DependencyProperty.Register(nameof(UnitType), typeof(UnitType?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnCreditPerQuarterChanging)));

        /// <summary>
        /// <see cref="Credit"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CreditProperty =
            DependencyProperty.Register(nameof(Credit), typeof(decimal?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnCreditPerQuarterChanging)));

        /// <summary>
        /// <see cref="CreditPerQuarter"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty CreditPerQuarterProperty =
            DependencyProperty.Register(nameof(CreditPerQuarter), typeof(decimal?), typeof(Course), new PropertyMetadata(default(decimal?)));

        /// <summary>
        /// <see cref="MaxSize"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty MaxSizeProperty =
            DependencyProperty.Register(nameof(MaxSize), typeof(int?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRestSizeChanging)));

        /// <summary>
        /// <see cref="RegisteredSize"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RegisteredSizeProperty =
            DependencyProperty.Register(nameof(RegisteredSize), typeof(int?), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRestSizeChanging)));

        /// <summary>
        /// <see cref="RestSize"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RestSizeProperty =
            DependencyProperty.Register(nameof(RestSize), typeof(int?), typeof(Course), new PropertyMetadata(default(int?)));

        /// <summary>
        /// <see cref="IsDoneLottery"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsDoneLotteryProperty =
            DependencyProperty.Register(nameof(IsDoneLottery), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));
        
        /// <summary>
        /// <see cref="IsToAdd"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsToAddProperty =
            DependencyProperty.Register(nameof(IsToAdd), typeof(bool), typeof(Course), new PropertyMetadata(default(bool)));
        
        /// <summary>
        /// <see cref="IsToDelete"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsToDeleteProperty =
            DependencyProperty.Register(nameof(IsToDelete), typeof(bool), typeof(Course), new PropertyMetadata(default(bool)));

        #endregion

        #region 依存関係プロパティ(シラバス)

        /// <summary>
        /// <see cref="RawSyllabusSchoolOrDivision"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RawSyllabusSchoolOrDivisionProperty =
            DependencyProperty.Register(nameof(RawSyllabusSchoolOrDivision), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRawSyllabusSchoolOrDivisionChanged)));

        /// <summary>
        /// <see cref="SyllabusTitle"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusTitleProperty =
            DependencyProperty.Register(nameof(SyllabusTitle), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnSyllabusTitleChanged)));

        /// <summary>
        /// <see cref="IsCityCollege"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsCityCollegeProperty =
            DependencyProperty.Register(nameof(IsCityCollege), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));

        /// <summary>
        /// <see cref="SyllabusInstructor"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusInstructorProperty =
            DependencyProperty.Register(nameof(SyllabusInstructor), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnSyllabusInstructorChanged)));

        /// <summary>
        /// <see cref="SyllabusNumberingCode"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusNumberingCodeProperty =
            DependencyProperty.Register(nameof(SyllabusNumberingCode), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusCategory"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusCategoryProperty =
            DependencyProperty.Register(nameof(SyllabusCategory), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusLectureForm"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusLectureFormProperty =
            DependencyProperty.Register(nameof(SyllabusLectureForm), typeof(LectureForm?), typeof(Course), new PropertyMetadata(default(LectureForm?)));

        /// <summary>
        /// <see cref="SyllabusAssignedYear"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusAssignedYearProperty =
            DependencyProperty.Register(nameof(SyllabusAssignedYear), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="OffersInQ1"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty OffersInQ1Property =
            DependencyProperty.Register(nameof(OffersInQ1), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));

        /// <summary>
        /// <see cref="OffersInQ2"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty OffersInQ2Property =
            DependencyProperty.Register(nameof(OffersInQ2), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));

        /// <summary>
        /// <see cref="OffersInQ3"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty OffersInQ3Property =
            DependencyProperty.Register(nameof(OffersInQ3), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));

        /// <summary>
        /// <see cref="OffersInQ4"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty OffersInQ4Property =
            DependencyProperty.Register(nameof(OffersInQ4), typeof(bool?), typeof(Course), new PropertyMetadata(default(bool?)));

        /// <summary>
        /// <see cref="SyllabusKeyword"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusKeywordProperty =
            DependencyProperty.Register(nameof(SyllabusKeyword), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnSyllabusKeywordsChanged)));

        /// <summary>
        /// <see cref="SyllabusTopic"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusTopicProperty =
            DependencyProperty.Register(nameof(SyllabusTopic), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusObjective"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusObjectiveProperty =
            DependencyProperty.Register(nameof(SyllabusObjective), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusLearningOutcomes"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusLearningOutcomesProperty =
            DependencyProperty.Register(nameof(SyllabusLearningOutcomes), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusOutline"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusOutlineProperty =
            DependencyProperty.Register(nameof(SyllabusOutline), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="RawSyllabusSchedule"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RawSyllabusScheduleProperty =
            DependencyProperty.Register(nameof(RawSyllabusSchedule), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusGradingMethod"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingMethodProperty =
            DependencyProperty.Register(nameof(SyllabusGradingMethod), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="RawSyllabusGradingRate"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty RawSyllabusGradingRateProperty =
            DependencyProperty.Register(nameof(RawSyllabusGradingRate), typeof(string), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnRawSyllabusGradingRateChanged)));

        /// <summary>
        /// <see cref="SyllabusTeachingMaterials"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusTeachingMaterialsProperty =
            DependencyProperty.Register(nameof(SyllabusTeachingMaterials), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusOthers"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusOthersProperty =
            DependencyProperty.Register(nameof(SyllabusOthers), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusConsultationTime"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusConsultationTimeProperty =
            DependencyProperty.Register(nameof(SyllabusConsultationTime), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusPrerequisites"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusPrerequisitesProperty =
            DependencyProperty.Register(nameof(SyllabusPrerequisites), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusMethodForAdjustingClassSize"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusMethodForAdjustingClassSizeProperty =
            DependencyProperty.Register(nameof(SyllabusMethodForAdjustingClassSize), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusSubjectOfOpen"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusSubjectOfOpenProperty =
            DependencyProperty.Register(nameof(SyllabusSubjectOfOpen), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusRelatedCourses"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusRelatedCoursesProperty =
            DependencyProperty.Register(nameof(SyllabusRelatedCourses), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusRelationsWithTheOtherCoursesInTheCurriculum"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusRelationsWithTheOtherCoursesInTheCurriculumProperty =
            DependencyProperty.Register(nameof(SyllabusRelationsWithTheOtherCoursesInTheCurriculum), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusSpecialNote"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusSpecialNoteProperty =
            DependencyProperty.Register(nameof(SyllabusSpecialNote), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        #endregion

        #region 依存関係プロパティ(整形後シラバス)

        /// <summary>
        /// <see cref="JPSubjectType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPSubjectTypeProperty =
            DependencyProperty.Register(nameof(JPSubjectType), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="ENSubjectType"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENSubjectTypeProperty =
            DependencyProperty.Register(nameof(ENSubjectType), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="JPSyllabusSchoolOrDivision"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPSyllabusSchoolOrDivisionProperty =
            DependencyProperty.Register(nameof(JPSyllabusSchoolOrDivision), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="ENSyllabusSchoolOrDivision"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENSyllabusSchoolOrDivisionProperty =
            DependencyProperty.Register(nameof(ENSyllabusSchoolOrDivision), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="JPSyllabusTitle"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPSyllabusTitleProperty =
            DependencyProperty.Register(nameof(JPSyllabusTitle), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="ENSyllabusTitle"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENSyllabusTitleProperty =
            DependencyProperty.Register(nameof(ENSyllabusTitle), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="JPSyllabusInstructors"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPSyllabusInstructorsProperty =
            DependencyProperty.Register(nameof(JPSyllabusInstructors), typeof(ReadOnlyCollection<string>), typeof(Course), new PropertyMetadata(default(ReadOnlyCollection<string>)));

        /// <summary>
        /// <see cref="ENSyllabusInstructors"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENSyllabusInstructorsProperty =
            DependencyProperty.Register(nameof(ENSyllabusInstructors), typeof(ReadOnlyCollection<string>), typeof(Course), new PropertyMetadata(default(ReadOnlyCollection<string>)));

        /// <summary>
        /// <see cref="JPSyllabusKeywords"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty JPSyllabusKeywordsProperty =
            DependencyProperty.Register(nameof(JPSyllabusKeywords), typeof(ReadOnlyCollection<string>), typeof(Course), new PropertyMetadata(default(ReadOnlyCollection<string>)));

        /// <summary>
        /// <see cref="ENSyllabusKeywords"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ENSyllabusKeywordsProperty =
            DependencyProperty.Register(nameof(ENSyllabusKeywords), typeof(ReadOnlyCollection<string>), typeof(Course), new PropertyMetadata(default(ReadOnlyCollection<string>)));

        /// <summary>
        /// <see cref="SyllabusGradingRate"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRate), typeof(string), typeof(Course), new PropertyMetadata(default(string)));

        /// <summary>
        /// <see cref="SyllabusGradingRateDetails"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateDetailsProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateDetails), typeof(ReadOnlyCollection<GradingRate>), typeof(Course), new PropertyMetadata(new PropertyChangedCallback(OnSyllabusGradingRateDetailsChanged)));

        /// <summary>
        /// <see cref="SyllabusSumOfGradingRate"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusSumOfGradingRateProperty =
            DependencyProperty.Register(nameof(SyllabusSumOfGradingRate), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfAttendance"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfAttendanceProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfAttendance), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfThesis"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfThesisProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfThesis), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfReport"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfReportProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfReport), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfHomework"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfHomeworkProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfHomework), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfMiniExam"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfMiniExamProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfMiniExam), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfMidtermExam"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfMidtermExamProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfMidtermExam), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfFinalExam"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfFinalExamProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfFinalExam), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfPresentation"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfPresentationProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfPresentation), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfAttitude"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfAttitudeProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfAttitude), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfExercise"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfExerciseProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfExercise), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfExperiment"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfExperimentProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfExperiment), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        /// <summary>
        /// <see cref="SyllabusGradingRateOfOthers"/> 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty SyllabusGradingRateOfOthersProperty =
            DependencyProperty.Register(nameof(SyllabusGradingRateOfOthers), typeof(decimal), typeof(Course), new PropertyMetadata(default(decimal)));

        #endregion

        #region コンストラクタ

        /// <summary>
        /// <see cref="Course"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public Course()
        {
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 登録されている時間割を表す番号を取得または設定します。
        /// </summary>
        [XmlElement]
        public int? RegisterNumber
        {
            get { return (int?)this.GetValue(RegisterNumberProperty); }
            set { this.SetValue(RegisterNumberProperty, value); }
        }

        /// <summary>
        /// 時間割が登録されているかを取得します。
        /// </summary>
        [XmlIgnore]
        public bool Registers
        {
            get { return (bool)this.GetValue(RegistersProperty); }
            private set { this.SetValue(RegistersProperty, value); }
        }

        /// <summary>
        /// <see cref="Registers"/> の否定を取得します。
        /// </summary>
        [XmlIgnore]
        public bool NotRegisters
        {
            get { return (bool)this.GetValue(NotRegistersProperty); }
            private set { this.SetValue(NotRegistersProperty, value); }
        }

        /// <summary>
        /// 科目の種類を表す元のテキストを取得または設定します。
        /// </summary>
        [XmlElement]
        public string RawSubjectType
        {
            get { return (string)this.GetValue(RawSubjectTypeProperty); }
            set { this.SetValue(RawSubjectTypeProperty, value); }
        }

        /// <summary>
        /// GS科目の群を取得します。
        /// </summary>
        [XmlIgnore]
        public int? KugsOfGS
        {
            get { return (int?)this.GetValue(KugsOfGSProperty); }
            private set { this.SetValue(KugsOfGSProperty, value); }
        }

        /// <summary>
        /// GS科目の群内の記号を取得します。
        /// </summary>
        [XmlIgnore]
        public string SignOfGS
        {
            get { return (string)this.GetValue(SignOfGSProperty); }
            private set { this.SetValue(SignOfGSProperty, value); }
        }

        /// <summary>
        /// GS科目における同じGS番号内の連続番号を取得します。
        /// </summary>
        [XmlIgnore]
        public string SignDetailOfGS
        {
            get { return (string)this.GetValue(SignDetailOfGSProperty); }
            protected set { this.SetValue(SignDetailOfGSProperty, value); }
        }

        /// <summary>
        /// 履修希望の行IDを取得または設定します。
        /// </summary>
        [XmlElement]
        public string RowId
        {
            get { return (string)this.GetValue(RowIdProperty); }
            set { this.SetValue(RowIdProperty, value); }
        }

        /// <summary>
        /// 登録追加時の lct_cd を取得または設定します。
        /// </summary>
        [XmlElement]
        public string Lctcd
        {
            get { return (string)this.GetValue(LctcdProperty); }
            set { this.SetValue(LctcdProperty, value); }
        }

        /// <summary>
        /// 時間割番号を取得または設定します。
        /// </summary>
        [XmlElement]
        public string Id
        {
            get { return (string)this.GetValue(IdProperty); }
            set { this.SetValue(IdProperty, value); }
        }

        /// <summary>
        /// 授業科目名を取得または設定します。
        /// </summary>
        [XmlElement]
        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 担当教員を取得または設定します。
        /// </summary>
        [XmlElement]
        public string Instructor
        {
            get { return (string)this.GetValue(InstructorProperty); }
            set { this.SetValue(InstructorProperty, value); }
        }

        /// <summary>
        /// 曜日を取得または設定します。
        /// </summary>
        [XmlElement]
        public DayOfWeek DayOfWeek
        {
            get { return (DayOfWeek)this.GetValue(DayOfWeekProperty); }
            set { this.SetValue(DayOfWeekProperty, value); }
        }

        /// <summary>
        /// 時限を取得または設定します。
        /// </summary>
        [XmlElement]
        public int? Period
        {
            get { return (int?)this.GetValue(PeriodProperty); }
            set { this.SetValue(PeriodProperty, value); }
        }

        /// <summary>
        /// 曜日と時限を取得します。
        /// </summary>
        [XmlIgnore]
        public DayOfWeekAndPeriod DayOfWeekAndPeriod
        {
            get { return (DayOfWeekAndPeriod)this.GetValue(DayOfWeekAndPeriodProperty); }
            private set { this.SetValue(DayOfWeekAndPeriodProperty, value); }
        }

        /// <summary>
        /// 開講期間の種類を取得または設定します。
        /// </summary>
        [XmlElement]
        public UnitType? UnitType
        {
            get { return (UnitType?)this.GetValue(UnitTypeProperty); }
            set { this.SetValue(UnitTypeProperty, value); }
        }

        /// <summary>
        /// 単位数を取得または設定します。
        /// </summary>
        [XmlElement]
        public decimal? Credit
        {
            get { return (decimal?)this.GetValue(CreditProperty); }
            set { this.SetValue(CreditProperty, value); }
        }

        /// <summary>
        /// クォーター基準の単位数を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal? CreditPerQuarter
        {
            get { return (decimal?)this.GetValue(CreditPerQuarterProperty); }
            protected set { this.SetValue(CreditPerQuarterProperty, value); }
        }

        /// <summary>
        /// 適正人数を取得または設定します。
        /// </summary>
        [XmlElement]
        public int? MaxSize
        {
            get { return (int?)this.GetValue(MaxSizeProperty); }
            set { this.SetValue(MaxSizeProperty, value); }
        }

        /// <summary>
        /// 登録者数を取得または設定します。
        /// </summary>
        [XmlElement]
        public int? RegisteredSize
        {
            get { return (int?)this.GetValue(RegisteredSizeProperty); }
            set { this.SetValue(RegisteredSizeProperty, value); }
        }

        /// <summary>
        /// 登録の残り枠数を取得します。
        /// </summary>
        [XmlIgnore]
        public int? RestSize
        {
            get { return (int?)this.GetValue(RestSizeProperty); }
            protected set { this.SetValue(RestSizeProperty, value); }
        }

        /// <summary>
        /// 抽選対象かどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? IsDoneLottery
        {
            get { return (bool?)this.GetValue(IsDoneLotteryProperty); }
            set { this.SetValue(IsDoneLotteryProperty, value); }
        }

        /// <summary>
        /// 追加予定かどうかを取得または設定します。
        /// </summary>
        [XmlIgnore]
        public bool IsToAdd
        {
            get { return (bool)this.GetValue(IsToAddProperty); }
            set { this.SetValue(IsToAddProperty, value); }
        }

        /// <summary>
        /// 削除予定かどうかを取得または設定します。
        /// </summary>
        [XmlIgnore]
        public bool IsToDelete
        {
            get { return (bool)this.GetValue(IsToDeleteProperty); }
            set { this.SetValue(IsToDeleteProperty, value); }
        }

        #endregion

        #region プロパティ(シラバス)

        /// <summary>
        /// シラバスの詳細分類を取得または設定します。
        /// </summary>
        [XmlElement]
        public string RawSyllabusSchoolOrDivision
        {
            get { return (string)this.GetValue(RawSyllabusSchoolOrDivisionProperty); }
            set { this.SetValue(RawSyllabusSchoolOrDivisionProperty, value); }
        }

        /// <summary>
        /// シラバスの授業科目名を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusTitle
        {
            get { return (string)this.GetValue(SyllabusTitleProperty); }
            set { this.SetValue(SyllabusTitleProperty, value); }
        }

        /// <summary>
        /// シティカレッジかどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? IsCityCollege
        {
            get { return (bool?)this.GetValue(IsCityCollegeProperty); }
            set { this.SetValue(IsCityCollegeProperty, value); }
        }

        /// <summary>
        /// シラバスの担当教員名を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusInstructor
        {
            get { return (string)this.GetValue(SyllabusInstructorProperty); }
            set { this.SetValue(SyllabusInstructorProperty, value); }
        }

        /// <summary>
        /// シラバスの科目ナンバーを取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusNumberingCode
        {
            get { return (string)this.GetValue(SyllabusNumberingCodeProperty); }
            set { this.SetValue(SyllabusNumberingCodeProperty, value); }
        }

        /// <summary>
        /// シラバスの科目区分を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusCategory
        {
            get { return (string)this.GetValue(SyllabusCategoryProperty); }
            set { this.SetValue(SyllabusCategoryProperty, value); }
        }

        /// <summary>
        /// シラバスの講義形態を取得または設定します。
        /// </summary>
        [XmlElement]
        public LectureForm? SyllabusLectureForm
        {
            get { return (LectureForm?)this.GetValue(SyllabusLectureFormProperty); }
            set { this.SetValue(SyllabusLectureFormProperty, value); }
        }

        /// <summary>
        /// シラバスの対象学生を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusAssignedYear
        {
            get { return (string)this.GetValue(SyllabusAssignedYearProperty); }
            set { this.SetValue(SyllabusAssignedYearProperty, value); }
        }

        /// <summary>
        /// 第1クォーターに開講するかどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? OffersInQ1
        {
            get { return (bool?)this.GetValue(OffersInQ1Property); }
            set { this.SetValue(OffersInQ1Property, value); }
        }

        /// <summary>
        /// 第2クォーターに開講するかどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? OffersInQ2
        {
            get { return (bool?)this.GetValue(OffersInQ2Property); }
            set { this.SetValue(OffersInQ2Property, value); }
        }

        /// <summary>
        /// 第3クォーターに開講するかどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? OffersInQ3
        {
            get { return (bool?)this.GetValue(OffersInQ3Property); }
            set { this.SetValue(OffersInQ3Property, value); }
        }

        /// <summary>
        /// 第4クォーターに開講するかどうかを取得または設定します。
        /// </summary>
        [XmlElement]
        public bool? OffersInQ4
        {
            get { return (bool?)this.GetValue(OffersInQ4Property); }
            set { this.SetValue(OffersInQ4Property, value); }
        }

        /// <summary>
        /// シラバスのキーワードを取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusKeyword
        {
            get { return (string)this.GetValue(SyllabusKeywordProperty); }
            set { this.SetValue(SyllabusKeywordProperty, value); }
        }

        /// <summary>
        /// シラバスの授業の主題を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusTopic
        {
            get { return (string)this.GetValue(SyllabusTopicProperty); }
            set { this.SetValue(SyllabusTopicProperty, value); }
        }

        /// <summary>
        /// シラバスの授業の目標を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusObjective
        {
            get { return (string)this.GetValue(SyllabusObjectiveProperty); }
            set { this.SetValue(SyllabusObjectiveProperty, value); }
        }

        /// <summary>
        /// シラバスの学生の学習目標を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusLearningOutcomes
        {
            get { return (string)this.GetValue(SyllabusLearningOutcomesProperty); }
            set { this.SetValue(SyllabusLearningOutcomesProperty, value); }
        }

        /// <summary>
        /// シラバスの授業の概要を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusOutline
        {
            get { return (string)this.GetValue(SyllabusOutlineProperty); }
            set { this.SetValue(SyllabusOutlineProperty, value); }
        }

        /// <summary>
        /// シラバスの講義スケジュールを表す元のHTMLテキストを取得または設定します。
        /// </summary>
        [XmlElement]
        public string RawSyllabusSchedule
        {
            get { return (string)this.GetValue(RawSyllabusScheduleProperty); }
            set { this.SetValue(RawSyllabusScheduleProperty, value); }
        }

        /// <summary>
        /// シラバスの評価の方法を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusGradingMethod
        {
            get { return (string)this.GetValue(SyllabusGradingMethodProperty); }
            set { this.SetValue(SyllabusGradingMethodProperty, value); }
        }

        /// <summary>
        /// シラバスの評価の割合を表す元のHTMLテキストを取得または設定します。
        /// </summary>
        [XmlElement]
        public string RawSyllabusGradingRate
        {
            get { return (string)this.GetValue(RawSyllabusGradingRateProperty); }
            set { this.SetValue(RawSyllabusGradingRateProperty, value); }
        }

        /// <summary>
        /// シラバスのテキスト・教材・参考書等を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusTeachingMaterials
        {
            get { return (string)this.GetValue(SyllabusTeachingMaterialsProperty); }
            set { this.SetValue(SyllabusTeachingMaterialsProperty, value); }
        }

        /// <summary>
        /// シラバスのその他履修上の注意事項や学習上の助言を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusOthers
        {
            get { return (string)this.GetValue(SyllabusOthersProperty); }
            set { this.SetValue(SyllabusOthersProperty, value); }
        }

        /// <summary>
        /// シラバスのオフィスアワー等（学生からの質問への対応方法等）を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusConsultationTime
        {
            get { return (string)this.GetValue(SyllabusConsultationTimeProperty); }
            set { this.SetValue(SyllabusConsultationTimeProperty, value); }
        }

        /// <summary>
        /// シラバスの履修条件を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusPrerequisites
        {
            get { return (string)this.GetValue(SyllabusPrerequisitesProperty); }
            set { this.SetValue(SyllabusPrerequisitesProperty, value); }
        }

        /// <summary>
        /// シラバスの受講者数調整の方法または適正人数と受講者の調整方法を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusMethodForAdjustingClassSize
        {
            get { return (string)this.GetValue(SyllabusMethodForAdjustingClassSizeProperty); }
            set { this.SetValue(SyllabusMethodForAdjustingClassSizeProperty, value); }
        }

        /// <summary>
        /// シラバスの開放科目を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusSubjectOfOpen
        {
            get { return (string)this.GetValue(SyllabusSubjectOfOpenProperty); }
            set { this.SetValue(SyllabusSubjectOfOpenProperty, value); }
        }

        /// <summary>
        /// シラバスの関連科目を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusRelatedCourses
        {
            get { return (string)this.GetValue(SyllabusRelatedCoursesProperty); }
            set { this.SetValue(SyllabusRelatedCoursesProperty, value); }
        }

        /// <summary>
        /// シラバスのカリキュラムの中の位置づけ（関連科目、履修条件等）を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusRelationsWithTheOtherCoursesInTheCurriculum
        {
            get { return (string)this.GetValue(SyllabusRelationsWithTheOtherCoursesInTheCurriculumProperty); }
            set { this.SetValue(SyllabusRelationsWithTheOtherCoursesInTheCurriculumProperty, value); }
        }

        /// <summary>
        /// シラバスの特記事項を取得または設定します。
        /// </summary>
        [XmlElement]
        public string SyllabusSpecialNote
        {
            get { return (string)this.GetValue(SyllabusSpecialNoteProperty); }
            set { this.SetValue(SyllabusSpecialNoteProperty, value); }
        }

        #endregion

        #region プロパティ(整形後シラバス)

        /// <summary>
        /// 日本語の科目の種類を取得します。
        /// </summary>
        [XmlIgnore]
        public string JPSubjectType
        {
            get { return (string)this.GetValue(JPSubjectTypeProperty); }
            private set { this.SetValue(JPSubjectTypeProperty, value); }
        }

        /// <summary>
        /// 英語の科目の種類を取得します。
        /// </summary>
        [XmlIgnore]
        public string ENSubjectType
        {
            get { return (string)this.GetValue(ENSubjectTypeProperty); }
            private set { this.SetValue(ENSubjectTypeProperty, value); }
        }

        /// <summary>
        /// シラバスの日本語の詳細分類を取得します。
        /// </summary>
        [XmlIgnore]
        public string JPSyllabusSchoolOrDivision
        {
            get { return (string)this.GetValue(JPSyllabusSchoolOrDivisionProperty); }
            private set { this.SetValue(JPSyllabusSchoolOrDivisionProperty, value); }
        }

        /// <summary>
        /// シラバスの英語の詳細分類を取得します。
        /// </summary>
        [XmlIgnore]
        public string ENSyllabusSchoolOrDivision
        {
            get { return (string)this.GetValue(ENSyllabusSchoolOrDivisionProperty); }
            private set { this.SetValue(ENSyllabusSchoolOrDivisionProperty, value); }
        }

        /// <summary>
        /// シラバスの日本語の授業科目名を取得します。
        /// </summary>
        [XmlIgnore]
        public string JPSyllabusTitle
        {
            get { return (string)this.GetValue(JPSyllabusTitleProperty); }
            protected set { this.SetValue(JPSyllabusTitleProperty, value); }
        }

        /// <summary>
        /// シラバスの英語の授業科目名を取得します。
        /// </summary>
        [XmlIgnore]
        public string ENSyllabusTitle
        {
            get { return (string)this.GetValue(ENSyllabusTitleProperty); }
            protected set { this.SetValue(ENSyllabusTitleProperty, value); }
        }

        /// <summary>
        /// シラバスの日本語の担当教員名のリストを取得します。
        /// </summary>
        [XmlIgnore]
        public ReadOnlyCollection<string> JPSyllabusInstructors
        {
            get { return (ReadOnlyCollection<string>)this.GetValue(JPSyllabusInstructorsProperty); }
            private set { this.SetValue(JPSyllabusInstructorsProperty, value); }
        }

        /// <summary>
        /// シラバスの英語の担当教員名のリストを取得します。
        /// </summary>
        [XmlIgnore]
        public ReadOnlyCollection<string> ENSyllabusInstructors
        {
            get { return (ReadOnlyCollection<string>)this.GetValue(ENSyllabusInstructorsProperty); }
            private set { this.SetValue(ENSyllabusInstructorsProperty, value); }
        }

        /// <summary>
        /// シラバスの日本語のキーワードを取得します。
        /// </summary>
        [XmlIgnore]
        public ReadOnlyCollection<string> JPSyllabusKeywords
        {
            get { return (ReadOnlyCollection<string>)this.GetValue(JPSyllabusKeywordsProperty); }
            private set { this.SetValue(JPSyllabusKeywordsProperty, value); }
        }

        /// <summary>
        /// シラバスの英語のキーワードを取得します。
        /// </summary>
        [XmlIgnore]
        public ReadOnlyCollection<string> ENSyllabusKeywords
        {
            get { return (ReadOnlyCollection<string>)this.GetValue(ENSyllabusKeywordsProperty); }
            private set { this.SetValue(ENSyllabusKeywordsProperty, value); }
        }

        /// <summary>
        /// シラバスの評価の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public string SyllabusGradingRate
        {
            get { return (string)this.GetValue(SyllabusGradingRateProperty); }
            private set { this.SetValue(SyllabusGradingRateProperty, value); }
        }

        /// <summary>
        /// シラバスの評価の割合を表すデータの詳細を取得します。
        /// </summary>
        [XmlIgnore]
        public ReadOnlyCollection<GradingRate> SyllabusGradingRateDetails
        {
            get { return (ReadOnlyCollection<GradingRate>)this.GetValue(SyllabusGradingRateDetailsProperty); }
            private set { this.SetValue(SyllabusGradingRateDetailsProperty, value); }
        }

        /// <summary>
        /// シラバスの評価の割合の総計値を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusSumOfGradingRate
        {
            get { return (decimal)this.GetValue(SyllabusSumOfGradingRateProperty); }
            private set { this.SetValue(SyllabusSumOfGradingRateProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する出席の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfAttendance
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfAttendanceProperty); }
            private set { this.SetValue(SyllabusGradingRateOfAttendanceProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する論文の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfThesis
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfThesisProperty); }
            private set { this.SetValue(SyllabusGradingRateOfThesisProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対するレポートの割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfReport
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfReportProperty); }
            private set { this.SetValue(SyllabusGradingRateOfReportProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する課題の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfHomework
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfHomeworkProperty); }
            private set { this.SetValue(SyllabusGradingRateOfHomeworkProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する小テストの割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfMiniExam
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfMiniExamProperty); }
            private set { this.SetValue(SyllabusGradingRateOfMiniExamProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する中間試験の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfMidtermExam
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfMidtermExamProperty); }
            private set { this.SetValue(SyllabusGradingRateOfMidtermExamProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する期末試験の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfFinalExam
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfFinalExamProperty); }
            private set { this.SetValue(SyllabusGradingRateOfFinalExamProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する発表の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfPresentation
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfPresentationProperty); }
            private set { this.SetValue(SyllabusGradingRateOfPresentationProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する授業態度の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfAttitude
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfAttitudeProperty); }
            private set { this.SetValue(SyllabusGradingRateOfAttitudeProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する演習の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfExercise
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfExerciseProperty); }
            private set { this.SetValue(SyllabusGradingRateOfExerciseProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対する実験の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfExperiment
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfExperimentProperty); }
            private set { this.SetValue(SyllabusGradingRateOfExperimentProperty, value); }
        }

        /// <summary>
        /// シラバスの評価に対するその他の割合を取得します。
        /// </summary>
        [XmlIgnore]
        public decimal SyllabusGradingRateOfOthers
        {
            get { return (decimal)this.GetValue(SyllabusGradingRateOfOthersProperty); }
            private set { this.SetValue(SyllabusGradingRateOfOthersProperty, value); }
        }

        #endregion
        
        #region 静的メンバー・静的メソッド

        /// <summary>
        /// GS科目の形式の <see cref="Id"/> を表す正規表現を取得します。
        /// </summary>
        private static Regex GSIDRegex { get; } = new Regex(
            "7(?<kugs>[1-5])(?<sign>[A-F]).(?<sign_detail>.)\\..*?$",
            RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// 評価の割合の項目名比較時に使われる <see cref="CompareInfo"/> を取得します。
        /// </summary>
        private static CompareInfo CompareInfo { get; } = CultureInfo.InvariantCulture.CompareInfo;

        /// <summary>
        /// 指定した日本語の文字列が与えられた文字列内に存在するかどうかを示す値を返します。
        /// </summary>
        /// <param name="source"> 検索対象の文字列。 </param>
        /// <param name="value"> <see cref="source"/> 内で検索する文字列。 </param>
        /// <returns> <see cref="source"/> 全体内で <see cref="value"/> が見つかった場合は <see cref="true"/> 、それ以外の場合は <see cref="false"/> 。 </returns>
        private static bool ContainsJP(string source, string value)
            => !string.IsNullOrEmpty(source) && CompareInfo.IndexOf(source, value, CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth) >= 0;

        /// <summary>
        /// 指定した英語の文字列が与えられた文字列内に存在するかどうかを示す値を返します。
        /// </summary>
        /// <param name="source"> 検索対象の文字列。 </param>
        /// <param name="value"> <see cref="source"/> 内で検索する文字列。 </param>
        /// <returns> <see cref="source"/> 全体内で <see cref="value"/> が見つかった場合は <see cref="true"/> 、それ以外の場合は <see cref="false"/> 。 </returns>
        private static bool ContainsEN(string source, string value)
            => !string.IsNullOrEmpty(source) && CompareInfo.IndexOf(source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreWidth | CompareOptions.IgnoreSymbols) >= 0;

        #endregion

        #region プロパティ変更時のコールバック関数

        /// <summary>
        /// <see cref="RegisterNumber"/> プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnRegisterNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(RegistersProperty, ((int?)d.GetValue(RegisterNumberProperty)).HasValue);
            d.SetValue(NotRegistersProperty, !((int?)d.GetValue(RegisterNumberProperty)).HasValue);
            ////d.SetValue(RegistersProperty, ((int?)e.NewValue).HasValue);
            ////d.SetValue(NotRegistersProperty, !((int?)e.NewValue).HasValue);
        }

        /// <summary>
        /// <see cref="Id"/> プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ////var gsidRegex = new Regex("7(?<kugs>[1-5])(?<sign>[A-F]).(?<sign_detail>.)\\..*?$", RegexOptions.Singleline);
            var gsidMatch = Course.GSIDRegex.Match((string)e.NewValue);

            if (gsidMatch.Success)
            {
                d.SetValue(KugsOfGSProperty, int.Parse(gsidMatch.Groups["kugs"].Value, CultureInfo.InvariantCulture));
                d.SetValue(SignOfGSProperty, gsidMatch.Groups["sign"].Value);
                d.SetValue(SignDetailOfGSProperty, gsidMatch.Groups["sign_detail"].Value == "0" ? string.Empty : gsidMatch.Groups["sign_detail"].Value);
            }
            else
            {
                d.SetValue(KugsOfGSProperty, null);
                d.SetValue(SignOfGSProperty, string.Empty);
                d.SetValue(SignDetailOfGSProperty, string.Empty);
            }
        }

        /// <summary>
        /// <see cref="DayOfWeek"/> プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnDayOfWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(
                DayOfWeekAndPeriodProperty,
                new DayOfWeekAndPeriod((DayOfWeek)e.NewValue, ((DayOfWeekAndPeriod)d.GetValue(DayOfWeekAndPeriodProperty)).Period));
        }

        /// <summary>
        /// <see cref="Period"/> プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnPeriodChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(
                DayOfWeekAndPeriodProperty,
                new DayOfWeekAndPeriod(((DayOfWeekAndPeriod)d.GetValue(DayOfWeekAndPeriodProperty)).DayOfWeek, (int?)e.NewValue));
        }

        /// <summary>
        /// <see cref="CreditPerQuarter"/> プロパティの有効なプロパティ値を変更したいときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnCreditPerQuarterChanging(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var unitType = (UnitType?)d.GetValue(UnitTypeProperty);
            var credit = (decimal?)d.GetValue(CreditProperty);

            if (!unitType.HasValue || !credit.HasValue)
            {
                d.SetValue(CreditPerQuarterProperty, null);
                return;
            }

            switch (unitType.Value)
            {
                case AutoCourseRegistration.UnitType.Quarter:
                    d.SetValue(CreditPerQuarterProperty, credit.Value);
                    return;
                case AutoCourseRegistration.UnitType.Semester:
                    d.SetValue(CreditPerQuarterProperty, credit.Value / 2);
                    return;
                case AutoCourseRegistration.UnitType.Year:
                    d.SetValue(CreditPerQuarterProperty, credit.Value / 4);
                    return;
                default:
                    d.SetValue(CreditPerQuarterProperty, null);
                    return;
            }
        }

        /// <summary>
        /// <see cref="RestSize"/> プロパティの有効なプロパティ値を変更したいときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnRestSizeChanging(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var maxSize = (int?)d.GetValue(MaxSizeProperty);
            var registeredSize = (int?)d.GetValue(RegisteredSizeProperty);

            if (!maxSize.HasValue || !registeredSize.HasValue)
            {
                d.SetValue(RestSizeProperty, null);
                return;
            }

            var result = maxSize.Value - registeredSize.Value;

            if (result > 0)
            {
                d.SetValue(RestSizeProperty, result);
                return;
            }

            d.SetValue(RestSizeProperty, null);
        }

        #endregion

        #region プロパティ(シラバス)変更時のコールバック関数

        /// <summary>
        /// <see cref="RawSubjectType"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnRawSubjectTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyword = GetSubjectTypeAndSyllabusSchoolOrDivision((string)e.NewValue);

            d.SetValue(JPSubjectTypeProperty, keyword.JP);
            d.SetValue(ENSubjectTypeProperty, keyword.EN);
        }

        /// <summary>
        /// <see cref="RawSyllabusSchoolOrDivision"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnRawSyllabusSchoolOrDivisionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var keyword = GetSubjectTypeAndSyllabusSchoolOrDivision((string)e.NewValue);

            d.SetValue(JPSyllabusSchoolOrDivisionProperty, keyword.JP);
            d.SetValue(ENSyllabusSchoolOrDivisionProperty, keyword.EN);
        }

        /// <summary>
        /// <see cref="SyllabusTitle"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnSyllabusTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string)e.NewValue;
            var match = SyllabusRegex.Title.Match(value);

            if (!match.Success)
            {
                d.SetValue(JPSyllabusTitleProperty, value);
                d.SetValue(ENSyllabusTitleProperty, null);
                return;
            }

            d.SetValue(JPSyllabusTitleProperty, match.Groups["jp"].Value.Trim());
            d.SetValue(ENSyllabusTitleProperty, match.Groups["en"].Value.Trim());
            return;
        }

        /// <summary>
        /// <see cref="SyllabusInstructor"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnSyllabusInstructorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var jp = new List<string>();
            var en = new List<string>();

            for (var match = SyllabusRegex.Instructor.Match((string)e.NewValue);
                match.Success;
                match = match.NextMatch())
            {
                jp.Add(match.Groups["jp"].Value.Trim());
                en.Add(match.Groups["en"].Value.Trim());
            }

            d.SetValue(JPSyllabusInstructorsProperty, new ReadOnlyCollection<string>(jp));
            d.SetValue(ENSyllabusInstructorsProperty, new ReadOnlyCollection<string>(en));
            return;
        }

        /// <summary>
        /// <see cref="SyllabusKeyword"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnSyllabusKeywordsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string)e.NewValue;

            var jp = new List<string>();
            var en = new List<string>();

            ////var id = (string)d.GetValue(IdProperty); // デバッグ用

            if (value.Contains(Environment.NewLine) || value.Contains("\n"))
            {
                #region 複数行の場合

                for (var lineMatch = SyllabusRegex.NewLine.Match(value);
                    lineMatch.Success;
                    lineMatch = lineMatch.NextMatch())
                {
                    var line = lineMatch.Groups["line"].Value;
                    Match allMatch;

                    if ((allMatch = SyllabusRegex.MultilineKeywordsEN.Match(line)).Success)
                    {
                        en.AddRange(SyllabusRegex.MatchKeyword3EN(allMatch.Groups["body"].Value));
                        ////for (var match = SyllabusRegex.Keyword3EN.Match(allMatch.Groups["body"].Value);
                        ////    match.Success;
                        ////    match = match.NextMatch())
                        ////{
                        ////    en.Add(match.Groups["keyword"].Value);
                        ////}
                    }
                    else if ((allMatch = SyllabusRegex.MultilineKeywordsJP.Match(line)).Success)
                    {
                        jp.AddRange(SyllabusRegex.MatchKeyword3JP(allMatch.Groups["body"].Value));
                        ////for (var match = SyllabusRegex.Keyword3JP.Match(allMatch.Groups["body"].Value);
                        ////    match.Success;
                        ////    match = match.NextMatch())
                        ////{
                        ////    jp.Add(match.Groups["keyword"].Value);
                        ////}
                    }
                    else
                    {
                    }
                }

                #endregion
            }
            else
            {
                #region 単一行の場合

                Match allMatch;

                if (value.Any(c => SyllabusRegex.LeftParenthesisAndDoubleQuoteCharacters.Any(l => l == c))
                    && (allMatch = SyllabusRegex.SinglelineKeywords1.Match(value)).Success)
                {
                    jp.AddRange(SyllabusRegex.MatchKeyword1JP(allMatch.Groups["jpBody"].Value));
                    en.AddRange(SyllabusRegex.MatchKeyword1EN(allMatch.Groups["enBody"].Value));
                }
                else if ((allMatch = SyllabusRegex.MultilineKeywordsEN.Match(value)).Success)
                {
                    en.AddRange(SyllabusRegex.MatchKeyword3EN(allMatch.Groups["body"].Value));
                }
                else if ((allMatch = SyllabusRegex.SinglelineKeywords2.Match(value)).Success)
                {
                    foreach (var k in SyllabusRegex.MatchKeyword2(allMatch.Groups["body"].Value))
                    {
                        jp.Add(k.JP);
                        en.Add(k.EN);
                    }
                }
                else if ((allMatch = SyllabusRegex.SinglelineKeywords3.Match(value)).Success)
                {
                    jp.AddRange(SyllabusRegex.MatchKeyword3JP(allMatch.Groups["jpBody"].Value));
                    en.AddRange(SyllabusRegex.MatchKeyword3EN(allMatch.Groups["enBody"].Value));
                }
                else if ((allMatch = SyllabusRegex.MultilineKeywordsJP.Match(value)).Success)
                {
                    jp.AddRange(SyllabusRegex.MatchKeyword3JP(allMatch.Groups["body"].Value));
                }
                else
                {
                }

                #endregion
            }

            #region それぞれの日本語キーワードに英語キーワードが対応しているか判定・処理
            {
                if (jp.Count > 0 && jp.All(s => SyllabusRegex.KeywordMixed.Match(s).Success))
                {
                    for (var index = 0; index < jp.Count; index++)
                    {
                        var match = SyllabusRegex.KeywordMixed.Match(jp[index]);
                        jp[index] = match.Groups["jp"].Value;
                        en.Add(match.Groups["en"].Value);
                    }
                }
            }
            #endregion

            #region コメントアウト

            ////string valueJp = string.Empty;
            ////string valueEn = string.Empty;

            ////var firstNewLineIndex = value.IndexOf(Environment.NewLine);

            ////if (firstNewLineIndex >= 0)
            ////{
            ////    valueJp = value.Substring(0, firstNewLineIndex);
            ////    valueEn = value.Substring(firstNewLineIndex);
            ////}
            ////else
            ////{
            ////    valueJp = value;
            ////}

            ////for (var match = SyllabusRegex.SyllabusKeywordsRegex.Match(valueJp);
            ////    match.Success;
            ////    match = match.NextMatch())
            ////{
            ////    var str = match.Groups[1].Value.Trim();

            ////    if (!string.IsNullOrWhiteSpace(str))
            ////    {
            ////        jp.Add(str);
            ////    }
            ////}

            ////for (var match = SyllabusRegex.SyllabusKeywordsRegex.Match(valueEn);
            ////    match.Success;
            ////    match = match.NextMatch())
            ////{
            ////    var str = match.Groups[1].Value.Trim();

            ////    if (!string.IsNullOrWhiteSpace(str))
            ////    {
            ////        en.Add(str);
            ////    }
            ////}

            #endregion

            d.SetValue(
                JPSyllabusKeywordsProperty, 
                new ReadOnlyCollection<string>(
                    jp.Select(s => s.Trim().Trim('・'))
                    .Where(s => !string.IsNullOrEmpty(s) && !SyllabusRegex.KeywordOwnStringJP.Match(s).Success).ToList()));
            d.SetValue(
                ENSyllabusKeywordsProperty,
                new ReadOnlyCollection<string>(
                    en.Select(s => SyllabusRegex.ConvertKeyword(s.Trim().Trim('・').Replace('・', '/').Replace('　', ' ')))
                    .Where(s => !string.IsNullOrEmpty(s) && !SyllabusRegex.KeywordOwnStringEN.Match(s).Success).ToList()));
            return;
        }

        /// <summary>
        /// <see cref="RawSyllabusGradingRate"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnRawSyllabusGradingRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string)e.NewValue;

            StringBuilder gradingRate = new StringBuilder();

            var singlelineDetails = new List<GradingRate>();
            var lineDetails = new List<GradingRate>();
            var tableDetails = new List<GradingRate>();

            for (var rowMatch = SyllabusRegex.TableRow.Match(value);
                rowMatch.Success;
                rowMatch = rowMatch.NextMatch())
            {
                var rowBodyMatch = SyllabusRegex.SinglelineGradingRateString.Match(rowMatch.Groups[1].Value);

                if (rowBodyMatch.Success)
                {
                    #region 単一行形式の場合

                    for (var dataMatch = SyllabusRegex.TableData.Match(rowBodyMatch.Groups[1].Value);
                        dataMatch.Success;
                        dataMatch = dataMatch.NextMatch())
                    {
                        var dataText = MainWindow.GetHtmlDecodedAndFormattedString(dataMatch.Groups[1].Value, true);
                        gradingRate.AppendLine(dataText);

                        for (var lineMatch = SyllabusRegex.NewLine.Match(dataText);
                            lineMatch.Success;
                            lineMatch = lineMatch.NextMatch())
                        {
                            var line = lineMatch.Groups["line"].Value;

                            if (line.Contains("%") || line.Contains("％"))
                            {
                                lineDetails.AddDetails(line, SyllabusRegex.GradingRate, SyllabusRegex.GradingRate2);
                            }

                            var specialSinglelineMatch = SyllabusRegex.SpecialSinglelineGradingRate.Match(line);

                            if (specialSinglelineMatch.Success)
                            {
                                for (var match = SyllabusRegex.SpecialSinglelineGradingRate2.Match(line);
                                    match.Success;
                                    match = match.NextMatch())
                                {
                                    string keyEN = match.Groups["en"].Value.Trim(SyllabusRegex.DelimiterAndWhiteSpaceCharacters);
                                    decimal? percentage = MainWindow.TryParseNullableDecimal(SyllabusRegex.ConvertAscii(match.Groups["percentage"].Value));

                                    if (!string.IsNullOrEmpty(keyEN) && percentage.HasValue)
                                    {
                                        singlelineDetails.AddOrFix(new GradingRate(string.Empty, string.IsNullOrEmpty(keyEN) ? null : keyEN, percentage));
                                    }
                                }
                            }
                            else
                            {
                                singlelineDetails.AddDetails(line, SyllabusRegex.SinglelineGradingRate, SyllabusRegex.SinglelineGradingRate2);
                            }
                        }
                    }

                    #endregion
                }
                else
                {
                    #region テーブル形式の場合

                    Match dataMatch;

                    string keyJP = null;
                    string keyEN = null;

                    #region keyJP, keyEN
                    {
                        dataMatch = SyllabusRegex.TableData.Match(rowMatch.Groups[1].Value);

                        if (!dataMatch.Success)
                        {
                            gradingRate.AppendLine();
                            continue;
                        }

                        var keyString = MainWindow.GetHtmlDecodedAndFormattedString(dataMatch.Groups[1].Value, true);
                        var keyMatch = SyllabusRegex.GradingRateKey.Match(keyString);
                        var keyENMatch = SyllabusRegex.GradingRateENKey.Match(keyString);

                        gradingRate.Append(keyString);

                        if (keyENMatch.Success)
                        {
                            keyEN = keyString;
                            keyJP = keyString;
                        }
                        else if (keyMatch.Success)
                        {
                            keyJP = keyMatch.Groups["jp"].Value.Trim();
                            keyEN = keyMatch.Groups["en"].Value.Trim();
                        }
                        else if (!string.IsNullOrEmpty(keyString))
                        {
                            keyJP = keyString;

                            if (keyENMatch.Success)
                            {
                                keyEN = keyString;
                            }
                        }
                    }
                    #endregion

                    decimal? percentage = null;

                    #region percentage
                    {
                        dataMatch = dataMatch.NextMatch();

                        if (!dataMatch.Success)
                        {
                            gradingRate.AppendLine();
                            continue;
                        }

                        var valueString = MainWindow.GetHtmlDecodedAndFormattedString(dataMatch.Groups[1].Value, true);
                        var valueMatch = SyllabusRegex.GradingRatePercentage.Match(valueString);

                        gradingRate.Append("\t");
                        gradingRate.Append(valueString);

                        if (valueMatch.Success)
                        {
                            percentage = MainWindow.TryParseNullableDecimal(SyllabusRegex.ConvertAscii(valueMatch.Groups[1].Value));
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(keyJP) || !string.IsNullOrEmpty(keyEN) || percentage.HasValue)
                    {
                        tableDetails.AddOrFix(new GradingRate(keyJP, keyEN, percentage));
                    }

                    dataMatch = dataMatch.NextMatch();

                    while (dataMatch.Success)
                    {
                        gradingRate.Append("\t");
                        gradingRate.Append(MainWindow.GetHtmlDecodedAndFormattedString(dataMatch.Groups[1].Value, true));
                        dataMatch = dataMatch.NextMatch();
                    }

                    gradingRate.AppendLine();

                    #endregion
                }
            }

            var sumOfSinglelineDetails = singlelineDetails.Sum(s => s.Percentage.HasValue ? s.Percentage.Value : 0);
            var sumOfLineDetails = lineDetails.Sum(s => s.Percentage.HasValue ? s.Percentage.Value : 0);
            var sumOfTableDetails = tableDetails.Sum(s => s.Percentage.HasValue ? s.Percentage.Value : 0);

            List<GradingRate> result;

            #region どの詳細をresultにするか判定
            {
                if (sumOfTableDetails == 0 && sumOfLineDetails == 0)
                {
                    tableDetails.AddRange(lineDetails);
                    result = tableDetails;
                }
                else if (sumOfTableDetails == 100)
                {
                    result = tableDetails;
                }
                else if (sumOfSinglelineDetails == 100)
                {
                    result = singlelineDetails;
                }
                else if (sumOfLineDetails == 100)
                {
                    result = lineDetails;
                }
                else if (sumOfTableDetails + sumOfSinglelineDetails == 100)
                {
                    tableDetails.AddRange(singlelineDetails);
                    result = tableDetails;
                }
                else if (sumOfTableDetails + sumOfLineDetails == 100)
                {
                    tableDetails.AddRange(lineDetails);
                    result = tableDetails;
                }
                else
                {
                    result = tableDetails;
                    decimal sum = sumOfTableDetails > 0 ? sumOfTableDetails : decimal.MaxValue;

                    if (sumOfSinglelineDetails > 0 && sumOfSinglelineDetails < sum)
                    {
                        result = singlelineDetails;
                        sum = sumOfSinglelineDetails;
                    }

                    if (sumOfLineDetails > 0 && sumOfLineDetails < sum)
                    {
                        result = lineDetails;
                        sum = sumOfLineDetails;
                    }
                }
            }
            #endregion

            d.SetValue(SyllabusGradingRateProperty, gradingRate.ToString());
            d.SetValue(SyllabusGradingRateDetailsProperty, new ReadOnlyCollection<GradingRate>(result));
        }

        /// <summary>
        /// <see cref="SyllabusGradingRateDetails"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /// </summary>
        /// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        private static void OnSyllabusGradingRateDetailsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (ReadOnlyCollection<GradingRate>)e.NewValue;

            d.SetValue(SyllabusSumOfGradingRateProperty, value.Sum(r => r.Percentage.HasValue ? r.Percentage.Value : 0));
            d.SetValue(SyllabusGradingRateOfAttendanceProperty, GetSumOfPercentage(value, GradingType.Attendance));
            d.SetValue(SyllabusGradingRateOfThesisProperty, GetSumOfPercentage(value, GradingType.Thesis));
            d.SetValue(SyllabusGradingRateOfReportProperty, GetSumOfPercentage(value, GradingType.Report));
            d.SetValue(SyllabusGradingRateOfHomeworkProperty, GetSumOfPercentage(value, GradingType.Homework));
            d.SetValue(SyllabusGradingRateOfMiniExamProperty, GetSumOfPercentage(value, GradingType.MiniExam));
            d.SetValue(SyllabusGradingRateOfMidtermExamProperty, GetSumOfPercentage(value, GradingType.MidtermExam));
            d.SetValue(SyllabusGradingRateOfFinalExamProperty, GetSumOfPercentage(value, GradingType.FinalExam));
            d.SetValue(SyllabusGradingRateOfPresentationProperty, GetSumOfPercentage(value, GradingType.Presentation));
            d.SetValue(SyllabusGradingRateOfAttitudeProperty, GetSumOfPercentage(value, GradingType.Attitude));
            d.SetValue(SyllabusGradingRateOfExerciseProperty, GetSumOfPercentage(value, GradingType.Exercise));
            d.SetValue(SyllabusGradingRateOfExperimentProperty, GetSumOfPercentage(value, GradingType.Experiment));
            d.SetValue(SyllabusGradingRateOfOthersProperty, GetSumOfPercentage(value, GradingType.Others));
        }

        /////// <summary>
        /////// <see cref="SyllabusGradingRateDetails"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /////// </summary>
        /////// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /////// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        ////private static void OnSyllabusGradingRateDetailsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        ////{
        ////    var value = (ReadOnlyCollection<GradingRate>)e.NewValue;

        ////    decimal sum = 0;

        ////    decimal attendance = 0;
        ////    decimal report = 0;
        ////    decimal homework = 0;
        ////    decimal miniExam = 0;
        ////    decimal midtermExam = 0;
        ////    decimal finalExam = 0;
        ////    decimal presentation = 0;
        ////    decimal attitude = 0;
        ////    decimal exercise = 0;
        ////    decimal experiment = 0;
        ////    decimal others = 0;

        ////    foreach (var rate in value)
        ////    {
        ////        if (!rate.Percentage.HasValue)
        ////        {
        ////            continue;
        ////        }

        ////        sum += rate.Percentage.Value;

        ////        if (ContainsJP(rate.JPKey, "出席") || ContainsEN(rate.ENKey, "attendance") || ContainsEN(rate.JPKey, "attendance"))
        ////        {
        ////            attendance += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "レポート") || ContainsEN(rate.ENKey, "report") || ContainsEN(rate.JPKey, "report"))
        ////        {
        ////            report += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "課題") || ContainsJP(rate.JPKey, "宿題") || ContainsJP(rate.JPKey, "提出") || ContainsEN(rate.ENKey, "homework") || ContainsEN(rate.JPKey, "homework"))
        ////        {
        ////            homework += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "小テスト") || ContainsEN(rate.ENKey, "mini") || ContainsEN(rate.JPKey, "mini"))
        ////        {
        ////            miniExam += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "中間") || ContainsEN(rate.ENKey, "midterm") || ContainsEN(rate.JPKey, "midterm"))
        ////        {
        ////            midtermExam += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "期末")
        ////            || ContainsEN(rate.ENKey, "final") || ContainsEN(rate.JPKey, "final")
        ////            || ContainsJP(rate.JPKey, "定期") || ContainsJP(rate.JPKey, "試験") || ContainsJP(rate.JPKey, "テスト")
        ////            || ContainsEN(rate.ENKey, "exam") || ContainsEN(rate.JPKey, "exam")
        ////            || ContainsEN(rate.ENKey, "test") || ContainsEN(rate.JPKey, "test"))
        ////        {
        ////            finalExam += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "発表") || ContainsJP(rate.JPKey, "プレゼン") || ContainsEN(rate.ENKey, "presentation") || ContainsEN(rate.JPKey, "presentation"))
        ////        {
        ////            presentation += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "授業") || ContainsJP(rate.JPKey, "態度") || ContainsJP(rate.JPKey, "平常")
        ////            || ContainsJP(rate.JPKey, "取組") || ContainsJP(rate.JPKey, "取り組み")
        ////            || ContainsEN(rate.ENKey, "attitude") || ContainsEN(rate.JPKey, "attitude"))
        ////        {
        ////            attitude += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "演習") || ContainsJP(rate.JPKey, "実習") || ContainsJP(rate.JPKey, "実技") || ContainsEN(rate.ENKey, "exercise") || ContainsEN(rate.JPKey, "exercise"))
        ////        {
        ////            exercise += rate.Percentage.Value;
        ////        }
        ////        else if (ContainsJP(rate.JPKey, "実験") || ContainsEN(rate.ENKey, "experiment") || ContainsEN(rate.JPKey, "experiment"))
        ////        {
        ////            experiment += rate.Percentage.Value;
        ////        }
        ////        else if ((ContainsJP(rate.JPKey, "合計") && rate.JPKey.Length == "合計".Length)
        ////            || (ContainsJP(rate.JPKey, "計") && rate.JPKey.Length == "計".Length)
        ////            || (ContainsJP(rate.ENKey, "sum") && rate.ENKey.Length == "sum".Length)
        ////            || (ContainsJP(rate.JPKey, "sum") && rate.JPKey.Length == "sum".Length))
        ////        {
        ////            sum -= rate.Percentage.Value;
        ////        }
        ////        else
        ////        {
        ////            others += rate.Percentage.Value;
        ////        }
        ////    }

        ////    d.SetValue(SyllabusSumOfGradingRateProperty, sum);
        ////    d.SetValue(SyllabusGradingRateOfAttendanceProperty, attendance);
        ////    d.SetValue(SyllabusGradingRateOfReportProperty, report);
        ////    d.SetValue(SyllabusGradingRateOfHomeworkProperty, homework);
        ////    d.SetValue(SyllabusGradingRateOfMiniExamProperty, miniExam);
        ////    d.SetValue(SyllabusGradingRateOfMidtermExamProperty, midtermExam);
        ////    d.SetValue(SyllabusGradingRateOfFinalExamProperty, finalExam);
        ////    d.SetValue(SyllabusGradingRateOfPresentationProperty, presentation);
        ////    d.SetValue(SyllabusGradingRateOfAttitudeProperty, attitude);
        ////    d.SetValue(SyllabusGradingRateOfExerciseProperty, exercise);
        ////    d.SetValue(SyllabusGradingRateOfExperimentProperty, experiment);
        ////    d.SetValue(SyllabusGradingRateOfOthersProperty, others);
        ////}

        /////// <summary>
        /////// <see cref="Syllabus"/> 依存関係プロパティの有効なプロパティ値が変更されたときに呼び出されるコールバックを表します。
        /////// </summary>
        /////// <param name="d"> プロパティの値が変更された <see cref="DependencyObject"/> 。 </param>
        /////// <param name="e"> このプロパティの有効値に対する変更を追跡するイベントによって発行されるイベント データ。 </param>
        ////private static void OnSyllabusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        ////{
        ////    var value = (string)e.NewValue;

        ////    d.SetValue(JpSyllabusProperty, match.Groups["jp"].Value.Trim());
        ////    d.SetValue(EnSyllabusProperty, match.Groups["en"].Value.Trim());
        ////    return;
        ////}

        #endregion

        #region メソッド

        /// <summary>
        /// 元の文字列から科目の種類またはシラバスの詳細分類を表す日本語と英語のキーワードを取得します。
        /// </summary>
        /// <param name="origin"> 元の文字列。 </param>
        /// <returns> 日本語と英語のキーワード。 </returns>
        private static SyllabusKeyword GetSubjectTypeAndSyllabusSchoolOrDivision(string origin)
        {
            var match = SyllabusRegex.SubjectTypeAndSyllabusSchoolOrDivision.Match(origin);

            if (match.Success)
            {
                return new SyllabusKeyword(
                    MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["jp"].Value, true),
                    MainWindow.GetHtmlDecodedAndFormattedString(match.Groups["en"].Value, true));
            }
            else
            {
                return new SyllabusKeyword(
                    MainWindow.GetHtmlDecodedAndFormattedString(origin, true),
                    string.Empty);
            }
        }

        /// <summary>
        /// 指定した評価の種類と一致する評価の割合の合計を取得します。
        /// </summary>
        /// <param name="items"> シラバスの評価の割合を表すデータ。 </param>
        /// <param name="gradingType"> 評価の種類。 </param>
        /// <returns> 評価の割合の合計。 </returns>
        private static decimal GetSumOfPercentage(ICollection<GradingRate> items, GradingType gradingType)
            => items.Where(r => r.GradingType == gradingType).Sum(r => r.Percentage.HasValue ? r.Percentage.Value : 0);

        /////// <summary>
        /////// 現在の <see cref="DayOfWeekAndPeriod"/> の値を取得します。
        /////// </summary>
        /////// <returns> 現在の <see cref="DayOfWeekAndPeriod"/> の値。 </returns>
        ////private string GetDayOfWeekAndPeriod()
        ////{
        ////    return Course.DayOfWeekConverter.Convert(this.DayOfWeek, typeof(DayOfWeek), null, CultureInfo.InvariantCulture)
        ////        + (this.Period == 0 ? string.Empty : this.Period.ToString());
        ////}

        /////// <summary>
        /////// 現在の <see cref="CreditPerQuarter"/> の値を取得します。
        /////// </summary>
        /////// <returns> 現在の <see cref="CreditPerQuarter"/> の値。 </returns>
        ////private decimal? GetCreditPerQuarter()
        ////{
        ////    if (!this.UnitType.HasValue || !this.Credit.HasValue)
        ////    {
        ////        return null;
        ////    }

        ////    switch (this.UnitType)
        ////    {
        ////        case AutoCourseRegistration.UnitType.Quarter:
        ////            return this.Credit;
        ////        case AutoCourseRegistration.UnitType.Semester:
        ////            return this.Credit / 2;
        ////        case AutoCourseRegistration.UnitType.Year:
        ////            return this.Credit / 4;
        ////        default:
        ////            return null;
        ////    }
        ////}

        /////// <summary>
        /////// 現在の <see cref="RestSize"/> の値を取得します。
        /////// </summary>
        /////// <returns> 現在の <see cref="RestSize"/> の値。 </returns>
        ////private int? GetRestSize()
        ////{
        ////    if (!this.MaxSize.HasValue || !this.RegisteredSize.HasValue)
        ////    {
        ////        return null;
        ////    }

        ////    var result = this.MaxSize - this.RegisteredSize;
        ////    return result > 0 ? result : null;
        ////}

        #endregion
    }
}