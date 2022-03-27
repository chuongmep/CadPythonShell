#Copyright(c) 2021  Hồ Văn Chương
# @chuongmep  https://chuongmep.com/
import clr
import sys
sys.path.append('C:\Program Files (x86)\IronPython 2.7\Lib')
import os
import math
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
			btr = t.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite)
			# Do action here
			box = Solid3d()
			box.CreateBox(1000,2000,3000)
			matrix = ed.CurrentUserCoordinateSystem
			matrix = matrix * Matrix3d.Displacement(Vector3d(111, 222, 333))
			box.TransformBy(matrix)
			btr.AppendEntity(box)
			t.AddNewlyCreatedDBObject(box, True)
			t.Commit()
			print("Created Box")