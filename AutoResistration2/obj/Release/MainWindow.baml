   M S B A M L   `   `   `  ���� Q  MAutoCourseRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Kntacooh.AutoCourseRegistrationKntacooh.AutoCourseRegistration  0    *Kntacooh.AutoCourseRegistration.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?local-clr-namespace:Kntacooh.AutoCourseRegistration     -     � Title$0  *履修登録管理ツールかっこかり��$	��600��$
��1200�� � 	Resources 8   2Kntacooh.AutoCourseRegistration.ViewIndexConverter   ViewIndexConverterC   =Kntacooh.AutoCourseRegistration.DayOfWeekToShortTextConverter  DayOfWeekConverterD   >Kntacooh.AutoCourseRegistration.NullableBooleanToMarkConverter " NullableBooleanToMarkConverterJ   DKntacooh.AutoCourseRegistration.NullableLectureFormToStringConverter   NullableLectureFormConverterG   AKntacooh.AutoCourseRegistration.ReadOnlyStringCollectionConverter % !ReadOnlyStringCollectionConverterL   FKntacooh.AutoCourseRegistration.ReadOnlyGradingRateCollectionConverter  GradingRateConverter��%Z   &	        &	      &	 
     &	      &	      &	                  .���  � RowDefinitions �� $
��Auto ��� $��3* ��� $
��Auto ��� $��* ��� $
��Auto �.+�o� $��0��#��� ��.@�m� $��メニュー(_M)��.@�m� -   $+��%履修希望科目を読み込む(_H)��m� -   $+��%時間割検索から読み込む(_C)���� m� -   $"��科目データを保存(_O)��m� -   $"��科目データを開く(_I)���� m� -    m�Name$ IsRunningAuto��$��自動実行(_R)�� m� IsCheckable . �� m� -   $��ソフト終了(_X)��m� $��ウィンドウ(_W)��.@�m� -   $(��"設定ウィンドウを開く(_O)��m� -	   $(��"詳細ウィンドウを開く(_D)��m� -
   $1��+フィルターウィンドウを開く(_F)��m� $��
編集(_E)��.@�m� -   $"��フィルターを更新(_U)���� m� -   $"��ソートをリセット(_S)��m� -   $(��"フィルターをリセット(_F)���� m� -   $%��科目データを全削除(_C)���� -    ��Name$ CourseListView��$��1������  �� Path$ Courses�����  �� 
TargetType" ��.���  �� Property � ��	 �� Value$	 Stretch=�-   
 �� View
 �� -   CoursesGridView��.P��� $��No�� �� CellTemplate �� .R��� $��Right=�$��Center=�����  �� RelativeSource �� *FindAncestor+ �� AncestorType" �� �� 	Converter# [  �� $��登録�� �� .R��� $��Center=�$��Center=����� $ RegisterNumber��� $��時間割番号�� �� DisplayMemberBinding �� $ Id��� $��種類
(JP)�� �� .R��� $��Center=�$��Center=����� $ JPSubjectType��� $��群
(GS)�� �� .R��� $��Center=�$��Center=����� $ KugsOfGS��� $��2nd
(GS)�� �� .R��� $��Center=�$��Center=����� $ SignOfGS��� $��3rd
(GS)�� �� .R��� $��Center=�$��Center=����� $ SignDetailOfGS��� $��時間割名�� �� $ Title��� $��担当教員�� �� $ 
Instructor��� $��曜日�� �� .R��� $��Center=�$��Center=����� $ 	DayOfWeek�# [ �� $��時限�� �� .R��� $��Center=�$��Center=����� $ Period��� $��適正
人数�� �� .R��� $��Right=�$��Center=����� $ MaxSize��� $��登録
者数�� �� .R��� $��Right=�$��Center=����� $ RegisteredSize��� $��残枠�� �� .R��� $��Right=�$��Center=����� $ RestSize��� $��抽選
対象�� �� .R��� $��Center=�$��Center=����� $ IsDoneLottery�# [ �� $��追加�� �� .R��� $��Center=�$��Center=� d� 	IsChecked �� $ IsToAdd�{��� $ NotRegisters��� $��	単位数�� �� .R��� $��Center=�$��Center=����� $ Credit��� $��開講
単位�� �� .R��� $��Center=�$��Center=����� $ UnitType��� $��単位数
(Quarter)�� �� .R��� $��Center=�$��Center=����� $ CreditPerQuarter��� $��Q1�� �� .R��� $��Center=�$��Center=����� $ 
OffersInQ1�# [ �� $��Q2�� �� .R��� $��Center=�$��Center=����� $ 
OffersInQ2�# [ �� $��Q3�� �� .R��� $��Center=�$��Center=����� $ 
OffersInQ3�# [ �� $��Q4�� �� .R��� $��Center=�$��Center=����� $ 
OffersInQ4�# [ �� $��分類
(JP)�� �� .R��� $��Center=�$��Center=����� $  JPSyllabusSchoolOrDivision��� $��授業科目名
(JP)�� �� $ JPSyllabusTitle��� $ ��CityCollege
(シラバス)�� �� .R��� $��Center=�$��Center=����� $ IsCityCollege�# [ �� $'��!科目ナンバー
(シラバス)�� �� $ SyllabusNumberingCode��� $!��科目区分
(シラバス)�� �� .R��� $��Center=�$��Center=����� $ SyllabusCategory��� $!��講義形態
(シラバス)�� �� .R��� $��Center=�$��Center=����� $ SyllabusLectureForm�# [ �� $!��対象学生
(シラバス)�� �� $ SyllabusAssignedYear��� $$��評価の割合
(シラバス)�� �� $  SyllabusGradingRateDetails�# [ �� $��割合
出席�� �� .R��� $��Right=�$��Center=����� $% SyllabusGradingRateOfAttendance��� $��割合
レポート�� �� .R��� $��Right=�$��Center=����� $! SyllabusGradingRateOfReport��� $��割合
課題�� �� .R��� $��Right=�$��Center=����� $# SyllabusGradingRateOfHomework��� $��割合
小テスト�� �� .R��� $��Right=�$��Center=����� $# SyllabusGradingRateOfMiniExam��� $��割合
中間�� �� .R��� $��Right=�$��Center=����� $&  SyllabusGradingRateOfMidtermExam��� $��割合
期末�� �� .R��� $��Right=�$��Center=����� $$ SyllabusGradingRateOfFinalExam��� $��割合
態度�� �� .R��� $��Right=�$��Center=����� $# SyllabusGradingRateOfAttitude��� $��割合
発表�� �� .R��� $��Right=�$��Center=����� $' !SyllabusGradingRateOfPresentation��� $��割合
演習�� �� .R��� $��Right=�$��Center=����� $# SyllabusGradingRateOfExercise��� $��割合
実験�� �� .R��� $��Right=�$��Center=����� $% SyllabusGradingRateOfExperiment��� $��割合
論文�� �� .R��� $��Right=�$��Center=����� $! SyllabusGradingRateOfThesis��� $��
割合
他�� �� .R��� $��Right=�$��Center=����� $! SyllabusGradingRateOfOthers��� $��割合
総計�� �� .R��� $��Right=�$��Center=����� $ SyllabusSumOfGradingRate��� $��2��$��6��$��Stretch=��� $��3��$��Visible=�.���� -    ��Name$ LogLabel������ $ LogText� �� Mode$ OneWay=��� -    ��Name$ 	StatusBar��$��4��.@��� $
��Left=�.����  �� Orientation$ 
Horizontal=�.+��� -    ��Name$ StatusLoginUser��$��ログイン状態表示���� -   $ StatusMessage���� $��Right=�.���� $ 
Horizontal=�.+��� -   $ StatusViewCount��$��	0 / 0 件���� -   $ StatusIsRun��$��自動監視状態���� -   $ StatusInterval��$��実行間隔: xx秒���� -   $ 
StatusNext��$.��(次の実行時刻は 00:00:00 です。��