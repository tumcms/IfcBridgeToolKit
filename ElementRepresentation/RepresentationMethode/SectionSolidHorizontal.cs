using Xbim.Ifc;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.ProfileResource;


namespace ElementRepresentation.RepresentationMethode
{
    public class SectionSolidHorizontal
    {
        public void CreateSectionedSolidHorizontal(ref IfcStore model,
            IfcCurve ifcCurve,
            IfcProfileDef Crosssection_1,
            IfcProfileDef Crosssection_2,
            IfcDistanceExpression CSPositions_1,
            IfcDistanceExpression CSPositions_2,
            IfcBoolean ifcBoolean,
            out IfcSectionedSolidHorizontal ifcSectionedSolidHorizontal)
        {
            ifcSectionedSolidHorizontal = model.Instances.New<IfcSectionedSolidHorizontal>(iSSH =>
            {
                iSSH.Directrix = ifcCurve;
                iSSH.CrossSections.Add(Crosssection_1);
                iSSH.CrossSections.Add(Crosssection_2);
                iSSH.CrossSectionPositions.Add(CSPositions_1);
                iSSH.CrossSectionPositions.Add(CSPositions_2);
                iSSH.FixedAxisVertical = ifcBoolean;

            });
        }
    }
}
