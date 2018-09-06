using System.Windows;
using System.Windows.Controls;

namespace CodeGenerator.Property
{
    /// <summary>
    /// Interaktionslogik für PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        public CodePropertiesService ViewModel { get { return DataContext as CodePropertiesService; } }

        public PropertyControl()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as CodePropertiesService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(DataContext as CodePropertiesService);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }
    }
}
