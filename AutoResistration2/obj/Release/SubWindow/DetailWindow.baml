   M S B A M L   `   `   `  ���� Q  MAutoCourseRegistration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:Kntacooh.AutoCourseRegistrationKntacooh.AutoCourseRegistration  2    ,Kntacooh.AutoCourseRegistration.DetailWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ?local-clr-namespace:Kntacooh.AutoCourseRegistration     -     � Title$  DetailWindow��$	��600��$	��720�� � 	Resources C   =Kntacooh.AutoCourseRegistration.DayOfWeekToShortTextConverter   DayOfWeekConverterD   >Kntacooh.AutoCourseRegistration.NullableBooleanToMarkConverter " NullableBooleanToMarkConverterJ   DKntacooh.AutoCourseRegistration.NullableLectureFormToStringConverter   NullableLectureFormConverterG   AKntacooh.AutoCourseRegistration.ReadOnlyStringCollectionConverter % !ReadOnlyStringCollectionConverterL   FKntacooh.AutoCourseRegistration.ReadOnlyGradingRateCollectionConverter  GradingRateConverter��%K   &	        &	      &	 
     &	      &	                .���  � RowDefinitions �� $��* ��� $
��Auto ��� $��* ��� $
��Auto ��� $��* �.+��� $��0��$
��Auto=�$
��Auto=�.���  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � � ColumnDefinitions �� $
��Auto ��� $��* �.+��� $��0��$��0��$��
分類(JP)���� $��0��$��1������  �� Path$  JPSyllabusSchoolOrDivision��� $��1��$��0��$��
分類(EN)���� $��1��$��1������ $  ENSyllabusSchoolOrDivision��� $��2��$��0��$��授業科目名(JP)���� $��2��$��1������ $ JPSyllabusTitle��� $��3��$��0��$��授業科目名(EN)���� $��3��$��1������ $ ENSyllabusTitle��� $��4��$��0��$��CityCollege���� $��4��$��1������ $ IsCityCollege� �� 	Converter# [ �� $��5��$��0��$��担当教員名(JP)���� $��5��$��1������ $ JPSyllabusInstructors�# [ �� $��6��$��0��$��担当教員名(EN)���� $��6��$��1������ $ ENSyllabusInstructors�# [ �� $��7��$��0��$��科目ナンバー���� $��7��$��1������ $ SyllabusNumberingCode��� $��8��$��0��$��時間割番号���� $��8��$��1������ $ Id��� $��9��$��0��$��科目区分���� $��9��$��1������ $ SyllabusCategory��� $��10��$��0��$��講義形態���� $��10��$��1������ $ SyllabusLectureForm�# [ �� $��11��$��0��$��対象学生���� $��11��$��1������ $ SyllabusAssignedYear��� $��12��$��0��$��適正人数���� $��12��$��1������ $ MaxSize��� $��13��$��0��$��Q1���� $��13��$��1������ $ 
OffersInQ1�# [ �� $��14��$��0��$��Q2���� $��14��$��1������ $ 
OffersInQ2�# [ �� $��15��$��0��$��Q3���� $��15��$��1������ $ 
OffersInQ3�# [ �� $��16��$��0��$��Q4���� $��16��$��1������ $ 
OffersInQ4�# [ �� $��17��$��0��$��キーワード(JP)���� $��17��$��1������ $ JPSyllabusKeywords�# [ �� $��18��$��0��$��キーワード(EN)���� $��18��$��1������ $ ENSyllabusKeywords�# [ �� $��19��$��0��$��評価の割合���� $��19��$��1������ $  SyllabusGradingRateDetails�# [ �� $��1��$��6��$��Stretch=��� $��2��$
��Auto=�$
��Auto=�.���  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��登録番号���� $��0��$��1������ $ RegisterNumber��� $��1��$��0��$��時間割番号���� $��1��$��1������ $ Id��� $��2��$��0��$��
種類(JP)���� $��2��$��1������ $ JPSubjectType��� $��3��$��0��$��
種類(EN)���� $��3��$��1������ $ ENSubjectType��� $��4��$��0��$��GS群���� $��4��$��1������ $ KugsOfGS��� $��5��$��0��$
��GS_2���� $��5��$��1������ $ SignOfGS��� $��6��$��0��$
��GS_3���� $��6��$��1������ $ SignDetailOfGS��� $��7��$��0��$��時間割名���� $��7��$��1������ $ Title��� $��8��$��0��$��担当教員���� $��8��$��1������ $ 
Instructor��� $��9��$��0��$��曜日���� $��9��$��1������ $ 	DayOfWeek�# [  �� $��10��$��0��$��時限���� $��10��$��1������ $ Period��� $��11��$��0��$��適正人数���� $��11��$��1������ $ MaxSize��� $��12��$��0��$��登録者数���� $��12��$��1������ $ RegisteredSize��� $��13��$��0��$��残枠���� $��13��$��1������ $ RestSize��� $��14��$��0��$��抽選対象���� $��14��$��1������ $ IsDoneLottery��� $��15��$��0��$��	単位数���� $��15��$��1������ $ Credit��� $��16��$��0��$��開講単位���� $��16��$��1������ $ UnitType��� $��17��$��0��$��Q換算単位���� $��17��$��1������ $ CreditPerQuarter��� $��3��$��6��$��Stretch=��� $��4��$
��Auto=�$
��Auto=�.���  �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $
��Auto ��� $��* �.+��� $��0��$��0��$��授業科目名���� $��0��$��1������ $ SyllabusTitle��� $��1��$��0��$��担当教員名���� $��1��$��1������ $ SyllabusInstructor��� $��2��$��0��$��キーワード���� $��2��$��1������ $ SyllabusKeyword��� $��3��$��0��$��授業の主題���� $��3��$��1������ $ SyllabusTopic��� $��4��$��0��$��授業の目標���� $��4��$��1������ $ SyllabusObjective��� $��5��$��0��$��学生の学習目標���� $��5��$��1������ $ SyllabusLearningOutcomes��� $��6��$��0��$��授業の概要���� $��6��$��1������ $ SyllabusOutline��� $��7��$��0��$��講義スケジュール���� $��7��$��1������ $ SyllabusSchedule��� $��8��$��0��$��評価の方法���� $��8��$��1������ $ SyllabusGradingMethod��� $��9��$��0��$��評価の割合���� $��9��$��1������ $ SyllabusGradingRate��� $��10��$��0��$#��テキスト/教材/参考書���� $��10��$��1������ $ SyllabusTeachingMaterials��� $��11��$��0��$��その他注意事項���� $��11��$��1������ $ SyllabusOthers��� $��12��$��0��$��オフィスアワー���� $��12��$��1������ $ SyllabusConsultationTime��� $��13��$��0��$��履修条件���� $��13��$��1������ $# SyllabusSyllabusPrerequisites��� $��14��$��0��$��受講者数調整���� $��14��$��1������ $) #SyllabusMethodForAdjustingClassSize��� $��15��$��0��$��開放科目���� $��15��$��1������ $ SyllabusSubjectOfOpen��� $��16��$��0��$��関連科目���� $��16��$��1������ $ SyllabusRelatedCourses��� $��17��$��0��$��位置づけ���� $��17��$��1������ $9 3SyllabusRelationsWithTheOtherCoursesInTheCurriculum��� $��18��$��0��$��特記事項���� $��18��$��1������ $ SyllabusSpecialNote