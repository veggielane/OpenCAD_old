using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OpenCAD.Core.Formats;
using OpenCAD.Core.Modeling.Octree;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Veg.Maths;
using Veg.Maths.Geometry;
using Veg.OpenTK;
using Veg.OpenTK.Buffers;
using Veg.OpenTK.Camera;
using Veg.OpenTK.Shaders;
using Veg.OpenTK.Vertices;

namespace OpenCAD.TestApp.Octree
{
    public class Window : GameWindow
    {
        private readonly OctreeNode<TestVoxel> _octree;

        private IShaderProgram _shader;
        private VAO _vao;
        private VAO _vao2;
        private VAO _vaoFilled;
        private CameraUBO _ubo;
        private readonly ICamera _camera;
        STL stl;
        private int maxLevel = 7;

        private Instancer _instancer;

        public Window(OctreeNode<TestVoxel> octree)
            : base(1280, 720, new GraphicsMode(32, 0, 0, 4), "OpenCAD")
        {
            _octree = octree;

            stl = new STL("elephant.stl", Color.Green, STLType.Binary);


            VSync = VSyncMode.On;

            _camera = new Camera();
            Mouse.WheelChanged += (sender, args) =>
                {
                    _camera.View = _camera.View * Mat4.Translate(0, 0, args.DeltaPrecise * -10.0);
                    //_camera.Eye += new Vect3(0, 0, args.DeltaPrecise * -10.0);
                    // Console.WriteLine(_camera.Eye);
                };
        }

        private VAOInst _vaoInst;

        protected override void OnLoad(EventArgs e)
        {
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.PointSize(5f);

            _ubo = new CameraUBO();

            _shader = new BasicShaderProgram(_ubo);
           // _instancer = new Instancer(GenCube(), GenInstances(), _ubo);

            

            //InstVBO vboist = new InstVBO(GenCube(), GenInstances());
            //_vaoInst = new VAOInst(_shader, vboist);

           // _vao = new VAO(_shader, new VBO(new List<Vertex>(FetchData(_octree))) { BeginMode = BeginMode.Quads });

            //var green = new Color4(0.156f, 0.627f, 0.353f, 1.0f).ToVector4();
            //var data = new List<Vertex>();

            //foreach (var element in stl.Elements)
            //{
            //    data.Add(new Vertex { Colour = green, Position = element.P1.ToVector3() });
            //    data.Add(new Vertex { Colour = green, Position = element.P2.ToVector3() });
            //    data.Add(new Vertex { Colour = green, Position = element.P3.ToVector3() });
            //}

            //_vao2 = new VAO(_shader, new VBO(data) { BeginMode = BeginMode.Triangles });



            var filled = _octree.Flatten().Where(o => o.State == NodeState.Filled).ToArray();


            _vaoFilled = new VAO(_shader, new VBO(new List<Vertex>(FetchDataSolid(filled))) { BeginMode = BeginMode.Quads });

            var err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at OnLoad: " + err);
        }

