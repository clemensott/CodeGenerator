using System.Windows;
using System.Windows.Controls;

namespace CodeGenerator.DependencyProperty
{
    /// <summary>
    /// Interaktionslogik für DependencyPropertyControl.xaml
    /// </summary>
    public partial class DependencyPropertyControl : UserControl
    {
        public DependencyPropertyControl()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as CodeDependencyPropertiesService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(DataContext as CodeDependencyPropertiesService);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }
    }
}
