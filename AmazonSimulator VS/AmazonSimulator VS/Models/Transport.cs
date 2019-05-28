using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    public class Transport : Model
    {
        public Transport(double x, double y, double z, double rotationX, double rotationY, double rotationZ) : base(x, y, z, rotationX, rotationY, rotationZ)
        {
        }
    }
}