namespace Off_GeomLibrary
{
    public class Point3D
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}