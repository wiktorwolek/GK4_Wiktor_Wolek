using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4
{
     class Camera
    {

        public Vector3 position;
        public Vector3 target;
        public Func<float, Vector3> camerFunc;
        public Func<float, Vector3> camerDirFunc;
        public Camera( Func<float, Vector3> camerFunc, Func<float, Vector3> camerDirFunc)
        {
            this.position = camerFunc(0);
            this.camerFunc = camerFunc;
            this.target = camerDirFunc(0);
            this.camerDirFunc = camerDirFunc;
        }
    }
}
