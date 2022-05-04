#Many thanks Cyril-Pop
import clr
import sys
import System
pf_path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86)
sys.path.append(pf_path + '\\IronPython 2.7\\Lib')
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
all_blkName = []
with doc.LockDocument():
	with doc.Database as db:
		with db.TransactionManager.StartTransaction() as t:
			bt = t.GetObject(db.BlockTableId,OpenMode.ForRead)
			btr  = t.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForRead)
			for objectid in btr:
				blkRef = t.GetObject(objectid, OpenMode.ForRead)
				if isinstance(blkRef, BlockReference):
					if blkRef.IsDynamicBlock:
						block = t.GetObject(blkRef.DynamicBlockTableRecord, OpenMode.ForRead)
						all_blkName.append(block.Name)
					else:
						all_blkName.append(blkRef.Name)	
			resultcount = ["{} - total:{}".format(x, all_blkName.count(x)) for x in set(all_blkName)]
			print("\n".join(resultcount))
			t.Commit()