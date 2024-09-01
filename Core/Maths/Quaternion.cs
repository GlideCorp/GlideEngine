using Core.Maths.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Maths
{
    public class Quaternion
    {
        //Immaginry Axis component
        public float X {  get; set; }
        public float Y {  get; set; }
        public float Z {  get; set; }

        //Scalar of rotation
        public float W {  get; set; }

        public static Quaternion Identity => new(0, 0, 0, 1);

        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }


        public static Quaternion LookRotation(Vector3 forward, Vector3 upwards)
        {
            throw new NotImplementedException();
        }

        public static Quaternion AxisAngle(Vector3 axis, float angle)
        {
            throw new NotImplementedException();
        }
        public static Quaternion Euler(Vector3 eulerAngles)
        {
            throw new NotImplementedException();
        }
    }
}
