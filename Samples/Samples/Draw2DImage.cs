﻿using System.Drawing;
using WebAssembly;

namespace Samples
{
    // Based on: https://webglfundamentals.org/webgl/lessons/webgl-image-processing.html
    public class Draw2DImage : BaseSample
    {
        public override string Description => "The image is passed as byte[] in ARGB.";

        public override void Run(JSObject canvas, float canvasWidth, float canvasHeight, Color clearColor)
        {
            base.Run(canvas, canvasWidth, canvasHeight, clearColor);

            InitializeShaders(
                vertexShaderCode:
@"attribute vec2 a_position;
attribute vec2 a_texCoord;

uniform vec2 u_resolution;

varying vec2 v_texCoord;

void main() {
   vec2 zeroToOne = a_position / u_resolution;
   vec2 zeroToTwo = zeroToOne * 2.0;
   vec2 clipSpace = zeroToTwo - 1.0;
   gl_Position = vec4(clipSpace * vec2(1, -1), 0, 1);

   v_texCoord = a_texCoord;
}",
                fragmentShaderCode:
@"precision mediump float;

uniform sampler2D u_image;

varying vec2 v_texCoord;

void main() {
   gl_FragColor = texture2D(u_image, v_texCoord);
}");

            var positionAttribute = gl.GetAttribLocation(shaderProgram, "a_position");
            var texCoordAttribute = gl.GetAttribLocation(shaderProgram, "a_texCoord");

            var x1 = 0;
            var x2 = Image.Width;
            var y1 = 0;
            var y2 = Image.Height;
            var positions = new float[]
            {
                x1, y1,
                x2, y1,
                x1, y2,
                x1, y2,
                x2, y1,
                x2, y2
            };
            var positionBuffer = CreateArrayBuffer(positions);

            var texCoordBuffer = CreateArrayBuffer(new float[]
            {
                0, 0, 
                0, 1, 
                1, 0, 
                1, 0, 
                0, 1, 
                1, 1,
            });

            CreateTexture();

            var resolutionUniform = gl.GetUniformLocation(shaderProgram, "u_resolution");

            gl.EnableVertexAttribArray(positionAttribute);
            gl.BindBuffer(gl.ArrayBuffer, positionBuffer);
            gl.VertexAttribPointer(positionAttribute, 2, gl.Float, false, 0, 0);

            gl.EnableVertexAttribArray(texCoordAttribute);
            gl.BindBuffer(gl.ArrayBuffer, texCoordBuffer);
            gl.VertexAttribPointer(texCoordAttribute, 2, gl.Float, false, 0, 0);

            gl.Uniform2f(resolutionUniform, canvasWidth, canvasHeight);
        }

        public override void Draw()
        {
            base.Draw();

            gl.DrawArrays(gl.Triangles, 0, 6);
        }
    }
}
