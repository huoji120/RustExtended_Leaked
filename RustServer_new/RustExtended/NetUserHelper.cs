using System;
using UnityEngine;

namespace RustExtended
{
	public static class NetUserHelper
	{
		public static NetUser TeleportTo(this NetUser netuser, Vector3 position)
		{
			Character character;
			NetUser result;
            if (!Character.FindByUser(netuser.userID, out character))
			{
				result = netuser;
			}
			else if (float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z))
			{
				result = netuser;
			}
			else if (position == Vector3.zero || object.Equals(character.transform.position, position))
			{
				result = netuser;
			}
			else if (character.transform.position == position)
			{
				result = netuser;
			}
			else
			{
				float num = Vector3.Distance(character.transform.position, position);
				float num2 = Mathf.Round(num / 1000f);
				if (num2 > 5f)
				{
					num2 = 5f;
				}
				position += new Vector3(0f, num2, 0f);
				Helper.LogChat(string.Concat(new object[]
				{
					"User [",
					netuser.displayName,
					":",
					netuser.userID,
					"] teleported from ",
					character.transform.position,
					" to ",
					position,
					" (distance: ",
					num,
					"m, lifted: ",
					num2,
					"m)"
				}), false);
				RustServerManagement.Get().TeleportPlayerToWorld(netuser.networkPlayer, position);
				netuser.truthDetector.NoteTeleported(position, 0.0);
				result = netuser;
			}
			return result;
		}
	}
}
