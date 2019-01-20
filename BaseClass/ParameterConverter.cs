using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace CodeGenerator.BaseClass
{
    class ParameterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<Parameter> parameters = (IEnumerable<Parameter>)value;

            return string.Join(", ", parameters);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return CodeBaseClassService.GetParameters((string)value).ToArray();
        }
    }
}
