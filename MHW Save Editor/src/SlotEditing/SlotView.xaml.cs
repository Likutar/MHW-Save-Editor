using System.Windows.Controls;

namespace MHW_Save_Editor.SlotEditing
{
    public partial class SlotView : UserControl
    {
        public SlotView()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new SlotViewModel();
            }
        }
    }
}
