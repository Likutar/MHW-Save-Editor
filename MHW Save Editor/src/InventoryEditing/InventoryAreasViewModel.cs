using System.Windows;
using System.Windows.Data;
using MHW_Save_Editor.Data;

namespace MHW_Save_Editor.InventoryEditing
{
    public class InventoryAreasViewModel : NotifyUIBase
    {

            public ListCollectionView InventoryAreaCollectionView {get; set;}
            private InventoryArea CurrentInventoryArea
            {
                get { return InventoryAreaCollectionView.CurrentItem as InventoryArea; }
                set
                {
                    InventoryAreaCollectionView.MoveCurrentTo(value);
                    RaisePropertyChanged();

                }
            }
            public InventoryAreasViewModel()
            {
                InventoryAreaCollectionView = Application.Current.Resources["InventoryAreaCollectionView"] as ListCollectionView;
                InventoryAreaCollectionView.MoveCurrentToPosition(1);
            }
    }
}