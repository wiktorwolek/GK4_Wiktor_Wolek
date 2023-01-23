using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Projekt4
{
    class LightSource
    {
        public Obj owner;
        public Vector3 position;
        public Color color;
        public Vector3 direction;
        public Func<float, Vector3> lightMatrixFunc;

        private Func<float, Vector3> lightDirectionFunc;
        private float angle;
        public Matrix4x4 lightMatrix;
        private Vector3 initialPosition;
        private Vector3 initialDirection;
        public float iD = 1f;
        public float iS = 1f;

        public LightSource(Vector3 position, Color color, Func<float, Vector3> lightMatrixFunc, Func<float, Vector3> lightDirectionFunc,float angle,Vector3 direction, float iD, float iS)
        {
            this.position = position;
            this.color = color;
            this.angle = angle;
            this.initialPosition = position;
            this.initialDirection = direction;
            this.iD = iD;
            this.iS = iS;
            this.lightDirectionFunc = lightDirectionFunc;
            this.lightMatrixFunc =lightMatrixFunc;
        }
        public LightSource(Vector3 position, Color color)
        {
            this.position = position;
            this.color = color;
            this.lightMatrixFunc = (t)=>position;
            this.lightDirectionFunc = (t) => direction;

            this.initialPosition = position;
            this.initialDirection = new Vector3(0,0,-1);
            angle = -1;
        }
        float myCos(Vector3 v1, Vector3 v2, bool cut = true)
        {
            float cos = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z / v1.Length() / v2.Length();
            if (cut && cos < 0.0) cos = 0.0f;
            if (float.IsNaN(cos)) return 0.0f;
            return cos;
        }
        public (bool, Vector3, float[]) isInRange(Vector3 point)
        {
            Vector3 L = position - point;
            L = Vector3.Normalize(L);
            float cos = Vector3.Dot(-L, direction);
            if (cos > 0)
            {
                cos = MathF.Pow(cos, angle);
                float[] res = new float[3];
                res[0] = cos * color.R / 255;
                res[1] = cos * color.G / 255;
                res[2] = cos * color.B / 255;
                return (true, L, res);
            }

            return (false, Vector3.Zero, new float[3]);
        }

        public void update(float time)
        {
            if (lightMatrixFunc != null) this.position = lightMatrixFunc(time);
            if (lightDirectionFunc != null) this.direction =  lightDirectionFunc(time);
            if (owner != null)
            {
                this.position = Vector3.Transform(this.position, owner.modelMatrix);
                this.direction = Vector3.TransformNormal(this.direction, owner.modelMatrix);
            }
        }
        public float[] getRGB()
        {
            return new float[] { color.R/255f, color.G/255f, color.B/255f };
        }
    }
}
