import sys
import clr
# Add Assemblies for AutoCAD
clr.AddReference('AcMgd')
clr.AddReference('AcCoreMgd')
clr.AddReference('AcDbMgd')
clr.AddReference('AdWindows')

# Import references from AutoCAD
from Autodesk.AutoCAD.Runtime import *
from Autodesk.AutoCAD.ApplicationServices import *
from Autodesk.AutoCAD.EditorInput import *
from Autodesk.Windows import *


adoc = Application.DocumentManager.MdiActiveDocument
ed = adoc.Editor

td = TaskDialog()
td.WindowTitle = "The title"
td.MainInstruction = "Something has happened."
td.ContentText = "Here's some text, with a " + "<A HREF=\"http://adn.autodesk.com\">" + "link to the ADN site</A>"
td.VerificationText = "Verification text"
td.FooterText = "The footer with a "+ "<A HREF=\"http://blogs.autodesk.com/through" + "-the-interface\">link to Kean's blog</A>"
td.EnableHyperlinks = True
td.EnableVerificationHandler = True

td.CollapsedControlText = "This control can be collapsed."
td.ExpandedText = "This control can be expanded..." + "\nto multiple lines."
td.ExpandFooterArea = True
td.ExpandedByDefault = False

td.MainIcon = TaskDialogIcon.Shield
td.FooterIcon = TaskDialogIcon.Information

td.UseCommandLinks = True
td.Buttons.Add(TaskDialogButton(1, "This is one course of action."))
td.Buttons.Add(TaskDialogButton(2, "This is another course of action."))
td.Buttons.Add(TaskDialogButton(3, "And would you believe we have a third!"))
td.DefaultButton = 3

td.RadioButtons.Add(TaskDialogButton(4, "Yes"))
td.RadioButtons.Add(TaskDialogButton(5, "No"))
td.RadioButtons.Add(TaskDialogButton(6, "Maybe"))

td.DefaultRadioButton = 5
td.AllowDialogCancellation = False

"Need help with the Callback here"

td.Show(Application.MainWindow.Handle)
