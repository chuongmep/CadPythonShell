# Many thanks to Chuong Ho for this awesome repository
# Many thanks Cyril-Pop for the starting point in this script.

import clr
clr.AddReference('acmgd')
clr.AddReference('acdbmgd')
clr.AddReference('accoremgd')

# Import references from AutoCAD
from Autodesk.AutoCAD.Runtime import *
from Autodesk.AutoCAD.ApplicationServices import *
from Autodesk.AutoCAD.EditorInput import *
from Autodesk.AutoCAD.DatabaseServices import *
from Autodesk.AutoCAD.Geometry import *

# Global AutoCAD variables
doc = Application.DocumentManager.MdiActiveDocument
ed = doc.Editor
db = doc.Database

# Layer names
polyline_layer_name = "Aux-REVISION"
text_layer_name = "I-WALL"

def polyline_center(polyline):
    """
    Calculates the approximate geometric center of a polyline.
    """
    extents = polyline.GeometricExtents
    center = Point3d(
        (extents.MinPoint.X + extents.MaxPoint.X) / 2,
        (extents.MinPoint.Y + extents.MaxPoint.Y) / 2,
        (extents.MinPoint.Z + extents.MaxPoint.Z) / 2
    )
    return center

def create_text(btr, trans, position, text_value, text_layer):
    """
    Creates a text entity in AutoCAD and adds it to the model space.
    """
    text = DBText()
    text.Position = position
    text.Height = 3  # Text height
    text.TextString = text_value  # Text content
    text.Layer = text_layer  # Assign the text to the specified layer
    btr.AppendEntity(text)
    trans.AddNewlyCreatedDBObject(text, True)
    return text

def main():
    output = []  # List to store created texts
    errors = []  # List to record errors

    with doc.LockDocument():  # Lock the document
        with db.TransactionManager.StartTransaction() as t:
            # Access the model space
            bt = t.GetObject(db.BlockTableId, OpenMode.ForRead)
            btr = t.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite)

            # Iterate through the entities in model space
            for objectid in btr:
                blkRef = t.GetObject(objectid, OpenMode.ForRead)
                if isinstance(blkRef, Polyline) and blkRef.Layer == polyline_layer_name:
                    try:
                        # Get the geometric center of the polyline
                        center = polyline_center(blkRef)

                        # Create a unique text entity at the center of the polyline
                        text = create_text(btr, t, center, f"Text-{len(output) + 1}", text_layer_name)
                        output.append(text)
                    except Exception as e:
                        errors.append((blkRef, str(e)))
                        ed.WriteMessage(f"\nError processing polyline: {e}")
                else:
                    errors.append(blkRef)

            # Commit the transaction
            t.Commit()

    # Results
    ed.WriteMessage(f"\n{len(output)} texts were created.")
    if errors:
        ed.WriteMessage(f"\nErrors found: {len(errors)}")

if __name__ == "__main__":
    main()
