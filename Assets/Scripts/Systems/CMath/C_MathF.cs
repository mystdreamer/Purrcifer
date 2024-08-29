using System.Runtime.CompilerServices;
using UnityEngine;

namespace C_Math
{
    public static class C_MathF
    {
        public const double E = 2.7182818284590451D;
        
        public const double PI = 3.1415926535897931D;
        
        public const double Rad2Deg = 180F / PI;
        
        public const double Deg2Rad = PI / 180F;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cos(float value) => Mathf.Cos(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sin(float value) => Mathf.Sin(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sec(float value) => 1 / Mathf.Cos(value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cot(float value) => Mathf.Cos(value) / Mathf.Sin(value);

        /// <summary>
        /// Limit a value to the passed maximum.
        /// </summary>
        /// <param name="value"> The value to limit. </param>
        /// <param name="max"> The maximum that value can be.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Max(float value, float max) =>
            (value > max) ? max : value;

        /// <summary>
        /// Limit a value above a minimum threshold. 
        /// </summary>
        /// <param name="value"> The value to limit. </param>
        /// <param name="min"> The minimum vaue threshold. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Min(float value, float min) =>
            (value < min) ? min : value;

        /// <summary>
        /// Return the absolute value of a number.
        /// </summary>
        /// <param name="value"> The value to absolute. </param>
        /// <returns> The value with the sign set to positive. </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Abs(float value) =>
            ((value < 0) ? -1 : 1) * value;

        /// <summary>
        /// Returns a value clamped within the range [Min, Max]. 
        /// </summary>
        /// <param name="value"> The inital value to clamp. </param>
        /// <param name="min">   The minimum range. </param>
        /// <param name="max">   The maximum range. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, float min, float max) =>
            (value < min) ? min : (value > max) ? max : value;

        /// <summary>
        /// Returns a value clamped within the range [Min, Max]. 
        /// </summary>
        /// <param name="value"> The inital value to clamp. </param>
        /// <param name="range">   The range [Min, Max]. </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(float value, Vector2 range) =>
            Clamp(value, range.x, range.y);


        public static float GetLineMidpoint(Vector2 vec)
        {
            return vec.magnitude / 2;
        }
    }
}
