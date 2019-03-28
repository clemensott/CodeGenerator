using System.Windows;
using System.Windows.Controls;

namespace CodeGenerator.Property
{
    /// <summary>
    /// Interaktionslogik für PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        public CodePropertiesService ViewModel { get { return (CodePropertiesService)DataContext; } }

        public PropertyControl()
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

        private void BtnInterfaceImpl_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyAndShow(CodePropertiesService.NotifyPropertyChangeText);
        }

        public void FocusOnMainControl()
        {
            tbxParse.Focus();
        }
    }
}
