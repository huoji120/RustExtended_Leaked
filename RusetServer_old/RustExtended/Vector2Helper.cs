namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class Vector2Helper
    {
        public static string AsString(this Vector2 vector)
        {
            return vector.ToString().Trim(new char[] { '(', ')' });
        }
    }
}

