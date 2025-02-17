﻿using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.Data
{
    public class CurveGetEndPoint : Data
    {
        private readonly Curve _curve;

        public CurveGetEndPoint(string label, Curve curve) : base(label)
        {
            _curve = curve;
        }

        public override string StrValue()
        {
            return "< Get End Points >";
        }

        public override bool HasDrillDown => _curve != null && _curve.IsBound;

        public override System.Windows.Forms.Form DrillDown()
        {
            if (!HasDrillDown) return null;

            var xyzObjects = new List<SnoopableObjectWrapper>
            {
                new SnoopableObjectWrapper("[0] Start", _curve.GetEndPoint(0)), 
                new SnoopableObjectWrapper("[1] End", _curve.GetEndPoint(1))
            };

            if (!xyzObjects.Any()) return null;

            var form = new Forms.Objects(xyzObjects);
            return form;
        }
    }
}