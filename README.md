# CADPythonShell
![Autocad API](https://img.shields.io/badge/Autocad%20API%202022-blue.svg)
![Platform](https://img.shields.io/badge/platform-Windows-lightgray.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
<a href="https://twitter.com/intent/follow?screen_name=chuongmep">
        <img src="https://img.shields.io/twitter/follow/chuongmep?style=social&logo=twitter"
            alt="follow on Twitter"></a>

![ReSharper](https://img.shields.io/badge/ReSharper-2021.3.2-yellow)
![Rider](https://img.shields.io/badge/Rider-2021.3.2-yellow)
![Visual Studio 2022](https://img.shields.io/badge/Visual_Studio_2022_Preview_2.0-17.1.0-yellow)
![.NET Framework](https://img.shields.io/badge/.NET_6(LTS)-yellow)

[![Publish](../../actions/workflows/Workflow.yml/badge.svg)](../../actions)

## Introduction

This is obviously a fork of [RevitPythonShell](https://github.com/architecture-building-systems/revitpythonshell), bringing an IronPython interpreter to Autodesk Autocad,
and it would not be possible without the great work of everyone involved with the RPS project. It's still pretty rough around the edges and provides only basic functionality at this time. I'm sharing my work so far, in the hopes that together we can expand it further.

The CADPythonShell (CPS) ,lets you to write plugins for Autocad in Python, provides you with an interactive shell that lets you see the results of your code *as you type it*. This is great for exploring the Autocad API.

The biggest limitation is that you can't deploy DLLs with custom scripts at this time and you can't subscribe to events at startup time.

![](Images/Ribbon.png)

## Features

- Interactive IronPython interpreter for exploring the API
  - With syntax highlighting and autocompletion (in the console only)
  - Based on the [IronLab](http://code.google.com/p/ironlab/) project
- Batteries included! (Python standard library is bundled as a resource in the `CADRuntime.dll`)
- Full access to the .NET framework and the Autocad and Civil3D API
- Configurable "environment" variables that can be used in your scripts
- Save "external scripts" for reuse and start collecting your awesome hacks!
- Run scripts at Autocad or Civil startup
- Quick **Snoop** info object Autocad or Civil3D
- Interactive Snoop from console with IronPython
## Installation

- Download last install stable(msi) from [Release](https://github.com/chuongmep/CADPythonShell/releases/latest)
- Use command `PythonConsole` to open Console or use command `PythonShellSetting` to open form setting
- See guide install detail at [How-to-Install-CadPythonShell](https://github.com/chuongmep/CadPythonShell/wiki/How-to-Install-CadPythonShell)

Note : Support for 4 last version(2019-2022) Autocad or Civil 3D. Older versions can be used but will not guarantee the expected performance.


## Basic Usage

- <kbd>PythonConsole</kbd> - Open Python Console
- <kbd>PythonShellSetting</kbd> - Open Setting Config Console
- <kbd>Snoop</kbd> - Snoop Object In CAD or Civil3D
- <kbd>sn.Snoop(obj)</kbd> - Snoop Object by Python Console In CAD or Civil3D
- <kbd>snoop(obj)</kbd> - Snoop Object by Python Console or Execute python code In CAD or Civil3D

![](Images/pythoncmd.png)

- Write Console Sample

``` py
ed = doc.Editor
ed.WriteMessage("Hello")
```
![](Images/console.gif)

- Try create script get Object

```py
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
with adoc.LockDocument():
	with adoc.Database as db:
		with db.TransactionManager.StartTransaction() as t:
  			bt = t.GetObject(db.BlockTableId, OpenMode.ForWrite)
  			btr = t.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite)
  			for objectid in btr:
				obj1 = t.GetObject(objectid, OpenMode.ForRead)
				print(obj1)
ed.WriteMessage("Done")
```
![](Images/getobject.png)

Note : you can see more example in folder [Script Examples](https://github.com/chuongmep/CadPythonShell/tree/dev/Script%20Examples)

## Contribute

- Don't hesitate to file any issues you stumble uppon. (Tho I don't guarantee I'll be able to solve them all for you)

## Getting started:

Learn some python:

  * [Python W3schools](https://www.w3schools.com/python/python_intro.asp)
  * [Python Basic](https://www.python.org/about/gettingstarted/)

Learn about the Autocad API:

  * ADN [Autodesk Developer Network](https://www.autodesk.com/developer-network/overview)
  * [The Autocad SDK"](https://www.autodesk.com/developer-network/platform-technologies/autocad/objectarx)
   

## License

This project is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT).
## Sponsors

Thanks to [JetBrains](https://www.jetbrains.com/) for providing licenses for [Rider](https://www.jetbrains.com/rider/) and [dotUltimate](https://www.jetbrains.com/dotnet/) tools, which both make open-source development a real pleasure!

![](Images/jetbrains.png)

## Credits

  * Daren Thomas (original RPS Developer) [RPS](https://github.com/architecture-building-systems/revitpythonshell)
  * Joe Moorhouse (interactive shell was taken from his project [IronLab](http://ironlab.net/))
  * [Dimitar Venkov](https://github.com/dimven/NavisPythonShell) (original port to Navisworks)
  * [ChuongMep](https://github.com/chuongmep) (original port to Autocad)
  * The rest of the RPS contributors
  * [Nice 3 Point](https://github.com/Nice3point) for process CI/CD
  * [Icon 8](https://icons8.com/) Free Wiki icons in various UI design styles for web, mobile
  * [htlcnn](https://github.com/htlcnn) origin of project AutocadLookup
