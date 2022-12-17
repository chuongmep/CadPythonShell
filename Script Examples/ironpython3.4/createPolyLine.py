#Copyright(c) 2021, Hồ Văn Chương
# @chuongmep, https://chuongmep.com/
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
doc = Application.DocumentManager.MdiActiveDocument
ed = doc.Editor
db = doc.Database
# Write Code Below
with doc.LockDocument():
	with doc.Database as db:
		with db.TransactionManager.StartTransaction() as t:
			bt = t.GetObject(db.BlockTableId,OpenMode.ForRead)
			btr  = t.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForWrite)
			# Do action here
			pl = Polyline()
			pl.AddVertexAt(0,Point2d(0.0, 0.0), 0.0, 0.0, 0.0)
			pl.AddVertexAt(1,Point2d(100.0, 0.0), 0.0, 0.0, 0.0)
			pl.AddVertexAt(2,Point2d(100.0, 100.0), 0.0, 0.0, 0.0)
			pl.AddVertexAt(3,Point2d(0.0, 100.0), 0.0, 0.0, 0.0)
			btr.AppendEntity(pl)
			t.AddNewlyCreatedDBObject(pl,True)
			t.Commit()