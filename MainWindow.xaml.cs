using System.Windows;
using System.Windows.Input;

namespace CodeGenerator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new ViewModel();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                switch (e.Key)
                {
                    case Key.S:
                        tbcSimple.IsSelected = true;
                        simpleControl.FocusOnMainControl();
                        break;

                    case Key.R:
                        tbcReplace.IsSelected = true;
                        replaceControl.FocusOnMainControl();
                        break;

                    case Key.P:
                        tbcProperty.IsSelected = true;
                        propertyControl.FocusOnMainControl();
                        break;

                    case Key.D:
                        tbcDependencyProperty.IsSelected = true;
                        dependencyPropertyControl.FocusOnMainControl();
                        break;

                    case Key.T:
                        tbcSingleton.IsSelected = true;
                        singletonControl.FocusOnMainControl();
                        break;

                    case Key.B:
                        tbcBaseClass.IsSelected = true;
                        baseClassControl.FocusOnMainControl();
                        break;
                }
            }

            base.OnPreviewKeyDown(e);
        }
    }
}
