using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using StdOttStandard;

namespace CodeGenerator.BaseClass
{
    internal class ParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Parameter> parameters = (IEnumerable<Parameter>)value;

            return string.Join(", ", parameters.ToNotNull());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CodeBaseClassService.GetParameters((string)value).ToArray();
        }
    }
}
