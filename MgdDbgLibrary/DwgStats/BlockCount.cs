//
// (C) Copyright 2004 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using Autodesk.AutoCAD.DatabaseServices;
using System.Collections;

namespace MgdDbg.DwgStats
{
    /// <summary>
    /// Summary description for BlockCount.
    /// </summary>
    public class BlockCount
    {
        // data members
        public string m_blockName;

        public ObjectId m_blockDefId;
        public ArrayList m_objCounts = new ArrayList();

        public
        BlockCount()
        {
        }

        public ObjCount
        GetCount(string className, string displayName, bool addIfNotThere)
        {
            foreach (ObjCount tmpNode in m_objCounts)
            {
                if (tmpNode.m_className == className)
                    return tmpNode;
            }

            if (addIfNotThere)
            {
                ObjCount tmpNode = new ObjCount();
                tmpNode.m_className = className;
                tmpNode.m_displayName = displayName;

                m_objCounts.Add(tmpNode);

                return tmpNode;
            }

            return null;    // didn't find it
        }
    }
}