using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SharpDX;

namespace Soft3dEngine.Core
{
    public class Device
    {
        private readonly byte[] _backBuffer;
        private readonly WriteableBitmap _bitmap;
        private readonly float[] _depthBuffer;

        private readonly int _renderWidth;
        private readonly int _renderHeight;
        public Device(WriteableBitmap bitmap)
        {
            _bitmap = bitmap;
            _renderHeight = bitmap.PixelHeight;
            _renderWidth = bitmap.PixelWidth;
            // the back buffer size is equal to the number of pixels to draw
            // on screen (width*height) * 4 (R,G,B & Alpha values). 
            _backBuffer = new byte[bitmap.PixelWidth * bitmap.PixelHeight * 4];
            _depthBuffer = new float[bitmap.PixelWidth * bitmap.PixelHeight];
        }

        // This method is called to clear the back buffer with a specific color
        public void Clear(byte r, byte g, byte b, byte a)
        {
            for (var index = 0; index < _backBuffer.Length; index += 4)
            {
                // BGRA is used by Windows instead by RGBA in HTML5
                _backBuffer[index] = b;
                _backBuffer[index + 1] = g;
                _backBuffer[index + 2] = r;
                _backBuffer[index + 3] = a;
            }

            for (var index = 0; index < _depthBuffer.Length; index++)
            {
                _depthBuffer[index] = float.MaxValue;
            }
        }

        // Once everything is ready, we can flush the back buffer
        // into the front buffer. 
        public void Present()
        {
            _bitmap.WritePixels(
                new Int32Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
                _backBuffer,
                _bitmap.PixelWidth * 4,
                0
            );
        }

        // Called to put a pixel on screen at a specific X,Y coordinates
        public void PutPixel(int x, int y, float z, Color4 color)
        {
            // As we have a 1-D Array for our back buffer
            // we need to know the equivalent cell in 1-D based
            // on the 2D coordinates on screen
            var index = (x + y * _renderWidth);
            var index4 = index * 4;

            if (_depthBuffer[index] < z)
            {
                return; // Discard
            }

            _depthBuffer[index] = z;

            _backBuffer[index4] = (byte)(color.Blue * 255);
            _backBuffer[index4 + 1] = (byte)(color.Green * 255);
            _backBuffer[index4 + 2] = (byte)(color.Red * 255);
            _backBuffer[index4 + 3] = (byte)(color.Alpha * 255);
        }

        // Project takes some 3D coordinates and transform them
        // in 2D coordinates using the transformation matrix
        // It also transform the same coordinates and the norma to the vertex 
        // in the 3D world
        public Vertex Project(Vertex vertex, Matrix transMat, Matrix world)
        {
            // transforming the coordinates into 2D space
            var point2D = Vector3.TransformCoordinate(vertex.Coordinates, transMat);
            // transforming the coordinates & the normal to the vertex in the 3D world
            var point3DWorld = Vector3.TransformCoordinate(vertex.Coordinates, world);
            var normal3DWorld = Vector3.TransformCoordinate(vertex.Normal, world);

            // The transformed coordinates will be based on coordinate system
            // starting on the center of the screen. But drawing on screen normally starts
            // from top left. We then need to transform them again to have x:0, y:0 on top left.
            var x = point2D.X * _renderWidth + _renderWidth / 2.0f;
            var y = -point2D.Y * _renderHeight + _renderHeight / 2.0f;

            return new Vertex
            {
                Coordinates = new Vector3(x, y, point2D.Z),
                Normal = normal3DWorld,
                WorldCoordinates = point3DWorld
            };
        }

        // DrawPoint calls PutPixel but does the clipping operation before
        public void DrawPoint(Vector3 point, Color4 color)
        {
            // Clipping what's visible on screen
            if (point.X >= 0 &&
                point.Y >= 0 && 
                point.X < _bitmap.PixelWidth && 
                point.Y < _bitmap.PixelHeight)
            {
                // Drawing a point
                PutPixel((int)point.X, (int)point.Y, point.Z, color);
            }
        }

