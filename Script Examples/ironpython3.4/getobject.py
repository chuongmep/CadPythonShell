import clr
clr.AddReference('acmgd')
clr.AddReference('acdbmgd')
clr.AddReference('accoremgd')
# Import references from AutoCAD
from Autodesk.AutoCAD.Runtime import *
from Autodesk.AutoCAD.ApplicationServices import *
from Autodesk.AutoCAD.EditorInput import *
from Autodesk.AutoCAD.DatabaseServices import *
from Autodesk.AutoCAD.Geometry import *
adoc = Application.DocumentManager.MdiActiveDocument
ed = adoc.Editor
output =[]
with adoc.LockDocument():
    with adoc.Database as db:
        with db.TransactionManager.StartTransaction() as t:
            bt = t.GetObject(db.BlockTableId, OpenMode.ForWrite)
            btr = t.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite)
            for objectid in btr:
                obj1 = t.GetObject(objectid, OpenMode.ForRead)
                output.append(obj1)
                print(obj1)
ed.WriteMessage("Done")