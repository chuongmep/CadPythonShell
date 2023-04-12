using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Autodesk.AutoCAD.DatabaseServices;
using CADSnoop.Model;
using CADSnoop.ViewModel;
using Exception = System.Exception;

namespace CADSnoop.View
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SnoopViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(SnoopViewModel vm) :this()
        {
            this._viewModel = vm;
            this.DataContext = vm;
            SnoopViewModel.FrmMain = this;
        }

        private void Treeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            try
            {

                TreeViewCustomItem selected = e.NewValue as TreeViewCustomItem;
                object obj = selected.Object;
                DBObject dbObject = obj as DBObject;
                if (dbObject != null)
                {
                    foreach (KeyValuePair<string, List<ObjectDetails>> pair in _viewModel.DataMethod)
                    {
                        if (pair.Key == dbObject.Id.ToString())
                        {
                            _viewModel.LisViewItems = new List<ObjectDetails>();
                            _viewModel.LisViewItems = pair.Value;
                        }
                    }

                    _viewModel.UpdateDataSource(_viewModel.LisViewItems);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.StackTrace);
            }
        }

        private void ListViewItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.listview.SelectedItems.Count != 1)
            {
                return;
            }
            ObjectDetails selectedItem = this.listview.SelectedItem as ObjectDetails;
            object LinkObject = selectedItem.LinkObject;
            if (LinkObject is null)
            {
                return;
            }
            if (_viewModel.IsEnumerable(LinkObject))
            {
                using (_viewModel.doc.LockDocument())
                {
                    _viewModel.CollectionItemSelected(LinkObject);
                }

            }
            else if (LinkObject is ObjectId objectId)
            {
                if (!IsExcept(objectId))
                {
                    using (_viewModel.doc.LockDocument())
                    {
                        _viewModel.ObjectIdItemSelected(objectId);
                    }
                }
            }
        }


        private void ContextMenu_MouseDown(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            if (menuitem != null)
            {
                ContextMenu parent_contextmenu = menuitem.CommandParameter as ContextMenu;
                if (parent_contextmenu != null)
                {
                    string clip = "";
                    foreach (var item in this.listview.SelectedItems)
                    {
                        ObjectDetails objectDetails = item as ObjectDetails;
                        clip += objectDetails.PropName + "\t" + objectDetails.Type + "\t" + objectDetails.Value + "\n";
                    }
                    Clipboard.SetText(clip);
                }
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }


        bool IsExcept(ObjectId objectId)
        {
            if (objectId.IsNull) return true;
            if (objectId.ToString() == "0") return true;
            if (objectId.ToString() == "(0)") return true;
            return false;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

