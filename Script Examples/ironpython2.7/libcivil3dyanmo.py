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
clr.AddReference('Civil3DNodes')
clr.AddReference('AutoCADNodes')

# Add standard Python references
import sys
sys.path.append('C:\Program Files (x86)\IronPython 2.7\Lib')
import os
import math

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

# Import references for Civil 3D
from Autodesk.Civil.ApplicationServices import *
from Autodesk.Civil.DatabaseServices import *

# Import references for Dynamo for Civil 3D
from Autodesk.Civil.DynamoNodes import Surface as DynSurface
from Autodesk.Civil.DynamoNodes.Selection import SurfaceByName
from Autodesk.AutoCAD.DynamoNodes import Document as DynDocument