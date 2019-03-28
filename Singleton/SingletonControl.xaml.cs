using System.Windows;
using System.Windows.Controls;

namespace CodeGenerator.Singleton
{
    /// <summary>
    /// Interaktionslogik für SingletonControl.xaml
    /// </summary>
    public partial class SingletonControl : UserControl
    {
        public SingletonService ViewModel { get { return (SingletonService) DataContext; } }

        public SingletonControl()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(ViewModel);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(ViewModel);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }

        public void FocusOnMainControl()
        {
            tbxParse.Focus();
        }
    }
}
