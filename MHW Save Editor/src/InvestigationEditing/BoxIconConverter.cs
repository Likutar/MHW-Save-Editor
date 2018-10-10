using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;

namespace MHW_Save_Editor.InvestigationEditing
{
    public class BoxIconConverter: IValueConverter
    {
            private static readonly string iconpath =
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/src/Resources/BoxIcons/";
            public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
            {
                return iconpath + (int) value + ".png";
            }
            public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) => throw new NotImplementedException();

    }
}