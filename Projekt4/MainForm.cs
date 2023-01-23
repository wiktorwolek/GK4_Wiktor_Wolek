using FastBitmapLib;
using System.Numerics;
using static System.Reflection.Metadata.BlobBuilder;

namespace Projekt4
{
    public partial class MainForm : Form
    {
        const float virtualX = 1.0f;
        const float virtualY = 1.0f;
        int coloringMode = 1;
        List<Obj> objs = new List<Obj>();
        float fov = 60.0f;
        Obj train;
        List<Camera> cameras = new List<Camera>();
        int curCamera = 0;
        Matrix4x4 view = Matrix4x4.Identity;
        Matrix4x4 perspective = Matrix4x4.Identity;
        private bool isRunning = true;
        int speed = 10;
        Color fogColor = Color.White;
        float ia =1f;
        bool enableFog = false;
        Obj character;
        List<LightSource> lights = new List<LightSource>(); 
        float time = 0.0f;
        bool day = true;
        float fogMax = 1.5f;
        float fogMin = 1.4f;
        float charangle = 0f;
        float mstep = 0.1f;
        float angleStep = 0.1f;

        public MainForm()
        {
            InitializeComponent();
            cameras.Add(new Camera((t) => { return new Vector3(30f, 10f, 20f); }, (t) => { return Vector3.Zero; }));
            cameras.Add(new Camera((t) => {
                float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                return new Vector3(10f, 10, 20f); // return Vector3.Transform(new Vector3(10f, 10, 20f), Matrix4x4.CreateTranslation(new Vector3(x, 0, 0)));  
            },
                    (t) => { float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                        return Vector3.Transform(new Vector3(0f, 0, 2f), Matrix4x4.CreateTranslation(new Vector3(x, 0, 0)));
                    }));
            cameras.Add(new Camera((t) => {
                float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                return Vector3.Transform(new Vector3(20f, 0, 20f), Matrix4x4.CreateTranslation(new Vector3(x, 0, 0)));
            },
                   (t) => {
                       float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                       return Vector3.Transform(new Vector3(-10, 0, 5f), Matrix4x4.CreateTranslation(new Vector3(x, 0, 0)));
                   }));
             lights.Add(new LightSource(new Vector3(0, 0, 100), Color.White,
                 (t) => { return new Vector3(0, 0, 100); },
                 (t) => { return new Vector3(0, 0, -1); },
                 -1,
                 new Vector3(0, 0, -1), 0.5f, 0.5f));
            var lamp = new LightSource(new Vector3(0, 0, 0), Color.Yellow,
               (t) => new Vector3(0f, 0f, 0f),
               (t) => new Vector3(0f, 3f, -1f),

             2f,
                new Vector3(0, 0, 0), 1f, 1f);
            lights.Add(lamp);
            lights.Add(new LightSource(new Vector3(0, 0, 0), Color.White,
               /* (t) => {
                    float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                    return Vector3.Transform(new Vector3(0, 0, 2f), Matrix4x4.Identity * Matrix4x4.CreateTranslation(x, 0, 0f));
                    },
                (t) =>{
                    float x = -46 * Math.Abs((float)Math.Sin(t / speed));
                   return Vector3.Transform(new Vector3(-1, 0, 0), Matrix4x4.Identity * Matrix4x4.CreateTranslation(x, 0, 0f));
                    }*/
               (t) => new Vector3(0f, 0f, -2f),
               (t) => new Vector3(0f, 0f, -3f),

             4f,
                new Vector3(0, 0, 0), 1, 1));
           
            regenerateViewAndPerspective();

            
             objs.Add(new Obj(@"ground.obj", Color.FromArgb(255,63,155,11), (t) =>
            {
                return Matrix4x4.CreateScale(4)*Matrix4x4.CreateTranslation(new Vector3(-23,0,-3));
            },new float[] {0.2f,0.2f,0.2f},new float[] {1f,1f,1f}, new float[] {0.5f,0.5f,0.5f}, new float[] {4,4,4}));
            objs.Add(new Obj(@"trackBottom.obj", Color.Brown, (t) =>
            {
                return Matrix4x4.CreateRotationX((float)Math.PI / 2) * Matrix4x4.CreateTranslation(new Vector3(-23, 0, 0)); ;
            }));
            objs.Add(new Obj(@"trackTop.obj", Color.Silver, (t) =>
            {
                return Matrix4x4.CreateRotationX((float)Math.PI / 2) * Matrix4x4.CreateTranslation(new Vector3(-23, 0, 0)); ;
            }));
            character = new Obj(@"mysz.obj", Color.WhiteSmoke,
                (t) => Matrix4x4.CreateTranslation(new Vector3(0, 0, 4)));
            lamp.owner = character;
            character.modelMatrix = Matrix4x4.CreateTranslation(new Vector3(0, 0, 4));
            objs.Add(character);
            objs.Add(new Obj(@"train.obj", Color.Gray, (t) =>
            {
                float x = -46f * Math.Abs((float)Math.Sin(t / speed));
                return Matrix4x4.CreateRotationX((float)Math.PI / 2)* Matrix4x4.CreateRotationZ((float)Math.PI / 2)*Matrix4x4.CreateTranslation(new Vector3(0,0,2))*Matrix4x4.CreateTranslation(new Vector3(x,0,0));
            }));
            lights.Last().owner = objs.Last();
            train = objs.Last();
            Render();

            timer.Interval = 100;
            timer.Start();

        }
        
