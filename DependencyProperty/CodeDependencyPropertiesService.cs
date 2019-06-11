using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator.DependencyProperty
{
    public class CodeDependencyPropertiesService : MultipleCodeObjectsService<DependencyProperty>
    {
        private bool isWpf;
        private string controlType;

        public bool ForWpf
        {
            get { return isWpf; }
            set
            {
                if (value == isWpf) return;

                isWpf = value;
                OnPropertyChanged(nameof(ForWpf));
            }
        }

        public string ControlType
        {
            get { return controlType; }
            set
            {
                if (value == controlType) return;

                controlType = value;
                OnPropertyChanged(nameof(ControlType));
            }
        }

        protected override bool TryParse(string dataText, out DependencyProperty dependencyProperty)
        {
            //dataText = dataText.Replace("  ", " ");
            dependencyProperty = null;

            try
            {
                string[] data = dataText.Split(' ');

                if (data.Length < 2 || data.Take(2).Any(item => item.Length == 0)) return false;

                string propertyType = data[0];
                string name = data[1];
                bool withBody = false;
                bool ignoreChanges = false;
                string defaultValue = string.Join(" ", data.Skip(4));

                if (data.Length > 2 && !TryConvertToBoolean(data[2], ref withBody)) return false;
                if (data.Length > 3 && !TryConvertToBoolean(data[3], ref ignoreChanges)) return false;

                dependencyProperty = new DependencyProperty()
                {
                    WithBody = withBody,
                    IgnoreChanges = ignoreChanges,
                    PropertyType = propertyType,
                    Name = name,
                    DefaultValue = defaultValue
                };

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool TryConvertToBoolean(string s, ref bool value)
        {
            switch (s.ToLower())
            {
                case "":
                    break;

                case "0":
                case "false":
                    value = false;
                    break;

                case "1":
                case "true":
                    value = true;
                    break;

                default:
                    return false;
            }

            return true;
        }

        protected override IEnumerable<Func<DependencyProperty, string>> GetObjectsCodeGenerators()
        {
            yield return new Func<DependencyProperty, string>(GetCodePart1);
            yield return new Func<DependencyProperty, string>(GetCodePart2);
        }

        private string GetCodePart1(DependencyProperty dp)
        {
            string pcc = dp.WithBody ? "new PropertyChangedCallback(On{2}PropertyChanged)" : "On{2}PropertyChanged";
            string defaultValue = dp.DefaultValue;
            bool hasDefaultValue = !string.IsNullOrWhiteSpace(defaultValue);
            string f = "\r\n";

            if (ForWpf)
            {
                if (dp.IgnoreChanges && hasDefaultValue)
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),\r\n";
                    f += "\t\t\t\tnew PropertyMetadata({5}));\r\n";
                }
                else if (!dp.IgnoreChanges && hasDefaultValue)
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),\r\n";
                    f += "\t\t\t\tnew PropertyMetadata({5}, {6}));\r\n";
                }
                else if(dp.IgnoreChanges && !hasDefaultValue)
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}));\r\n";
                }
                else
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),\r\n";
                    f += "\t\t\t\tnew PropertyMetadata({6}));\r\n";
                }

            }
            else
            {
                if (dp.IgnoreChanges)
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),\r\n";
                    f += "\t\t\t\tnew PropertyMetadata({5}));\r\n";
                }
                else
                {
                    f += "\t\tpublic static readonly DependencyProperty {2}Property =\r\n";
                    f += "\t\t\tDependencyProperty.Register(\"{2}\", typeof({3}), typeof({4}),\r\n";
                    f += "\t\t\t\tnew PropertyMetadata({5}, {6}));\r\n";
                }

                if (hasDefaultValue) defaultValue = "default(" + dp.PropertyType + ")";
            }

            f += "\r\n";

            if (!dp.IgnoreChanges)
            {
                f += "\t\tprivate static void On{2}PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)\r\n";
                f += "\t\t{0}\r\n";
                f += "\t\t\t{4} s = ({4})sender;\r\n";
                f += "\t\t\t{3} value = ({3})e.NewValue;\r\n";
                f += "\t\t{1}\r\n";
                f += "\r\n";
            }

            pcc = string.Format(pcc, "{", "}", dp.Name);
            return string.Format(f, "{", "}", dp.Name, dp.PropertyType, ControlType, defaultValue, pcc);
        }

        private string GetCodePart2(DependencyProperty dp)
        {
            string f = string.Empty;

            f += "\t\tpublic {3} {2}\r\n";
            f += "\t\t{0}\r\n";

            if (dp.WithBody)
            {
                f += "\t\t\tget {0} return ({3})GetValue({2}Property); {1}\r\n";
                f += "\t\t\tset {0} SetValue({2}Property, value); {1}\r\n";
            }
            else
            {
                f += "\t\t\tget => ({3})GetValue({2}Property);\r\n";
                f += "\t\t\tset => SetValue({2}Property, value);\r\n";
            }

            f += "\t\t{1}\r\n";
            f += "\r\n";

            return string.Format(f, "{", "}", dp.Name, dp.PropertyType, ControlType, dp.DefaultValue);
        }
    }
}
