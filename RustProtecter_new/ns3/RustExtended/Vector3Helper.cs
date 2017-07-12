namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Vector3Helper
    {
        public static string AsString(this Vector3 vector)
        {
            return vector.ToString().Trim(new char[] { '(', ')' });
        }

        public static bool Invalid(this Vector3 value)
        {
            if (((!float.IsNaN(value.x) && !float.IsNaN(value.y)) && (!float.IsNaN(value.z) && !float.IsInfinity(value.x))) && !float.IsInfinity(value.y))
            {
                return float.IsInfinity(value.z);
            }
            return true;
        }
    }
}