        private void regenerateViewAndPerspective()
        {
            view = Matrix4x4.CreateLookAt(getCameraPos(), cameras[curCamera].target, Vector3.UnitZ);
            perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov / 180.0f * MathF.PI, ((float)canva.Width) / canva.Height, 1.0f, 3.0f);
        }
        (Vector4 max,Vector4 min) minmaxPoint(Obj obj)
        {
            Vector4 max = new Vector4(0, 0, float.MinValue,0), min = new Vector4(0,0,float.MaxValue,0);
            foreach(var p in obj.points)
            {
                if (p.Z > max.Z)
                    max = p;
                if(p.Z < min.Z)
                    min = p;
            }
            return (max,min);
        }

        void Render()
        {
            Bitmap nextBitmap = new Bitmap(canva.Width, canva.Height);
            int canvaWidth = canva.Width;
            int canvaHeight = canva.Height;
            int[] tmp = new int[canvaWidth * canvaHeight];
            float[] zBuffer = Enumerable.Repeat(float.MaxValue, canvaWidth * canvaHeight).ToArray();


            Parallel.ForEach(objs, (obj) =>
            {
                switch (coloringMode)
                {
                    case 0:
                        drawObjConst(obj, tmp, canvaWidth, zBuffer);
                        break;
                    case 1:
                        drawObjGouraud(obj, tmp, canvaWidth, zBuffer);
                        break;
                    case 2:
                        drawObjPhong(obj, tmp, canvaWidth, zBuffer);
                        break;
                    
                }
            });
            drawFog(tmp, zBuffer);
            using (var fastBitmap = nextBitmap.FastLock())
            {
                if (day) fastBitmap.Clear(Color.SkyBlue);
                else fastBitmap.Clear(Color.Black);
                fastBitmap.CopyFromArray(tmp,true);
            }

            using (Graphics g = Graphics.FromImage(nextBitmap))
            {
                drawInfo(g);
            }

            canva.Image?.Dispose();
            canva.Image = (Image)nextBitmap;
        }
        private void drawFog(int[] tmp, float[] zBuffer)
        {
            int backColor = day ? Color.White.ToArgb() : Color.Black.ToArgb();
            for (int i = 0; i < zBuffer.Length; i++)
            {
                if (zBuffer[i] < fogMax && zBuffer[i] > fogMin)
                {
                    float alpha = (zBuffer[i] - fogMin) / (fogMax - fogMin);
                    tmp[i] = blend(tmp[i], backColor, alpha);
                }
            }

            int blend(int color, int backColor, float amount)
            {
                int r = ((int)(((color & 0xFF0000) >> 16) * (1 - amount) + ((backColor & 0xFF0000) >> 16) * amount)) << 16;
                int g = ((int)(((color & 0xFF00) >> 8) * (1 - amount) + ((backColor & 0xFF00) >> 8) * amount)) << 8;
                int b = (int)((color & 0xFF) * (1 - amount) + (backColor & 0xFF) * amount);
                return (-16777216 | r | g | b);
            }
        }
        private void drawInfo(Graphics g)
        {
            string s = $"Camera pos:\nX:{String.Format("{0:0.00}", getCameraPos().X)}\nY:{String.Format("{0:0.00}", getCameraPos().Y)}\nZ:{String.Format("{0:0.00}", getCameraPos().Z)}";
            g.DrawString(s, new Font("Arial", 8), new SolidBrush(Color.White), canva.Width - 90, 30);
        }


