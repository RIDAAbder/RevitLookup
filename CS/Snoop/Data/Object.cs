#region Header
//
// Copyright 2003-2021 by Autodesk, Inc. 
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
#endregion // Header

using System;
using System.Collections;
using System.Windows.Forms;

namespace RevitLookup.Snoop.Data
{
	/// <summary>
	/// Snoop.Data class to hold and format an Object value.
	/// </summary>
	
	public class Object : Data
	{
		private string m_StrValue;

		protected System.Object m_val;
	    
		public
		Object(string label, System.Object val)
		:   base(label)
		{
		    m_val = val;
			m_StrValue = Utils.ObjToLabelStr(m_val);
		}
		
        public override string
        StrValue()
        {
			return m_StrValue;
        }
        
        public override bool
        HasDrillDown
        {
            get {
                if (m_val == null)
                    return false;
                else
                    return true;
            }
        }
        
        public override System.Windows.Forms.Form DrillDown()
        {
            if (m_val != null) {
				ArrayList objs = new ArrayList();
				objs.Add(m_val);

				Snoop.Forms.Objects form = new Snoop.Forms.Objects(objs);
				return form;				
			}
			return null;
        }
	}
}
