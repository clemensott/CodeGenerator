using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeGenerator.Simple
{
    public partial class SimpleControl : UserControl
    {
        public SimpleControl()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) CodeCopyService.Current.CopyWholeCode(DataContext as SimpleService);
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as SimpleService);
        }

        public void FocusOnMainControl()
        {
            tbxParse.Focus();
        }
    }
}
