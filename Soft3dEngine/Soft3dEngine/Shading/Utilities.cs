using System;
using SharpDX;

namespace Soft3dEngine.Shading
{
    public static class Utilities
    {
        // Compute the cosine of the angle between the light vector and the normal vector
        // Returns a value between 0 and 1
        public static float ComputeNormalDotLight(Vector3 vertex, Vector3 normal, Vector3 lightPosition)
        {
            var lightDirection = lightPosition - vertex;

            normal.Normalize();
            lightDirection.Normalize();

            return Math.Max(0, Vector3.Dot(normal, lightDirection));
        }
    }
}