        void drawObj(Obj obj, int[] tmp, int canvaWidth, float[] zBuffer)
        {
            

            Pen p = new Pen(Color.White);
            Parallel.ForEach(obj.faces, (face) =>
            {
                Vector3[] points = new Vector3[face.Item1.Length];
                bool isOK = true;
                for (int i = 0; i < face.Item1.Length; i++)
                {
                    points[i] = projectPoint(obj.points[face.Item1[i]], obj.modelMatrix);
                    if (!IsIn(points[i]))
                    {
                        isOK = false;
                        break;
                    }
                }

                Color[] colors = { Color.Magenta, Color.Cyan, Color.Yellow };
                Vector3[] normalVectors = new Vector3[3];
                for (int i = 0; i < 3; i++)
                {
                    normalVectors[i] = Vector3.TransformNormal(obj.normalVector[face.Item2[i]], obj.modelMatrix);
                    // colors[i] = calcColor(obj.color, new Vector3(obj.points[face.Item1[i]].X, obj.points[face.Item1[i]].Y, obj.points[face.Item1[i]].Z), normalVectors[i], obj);
                    colors[i] = calcColor(obj.color, new Vector3(obj.points[face.Item1[i]].X, obj.points[face.Item1[i]].Y, obj.points[face.Item1[i]].Z), normalVectors[i], obj);
                }




                if (isOK) ForEachPixel(points, (vec, x, y) =>
                {
                    Vector3 point = getPoint(vec, x, y);
                    if (point.Z < zBuffer[x + y * canvaWidth])
                    {
                        zBuffer[x + y * canvaWidth] = point.Z;
                        int color = 0;
                        switch (coloringMode)
                        {
                            case 0:
                                color = obj.color.ToArgb();
                                break;
                            case 1:
                                color = interpolateColor(vec, colors, point);
                                break;
                            case 2:
                                color = interpolateNormaVector(vec, normalVectors, point, obj.color, obj);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        tmp[x + y * canvaWidth] = color;
                    }
                });
            });
        }
        void drawObjGouraud(Obj obj, int[] tmp, int canvaWidth, float[] zBuffer)
        {
            Parallel.ForEach(obj.faces, (face) =>
            {
                Vector3[] points = new Vector3[face.Item1.Length];
                bool isOK = true;
                for (int i = 0; i < face.Item1.Length; i++)
                {
                    points[i] = projectPoint(obj.points[face.Item1[i]], obj.modelMatrix);
                    if (!IsIn(points[i]))
                    {
                        isOK = false;
                        break;
                    }
                }

                Color[] colors = new Color[3];
                Vector3[] normalVectors = new Vector3[3];
                for (int i = 0; i < 3; i++)
                {
                    normalVectors[i] = Vector3.TransformNormal(obj.normalVector[face.Item2[i]], obj.modelMatrix);
                    colors[i] = calcColor(obj.color, toVec3(Vector4.Transform(obj.points[face.Item1[i]], obj.modelMatrix)), normalVectors[i],obj);
                }

                if (isOK) ForEachPixel(points, (vec, x, y) =>
                {
                    Vector3 point = getPoint(vec, x, y);
                    if (point.Z < zBuffer[x + y * canvaWidth])
                    {
                        zBuffer[x + y * canvaWidth] = point.Z;
                        tmp[x + y * canvaWidth] = interpolateColor(vec, colors, point);
                    }
                });
            });
        }
        public  Vector3 toVec3( Vector4 v)
        {
            return new Vector3(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }
        void drawObjPhong(Obj obj, int[] tmp, int canvaWidth, float[] zBuffer)
        {
            Parallel.ForEach(obj.faces, (face) =>
            {
                Vector3[] points = new Vector3[face.Item1.Length];
                bool isOK = true;
                for (int i = 0; i < face.Item1.Length; i++)
                {
                    points[i] = projectPoint(obj.points[face.Item1[i]], obj.modelMatrix);
                    if (!IsIn(points[i]))
                    {
                        isOK = false;
                        break;
                    }
                }


                Vector4[] rotatedPoints = face.Item1
                .Select((i) => obj.points[i])
                .Select((v) => Vector4.Transform(v, obj.modelMatrix))
                .ToArray();

                Vector3[] normals = face.Item2
                .Select((i) => obj.normalVector[i])
                .Select((v) => Vector3.TransformNormal(v, obj.modelMatrix))
                .ToArray();

                if (isOK) ForEachPixel(points, (vec, x, y) =>
                {
                    Console.WriteLine(points);
                    Vector3 point = getPoint(vec, x, y);
                    if (point.Z < zBuffer[x + y * canvaWidth])
                    {
                        zBuffer[x + y * canvaWidth] = point.Z;
                        Vector3 localNormal = interpolateVector(vec, normals, point);
                        Vector3 localPoint = interpolateVector(vec, rotatedPoints.Select(v => toVec3(v)).ToArray(), point);
                        tmp[x + y * canvaWidth] = calcColor(obj.color, localPoint, localNormal,obj).ToArgb();
                    }
                });
            });
        }
        private Vector3 interpolateVector(Vector3[] face, Vector3[] vectors, Vector3 point)
        {
            float[] weights = barocentricWeigths(face, point);

            return weights[0] * vectors[0] + weights[1] * vectors[1] + weights[2] * vectors[2];
        }

        void drawObjConst(Obj obj, int[] tmp, int canvaWidth, float[] zBuffer)
        {
            Parallel.ForEach(obj.faces, (face) =>
            {
                Vector3[] points = new Vector3[face.Item1.Length];
                bool isOK = true;
                for (int i = 0; i < face.Item1.Length; i++)
                {
                    points[i] = projectPoint(obj.points[face.Item1[i]], obj.modelMatrix);
                    if (!IsIn(points[i]))
                    {
                        isOK = false;
                        break;
                    }
                }
                Vector3[] realPoints = face.Item1
                .Select((i) => toVec3(obj.points[i]))
                .ToArray();
                Vector3 centralpoint = centralPoint(realPoints);
                Vector3[] normals = face.Item2
                .Select((i) => obj.normalVector[i])
                .Select((v) => Vector3.TransformNormal(v, obj.modelMatrix))
                .ToArray();

                Vector3 normalInCenter = interpolateVector(realPoints, normals, centralpoint);

                int constColor = calcColor(obj.color, Vector3.Transform(centralpoint, obj.modelMatrix), normalInCenter,obj).ToArgb();

                if (isOK) ForEachPixel(points, (vec, x, y) =>
                {
                    Vector3 point = getPoint(vec, x, y);
                    if (point.Z < zBuffer[x + y * canvaWidth])
                    {
                        zBuffer[x + y * canvaWidth] = point.Z;
                        tmp[x + y * canvaWidth] = constColor;
                    }
                });
            });
        }
        private Vector3 centralPoint(Vector3[] points)
        {
            Vector3 avg = Vector3.Zero;
            foreach (var item in points)
            {
                avg += item;
            }
            return avg / points.Length;
        }
        float myCos(Vector3 v1, Vector3 v2, bool cut = true)
        {
            float cos = v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z / v1.Length() / v2.Length();
            if (cut && cos < 0.0) cos = 0.0f;
            if (float.IsNaN(cos)) return 0.0f;
            return cos;
        }
        private Color calcColor(Color c,Vector3 point, Vector3 normalVector,Obj obj)
        {
            float[] objColor = { c.R, c.G, c.B };
            int[] rgb = new int[3];
            for (int i = 0; i < 3; i++)
            {
            rgb[i] = (int)(obj.ka[i]*ia * objColor[i]);
            foreach (var light in lights)
            { 
                Vector3 N = Vector3.Normalize(normalVector);
                (bool ok, Vector3 L, float[]lcol) = light.isInRange(point);
                if (!ok) continue;
                Vector3 R = 2 * myCos(N, L, false) * N - L;
                    float cosTheta = myCos(N, L, true);
                float first = obj.kd[i] * cosTheta * light.iD;
                Vector3 V = Vector3.Normalize(getCameraPos() - point);
                float second = obj.ks[i] * MathF.Pow(myCos(V,R, true), obj.m[i])*light.iS;
                float[] lightRgb = light.getRGB();
               
                    rgb[i] += (int)((first + second) * objColor[i] * lcol[i]);
                    if (rgb[i] < 0) 
                        rgb[i] = 0;
                    if (rgb[i] > 255) 
                        rgb[i] = 255;
                }
            }
            var z = Vector3.Distance(point, getCameraPos());
            Color res = Color.FromArgb(rgb[0], rgb[1], rgb[2]);
            return res;
        }
        private Vector3 getCameraPos()
        {
            return cameras[curCamera].position;
        }
        private float[] barocentricWeigths(Vector3[] f, Vector3 point)
        {
            Vector3 A = f[1] - f[0];
            Vector3 B = f[2] - f[0];
            Vector3 C = f[2] - f[1];
            Vector3 D = f[0] - f[2];

            float P = (A * B).Length();
            float P0 = ((point - f[1]) * C).Length();
            float P1 = ((point - f[2]) * C).Length();
            float P2 = ((point - f[0]) * C).Length();
            float[] res = new float[] { P0 / P, P1 / P, P2 / P };
            float sum = res[0] + res[1] + res[2];
            return res.Select(i => i / sum).ToArray();
        }

        private int interpolateColor(Vector3[] face, Color[] colors, Vector3 point)
        {
            float[] weights = barocentricWeigths(face, point);
            int[] rgb = new int[3];
            for (int i = 0; i < 3; i++)
            {
                rgb[0] += (int)(weights[i] * colors[i].R);
                rgb[1] += (int)(weights[i] * colors[i].G);
                rgb[2] += (int)(weights[i] * colors[i].B);
            }
            return Color.FromArgb(rgb[0], rgb[1], rgb[2]).ToArgb();
        }

        private int interpolateNormaVector(Vector3[] face, Vector3[] normals, Vector3 point,Color objColor,Obj obj)
        {
            float[] weights = barocentricWeigths(face, point);
              Vector3 N = new Vector3(0, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                N.X += (float)(weights[i] * normals[i].X);
                N.Y += (float)(weights[i] * normals[i].Y);
                N.Z += (float)(weights[i] * normals[i].Z);
            }
            return calcColor(objColor, point, N,obj).ToArgb();
        }
        private Vector3 getPoint(Vector3[] face, int x, int y)
        {
            float det = (face[1].Y - face[2].Y) * (face[0].X - face[2].X) + (face[2].X - face[1].X) * (face[0].Y - face[2].Y);
            float l1 = ((face[1].Y - face[2].Y) * (x - face[2].X) + (face[2].X - face[1].X) * (y - face[2].Y)) / det;
            float l2 = ((face[2].Y - face[0].Y) * (x - face[2].X) + (face[0].X - face[2].X) * (y - face[2].Y)) / det;
            float l3 = 1.0f - l1 - l2;
            float res = (l1 * face[0].Z + l2 * face[1].Z + l3 * face[2].Z);
            return new Vector3(x, y,res);
        }

        private bool IsIn(Vector3 vec)
        {
            return vec.X >= 0 && vec.Y >= 0 && vec.X < canva.Width && vec.Y < canva.Height;
        }

        Vector3 projectPoint(Vector4 vec, Matrix4x4 modelMatrix)
        {
            Vector4 vecCanva = Vector4.Transform(
                Vector4.Transform(
                    Vector4.Transform(vec, modelMatrix),
                    view),
                perspective);
            return virtualToCanva(projection(vecCanva));
        }


        Vector3 virtualToCanva(Vector3 virtualPos)
        {
            float X = virtualPos.X;
            float Y = virtualPos.Y;

            float newX = 0.5f * canva.Width * (1.0f + X / virtualX);
            float newY = 0.5f * canva.Height * (1.0f - Y / virtualY);
            return new Vector3(newX, newY,virtualPos.Z);
        }

        Vector2 canvaToVirtual(Vector2 canvaPos)
        {
            throw new NotImplementedException();
        }

        Vector3 projection(Vector4 vec)
        {
            return new Vector3(vec.X / vec.W, vec.Y / vec.W,vec.Z/vec.W);
        }

        private void canva_Resize(object sender, EventArgs e)
        {
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov / 180.0f * MathF.PI, ((float)canva.Width)/ canva.Height, 1.0f, 3.0f);
            Render();
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            const float step = 0.1f;
            switch (e.KeyChar)
            {
               
                case 'z':
                    fov -= 10.0f;
                    if (fov < 10.0f) fov = 10.0f;
                    break;
                case 'x':
                    fov += 10.0f;
                    if (fov > 170.0f) fov = 170.0f;
                    break;
                case 'p':
                    isRunning = !isRunning;
                    if (isRunning) timer.Start();
                    else timer.Stop();
                    break;
                case 'c':
                    curCamera++;
                    curCamera = curCamera % cameras.Count;
                    regenerateViewAndPerspective();
                    break;
                case 'w':
                    character.modelMatrix *= Matrix4x4.CreateRotationZ(-charangle) * Matrix4x4.CreateTranslation(0f, mstep, 0f) * Matrix4x4.CreateRotationZ(charangle);
                    break;
                case 's':
                    character.modelMatrix *= Matrix4x4.CreateRotationZ(-charangle) * Matrix4x4.CreateTranslation(0f, -mstep, 0f) * Matrix4x4.CreateRotationZ(charangle);
                    break;
                case 'a':
                    character.modelMatrix *= Matrix4x4.CreateRotationZ(-charangle) * Matrix4x4.CreateTranslation(-mstep, 0f, 0f) * Matrix4x4.CreateRotationZ(charangle);
                    break;
                case 'd':
                    character.modelMatrix *= Matrix4x4.CreateRotationZ(-charangle) * Matrix4x4.CreateTranslation(mstep, 0f, 0f) * Matrix4x4.CreateRotationZ(charangle);
                    break;
                case 'q':
                    charangle += angleStep;
                    character.modelMatrix = Matrix4x4.CreateRotationZ(angleStep)* character.modelMatrix;
                    break;
                case 'e':
                    charangle -= angleStep;
                    character.modelMatrix = Matrix4x4.CreateRotationZ(-angleStep) * character.modelMatrix;
                    break;
            }
            perspective = Matrix4x4.CreatePerspectiveFieldOfView(fov / 180.0f * MathF.PI, ((float)canva.Width) / canva.Height, 1.0f, 3.0f);
            Render();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (var obj in objs)
            {
                if(obj!=character)
                    obj.modelMatrix = obj.modelMatrixFunc(time);
            }
            foreach(var light in lights)
            {
                light.update(time);
            }
            foreach (var camera in cameras)
            {
                camera.position = camera.camerFunc(time);
                camera.target = camera.camerDirFunc(time);
            }
            if (enableFog) { 
                if(fogMin==0)
                    enableFog = false;
                fogMin -= 0.001f;}
            else
                fogMin = fogMax;
            regenerateViewAndPerspective();
            time += timer.Interval / 1000.0f;

            Render();
        }
       
        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var d = new OpenFileDialog();
            d.Filter = "Obj Files|*.obj";
            if (d.ShowDialog() == DialogResult.OK)
            {
                var obj = new Obj(d.FileName);
                objs.Add(obj);
                Render();
            }
        }

