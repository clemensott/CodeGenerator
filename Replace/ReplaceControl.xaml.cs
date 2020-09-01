using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CodeGenerator.Replace
{
    /// <summary>
    /// Interaktionslogik für ReplaceControl.xaml
    /// </summary>
    public partial class ReplaceControl : UserControl
    {
        public ReplaceControl()
        {
            InitializeComponent();
        }

        private void TbxReplaceText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.F || (!Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.RightCtrl))) return;
            
            if (tbxReplaceText.SelectedText.Length > 0)
            {
                tbxParseText.Text = "2#" + tbxReplaceText.SelectedText + "#";
                tbxParseText.SelectionStart = tbxParseText.Text.Length;
            }

            tbxParseText.Focus();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as ReplaceService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(DataContext as ReplaceService);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }

        public void FocusOnMainControl()
        {
            tbxParseText.Focus();
        }
    }
}
