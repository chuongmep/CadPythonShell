using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using CADSnoop.Model;
using CADSnoop.View;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;

namespace CADSnoop.ViewModel
{
    public class SnoopViewModel : ViewModelBase

    {
        private Dictionary<string, List<ObjectDetails>> dataMethod;

        public Dictionary<string, List<ObjectDetails>> DataMethod
        {
            get => dataMethod;
            set => dataMethod = value;
        }

        private const string stringEmpty = "[Empty]";
        private const string stringEmptyCollection = "[Empty Collection]";
        private const string stringCollection = "[Collection]";
        public Editor Ed;
        public Document doc;
        public Database Database;
        public static MainWindow FrmMain;

        public List<ObjectId> ObjectIds;

        private List<ObjectDetails> listviewitems;

        public List<ObjectDetails> LisViewItems
        {
            get
            {
                if (listviewitems == null)
                {
                    listviewitems = new List<ObjectDetails>();
                }
                return listviewitems;
            }
            set => OnPropertyChanged(ref listviewitems, value);
        }

        private List<TreeViewCustomItem> treeViewItems;

        public List<TreeViewCustomItem> TreeViewItems
        {
            get
            {
                if (treeViewItems == null)
                {
                    treeViewItems = new List<TreeViewCustomItem>();
                }
                return treeViewItems;
            }
            set => OnPropertyChanged(ref treeViewItems, value);
        }

        public SnoopViewModel(Document doc, Database db)
        {
            this.doc = doc;
            this.Ed = doc.Editor;
            this.Database = db;
            this.dataMethod = new Dictionary<string, List<ObjectDetails>>();
           
        }
        /// <summary>
        /// Base Model
        /// </summary>
        /// <param name="db"></param>
        /// <param name="objectIds"></param>
        public SnoopViewModel(Document doc, Database db, List<ObjectId> objectIds) : this(doc,db)
        {
            this.ObjectIds = objectIds;
            GetListViewItem();
        }

        public SnoopViewModel(Document doc, Database db, DBObject dbObject)  :this(doc,db)
        {
            this.ObjectIds = new List<ObjectId>(){ dbObject.Id};
            GetListViewItem();
        }
        /// <summary>
        /// Get list item of object
        /// </summary>
        void GetListViewItem()
        {
            try
            {
                listviewitems = new List<ObjectDetails>();
                Transaction tran = Database.TransactionManager.StartTransaction();
                UpdateDataSource(listviewitems);
                if (ObjectIds.Any())
                {
                    foreach (ObjectId objectId in ObjectIds)
                    {
                        DBObject obj = tran.GetObject(objectId, OpenMode.ForWrite, true);
                        AddToTreeView(obj);
                        ListObjectInformation(obj);

                    }
                }

                tran.Commit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Update Source Tree By Group Name
        /// </summary>
        /// <param name="Items"></param>
        public void UpdateDataSource(List<ObjectDetails> Items)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Items);
            view.GroupDescriptions.Clear();
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("GroupName");
            view.GroupDescriptions.Add(groupDescription);
            view.Refresh();
        }

