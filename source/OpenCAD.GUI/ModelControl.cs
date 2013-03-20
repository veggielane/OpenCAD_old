using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenCAD.Core.Graphics;
using OpenCAD.Core.Maths;
using OpenCAD.Core.Modeling;
using OpenCAD.Core.Modeling.Datums;
using OpenCAD.Core.Topology;
using OpenCAD.GUI.Buffers;
using OpenCAD.GUI.SceneGraph;
using SharpGL;
using SharpGL.Enumerations;

namespace OpenCAD.GUI
{
    public class ModelControl:CADControl
    {


        private int _modelUniform;
        private int _viewUniform;
        private int _projectionUniform;


        private ISceneGraph _graph;
        private IShaderProgram _shader;
        private OrthographicCamera _camera;

        private readonly IModel _model;

        public ModelControl()
        {
            _model = new TestPart("test part");
            MouseWheel += (s, e) =>
                {
                    using (new Bind(_shader))
                    {
                        _camera.Scale += e.Delta/(120.0 * 2);
                    }
                };
           // MouseDown += ContextMouseDown;
           // MouseMove += ModelControlMouseMove;
        }

        public override void OnLoad(OpenGL gl)
        {
            gl.Enable(OpenGL.GL_CULL_FACE);
            gl.Enable(OpenGL.GL_DEPTH_TEST);
            gl.Enable(OpenGL.GL_BLEND);
            gl.BlendFunc(BlendingSourceFactor.SourceAlpha, BlendingDestinationFactor.OneMinusSourceAlpha);

            _camera = new OrthographicCamera();
            _camera.View *= Mat4.RotateY(Angle.FromDegrees(15));

            _shader = new BasicShader(gl);
            _modelUniform = gl.GetUniformLocation(_shader.Handle, "Model");
            _viewUniform = gl.GetUniformLocation(_shader.Handle, "View");
            _projectionUniform = gl.GetUniformLocation(_shader.Handle, "Projection");

            _graph = new OpenGLSceneGraph();
            _graph.Nodes.Add(new CoordinateSystemLeaf(gl, _shader, _model.Features.OfType<CoordinateSystem>()));
            _graph.Nodes.Add(new DatumPlaneLeaf(gl, _shader, _model.Features.OfType<DatumPlane>()));
            _graph.Nodes.Add(new GeometryLeaf(gl, _shader));

        }

        public override void OnUpdate(OpenGL gl)
        {
           _camera.View *= Mat4.RotateX(Angle.FromDegrees(0.6));
            using (new Bind(_shader))
            {
                _camera.Resize((int)ActualWidth, (int)ActualHeight);
                gl.UniformMatrix4(_modelUniform, 1, false, Mat4.Identity.ToColumnMajorArray());
                gl.UniformMatrix4(_viewUniform, 1, false, _camera.View.ToColumnMajorArray());
                gl.UniformMatrix4(_projectionUniform, 1, false, _camera.Projection.ToColumnMajorArray());
            }
        }

        public override void OnRender(OpenGL gl)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.ClearColor(0.137f, 0.121f, 0.125f, 0f);
            _graph.Render();
        }

        public override void OnResize(OpenGL gl, int width, int height)
        {
            if (width == -1 || height == -1) return;
            using (new Bind(_shader))
            {
                _camera.Resize(width, height);
                gl.UniformMatrix4(_projectionUniform, 1, false, _camera.Projection.ToColumnMajorArray());
            }
        }
    }

    public class PointRenderer
    {
        private readonly IShaderProgram _shader;

        private readonly VBO _vbo;
        private readonly VAO _vao;

        public PointRenderer(OpenGL gl, IShaderProgram shader, IEnumerable<Vert> data)
        {
            _shader = shader;
            _vbo = new VBO(gl, BeginMode.Lines, data);
            _vao = new VAO(gl, _shader, _vbo);
        }

        public void Render()
        {
            _vao.Render();
        }
    }
}
