using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using MHW_Save_Editor.InvestigationEditing;

namespace MHW_Save_Editor
{
    public partial class InvestigationView : UserControl
    {
        public InvestigationView()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new InvestigationViewModel();
            }
        }
    }
}