        private IEnumerable<Vertex> FetchData(OctreeNode<TestVoxel> node)
        {
            var green = new Color4(0.156f, 0.627f, 0.353f, 1.0f).ToVector4();
            var s = node.Size / 2.0;

            //+x
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };
            //-x
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
            //+y
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };
            //-y
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };
            //+z
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
            //-z
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
            yield return new Vertex { Colour = green, Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };


            if (node.Children != null)
            {
                foreach (var child in node.Children)
                {
                    foreach (var vertex in FetchData(child))
                    {
                        yield return vertex;

                    }
                }
            }

        }

        private IEnumerable<Vertex> FetchDataSolid(IEnumerable<OctreeNode<TestVoxel>> nodes)
        {

            foreach (var node in nodes)
            {
                var s = node.Size / 2.0;// -0.01;
                //+x
                yield return new Vertex { Colour = new Color4(Color.PaleVioletRed).ToVector4(), Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleVioletRed).ToVector4(), Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleVioletRed).ToVector4(), Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleVioletRed).ToVector4(), Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };
                //-x
                yield return new Vertex { Colour = new Color4(Color.PaleGreen).ToVector4(), Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGreen).ToVector4(), Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGreen).ToVector4(), Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGreen).ToVector4(), Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
                //+y
                yield return new Vertex { Colour = new Color4(Color.PaleTurquoise).ToVector4(), Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleTurquoise).ToVector4(), Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleTurquoise).ToVector4(), Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleTurquoise).ToVector4(), Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
                //-y                              
                yield return new Vertex { Colour = new Color4(Color.PaleGoldenrod).ToVector4(), Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGoldenrod).ToVector4(), Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGoldenrod).ToVector4(), Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.PaleGoldenrod).ToVector4(), Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };
                //+z                               
                yield return new Vertex { Colour = new Color4(Color.NavajoWhite).ToVector4(), Position = (node.Center + new Vect3(s, s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.NavajoWhite).ToVector4(), Position = (node.Center + new Vect3(-s, s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.NavajoWhite).ToVector4(), Position = (node.Center + new Vect3(-s, -s, s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.NavajoWhite).ToVector4(), Position = (node.Center + new Vect3(s, -s, s)).ToVector3() };
                //-z                               
                yield return new Vertex { Colour = new Color4(Color.Silver).ToVector4(), Position = (node.Center + new Vect3(s, -s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.Silver).ToVector4(), Position = (node.Center + new Vect3(-s, -s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.Silver).ToVector4(), Position = (node.Center + new Vect3(-s, s, -s)).ToVector3() };
                yield return new Vertex { Colour = new Color4(Color.Silver).ToVector4(), Position = (node.Center + new Vect3(s, s, -s)).ToVector3() };



            }


        }

        private IEnumerable<InstVertex> GenCube()
        {
            var s = 8;// -0.01;
            //+x
            yield return new InstVertex {Position = (new Vect3(s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, -s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(-s, s, -s)).ToVector3() };
            yield return new InstVertex {Position = (new Vect3(s, s, -s)).ToVector3() };
        }

        private IEnumerable<Instance> GenInstances()
        {
            yield return new Instance {Size = 3, Position = new Vector3(0,0,1), Colour = new Color4(Color.PaleGreen).ToVector4()};
        }



        public Bitmap GrabScreenshot()
        {
            if (GraphicsContext.CurrentContext == null)
                throw new GraphicsContextMissingException();

            Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            System.Drawing.Imaging.BitmapData data =
                bmp.LockBits(ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, ClientSize.Width, ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);
            bmp.UnlockBits(data);

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _camera.View *= Mat4.RotateZ(Angle.FromDegrees(0.6));
            _camera.View *= Mat4.RotateY(Angle.FromDegrees(0.4));
            _camera.View *= Mat4.RotateX(Angle.FromDegrees(0.15));
            _ubo.Update(_camera);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ClearColor(new Color4(0.137f, 0.121f, 0.125f, 0f));

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //_//vao.Render();

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            //_vao2.Render();
           _vaoFilled.Render();
           // _instancer.Render();
            //_vaoInst.Render();
            SwapBuffers();
            //GrabScreenshot().Save("test.png");
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at Swapbuffers: " + err.ToString());
            Title = String.Format(" FPS:{0} Mouse<{1},{2}>", 1.0 / e.Time, Mouse.X, Height - Mouse.Y);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _camera.Resize(Width, Height);
        }
    }


    public class InstanceShaderProgram : BaseShaderProgram
    {
        public InstanceShaderProgram(NewCameraUBO ubo)
        {
            CompileShader(ShaderType.VertexShader, @"#version 400
precision highp float; 

layout (location = 0) in vec3 vert_position;
layout (location = 1) in vec3 vert_normal;
layout (location = 2) in float instance_size;
layout (location = 3) in vec3 instance_position;
layout (location = 4) in vec4 instance_color;

layout(std140) uniform Camera {
    mat4 MVP;
};


out vec4 col;

void main(void) 
{ 
    gl_Position = (MVP) * vec4(instance_position + vert_position, 1); 
    col = instance_color;
}");
            CompileShader(ShaderType.FragmentShader, @"#version 400
in vec4 col;
layout( location = 0 ) out vec4 FragColor;
void main() {
    FragColor = col;
}");

            Link();
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
            GL.EnableVertexAttribArray(4);


            GL.Arb.VertexAttribDivisor(2, 1);//size
            GL.Arb.VertexAttribDivisor(3, 1);//position
            GL.Arb.VertexAttribDivisor(4, 1);//color

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            ubo.BindToShaderProgram(this);
        }
    }

    public class VAOInst : IBuffer
    {
        private readonly IShaderProgram _program;
        private readonly int _handle;
        public int Handle
        {
            get { return _handle; }
        }

        public InstVBO VBO { get; private set; }

        private int _stride;
        private int _offset;

        public VAOInst(IShaderProgram program, InstVBO vbo)
        {
            _program = program;
            VBO = vbo;
            GL.GenVertexArrays(1, out _handle);
            using (new Bind(program))
            using (new Bind(this))
            using (new Bind(vbo))
            {




                _stride = 8 * sizeof (float);
                _offset = vbo.Data.Count() * InstVertex.SizeInBytes;

                GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, _stride, _offset);
                GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, _stride, _offset + 1 * sizeof(float));
                GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, _stride, _offset + 4 * sizeof(float));

            }
        }

        public void Bind()
        {
            _program.Bind();
            GL.BindVertexArray(Handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
            _program.UnBind();
        }

        public void Render()
        {
            using (new Bind(this))
            {
                //GL.DrawArrays(VBO.BeginMode, 0, VBO.Count);
                GL.DrawArraysInstanced(VBO.BeginMode, 0, VBO.Data.Count()/4, VBO.Instances.Count());
            }
        }
    }

    public class InstVBO : IBuffer
    {
        public IEnumerable<Instance> Instances { get; set; }
        private int _handle;
        public int Handle
        {
            get { return _handle; }
            private set { _handle = value; }
        }

        public int Count { get; private set; }

        public BeginMode BeginMode { get; set; }

        public readonly IEnumerable<InstVertex> Data;
        public InstVBO()
        {
            GL.GenBuffers(1, out _handle);
            Count = 0;
            BeginMode = BeginMode.Points;
        }

        public InstVBO(IEnumerable<InstVertex> data, IEnumerable<Instance> instances)
            : this()
        {
            Instances = instances;
            Data = data;
            Buffer(Data.ToArray(), Instances.ToArray());
        }

        private void Buffer(InstVertex[] data, Instance[] instances)
        {
            using (new Bind(this))
            {
                var d = Gen(data, instances).ToArray();
                Count = data.Length;
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(data.Length * InstVertex.SizeInBytes + instances.Length * Instance.SizeInBytes),d , BufferUsageHint.DynamicDraw);
            }
        }

        private IEnumerable<float> Gen(InstVertex[] data, Instance[] instances)
        {
            foreach (var d in data)
            {
                yield return d.Position.X;
                yield return d.Position.Y;
                yield return d.Position.Z;
                yield return d.Normal.X;
                yield return d.Normal.Y;
                yield return d.Normal.Z;
            }
            foreach (var inst in instances)
            {
                yield return inst.Size;
                yield return inst.Position.X;
                yield return inst.Position.Y;
                yield return inst.Position.Z;
                yield return (float)MathsHelper.Map(inst.Colour.X, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.Y, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.Z, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.W, 0, 255, 0, 1);
            }
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        }
        public void UnBind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }
    }
    public struct InstVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public static int SizeInBytes { get { return Vector3.SizeInBytes*2; } }
    }
    public struct Instance
    {
        public float Size;
        public Vector3 Position;
        public Vector4 Colour;
        public static int SizeInBytes { get { return Vector3.SizeInBytes + Vector4.SizeInBytes + sizeof(float); } }
    }

    public class NewCameraUBO : BaseUBO<NewCameraUBO.CameraData>
    {

        public struct CameraData
        {
            public Matrix4 MVP;
 
        }

        public NewCameraUBO()
            : base("Camera", 5)
        {

        }

        public void Update(ICamera camera)
        {
            var normal = (camera.Model * camera.View).ToMatrix4();
            normal.Invert();
            normal.Transpose();
            Data = new CameraData
            {
                MVP = camera.MVP.ToMatrix4(),
            };
            Update();
        }
    }

    public class Instancer
    {
        private int _shaderHandle;
        int square_vao, square_vbo;

        public Instancer(IEnumerable<InstVertex> data, IEnumerable<Instance> instances, NewCameraUBO ubo)
        {
            _shaderHandle = GL.CreateProgram();
            CompileShader(ShaderType.VertexShader, @"#version 400
precision highp float; 

layout (location = 0) in vec3 vert_position;
layout (location = 1) in vec3 vert_normal;
layout (location = 2) in float instance_size;
layout (location = 3) in vec3 instance_position;
layout (location = 4) in vec4 instance_color;

layout(std140) uniform Camera {
    mat4 MVP;
};


out vec4 col;

void main(void) 
{ 
    gl_Position = (MVP) * vec4(instance_position + vert_position, 1); 
    col = instance_color;
}");
            CompileShader(ShaderType.FragmentShader, @"#version 400
in vec4 col;
layout( location = 0 ) out vec4 FragColor;
void main() {
    FragColor = col;
}");

            GL.BindAttribLocation(_shaderHandle, 0, "vert_position");
            GL.BindAttribLocation(_shaderHandle, 1, "vert_normal");
            GL.BindAttribLocation(_shaderHandle, 2, "instance_size");
            GL.BindAttribLocation(_shaderHandle, 3, "instance_position");
            GL.BindAttribLocation(_shaderHandle, 4, "instance_color");

            GL.LinkProgram(_shaderHandle);
            int linkResult;
            GL.GetProgram(_shaderHandle, ProgramParameter.LinkStatus, out linkResult);
            if (linkResult != 1)
            {
                string info;
                GL.GetProgramInfoLog(_shaderHandle, out info);
                Console.WriteLine(info);
            }
            Console.WriteLine("linked");
            ErrorCode err = GL.GetError();
            if (err != ErrorCode.NoError)
                Console.WriteLine("Error at Shader: " + err);

            GL.UniformBlockBinding(_shaderHandle, GL.GetUniformBlockIndex(_shaderHandle, "Camera"), 0);

            

            GL.GenVertexArrays(1, out square_vao);
            GL.GenBuffers(1, out square_vbo);
            GL.BindVertexArray(square_vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, square_vbo);

            var d = Gen(data.ToArray(), instances.ToArray()).ToArray();

            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(d.Count() * sizeof(float)), d, BufferUsageHint.DynamicDraw);


            var _stride = 8 * sizeof(float);
            var _offset = data.Count() * InstVertex.SizeInBytes;

            GL.VertexAttribPointer(2, 1, VertexAttribPointerType.Float, false, _stride, _offset);
            GL.VertexAttribPointer(3, 3, VertexAttribPointerType.Float, false, _stride, _offset + 1 * sizeof(float));
            GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, _stride, _offset + 4 * sizeof(float));



            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.EnableVertexAttribArray(3);
            GL.EnableVertexAttribArray(4);


            GL.Arb.VertexAttribDivisor(2, 1);//size
            GL.Arb.VertexAttribDivisor(3, 1);//position
            GL.Arb.VertexAttribDivisor(4, 1);//color



        }




        private IEnumerable<float> Gen(InstVertex[] data, Instance[] instances)
        {
            foreach (var d in data)
            {
                yield return d.Position.X;
                yield return d.Position.Y;
                yield return d.Position.Z;
                yield return d.Normal.X;
                yield return d.Normal.Y;
                yield return d.Normal.Z;
            }
            foreach (var inst in instances)
            {
                yield return inst.Size;
                yield return inst.Position.X;
                yield return inst.Position.Y;
                yield return inst.Position.Z;
                yield return (float)MathsHelper.Map(inst.Colour.X, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.Y, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.Z, 0, 255, 0, 1);
                yield return (float)MathsHelper.Map(inst.Colour.W, 0, 255, 0, 1);
            }
        }



        public void CompileShader(ShaderType type, string source)
        {
            int shaderHandle = GL.CreateShader(type);
            GL.ShaderSource(shaderHandle, source);
            GL.CompileShader(shaderHandle);
            var log = GL.GetShaderInfoLog(shaderHandle);
            int compileResult;
            GL.GetShader(shaderHandle, ShaderParameter.CompileStatus, out compileResult);
            if (compileResult != 1)
            {
                Console.WriteLine(log);
                Console.WriteLine("Compile Error:" + type);
            }
            GL.AttachShader(_shaderHandle, shaderHandle);
        }


        public void Render()
        {
            GL.BindVertexArray(square_vao);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArraysInstanced(BeginMode.Quads, 0, 4 * 6, 1);
        }
    }
}