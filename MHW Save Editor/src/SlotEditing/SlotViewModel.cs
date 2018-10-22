using System.Windows;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.SlotEditing
{
    public class SlotViewModel : NotifyUIBase
    {
        public CharacterSlotViewModel VisibleSlot { get; set; }

        public SlotViewModel()
        {
            VisibleSlot = Application.Current.Resources["CharacterSlotData"] as CharacterSlotViewModel;
        }

    }
}