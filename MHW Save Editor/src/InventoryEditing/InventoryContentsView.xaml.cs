using System.Windows.Controls;

namespace MHW_Save_Editor.InventoryEditing
{
    public partial class InventoryContentsView : UserControl
    {
        public InventoryContentsView()
        {
            InitializeComponent();
            if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                this.DataContext = new InventoryContentsViewModel();
            }
        }
    }
}
