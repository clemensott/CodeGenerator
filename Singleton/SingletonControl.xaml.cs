using System.Windows;
using System.Windows.Controls;

namespace CodeGenerator.Singleton
{
    /// <summary>
    /// Interaktionslogik für SingletonControl.xaml
    /// </summary>
    public partial class SingletonControl : UserControl
    {
        public SingletonService ViewModel { get { return DataContext as SingletonService; } }

        public SingletonControl()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as SingletonService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(DataContext as SingletonService);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }
    }
}
