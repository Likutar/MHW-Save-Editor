using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using MHW_Save_Editor.InvestigationEditing;
using MHW_Save_Editor.SaveSlot;
using MHW_Save_Editor.SlotEditing;
using MHW_Save_Editor.Tabs;

namespace MHW_Save_Editor
{
    public partial class MainWindow
    {
        
        private object PopulateSlotData(byte[] newdata)
        {
            int offset = CharacterSlot.SaveSlotOffset;
            int size = CharacterSlot.SaveSize;
            int current = Properties.Settings.Default.activeSlot;
            CharacterSlot Slot = new CharacterSlot(newdata.Slice(offset+size*current, offset+size*(current+1)));
            Application.Current.Resources["CharacterSlotData"] = new CharacterSlotViewModel(Slot);
            return new SlotTab();
        }

        private void CommitSlot()
        {
            int offset = CharacterSlot.SaveSlotOffset;
            int size = CharacterSlot.SaveSize;
            int current = Properties.Settings.Default.activeSlot;
            CharacterSlotViewModel source =
                (CharacterSlotViewModel) Application.Current.Resources["CharacterSlotData"];
            source.Commit(saveFile.data, offset+size*current);
        }
    }
}