        /// <summary>
        /// Add Data object to tree view
        /// </summary>
        /// <param name="obj"></param>
        public void AddToTreeView(DBObject obj)
        {
            try
            {
                string text = GetNameOrType(obj) + " " + obj.ObjectId;
                TreeViewCustomItem item = new TreeViewCustomItem() { Title = text, Object = obj };
                TreeViewItems.Add(item);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private string GetNameOrType(object obj)
        {
            PropertyInfo propName = obj.GetType().GetProperty("Name");
            if (propName != null && propName.CanRead)
                return propName.GetValue(obj).ToString();
            else
                return obj.GetType().Name;
        }

        public void ListObjectInformation(DBObject obj)
        {
            try
            {
                LisViewItems.Clear();
                Type objType = obj.GetType();
                ListProperties(obj, objType);
                ListMethods(obj, objType);
                dataMethod.Add(obj.Id.ToString(), new List<ObjectDetails>(listviewitems));
                ICollectionView view = CollectionViewSource.GetDefaultView(listviewitems);
                view.Refresh();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// Get all properties objects
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objType"></param>
        public void ListProperties(object obj, Type objType)
        {
            PropertyInfo[] properties = objType.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                if ((IsBanned(objType, prop.Name)))
                    continue;

                string propName = prop.Name;
                string propType = prop.PropertyType.Name;
                string propValue = GetValueAsString(prop, obj);
                object LinkObject = GetValue(prop, obj);
                listviewitems.Add(new ObjectDetails()
                {
                    GroupName = prop.DeclaringType.FullName,
                    PropName = propName,
                    Type = propType,
                    Value = propValue,
                    LinkObject = LinkObject
                });

            }
        }

        /// <summary>
        /// Get Value of method object
        /// </summary>
        /// <param name="method"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private object GetValue(MethodInfo method, object obj)
        {
            object methodValue = null;
            try
            {
                methodValue = method.Invoke(obj, null);
            }
            catch
            {
            }

            if ((methodValue != null))
                return methodValue;
            return stringEmpty;
        }

        private object GetValue(PropertyInfo prop, object obj)
        {
            object propValue = null;
            try
            {
                propValue = prop.GetValue(obj, null);
            }
            catch
            {
            }

            if ((propValue != null))
                return propValue;
            return stringEmpty;
        }

        private StringCollection _bannedList = new StringCollection();

        /// <summary>
        /// Check Tree View Is Expanded
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        private bool IsBanned(Type objectType, string propName)
        {
            return (_bannedList.Contains($"{objectType.Name}_{propName}"));
        }


        /// <summary>
        /// Get All Method Object Name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objType"></param>
        public void ListMethods(object obj, Type objType)
        {
            try
            {
                MethodInfo[] methods = objType.GetMethods();
                foreach (MethodInfo meth in methods)
                {
                    if (meth.Name.Contains("Reactor"))
                        continue; // skip some unwanted methods...
                    if ((meth.GetParameters().Length == 0 & !meth.IsSpecialName & meth.ReturnType != typeof(void)))
                    {
                        object methodValue = GetValue(meth, obj);
                        if ((IsEnumerable(methodValue)))
                        {
                            string propName = meth.Name;
                            string propType = meth.ReturnType.Name;
                            string propValue = stringCollection;
                            object LinkObject = GetValue(meth, obj);
                            listviewitems.Add(new ObjectDetails()
                            {
                                GroupName = meth.DeclaringType.FullName,
                                PropName = propName,
                                Type = propType,
                                Value = propValue,
                                LinkObject = LinkObject
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public bool IsEnumerable(object obj)
        {
            string asString = obj as string;
            if ((asString != null))
                return false; // strings are enumerable, but not collections
            IEnumerable asEnum = obj as IEnumerable;
            return (asEnum != null);
        }

        /// <summary>
        /// Get Value Object
        /// </summary>
        /// <param name="prop"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string GetValueAsString(PropertyInfo prop, object obj)
        {
            object propValue = null;
            try
            {
                if (!(prop.CanRead))
                    return "[Write-only]";

                propValue = prop.GetValue(obj, null);
                if (IsEnumerable(propValue))
                {
                    IEnumerable asEnum = propValue as IEnumerable;
                    foreach (object item in asEnum)
                        return stringCollection; // at least one, ok
                    return stringEmptyCollection; // empty collection then
                }
            }
            catch
            {
                // ignored
            }

            if ((propValue != null))
                return propValue.ToString();
            return stringEmpty;
        }

        /// <summary>
        /// Get Info Selected Object
        /// </summary>
        /// <param name="LinkObject"></param>
        public void ObjectIdItemSelected(object LinkObject)
        {
            List<ObjectId> objectIds = new List<ObjectId>();
            objectIds.Add((ObjectId)LinkObject);
            try
            {
                SnoopViewModel viewModel = new SnoopViewModel(doc, Database, objectIds);
                MainWindow form = new MainWindow(viewModel);
                Application.ShowModalWindow(FrmMain.Owner, form);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        /// <summary>
        /// Get Info Item Select Object
        /// </summary>
        /// <param name="LinkObject"></param>
        public void CollectionItemSelected(object LinkObject)
        {
            List<ObjectId> objectIds = new List<ObjectId>();

            foreach (var item in (IEnumerable)LinkObject)
            {
                if (item is ObjectId)
                {
                    objectIds.Add((ObjectId)item);
                }
            }

            if (objectIds.Count > 0)
            {
                try
                {
                    SnoopViewModel vm = new SnoopViewModel(doc, Database, objectIds);
                    MainWindow form = new MainWindow(vm);
                    form.WindowStartupLocation = WindowStartupLocation.Manual;
                    Application.ShowModalWindow(form);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No ObjectId inside this Collection");
            }
        }
    }
}