        public void DrawBresenhamLine(Vector2 point0, Vector2 point1)
        {
            var x0 = (int)point0.X;
            var y0 = (int)point0.Y;
            var x1 = (int)point1.X;
            var y1 = (int)point1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                
                if ((x0 == x1) && (y0 == y1)) break;
                var e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 >= dx) continue;
                err += dx; y0 += sy;
            }
        }

        // The main method of the engine that re-compute each vertex projection
        // during each frame
        public void Render(Camera camera, params Mesh[] meshes)
        {
            // To understand this part, please read the prerequisites resources
            var viewMatrix = Matrix.LookAtLH(camera.Position, camera.Target, Vector3.UnitY);

            const float fov = 0.78f;
            var aspect = (float) _bitmap.PixelWidth/_bitmap.PixelHeight;
            const float near = 0.01f;
            const float far = 1.0f;

            var projectionMatrix = Matrix.PerspectiveFovRH(fov, aspect, near, far);

            foreach (Mesh mesh in meshes)
            {
                // Beware to apply rotation before translation 
                var worldMatrix = Matrix.RotationYawPitchRoll(mesh.Rotation.Y,
                                                              mesh.Rotation.X, mesh.Rotation.Z) *
                                  Matrix.Translation(mesh.Position);

                var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                var faceIndex = 0;
                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var pixelA = Project(vertexA, transformMatrix, worldMatrix);
                    var pixelB = Project(vertexB, transformMatrix, worldMatrix);
                    var pixelC = Project(vertexC, transformMatrix, worldMatrix);

                    var color = 0.8f;
                    DrawTriangle(pixelA, pixelB, pixelC, new Color4(color, color, color, 1));
                    faceIndex++;
                }
            }
        }

        // Clamping values to keep them between 0 and 1
        float Clamp(float value, float min = 0, float max = 1)
        {
            return Math.Max(min, Math.Min(value, max));
        }

        // Interpolating the value between 2 vertices 
        // min is the starting point, max the ending point
        // and gradient the % between the 2 points
        float Interpolate(float min, float max, float gradient)
        {
            return min + (max - min) * Clamp(gradient);
        }

        // drawing line between 2 points from left to right
        // papb -> pcpd
        // pa, pb, pc, pd must then be sorted before
        void ProcessScanLine(ScanLineData data, Vertex va, Vertex vb, Vertex vc, Vertex vd, Color4 color)
        {
            var pa = va.Coordinates;
            var pb = vb.Coordinates;
            var pc = vc.Coordinates;
            var pd = vd.Coordinates;

            // Thanks to current Y, we can compute the gradient to compute others values like
            // the starting X (sx) and ending X (ex) to draw between
            // if pa.Y == pb.Y or pc.Y == pd.Y, gradient is forced to 1
            var gradient1 = Math.Abs(pa.Y - pb.Y) > float.Epsilon ? (data.CurrentY - pa.Y) / (pb.Y - pa.Y) : 1;
            var gradient2 = Math.Abs(pc.Y - pd.Y) > float.Epsilon ? (data.CurrentY - pc.Y) / (pd.Y - pc.Y) : 1;

            var sx = (int)Interpolate(pa.X, pb.X, gradient1);
            var ex = (int)Interpolate(pc.X, pd.X, gradient2);

            // starting Z & ending Z
            var z1 = Interpolate(pa.Z, pb.Z, gradient1);
            var z2 = Interpolate(pc.Z, pd.Z, gradient2);

            var snl = Interpolate(data.NormalDotLightA, data.NormalDotLightB, gradient1);
            var enl = Interpolate(data.NormalDotLightC, data.NormalDotLightD, gradient2);

            // drawing a line from left (sx) to right (ex) 
            for (var x = sx; x < ex; x++)
            {
                float gradient = (x - sx) / (float)(ex - sx);

                var z = Interpolate(z1, z2, gradient);
                var ndotl = Interpolate(snl, enl, gradient);
                // changing the color value using the cosine of the angle
                // between the light vector and the normal vector
                DrawPoint(new Vector3(x, data.CurrentY, z), color * ndotl);
            }
        }


        public void DrawTriangle(Vertex v1, Vertex v2, Vertex v3, Color4 color)
        {
            // Sorting the points in order to always have this order on screen p1, p2 & p3
            // with p1 always up (thus having the Y the lowest possible to be near the top screen)
            // then p2 between p1 & p3
            if (v1.Coordinates.Y > v2.Coordinates.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            if (v2.Coordinates.Y > v3.Coordinates.Y)
            {
                var temp = v2;
                v2 = v3;
                v3 = temp;
            }

            if (v1.Coordinates.Y > v2.Coordinates.Y)
            {
                var temp = v2;
                v2 = v1;
                v1 = temp;
            }

            var p1 = v1.Coordinates;
            var p2 = v2.Coordinates;
            var p3 = v3.Coordinates;

            // Light position 
            var lightPos = new Vector3(30, 20, 10);
            // computing the cos of the angle between the light vector and the normal vector
            // it will return a value between 0 and 1 that will be used as the intensity of the color
            var nl1 = Shading.Utilities.ComputeNormalDotLight(v1.WorldCoordinates, v1.Normal, lightPos);
            var nl2 = Shading.Utilities.ComputeNormalDotLight(v2.WorldCoordinates, v2.Normal, lightPos);
            var nl3 = Shading.Utilities.ComputeNormalDotLight(v3.WorldCoordinates, v3.Normal, lightPos);

            var data = new ScanLineData();

            // computing lines' directions
            float dP1P2, dP1P3;

            // http://en.wikipedia.org/wiki/Slope
            // Computing slopes
            if (p2.Y - p1.Y > 0)
                dP1P2 = (p2.X - p1.X) / (p2.Y - p1.Y);
            else
                dP1P2 = 0;

            if (p3.Y - p1.Y > 0)
                dP1P3 = (p3.X - p1.X) / (p3.Y - p1.Y);
            else
                dP1P3 = 0;

            if (dP1P2 > dP1P3)
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.CurrentY = y;

                    if (y < p2.Y)
                    {
                        data.NormalDotLightA = nl1;
                        data.NormalDotLightB = nl3;
                        data.NormalDotLightC = nl1;
                        data.NormalDotLightD = nl2;
                        ProcessScanLine(data, v1, v3, v1, v2, color);
                    } else
                    {
                        data.NormalDotLightA = nl1;
                        data.NormalDotLightB = nl3;
                        data.NormalDotLightC = nl2;
                        data.NormalDotLightD = nl3;
                        ProcessScanLine(data, v1, v3, v2, v3, color);
                    }
                }
            } else
            {
                for (var y = (int)p1.Y; y <= (int)p3.Y; y++)
                {
                    data.CurrentY = y;

                    if (y < p2.Y)
                    {
                        data.NormalDotLightA = nl1;
                        data.NormalDotLightB = nl2;
                        data.NormalDotLightC = nl1;
                        data.NormalDotLightD = nl3;
                        ProcessScanLine(data, v1, v2, v1, v3, color);
                    } else
                    {
                        data.NormalDotLightA = nl2;
                        data.NormalDotLightB = nl3;
                        data.NormalDotLightC = nl1;
                        data.NormalDotLightD = nl3;
                        ProcessScanLine(data, v2, v3, v1, v3, color);
                    }
                }
            }
        }
    }
}
