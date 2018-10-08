using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows.Data;

namespace MHW_Save_Editor.InvestigationEditing
{
    public class MonsterIconConverter : IValueConverter
    {
        private static readonly string iconpath =
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/src/Resources/MonsterIcons/";
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            return iconpath + Investigation._MonsterNames[(int) value] + ".png";
        }
        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}