using System;
using UnityEngine;

namespace RustExtended
{
	public static class Vector2Helper
	{
		public static string AsString(this Vector2 vector)
		{
			return vector.ToString().Trim(new char[]
			{
				'(',
				')'
			});
		}
	}
}
