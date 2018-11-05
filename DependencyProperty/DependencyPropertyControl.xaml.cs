using System.Linq;
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
            SetSameControlType();

            CodeCopyService.Current.CopyWholeCode(DataContext as CodeDependencyPropertiesService);
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            SetSameControlType();

            CodeCopyService.Current.CopyNextCodePart(DataContext as CodeDependencyPropertiesService);
        }

        private void SetSameControlType()
        {
            CodeDependencyPropertiesService service = DataContext as CodeDependencyPropertiesService;
            string controlType = service.CodeObjects.Select(o => o.ControlType).FirstOrDefault(t => !string.IsNullOrWhiteSpace(t));

            foreach (DependencyProperty property in service.CodeObjects) property.ControlType = controlType;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            CodeCopyService.Current.Stop();
        }
    }
}
