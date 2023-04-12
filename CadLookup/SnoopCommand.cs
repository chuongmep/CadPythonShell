using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using CADSnoop.Model;
using CADSnoop.View;
using CADSnoop.ViewModel;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using Exception = System.Exception;

namespace CADSnoop
{
    public class SnoopCommand
    {
        [CommandMethod("Snoop")]
        public void Snoop()
        {
            Snoop(new List<ObjectId>());
        }
        /// <summary>
        /// Snoop Snoop a object by id
        /// </summary>
        /// <param name="objectId"></param>
        public void Snoop(ObjectId objectId)
        {
            Snoop(new List<ObjectId>(){objectId});
        }
        /// <summary>
        /// snoop by dbobject
        /// </summary>
        /// <param name="dbObject"></param>
        /// <exception cref="ArgumentException"></exception>
        public void Snoop(DBObject dbObject)
        {
            if (dbObject == null) throw new ArgumentException(nameof(dbObject));
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            using (DocumentLock lockDoc = doc.LockDocument())
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    {
                        SnoopViewModel vm = new SnoopViewModel(doc, db, dbObject);
                        MainWindow form = new MainWindow(vm);
                        form.SetCadAsWindowOwner();
                        form.Show();
                    }
                    tran.Commit();
                }
            }
        }
        /// <summary>
        /// Snoop snoop object by list object id
        /// </summary>
        /// <param name="objectIds"></param>
        public void Snoop(List<ObjectId> objectIds)
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;
                using (DocumentLock lockDoc = doc.LockDocument())
                {
                    using (Transaction tran = db.TransactionManager.StartTransaction())
                    {
                        if (objectIds == null || objectIds.Count==0) objectIds = PickObjectBySelect(doc);
                        if (objectIds != null)
                        {
                            SnoopViewModel vm = new SnoopViewModel(doc, db, objectIds);
                            MainWindow form = new MainWindow(vm);
                            form.SetCadAsWindowOwner();
                            form.Show();
                        }
                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        List<ObjectId> PickObjectBySelect(Document doc)
        {
            try
            {
                //PromptSelectionOptions poOptions = new PromptSelectionOptions();
                //poOptions.SingleOnly = true;
                PromptSelectionResult promptSelectionResult = doc.Editor.GetSelection();
                if (promptSelectionResult.Status != PromptStatus.OK) return null;
                SelectionSet selectionSet = promptSelectionResult.Value;
                return selectionSet.GetObjectIds().ToList();
            }
            catch (ArgumentNullException) { }
            catch (NullReferenceException) { }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return null;
        }

    }
}
