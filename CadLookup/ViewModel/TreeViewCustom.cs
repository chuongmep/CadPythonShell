using System.Collections.ObjectModel;
using Autodesk.AutoCAD.DatabaseServices;

namespace CADSnoop.ViewModel
{
    public class TreeViewCustomItem
    {
        public TreeViewCustomItem()
        {
            this.ChildItems = new ObservableCollection<TreeViewCustomItem>();
        }

        public string Title { get; set; }
        public DBObject Object { get; set; }
        public ObservableCollection<TreeViewCustomItem> ChildItems { get; set; }
    }
}
