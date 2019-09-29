using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeGenerator.Snippets
{
    /// <summary>
    /// Interaktionslogik für SnippetsControl.xaml
    /// </summary>
    public partial class SnippetsControl : UserControl
    {
        public SnippetsControl()
        {
            InitializeComponent();
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            SnippetService service = (SnippetService)((FrameworkElement)sender).DataContext;

            CodeCopyService.Current.CopyWholeCode(service);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SnippetService service = (SnippetService)((FrameworkElement)sender).DataContext;

            CodeCopyService.Current.CopyNextCodePart(service);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            SnippetService service = (SnippetService)((FrameworkElement)sender).DataContext;

            CodeCopyService.Current.Stop();
        }
    }
}
