# these commands get executed in the current scope
# of each new shell (but not for canned commands)
from Autodesk.AutoCAD.ApplicationServices import *
from Autodesk.AutoCAD.ApplicationServices.Core import *
from Autodesk.AutoCAD.Runtime import *
app = Application
doc = app.DocumentManager.MdiActiveDocument
db = doc.Database
ed = doc.Editor
def quit():
    __window__.Close()
exit = quit

# a fix for the __window__.Close() bug introduced with the non-modal console
class WindowWrapper(object):
    def __init__(self, win):
        self.win = win

    def Close(self):
        self.win.Dispatcher.Invoke(lambda *_: self.win.Close() )

    def __getattr__(self, name):
        return getattr(self.win, name)
__window__ = WindowWrapper(__window__)
