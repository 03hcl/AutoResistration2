namespace Kntacooh.AutoCourseRegistration
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
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
    /// FilterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class FilterWindow : Window
    {
        #region コンストラクタ

        /// <summary>
        /// <see cref="FilterWindow"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="main"> メインウィンドウ。 </param>
        public FilterWindow(MainWindow main)
        {
            this.InitializeComponent();

            this.MainWindow = main;

            this.UpdatesFilter = false;

            #region DayOfWeekAndPeriodCheckBoxes の設定
            {
                this.DayOfWeekAndPeriodFilterItems = new Dictionary<DayOfWeekAndPeriod, FilterItem>();

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 1), new FilterItem(this.CheckBoxMonday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 2), new FilterItem(this.CheckBoxMonday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 3), new FilterItem(this.CheckBoxMonday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 4), new FilterItem(this.CheckBoxMonday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 5), new FilterItem(this.CheckBoxMonday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 6), new FilterItem(this.CheckBoxMonday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Monday, 7), new FilterItem(this.CheckBoxMonday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 1), new FilterItem(this.CheckBoxTuesday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 2), new FilterItem(this.CheckBoxTuesday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 3), new FilterItem(this.CheckBoxTuesday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 4), new FilterItem(this.CheckBoxTuesday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 5), new FilterItem(this.CheckBoxTuesday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 6), new FilterItem(this.CheckBoxTuesday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Tuesday, 7), new FilterItem(this.CheckBoxTuesday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 1), new FilterItem(this.CheckBoxWednesday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 2), new FilterItem(this.CheckBoxWednesday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 3), new FilterItem(this.CheckBoxWednesday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 4), new FilterItem(this.CheckBoxWednesday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 5), new FilterItem(this.CheckBoxWednesday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 6), new FilterItem(this.CheckBoxWednesday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Wednesday, 7), new FilterItem(this.CheckBoxWednesday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 1), new FilterItem(this.CheckBoxThursday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 2), new FilterItem(this.CheckBoxThursday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 3), new FilterItem(this.CheckBoxThursday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 4), new FilterItem(this.CheckBoxThursday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 5), new FilterItem(this.CheckBoxThursday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 6), new FilterItem(this.CheckBoxThursday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Thursday, 7), new FilterItem(this.CheckBoxThursday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 1), new FilterItem(this.CheckBoxFriday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 2), new FilterItem(this.CheckBoxFriday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 3), new FilterItem(this.CheckBoxFriday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 4), new FilterItem(this.CheckBoxFriday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 5), new FilterItem(this.CheckBoxFriday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 6), new FilterItem(this.CheckBoxFriday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Friday, 7), new FilterItem(this.CheckBoxFriday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 1), new FilterItem(this.CheckBoxSaturday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 2), new FilterItem(this.CheckBoxSaturday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 3), new FilterItem(this.CheckBoxSaturday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 4), new FilterItem(this.CheckBoxSaturday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 5), new FilterItem(this.CheckBoxSaturday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 6), new FilterItem(this.CheckBoxSaturday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Saturday, 7), new FilterItem(this.CheckBoxSaturday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 1), new FilterItem(this.CheckBoxSunday1));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 2), new FilterItem(this.CheckBoxSunday2));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 3), new FilterItem(this.CheckBoxSunday3));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 4), new FilterItem(this.CheckBoxSunday4));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 5), new FilterItem(this.CheckBoxSunday5));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 6), new FilterItem(this.CheckBoxSunday6));
                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Sunday, 7), new FilterItem(this.CheckBoxSunday7));

                this.DayOfWeekAndPeriodFilterItems.Add(new DayOfWeekAndPeriod(DayOfWeek.Intensive, 0), new FilterItem(this.CheckBoxIntensive));
            }
            #endregion

            #region UnitTypeFilterItems の設定
            {
                this.UnitTypeFilterItems = new Dictionary<UnitType?, FilterItem>();

                this.UnitTypeFilterItems.Add(UnitType.Quarter, new FilterItem(this.CheckBoxQuarter));
                this.UnitTypeFilterItems.Add(UnitType.Semester, new FilterItem(this.CheckBoxSemester));
                this.UnitTypeFilterItems.Add(UnitType.Year, new FilterItem(this.CheckBoxYear));
            }
            #endregion

            this.UpdatesFilter = true;
        }

        #endregion

        #region プロパティ(private)

        /// <summary>
        /// メインウィンドウを取得します。
        /// </summary>
        private MainWindow MainWindow { get; }

        /// <summary>
        /// 曜日・時限に対応するフィルターのアイテムを取得します。
        /// </summary>
        private Dictionary<DayOfWeekAndPeriod, FilterItem> DayOfWeekAndPeriodFilterItems { get; }

        /// <summary>
        /// 開講期間の単位に対応するフィルターのアイテムを取得します。
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Dictionary<UnitType?, FilterItem> は正当なデザインである。")]
        private Dictionary<UnitType?, FilterItem> UnitTypeFilterItems { get; }

        /// <summary>
        /// 日本語の科目の種類に対応するチェックボックスを取得または設定します。
        /// </summary>
        private Dictionary<string, FilterItem> JPSubjectTypeFilterItems { get; set; } = new Dictionary<string, FilterItem>();

        /// <summary>
        /// シラバスの日本語の詳細分類に対応するチェックボックスを取得または設定します。
        /// </summary>
        private Dictionary<string, FilterItem> JPSyllabusSchoolOrDivisionFilterItems { get; set; } = new Dictionary<string, FilterItem>();

        /// <summary>
        /// シラバスの科目区分に対応するチェックボックスを取得または設定します。
        /// </summary>
        private Dictionary<string, FilterItem> SyllabusCategoryFilterItems { get; set; } = new Dictionary<string, FilterItem>();

        /// <summary>
        /// シラバスの詳細分類に対応するチェックボックスを取得または設定します。
        /// </summary>
        private Dictionary<LectureForm?, FilterItem> SyllabusLectureFormFilterItems { get; set; } = new Dictionary<LectureForm?, FilterItem>();

        /// <summary>
        /// フィルターをすぐに更新するかどうかを取得または設定します。
        /// </summary>
        private bool UpdatesFilter { get; set; }

        #endregion

        #region メソッド

        /// <summary>
        /// フィルターをリセットします。
        /// </summary>
        public void ResetFilter()
        {
            this.UpdateCourseViewFilter();

            this.UpdatesFilter = false;

            #region 登録/追加/削除
            {
                this.CheckBoxNotRegistered.IsChecked = true;
                this.CheckBoxNotRegisteredAndToAdd.IsChecked = true;
                this.CheckBoxRegistered.IsChecked = true;
                this.CheckBoxRegisteredAndToDelete.IsChecked = true;
            }
            #endregion

            #region 曜日・時限
            {
                FilterWindow.ResetFilterItems(this.DayOfWeekAndPeriodFilterItems);
                this.CheckBoxOthers.IsChecked = true;
            }
            #endregion

            #region クォーター
            {
                this.CheckBoxNullOfOffering.IsChecked = true;
                this.CheckBoxQ1.IsChecked = true;
                this.CheckBoxQ2.IsChecked = true;
                this.CheckBoxQ3.IsChecked = true;
                this.CheckBoxQ4.IsChecked = true;
            }
            #endregion

            #region UnitType
            {
                this.CheckBoxNullOfUnitType.IsChecked = true;
                FilterWindow.ResetFilterItems(this.UnitTypeFilterItems);
            }
            #endregion

            #region SubjectType
            {
                this.CheckBoxNullOfJPSubjectType.IsChecked = true;
                FilterWindow.ResetFilterItems(this.JPSubjectTypeFilterItems);
            }
            #endregion

            #region SyllabusSchoolOrDivision
            {
                this.CheckBoxNullOfJPSyllabusSchoolOrDivision.IsChecked = true;
                FilterWindow.ResetFilterItems(this.JPSyllabusSchoolOrDivisionFilterItems);
            }
            #endregion

            #region SyllabusCategory
            {
                this.CheckBoxNullOfSyllabusCategory.IsChecked = true;
                FilterWindow.ResetFilterItems(this.SyllabusCategoryFilterItems);
            }
            #endregion

            #region SyllabusLectureForm
            {
                this.CheckBoxNullOfSyllabusLectureForm.IsChecked = true;
                FilterWindow.ResetFilterItems(this.SyllabusLectureFormFilterItems);
            }
            #endregion

            this.UpdatesFilter = true;

            this.UpdateCourseViewFilter();
        }

        /// <summary>
        /// フィルターのアイテムを更新します。
        /// </summary>
        public void UpdateFilterItem()
        {
            var checkBoxScale = (ScaleTransform)this.FilterGrid.FindResource("CheckBoxScale");

            this.JPSubjectTypeFilterItems = this.CreateFilterItems(
                this.JPSubjectTypeFilterItems,
                this.JPSubjectTypeFilterGrid,
                c => c.JPSubjectType,
                (items, key) => items.FirstOrDefault(item => item.Key == key).Value,
                key => key,
                checkBoxScale);

            this.JPSyllabusSchoolOrDivisionFilterItems = this.CreateFilterItems(
                this.JPSyllabusSchoolOrDivisionFilterItems,
                this.JPSyllabusSchoolOrDivisionFilterGrid,
                c => c.JPSyllabusSchoolOrDivision,
                (items, key) => items.FirstOrDefault(item => item.Key == key).Value,
                key => key,
                checkBoxScale);

            this.SyllabusCategoryFilterItems = this.CreateFilterItems(
                this.SyllabusCategoryFilterItems,
                this.SyllabusCategoryFilterGrid,
                c => c.SyllabusCategory,
                (items, key) => items.FirstOrDefault(item => item.Key == key).Value,
                key => key,
                checkBoxScale);

            this.SyllabusLectureFormFilterItems = this.CreateFilterItems(
                this.SyllabusLectureFormFilterItems,
                this.SyllabusLectureFormFilterGrid,
                c => c.SyllabusLectureForm,
                (items, key) => items.FirstOrDefault(item => item.Key == key).Value,
                key => NullableLectureFormToStringConverter.Convert(key),
                checkBoxScale);

            this.UpdateCourseViewFilter();
        }

        #endregion

        #region メソッド(private)

        /// <summary>
        /// 項目ごとのフィルターのチェックボックスをリセットします。
        /// </summary>
        /// <typeparam name="T"> 項目の型。 </typeparam>
        /// <param name="filterItems"> 現在の項目に対応するアイテムのリスト。 </param>
        private static void ResetFilterItems<T>(Dictionary<T, FilterItem> filterItems)
        {
            foreach (var f in filterItems)
            {
                f.Value.CheckBox.IsChecked = true;
            }
        }

        /// <summary>
        /// 項目に対応する新たなフィルターのアイテムのリストを作成します。
        /// </summary>
        /// <typeparam name="T"> 項目の型。 </typeparam>
        /// <param name="filterItems"> 現在の項目に対応するアイテムのリスト。 </param>
        /// <param name="filterGlid"> アイテムを配置する <see cref="Grid"/> 。 </param>
        /// <param name="courseElementFunction"> <see cref="MainWindow.Courses"/> から要素を射影する関数。 </param>
        /// <param name="filterItemFunction"> アイテムのリストからキーが一致するアイテムの値を抽出する関数。 </param>
        /// <param name="labelFunction"> アイテムから項目に対応するラベル名を抽出する関数。 </param>
        /// <param name="checkBoxScale"> チェックボックスのスケール。 </param>
        /// <returns> 新たなフィルターのアイテムのリスト。 </returns>
        private Dictionary<T, FilterItem> CreateFilterItems<T>(
            Dictionary<T, FilterItem> filterItems,
            Grid filterGlid,
            Func<Course, T> courseElementFunction,
            Func<Dictionary<T, FilterItem>, T, FilterItem> filterItemFunction,
            Func<T, string> labelFunction,
            ScaleTransform checkBoxScale)
        {
            foreach (var s in filterItems)
            {
                filterGlid.Children.Remove(s.Value.Label);
                filterGlid.Children.Remove(s.Value.CheckBox);
            }

            var result = new Dictionary<T, FilterItem>();
            int count = 0;

            foreach (var key in this.MainWindow.Courses.Select(courseElementFunction).Distinct())
            {
                if (key == null)
                {
                    continue;
                }

                count++;

                var presentItem = filterItemFunction(filterItems, key);
                ////var presentItem = filterItems.FirstOrDefault(item => item.Key.Equals(key)).Value;

                if (presentItem != null)
                {
                    Grid.SetRow(presentItem.CheckBox, count);
                    Grid.SetRow(presentItem.Label, count);

                    filterGlid.Children.Add(presentItem.CheckBox);
                    filterGlid.Children.Add(presentItem.Label);
                    result.Add(key, presentItem);
                }
                else
                {
                    var checkbox = new CheckBox();
                    checkbox.LayoutTransform = checkBoxScale;
                    checkbox.IsChecked = true;
                    Grid.SetRow(checkbox, count);
                    Grid.SetColumn(checkbox, 0);

                    var label = new Label();
                    label.Content = labelFunction(key);
                    Grid.SetRow(label, count);
                    Grid.SetColumn(label, 1);

                    filterGlid.Children.Add(checkbox);
                    filterGlid.Children.Add(label);
                    result.Add(key, new FilterItem(checkbox, label));
                }
            }

            filterGlid.RowDefinitions.Clear();

            for (var index = 0; index < result.Count + 1; index++)
            {
                filterGlid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0, GridUnitType.Auto) });
            }

            return result;
        }

        /// <summary>
        /// フィルターを更新します。
        /// </summary>
        private void UpdateCourseViewFilter()
        {
            if (this.MainWindow?.View == null)
            {
                return;
            }

            this.MainWindow.View.Filter =
                (object c) =>
                {
                    var course = c as Course;

                    #region 登録/追加/削除
                    {
                        if (!((course.NotRegisters && !course.IsToAdd && this.CheckBoxNotRegistered.IsChecked == true)
                            || (course.NotRegisters && course.IsToAdd && this.CheckBoxNotRegisteredAndToAdd.IsChecked == true)
                            || (course.Registers && !course.IsToDelete && this.CheckBoxRegistered.IsChecked == true)
                            || (course.Registers && course.IsToDelete && this.CheckBoxRegisteredAndToDelete.IsChecked == true)))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region 曜日/時限
                    {
                        FilterItem filterItem = null;
                        this.DayOfWeekAndPeriodFilterItems.TryGetValue(course.DayOfWeekAndPeriod, out filterItem);

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxOthers.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region クォーター
                    {
                        if (this.CheckBoxNullOfOffering.IsChecked == true)
                        {
                            if (!((course.OffersInQ1 != false && this.CheckBoxQ1.IsChecked == true)
                                    || (course.OffersInQ2 != false && this.CheckBoxQ2.IsChecked == true)
                                    || (course.OffersInQ3 != false && this.CheckBoxQ3.IsChecked == true)
                                    || (course.OffersInQ4 != false && this.CheckBoxQ4.IsChecked == true))
                                && !(course.OffersInQ1 == null && course.OffersInQ2 == null && course.OffersInQ3 == null && course.OffersInQ4 == null)
                                && !(this.CheckBoxQ1.IsChecked == true && this.CheckBoxQ2.IsChecked == true && this.CheckBoxQ3.IsChecked == true && this.CheckBoxQ4.IsChecked == true))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!((course.OffersInQ1 == true && this.CheckBoxQ1.IsChecked == true)
                                    || (course.OffersInQ2 == true && this.CheckBoxQ2.IsChecked == true)
                                    || (course.OffersInQ3 == true && this.CheckBoxQ3.IsChecked == true)
                                    || (course.OffersInQ4 == true && this.CheckBoxQ4.IsChecked == true)))
                            {
                                return false;
                            }
                        }
                    }
                    #endregion

                    #region UnitType
                    {
                        FilterItem filterItem = null;

                        if (course.UnitType != null)
                        {
                            this.UnitTypeFilterItems.TryGetValue(course.UnitType, out filterItem);
                        }

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxNullOfUnitType.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region JpSubjectType
                    {
                        FilterItem filterItem = null;

                        if (course.JPSubjectType != null)
                        {
                            this.JPSubjectTypeFilterItems.TryGetValue(course.JPSubjectType, out filterItem);
                        }

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxNullOfJPSubjectType.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region JpSyllabusSchoolOrDivision
                    {
                        FilterItem filterItem = null;

                        if (course.JPSyllabusSchoolOrDivision != null)
                        {
                            this.JPSyllabusSchoolOrDivisionFilterItems.TryGetValue(course.JPSyllabusSchoolOrDivision, out filterItem);
                        }

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxNullOfJPSyllabusSchoolOrDivision.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region SyllabusCategory
                    {
                        FilterItem filterItem = null;

                        if (course.SyllabusCategory != null)
                        {
                            this.SyllabusCategoryFilterItems.TryGetValue(course.SyllabusCategory, out filterItem);
                        }

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxNullOfSyllabusCategory.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    #region SyllabusLectureForm
                    {
                        FilterItem filterItem = null;

                        if (course.SyllabusLectureForm != null)
                        {
                            this.SyllabusLectureFormFilterItems.TryGetValue(course.SyllabusLectureForm, out filterItem);
                        }

                        var isChecked = filterItem?.CheckBox?.IsChecked;

                        if (isChecked == false || (isChecked == null && this.CheckBoxNullOfSyllabusLectureForm.IsChecked != true))
                        {
                            return false;
                        }
                    }
                    #endregion

                    return true;
                };

            this.MainWindow.UpdateViewCount();
        }

        /////// <summary>
        /////// 項目に対応するフィルターを設定します。
        /////// </summary>
        /////// <typeparam name="T"> 項目の型。 </typeparam>
        /////// <param name="filterItems"> 現在の項目に対応するアイテムのリスト。 </param>
        /////// <param name="nullCheckBox"> 未設定に対応するチェックボックス。 </param>
        /////// <param name="key"> コース </param>
        ////private bool Filter<T>(Dictionary<T, FilterItem> filterItems, CheckBox nullCheckBox, T key)
        ////{
        ////    FilterItem filterItem = null;

        ////    if (key != null)
        ////    {
        ////        filterItems.TryGetValue(key, out filterItem);
        ////    }

        ////    var isChecked = filterItem?.CheckBox?.IsChecked;

        ////    if (isChecked == false || (isChecked == null && nullCheckBox.IsChecked != true))
        ////    {
        ////        return false;
        ////    }

        ////    return true;
        ////}

        /// <summary>
        /// 各ラベルをクリックした時の処理を実行します。
        /// </summary>
        /// <typeparam name="T"> 項目の型。 </typeparam>
        /// <param name="filterItems"> 現在の項目に対応するアイテムのリスト。 </param>
        /// <param name="nullCheckBox"> 未設定に対応するチェックボックス。 </param>
        private void Label_Click<T>(Dictionary<T, FilterItem> filterItems, CheckBox nullCheckBox)
        {
            var value = !(filterItems.All(c => c.Value.CheckBox.IsChecked == true) && nullCheckBox.IsChecked == true);

            this.UpdatesFilter = false;

            nullCheckBox.IsChecked = value;

            foreach (var c in filterItems)
            {
                c.Value.CheckBox.IsChecked = value;
            }

            this.UpdatesFilter = true;

            this.UpdateCourseViewFilter();
        }

        #endregion

        #region メソッド(イベント)

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
        /// 曜日・時限をクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void DayOfWeekAndPeriodButton_Click(object sender, RoutedEventArgs e)
        {
            var content = (sender as Button).Content as string;
            DayOfWeek dayOfWeek;

            try
            {
                dayOfWeek = DayOfWeekToShortTextConverter.ConvertBack(content);
            }
            catch (ArgumentException)
            {
                dayOfWeek = DayOfWeek.None;
            }

            IEnumerable<KeyValuePair<DayOfWeekAndPeriod, FilterItem>> changedFilterItems;

            if (dayOfWeek != DayOfWeek.None)
            {
                changedFilterItems = this.DayOfWeekAndPeriodFilterItems
                    .Where((KeyValuePair<DayOfWeekAndPeriod, FilterItem> c) => c.Key.DayOfWeek == dayOfWeek);
            }
            else
            {
                changedFilterItems = this.DayOfWeekAndPeriodFilterItems
                    .Where((KeyValuePair<DayOfWeekAndPeriod, FilterItem> c) => c.Key.Period == MainWindow.TryParseNullableInt(content));
            }

            var value = !changedFilterItems.All((KeyValuePair<DayOfWeekAndPeriod, FilterItem> c) => c.Value.CheckBox.IsChecked == true);

            this.UpdatesFilter = false;

            foreach (var c in changedFilterItems)
            {
                c.Value.CheckBox.IsChecked = value;
            }

            this.UpdatesFilter = true;

            this.UpdateCourseViewFilter();
        }

        /// <summary>
        /// チェックボックスの変化
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void CheckBox_Changed(object sender, RoutedEventArgs e)
        {
            if (this.UpdatesFilter)
            {
                this.UpdateCourseViewFilter();
            }
        }

        /// <summary>
        /// ・曜日/時限 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void DayOfWeekAndPeriodLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.DayOfWeekAndPeriodFilterItems, this.CheckBoxOthers);
        }

        /// <summary>
        /// ・クォーター ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void OffersInEachQuarterLabel_Click(object sender, RoutedEventArgs e)
        {
            var value = !(this.CheckBoxQ1.IsChecked == true
                && this.CheckBoxQ2.IsChecked == true 
                && this.CheckBoxQ3.IsChecked == true 
                && this.CheckBoxQ4.IsChecked == true
                && this.CheckBoxNullOfOffering.IsChecked == true);

            this.UpdatesFilter = false;

            this.CheckBoxQ1.IsChecked = value;
            this.CheckBoxQ2.IsChecked = value;
            this.CheckBoxQ3.IsChecked = value;
            this.CheckBoxQ4.IsChecked = value;
            this.CheckBoxNullOfOffering.IsChecked = value;

            this.UpdatesFilter = true;

            this.UpdateCourseViewFilter();
        }

        /// <summary>
        /// ・開講単位 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void UnitTypeLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.UnitTypeFilterItems, this.CheckBoxNullOfUnitType);
        }

        /// <summary>
        /// ・種類 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void JPSubjectTypeLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.JPSubjectTypeFilterItems, this.CheckBoxNullOfJPSubjectType);
        }

        /// <summary>
        /// ・詳細分類 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void JPSyllabusSchoolOrDivisionLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.JPSyllabusSchoolOrDivisionFilterItems, this.CheckBoxNullOfJPSyllabusSchoolOrDivision);
        }

        /// <summary>
        /// ・科目区分 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void SyllabusCategoryLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.SyllabusCategoryFilterItems, this.CheckBoxNullOfSyllabusCategory);
        }

        /// <summary>
        /// ・講義形態 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void SyllabusLectureFormLabel_Click(object sender, RoutedEventArgs e)
        {
            this.Label_Click(this.SyllabusLectureFormFilterItems, this.CheckBoxNullOfSyllabusLectureForm);
        }

        /// <summary>
        /// ・登録/追加/削除 ラベルをクリック
        /// </summary>
        /// <param name="sender"> イベントを発生させたオブジェクト。 </param>
        /// <param name="e"> イベントオブジェクト。 </param>
        private void RegisterAndAddOrDeleteLabel_Click(object sender, RoutedEventArgs e)
        {
            var value = !(this.CheckBoxNotRegistered.IsChecked == true
                && this.CheckBoxNotRegisteredAndToAdd.IsChecked == true
                && this.CheckBoxRegistered.IsChecked == true
                && this.CheckBoxRegisteredAndToDelete.IsChecked == true);

            this.UpdatesFilter = false;

            this.CheckBoxNotRegistered.IsChecked = value;
            this.CheckBoxNotRegisteredAndToAdd.IsChecked = value;
            this.CheckBoxRegistered.IsChecked = value;
            this.CheckBoxRegisteredAndToDelete.IsChecked = value;

            this.UpdatesFilter = true;

            this.UpdateCourseViewFilter();
        }

        #endregion
    }
}
