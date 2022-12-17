"""
Copyright 2019 Autodesk, Inc. All rights reserved.
# https://forum.dynamobim.com/t/use-dynamo-to-create-and-manipulate-autocad-civil-3d-layers/38567/8
This file is part of the Civil 3D Python Module.

"""
__author__ = 'Paolo Emilio Serra - paolo.serra@autodesk.com'
__modifiedby__ = 'Deivid Steffens - deividsteffens@msn.com'
__copyright__ = '2020'
__version__ = '1.0.0'

import clr

# Add Assemblies for AutoCAD and Civil 3D APIs
clr.AddReference('acmgd')
clr.AddReference('acdbmgd')
clr.AddReference('accoremgd')
clr.AddReference('AecBaseMgd')
clr.AddReference('AecPropDataMgd')
clr.AddReference('AeccDbMgd')
clr.AddReference('AeccPressurePipesMgd')
clr.AddReference('acdbmgdbrep')
clr.AddReference('System.Windows.Forms')

# Add references to manage arrays, collections and interact with the user
from System import *
from System.IO import *
from System.Collections.Specialized import *
from System.Windows.Forms import MessageBox

# Create an alias to the Autodesk.AutoCAD.ApplicationServices.Application class
import Autodesk.AutoCAD.ApplicationServices.Application as acapp

# Import references from AutoCAD
from Autodesk.AutoCAD.Runtime import *
from Autodesk.AutoCAD.ApplicationServices import *
from Autodesk.AutoCAD.EditorInput import *
from Autodesk.AutoCAD.DatabaseServices import *
from Autodesk.AutoCAD.Geometry import *
from Autodesk.AutoCAD.Colors import Color
from Autodesk.AutoCAD.Colors import ColorMethod
# Import references for PropertySets
from Autodesk.Aec.PropertyData import *
from Autodesk.Aec.PropertyData.DatabaseServices import *

# Import references for Civil 3D
from Autodesk.Civil.ApplicationServices import *
from Autodesk.Civil.DatabaseServices import *

adoc = acapp.DocumentManager.MdiActiveDocument
ed = adoc.Editor

# Example function
def create_layer(name, index):

	indx = int(index)
	
	# Fail gracefully
	if not isinstance(name, str) or len(name) == 0:
		raise Exception('The name is not a valid string')
	if not isinstance(indx, int) or indx < 0 or indx > 255:
		raise Exception('The index component is not a valid integer')
	
	# replace invalid characters in the name
	name = name.replace(':', '_').replace(';', '_').replace(',', '_')
		
	global adoc
	result = False

	with adoc.LockDocument():
		with adoc.Database as db:
			with db.TransactionManager.StartTransaction() as t:
				# Get the Layer Table Object
				lt = t.GetObject(db.LayerTableId, OpenMode.ForRead)
				# Check if the layer already exists
				if not lt.Has(name):
					with LayerTableRecord() as ltr:
						ltr = LayerTableRecord()
						ltr.Name = name
						ltr.Color = Color.FromColorIndex(ColorMethod.ByAci,indx)
						lt.UpgradeOpen()
						lt.Add(ltr)
						lt.DowngradeOpen()
						t.AddNewlyCreatedDBObject(ltr, True)
				else:
					ltr = t.GetObject(lt[name], OpenMode.ForWrite)
				# Set the color
				if ltr is not None:
					ltr.Color = Color.FromColorIndex(ColorMethod.ByAci,indx)
				
				result = True
				t.Commit()
		
	return result
	
for i in range(10):
	name = "Test"+ str(i)
	create_layer(name,i)
	print("Created Layer",name)
	
