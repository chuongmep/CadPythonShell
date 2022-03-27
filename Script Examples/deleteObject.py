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
result = ed.GetEntity("Please Select Object To Delete")
if(result.Status==PromptStatus.OK): objId = result.ObjectId
#Code Here : 
with doc.LockDocument():
	with doc.Database as db:
		with db.TransactionManager.StartTransaction() as t:
			obj = t.GetObject(objId, OpenMode.ForWrite)
			obj.Erase()
			t.Commit()
			print("Object Deleted")