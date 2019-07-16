using Xbim.Ifc;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.ProfileResource;


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
