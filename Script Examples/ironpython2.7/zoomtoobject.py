#Copyright(c) 2021, Hồ Văn Chương
# @chuongmep, https://chuongmep.com/
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
def ZoomToObj(ed,min,max):
	min2d = Point2d(min.X, min.Y)
	max2d = Point2d(max.X, max.Y)
	view = ViewTableRecord()
	view.CenterPoint = min2d + ((max2d - min2d) / 2.0)
	view.Height = max2d.Y - min2d.Y
	view.Width = max2d.X - min2d.X
	ed.SetCurrentView(view)
peo = PromptEntityOptions("Select An Entity:")
result = ed.GetEntity(peo)
if(result.Status==PromptStatus.OK):
	with doc.LockDocument():
		with doc.Database as db:
			with db.TransactionManager.StartTransaction() as t:
				ent = t.GetObject(result.ObjectId,OpenMode.ForRead)
				ext = ent.GeometricExtents
				# Do action here
				ZoomToObj(ed, ext.MinPoint, ext.MaxPoint)
			
				t.Commit()