        class AET
        {
            public int y_max;
            public double x;
            public double jedenprzezm;

            public AET(int y_max, double x, double jedenprzezm)
            {
                this.y_max = y_max;
                this.x = x;
                this.jedenprzezm = jedenprzezm;
            }
        }

        void ForEachPixel(Vector3[] vectorArr, Action<Vector3[], int, int> action)
        {
            Vector3[] v2 = (Vector3[])vectorArr.Clone();
            Array.Sort(v2, (x, y) => (int)(x.Y - y.Y));
            Queue<Vector3> et = new Queue<Vector3>(v2);
            List<AET> aet = new List<AET>();
            int y = (int) et.First().Y;

            while (aet.Count != 0 || et.Count != 0)
            {
                while (et.Count > 0 && et.First().Y < y)
                {
                    Vector3 vertex = et.Dequeue();
                    int index = Array.IndexOf(vectorArr, vertex);
                    int before = index == 0 ? vectorArr.Length - 1 : index - 1;
                    int after = (index + 1) % vectorArr.Length;
                    if (vectorArr[before].Y >= y)
                    {
                        aet.Add(
                            new AET(
                                (int)vectorArr[before].Y,
                                vertex.X,
                                ((double)(vectorArr[before].X - vertex.X)) / (vectorArr[before].Y - vertex.Y)
                                )
                            );
                    }
                    if (vectorArr[after].Y >= y)
                    {
                        aet.Add(
                            new AET(
                                (int)vectorArr[after].Y,
                                vertex.X,
                                ((double)(vectorArr[after].X - vertex.X)) / (vectorArr[after].Y - vertex.Y)
                                )
                            );
                    }
                }

                if (aet.Count > 0)
                {
                    aet.Sort((x, y) => (int)(x.x - y.x));
                    bool paint = true;
                    int[] changePoints = (int[])aet.Select(x => (int)x.x).Distinct().ToArray();
                    int x = changePoints[0];
                    foreach (var change in changePoints)
                    {
                        int ago = 0;
                        while (x < change)
                        {
                            ago++;
                            if (paint) action(vectorArr, x, y);
                            x++;
                        }
                        if (ago > 1) paint = !paint;
                    }
                    aet.RemoveAll(x => x.y_max <= y);
                    for (int i = 0; i < aet.Count; i++)
                    {
                        aet[i].x += aet[i].jedenprzezm;
                    }
                }
                y++;
            }
        }

        private void solidColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coloringMode = 0;
        }

        private void colorInterpolationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coloringMode = 1;
        }

        private void normalsInterpolationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            coloringMode = 2;
        }

        private void enableDisableFogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            enableFog = !enableFog;
        }

        private void dayNightToolStripMenuItem_Click(object sender, EventArgs e)
        {

            day = !day;
            lights.First().color = day?Color.White:Color.Black;
        }
    }
}