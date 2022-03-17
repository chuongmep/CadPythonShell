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

## Get Color
import Autodesk.AutoCAD.Colors.EntityColor as cl
rgbcad = cl.LookUpRgb(111)
print(rgbcad)
blue = rgbcad & 255
green = (rgbcad >> 8) & 255
red = (rgbcad >> 16) & 255
print("Color RGB is", (red,green,blue))