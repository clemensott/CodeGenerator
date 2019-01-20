using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CodeGenerator.BaseClass
{
    /// <summary>
    /// Interaktionslogik für BaseClassControl.xaml
    /// </summary>
    public partial class BaseClassControl : UserControl
    {
        public BaseClassControl()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyWholeCode(DataContext as ICodeObjectsService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyNextCodePart(DataContext as ICodeObjectsService);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }

        private void BtnPropChangeImpl_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.CopyAndShow(CodeBaseClassService.RaisePropertyChangedCode);
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            BaseClassElement element;

            if (e.PropertyName == nameof(element.Parameters))
            {
                DataGridTextColumn column = (DataGridTextColumn)e.Column;
                Binding binding = (Binding)column.Binding;
                binding.Converter = new ParameterConverter();
            }
        }
    }
}
