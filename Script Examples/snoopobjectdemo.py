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
if(len(output)==0):
	print("Dont have any object in view \nPlease draw something!")
else:
	from CADSnoop import SnoopCommand
	a = output[0]
	sn = SnoopCommand()
	sn.Snoop(a)
## you can use shell to snoop with method : 
# snoop(a)
## or use sn.Snoop(a) from shell command
# note with option snoop() will be snoop by pick object same with button from in ribbon