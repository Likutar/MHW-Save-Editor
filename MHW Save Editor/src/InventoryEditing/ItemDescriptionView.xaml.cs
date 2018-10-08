using System;
using System.Windows;
using System.Windows.Controls;

namespace MHW_Save_Editor.InventoryEditing
{
    public partial class ItemDescriptionView : UserControl
    {
        public ItemDescriptionView()
        {
            InitializeComponent();
        }

        public void ItemSelection(object e, EventArgs arg)
        {
            if (((ComboBox)e).SelectedItem!=null)
            ((InventorySlot) DataContext).Item = (Item)((ComboBox)e).SelectedItem ;
        }
    }
}
