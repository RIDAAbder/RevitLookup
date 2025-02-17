using System;
using System.Linq;
using System.Collections;
using System.Reflection;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using RevitLookup.Snoop.Data.PlaceHolders;

namespace RevitLookup.Snoop.CollectorExts
{
    public static class DataTypeInfoHelper
    {
        public static Data.Data CreateFrom(Document document, MethodInfo info, object returnValue, object elem)
        {
            return CreateFrom(document, info, info.ReturnType, returnValue, elem);
        }

        public static Data.Data CreateFrom(Document document, MemberInfo info, Type expectedType, object returnValue, object elem)
        {
            try
            {
                if (expectedType == typeof(bool))
                {
                    var val = returnValue as bool?;

                    return new Data.Bool(info.Name, val.GetValueOrDefault());
                }
                if (expectedType == typeof (CategoryNameMap))
                    return new Data.CategoryNameMap(info.Name, returnValue as CategoryNameMap);

                if (expectedType == typeof (double))
                    return new Data.Double(info.Name, (double) returnValue);

                if (expectedType == typeof (double?))
                {
                    var value = (double?) returnValue;

                    if (value.HasValue)
                        return new Data.Double(info.Name, value.Value);

                    return new Data.EmptyValue(info.Name);
                }

                if (expectedType == typeof (byte))
                    return new Data.Int(info.Name, (byte) returnValue);

                if ((expectedType == typeof (GeometryObject) || expectedType == typeof (GeometryElement)) && elem is Element)
                    return new Data.ElementGeometry(info.Name, elem as Element, document.Application);

                if (expectedType == typeof (ElementId))
                {
                    if (info.Name == nameof(Element.Id))
                        return new Data.String(info.Name, (returnValue as ElementId)?.IntegerValue.ToString());

                    return new Data.ElementId(info.Name, returnValue as ElementId, document);
                }

                if (expectedType == typeof (ElementSet))
                    return new Data.ElementSet(info.Name, returnValue as ElementSet);

                if (expectedType == typeof (AssetProperty))
                    return new Data.AssetProperty(info.Name, elem as AssetProperties, returnValue as AssetProperty);

                if (expectedType == typeof (Color))
                    return new Data.Color(info.Name, returnValue as Color);

                if (expectedType == typeof (DoubleArray))
                    return new Data.DoubleArray(info.Name, returnValue as DoubleArray);

                if (expectedType == typeof (int))
                {
                    var val = returnValue as int?;

                    return new Data.Int(info.Name, val.GetValueOrDefault());
                }

                if (expectedType == typeof (ParameterSet))
                    return new Data.ParameterSet(info.Name, elem as Element, returnValue as ParameterSet);

                if (expectedType == typeof (string))
                    return new Data.String(info.Name, returnValue as string);

                if (expectedType == typeof (UV))
                    return new Data.Uv(info.Name, returnValue as UV);

                if (expectedType == typeof (XYZ))
                    return new Data.Xyz(info.Name, returnValue as XYZ);

                if (expectedType == typeof(PlanTopologySet))
                {
                    var set = returnValue as PlanTopologySet;
                    var placeholders = set.ToList<PlanTopology>().Select(x => new PlanTopologyPlaceholder(x)).ToList();
                    return new Data.Enumerable(info.Name, placeholders, document);
                }

                if ((typeof (IEnumerable).IsAssignableFrom(expectedType)
                     && expectedType.IsGenericType
                     && (expectedType.GenericTypeArguments[0] == typeof (double)
                         || expectedType.GenericTypeArguments[0] == typeof (int))) || expectedType == typeof (DoubleArray))
                    return new Data.EnumerableAsString(info.Name, returnValue as IEnumerable);

                if (typeof (IEnumerable).IsAssignableFrom(expectedType))
                    return new Data.Enumerable(info.Name, returnValue as IEnumerable, document);

                if (expectedType.IsEnum)
                    return new Data.String(info.Name, returnValue.ToString());

                if (expectedType == typeof (Guid))
                {
                    var guidValue = (Guid) returnValue;

                    return new Data.String(info.Name, guidValue.ToString());
                }

                return new Data.Object(info.Name, returnValue);
            }
            catch (Exception ex)
            {
                return new Data.Exception(info.Name, ex);
            }
        }

        public static void AddDataFromTypeInfo(Document document, MemberInfo info, Type expectedType, object returnValue, object elem, ArrayList data)
        {
            data.Add(CreateFrom(document, info, expectedType, returnValue, elem));
        }
    }
}