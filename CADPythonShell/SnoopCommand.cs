using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using CADSnoop.Model;
using CADSnoop.View;
using CADSnoop.ViewModel;
using Exception = System.Exception;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;
using MessageBox = System.Windows.MessageBox;

namespace CADPythonShell
{
    public class SnoopCommand
    {
        [CommandMethod("Snoop")]
        public void Execute()
        {
            try
            {
                Document doc = Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;
                Database db = doc.Database;
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    List<ObjectId> objectIds = PickObjectBySelect(doc);
                    if (objectIds!=null)
                    {
                        SnoopViewModel vm = new SnoopViewModel(doc, db, objectIds);
                        MainWindow form = new MainWindow(vm);
                        form.SetCadAsWindowOwner();
                        form.Show();
                    }
                    tran.Commit();

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
            catch(ArgumentNullException){}
            catch(NullReferenceException){}
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            return null;
        }

    }
}
