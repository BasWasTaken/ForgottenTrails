using UnityEngine;

namespace Utility.Extensions
{
    /// <summary>
    /// Extension class for easing vector calculations by handling associated convertions.
    /// </summary>
    public static class VectorExtensions
    {
        ///___METHODS___///

        /// Converts an angle to a vector representing its corresponding direction
        public static Vector3 ToDirection(this float angleRadians)
        {
            return new Vector3(Mathf.Sin(angleRadians), Mathf.Cos(angleRadians));
        }

        /// Converts a direction vector to its corresponding angle in radians
        public static float ToAngle(this Vector3 direction)
        {
            return ToAngle(new Vector2(direction.x, direction.y));
        }

        /// Converts a direction vector to its corresponding angle in radians
        public static float ToAngle(this Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) - Mathf.PI / 2;
        }
    }
}