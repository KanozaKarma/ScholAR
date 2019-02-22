using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleARCore.Examples.HelloAR
{
    class Data
    {
        private float xCo;

        private float yCo;

        private float zCo;

        Data(float x, float y, float z)
        {
            xCo = x;
            yCo = y;
            zCo = z;
        }

        public void SetX(float x)
        {
            xCo = x;
        }

        public void SetY(float y)
        {
            yCo = y;
        }

        public void SetZ(float z)
        {
            zCo = z;
        }

        public float GetX()
        {
            return (xCo);
        }

        public float GetY()
        {
            return (yCo);
        }

        public float GetZ()
        {
            return (zCo);
        }
    }
}
