using System;
using System.Collections.Generic;
using System.Linq;
using CodeGenerator.Base;

namespace CodeGenerator.DependencyProperty
{
    public class CodeDependencyPropertiesService : MultipleCodeObjectsService<DependencyProperty>
    {
        private bool isWpf;
        private string controlType;

        public bool ForWpf
        {
            get => isWpf;
            set
            {
                if (value == isWpf) return;

                isWpf = value;
                OnPropertyChanged(nameof(ForWpf));
            }
        }

        public string ControlType
        {
            get => controlType;
            set
            {
                if (value == controlType) return;

                controlType = value;
                OnPropertyChanged(nameof(ControlType));
            }
        }

        protected override bool TryParse(string dataText, out DependencyProperty dependencyProperty)
        {
            dependencyProperty = null;

            try
            {
                string[] data = dataText.Split(' ');

                if (data.Length < 2 || data.Take(2).Any(item => item.Length == 0)) return false;

                string propertyType = data[0];
                string name = data[1];
                bool isReadonly = false, withBody = false, withPropertyChanged = true,
                    withNewValue = true, withOldValue = true, withValidation = false;
                string defaultValue = string.Join(" ", data.Skip(8));

                if (data.Length > 2 && !TryConvertToBoolean(data[2], ref isReadonly)) return false;
                if (data.Length > 3 && !TryConvertToBoolean(data[3], ref withBody)) return false;
                if (data.Length > 4 && !TryConvertToBoolean(data[4], ref withPropertyChanged)) return false;
                if (data.Length > 5 && !TryConvertToBoolean(data[5], ref withNewValue)) return false;
                if (data.Length > 6 && !TryConvertToBoolean(data[6], ref withOldValue)) return false;
                if (data.Length > 7 && !TryConvertToBoolean(data[7], ref withValidation)) return false;

                dependencyProperty = new DependencyProperty()
                {
                    WithBody = withBody,
                    WithPropertyChanged = withPropertyChanged,
                    IsReadonly = isReadonly,
                    WithValidation = withValidation,
                    WithNewValue = withNewValue,
                    WithOldValue = withOldValue,
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

        protected override IEnumerable<Func<DependencyProperty, string>> GetObjectsCodeGenerators(bool getWhole)
        {
            yield return GetCodePart1;
            yield return GetCodePart2;
        }

        private string GetCodePart1(DependencyProperty dp)
        {
            return string.Join("\r\n", GetCodePart1Lines(dp));
        }

        private IEnumerable<string> GetCodePart1Lines(DependencyProperty dp)
        {
            bool isReadonly = dp.IsReadonly && ForWpf;
            string registerMethod = isReadonly ? "RegisterReadOnly" : "Register";
            string valueValidation = dp.WithBody ?
                $"new ValidateValueCallback(On{dp.Name}PropertyValidation)" : $"On{dp.Name}PropertyValidation";
            string propertyMetadata = GetPropertyMetadataDefinition(dp);
            bool withValidation = dp.WithValidation && ForWpf;

            yield return "";

            if (isReadonly) yield return $"\t\tprivate static readonly DependencyPropertyKey {dp.Name}PropertyKey =";
            else yield return $"\t\tpublic static readonly DependencyProperty {dp.Name}Property =";

            if (withValidation)
            {
                if (propertyMetadata == null) propertyMetadata = "new PropertyMetadata()";

                yield return $"\t\t\tDependencyProperty.{registerMethod}(nameof({dp.Name}), typeof({dp.PropertyType}), typeof({ControlType}),";
                yield return $"\t\t\t\t{propertyMetadata},";
                yield return $"\t\t\t\t{valueValidation});";
            }
            else if (propertyMetadata != null)
            {
                yield return $"\t\t\tDependencyProperty.{registerMethod}(nameof({dp.Name}), typeof({dp.PropertyType}), typeof({ControlType}),";
                yield return $"\t\t\t\t{propertyMetadata});";
            }
            else
            {
                yield return $"\t\t\tDependencyProperty.{registerMethod}(nameof({dp.Name}), typeof({dp.PropertyType}), typeof({ControlType}));";
            }

            yield return "";

            if (isReadonly)
            {
                yield return $"\t\tpublic static readonly DependencyProperty {dp.Name}Property = {dp.Name}PropertyKey.DependencyProperty;";
                yield return "";
            }

            if (dp.WithPropertyChanged)
            {
                yield return $"\t\tprivate static void On{dp.Name}PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)";
                yield return "\t\t{";
                yield return $"\t\t\t{ControlType} s = ({ControlType})sender;";
                if (dp.WithOldValue) yield return $"\t\t\t{dp.PropertyType} oldValue = ({dp.PropertyType})e.OldValue;";
                if (dp.WithNewValue) yield return $"\t\t\t{dp.PropertyType} newValue = ({dp.PropertyType})e.NewValue;";
                yield return "";
                yield return "\t\t\tthrow new NotImplementedException();";
                yield return "\t\t}";
                yield return "";
            }

            if (withValidation)
            {
                yield return $"\t\tprivate static bool On{dp.Name}PropertyValidation(object obj)";
                yield return "\t\t{";
                yield return $"\t\t\t{dp.PropertyType} value = ({dp.PropertyType})obj;";
                yield return "";
                yield return "\t\t\tthrow new NotImplementedException();";
                yield return "\t\t}";
                yield return "";
            }
        }

        private string GetPropertyMetadataDefinition(DependencyProperty dp)
        {
            bool hasDefaultValue = !string.IsNullOrWhiteSpace(dp.DefaultValue);
            string defaultValue = hasDefaultValue ? dp.DefaultValue : $"default({dp.PropertyType})";
            string propertyChangedCallback = dp.WithBody ?
                $"new PropertyChangedCallback(On{dp.Name}PropertyChanged)" : $"On{dp.Name}PropertyChanged";

            if (hasDefaultValue || !ForWpf || dp.IsReadonly)
            {
                return !dp.WithPropertyChanged ? $"new PropertyMetadata({defaultValue})" : 
                    $"new PropertyMetadata({defaultValue}, {propertyChangedCallback})";
            }

            return !dp.WithPropertyChanged ? null : $"new PropertyMetadata({propertyChangedCallback})";
        }

        private string GetCodePart2(DependencyProperty dp)
        {
            return string.Join("\r\n", GetCodePart2Lines(dp));
        }

        private IEnumerable<string> GetCodePart2Lines(DependencyProperty dp)
        {
            string setModifier = dp.IsReadonly ? "private " : "";
            string dependencyPropertyName = dp.IsReadonly && ForWpf ? $"{dp.Name}PropertyKey" : $"{dp.Name}Property";

            yield return "";
            yield return $"\t\tpublic {dp.PropertyType} {dp.Name}";
            yield return "\t\t{";

            if (dp.WithBody)
            {
                yield return $"\t\t\tget {{ return ({dp.PropertyType})GetValue({dp.Name}Property); }}";
                yield return $"\t\t\t{setModifier}set {{ SetValue({dp.Name}Property, value); }}";
            }
            else
            {
                yield return $"\t\t\tget => ({dp.PropertyType})GetValue({dp.Name}Property);";
                yield return $"\t\t\t{setModifier}set => SetValue({dependencyPropertyName}, value);";
            }

            yield return "\t\t}";
            yield return "";
        }
    }
}
