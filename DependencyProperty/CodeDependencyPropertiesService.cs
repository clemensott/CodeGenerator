using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.DependencyProperty
{
    public class CodeDependencyPropertiesService : MultipleCodeObjectsService<DependencyProperty>
    {
        protected override bool TryParse(string dataText, out DependencyProperty dependencyProperty)
        {
            dataText = dataText.Replace("  ", " ");
            dependencyProperty = null;

            try
            {
                string[] data = dataText.Split(' ');

                if (data.Length < 4) return false;
                foreach (string item in data) if (item.Length == 0) return false;

                string name = data[0], propertyType = data[1], controlType = data[2], defaultValue = string.Join(" ", data.Skip(3));
                dependencyProperty = new DependencyProperty()
                {
                    Name = name,
                    PropertyType = propertyType,
                    ControlType = controlType,
                    DefaultValue = defaultValue
                };

                return true;
            }
            catch { }

            return false;
        }

        protected override IEnumerable<Func<DependencyProperty, string>> GetObjectsCodeGenerators()
        {
            yield return new Func<DependencyProperty, string>(GetCodePart1);
            yield return new Func<DependencyProperty, string>(GetCodePart2);
        }

        private string GetCodePart1(DependencyProperty dp)
        {
            string format = string.Empty;

            format += "\r\n        public static readonly DependencyProperty {2}Property =";
            format += "\r\n            DependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),";
            format += "\r\n                new PropertyMetadata({5}, new PropertyChangedCallback(On{2}PropertyChanged)));";
            format += "\r\n";
            format += "\r\n        private static void On{2}PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)";
            format += "\r\n{0}";
            format += "\r\n            var s = ({4})sender;";
            format += "\r\n            var value = ({3})e.NewValue;";
            format += "\r\n{1}";
            format += "\r\n";

            return string.Format(format, "{", "}", dp.Name, dp.PropertyType, dp.ControlType, dp.DefaultValue);
        }

        private string GetCodePart2(DependencyProperty dp)
        {
            string format = string.Empty;

            format += "\r\n        public {3} {2}";
            format += "\r\n        {0}";
            format += "\r\n            get {0} return ({3})GetValue({2}Property); {1}";
            format += "\r\n            set {0} SetValue({2}Property, value); {1}";
            format += "\r\n        {1}";
            format += "\r\n";

            return string.Format(format, "{", "}", dp.Name, dp.PropertyType, dp.ControlType, dp.DefaultValue);
        }
    }
}
