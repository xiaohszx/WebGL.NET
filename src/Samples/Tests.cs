﻿using Samples.Helpers;
using WebGLDotNET;

namespace Samples
{
    public static class Tests
    {
        private static WebGL2RenderingContext gl;

        public static void Run()
        {
            using (var canvas = HtmlHelper.AddCanvas("tests-canvas-wrapper", "tests-canvas", 1, 1))
            {
                gl = new WebGL2RenderingContext(canvas);

                GetErrorRegression();
                GetUniformBlockIndexRegression();
                BindBufferRangeRegression();
            }
        }

        // https://github.com/WaveEngine/WebGL.NET/issues/5
        private static void GetErrorRegression()
        {
            var error = gl.GetError();
            var test = error != WebGLRenderingContextBase.NO_ERROR;
        }

        // https://github.com/WaveEngine/WebGL.NET/issues/5
        private static void GetUniformBlockIndexRegression()
        {
            var program = gl.CreateProgram();
            gl.GetUniformBlockIndex(program, "foo");
        }

        // https://github.com/WaveEngine/WebGL.NET/issues/6
        private static void BindBufferRangeRegression()
        {
            var buffer = gl.CreateBuffer();
            gl.BindBufferRange(WebGL2RenderingContextBase.UNIFORM_BUFFER, 0, buffer, 0, 4);
        }
    }
}
