﻿#pragma checksum "..\..\..\Window\MainWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "F2E0B2D41C89016994D140F5D6D7D175"
//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

using Kntacooh.AutoCourseRegistration;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace Kntacooh.AutoCourseRegistration {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 48 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView CourseListView;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.GridView CoursesGridView;
        
        #line default
        #line hidden
        
        
        #line 345 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label LogLabel;
        
        #line default
        #line hidden
        
        
        #line 349 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBar StatusBar;
        
        #line default
        #line hidden
        
        
        #line 353 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBarItem StatusLoginUser;
        
        #line default
        #line hidden
        
        
        #line 354 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBarItem StatusMessage;
        
        #line default
        #line hidden
        
        
        #line 360 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBarItem StatusIsRun;
        
        #line default
        #line hidden
        
        
        #line 361 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBarItem StatusInterval;
        
        #line default
        #line hidden
        
        
        #line 362 "..\..\..\Window\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.StatusBarItem StatusNext;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AutoCourseRegistration;component/window/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Window\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\Window\MainWindow.xaml"
            ((Kntacooh.AutoCourseRegistration.MainWindow)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.Window_Unloaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\Window\MainWindow.xaml"
            ((Kntacooh.AutoCourseRegistration.MainWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 8 "..\..\..\Window\MainWindow.xaml"
            ((Kntacooh.AutoCourseRegistration.MainWindow)(target)).Activated += new System.EventHandler(this.Window_Activated);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 32 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ReadSchedule);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 33 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ReadAllCourses);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 34 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ReadFromSyllabus);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 35 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.RunAuto);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 37 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.Quit);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 40 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ResetSorting);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 41 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteCourses);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 43 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenConfig);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 44 "..\..\..\Window\MainWindow.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenDetail);
            
            #line default
            #line hidden
            return;
            case 11:
            this.CourseListView = ((System.Windows.Controls.ListView)(target));
            
            #line 50 "..\..\..\Window\MainWindow.xaml"
            this.CourseListView.AddHandler(System.Windows.Controls.Primitives.ButtonBase.ClickEvent, new System.Windows.RoutedEventHandler(this.CourseListViewColumnHeaderClickedHandler));
            
            #line default
            #line hidden
            return;
            case 13:
            this.CoursesGridView = ((System.Windows.Controls.GridView)(target));
            return;
            case 14:
            this.LogLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 15:
            this.StatusBar = ((System.Windows.Controls.Primitives.StatusBar)(target));
            return;
            case 16:
            this.StatusLoginUser = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 17:
            this.StatusMessage = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 18:
            this.StatusIsRun = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 19:
            this.StatusInterval = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            case 20:
            this.StatusNext = ((System.Windows.Controls.Primitives.StatusBarItem)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 12:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.ListBoxItem.SelectedEvent;
            
            #line 57 "..\..\..\Window\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.SelectedCourse);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.ListBoxItem.UnselectedEvent;
            
            #line 58 "..\..\..\Window\MainWindow.xaml"
            eventSetter.Handler = new System.Windows.RoutedEventHandler(this.UnselectedCourse);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

