using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.ProfileResource;


namespace ElementCreator.ProfileTypes
{
    public class ArbitrayClosedProfileDef
    {
        public void CreateArbitraryClosedProfileDef(ref IfcStore model,IfcCartesianPointList2D CartesianPoinList, out IfcArbitraryClosedProfileDef ArbitraryClosedProfile)
        {

            ArbitraryClosedProfile = model.Instances.New<IfcArbitraryClosedProfileDef>();
                               
            var Polycurve = model.Instances.New<IfcIndexedPolyCurve>();
            Polycurve.Points = CartesianPoinList;
            Polycurve.SelfIntersect = false;

            ArbitraryClosedProfile.ProfileType = IfcProfileTypeEnum.AREA;
            ArbitraryClosedProfile.OuterCurve = Polycurve;
        }
    }
}
