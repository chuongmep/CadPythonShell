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
#Code Here : 
objects = []
with doc.LockDocument():
	with doc.Database as db:
		with db.TransactionManager.StartTransaction() as t:
			acblkbl = t.GetObject(db.BlockTableId,OpenMode.ForRead)
			print(type(acblkbl))
			acblktblrec = t.GetObject(acblkbl[BlockTableRecord.ModelSpace],OpenMode.ForWrite)
			print(type(acblktblrec))
			sel = doc.Editor.GetSelection()
			if(sel.Status== PromptStatus.OK):
				results = sel.Value
				for i in range(len(results)):
					if(results[i] != None) : objects.append(i)
			else : pass
print("Count Object Exploded:",len(objects))