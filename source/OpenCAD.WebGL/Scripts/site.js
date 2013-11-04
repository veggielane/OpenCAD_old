$(function () {
    console.log("starting site.js");


    window.requestAnimationFrame = window.mozRequestAnimationFrame || window.webkitRequestAnimationFrame;
    $("canvas.webgl").each(function (index, element) {
        if ($(element).data("model")) {
            $.getJSON($(element).data("model"), function (data) {
                new WebGL(element, data);
            });
        }
    });
});

function WebGL(canvas, data) {
    
    //alert(data.Name)
    this.Init(canvas);
    this.Load(this.gl,data);
    this.Resize(this.gl);
    this.RenderLoop();

}
WebGL.prototype = {

    shader:null,
    mvMatrix: null,
    pMatrix: null,

    Init: function (canvas) {
        //try {
            this.gl = canvas.getContext("webgl", { antialias: true }) || canvas.getContext("experimental-webgl", { antialias: true });
            this.gl.viewportWidth = canvas.width;
            this.gl.viewportHeight = canvas.height;

            this.shader = new Shader(this.gl, "shader-vs", "shader-fs");
            this.shader.use();
            
            this.shader.setModel(mat4.create());
            this.shader.setView(mat4.translate(mat4.create(), mat4.create(), [0.0, 0.0, -7.0]));
            this.shader.setProjection(mat4.perspective(mat4.create(), Math.PI * 0.5, this.gl.viewportWidth / this.gl.viewportHeight, 0.1, 100.0));

        //} catch (e) {
        //    alert(e);
        //}
        if (!this.gl) {
            alert("Could not initialise WebGL");
        }
    },


    vertbuffer: null,
    

    
    Load: function (gl,data) {
        console.log("load");

        gl.clearColor(0.118, 0.118, 0.118, 1.0);
        gl.enable(gl.DEPTH_TEST);
       
        this.vertbuffer = gl.createBuffer();
        gl.bindBuffer(gl.ARRAY_BUFFER, this.vertbuffer);
 
        var vertices = [];
        $.each(data.Data, function (i, triangle) {
            $.each(triangle.L, function (j, vert) {
                $.each(vert, function (k, vec) {
                    vertices.push(vec);
                });
            });
        });
        gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(vertices), gl.STATIC_DRAW);
        this.vertbuffer.itemSize = 3;
        this.vertbuffer.numItems = data.Data.length * 3;
    },
    
    RenderLoop:function () {
        var temp = this;
        window.requestAnimationFrame(function () {
            temp.Render(temp.gl);
            temp.RenderLoop();
        });
    },

    Render: function (gl) {
        console.log("render");

        gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
        
        gl.bindBuffer(gl.ARRAY_BUFFER, this.vertbuffer);
        gl.vertexAttribPointer(this.shader.vertexPositionAttribute, this.vertbuffer.itemSize, gl.FLOAT, false, 0, 0);
        gl.drawArrays(gl.TRIANGLES, 0, this.vertbuffer.numItems);
        
    },
    Resize: function (gl) {
        console.log("resize");
        gl.viewport(0, 0, gl.viewportWidth, gl.viewportHeight);
    },
};

function Shader(gl, vertid, fragid) {
    this.gl = gl;
    this.program = gl.createProgram();
    
    gl.attachShader(this.program, this.getShader(gl, vertid));
    gl.attachShader(this.program, this.getShader(gl, fragid));
    gl.linkProgram(this.program);
    if (!gl.getProgramParameter(this.program, gl.LINK_STATUS)) {
        alert("Could not initialise shaders");
    }
    this.use();
    
    this.vertexPositionAttribute = gl.getAttribLocation(this.program, "aVertexPosition");
    gl.enableVertexAttribArray(this.vertexPositionAttribute);

    this.mUniform = gl.getUniformLocation(this.program, "uMMatrix");
    this.vUniform = gl.getUniformLocation(this.program, "uVMatrix");
    this.pUniform = gl.getUniformLocation(this.program, "uPMatrix");

    return this;
}

Shader.prototype = {
    program: null,
    gl: null,
    
    mUniform: null,
    vUniform: null,
    pUniform: null,

    vertexPositionAttribute: null,

    use: function () {
        this.gl.useProgram(this.program);
    },

    setModel: function (m) {
        this.gl.uniformMatrix4fv(this.mUniform, false, m);
    },
    setView: function (m) {
        this.gl.uniformMatrix4fv(this.vUniform, false, m);
    },
    setProjection: function (m) {
        this.gl.uniformMatrix4fv(this.pUniform, false, m);
    },

    getShader: function (gl, id) {
        var shaderScript = document.getElementById(id);
        if (!shaderScript) {
            return null;
        }

        var str = "";
        var k = shaderScript.firstChild;
        while (k) {
            if (k.nodeType == 3)
                str += k.textContent;
            k = k.nextSibling;
        }

        var shader;
        if (shaderScript.type == "x-shader/x-fragment") {
            shader = gl.createShader(gl.FRAGMENT_SHADER);
        } else if (shaderScript.type == "x-shader/x-vertex") {
            shader = gl.createShader(gl.VERTEX_SHADER);
        } else {
            return null;
        }
        gl.shaderSource(shader, str);
        gl.compileShader(shader);

        if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
            alert(gl.getShaderInfoLog(shader));
            return null;
        }
        return shader;
    }
}