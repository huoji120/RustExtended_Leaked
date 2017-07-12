namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public static class NetUserHelper
    {
        public static NetUser TeleportTo(this NetUser netuser, Vector3 position)
        {
            Character character;
            if (Character.FindByUser(netuser.userID, out character))
            {
                if ((float.IsNaN(position.x) || float.IsNaN(position.y)) || float.IsNaN(position.z))
                {
                    return netuser;
                }
                if ((position == Vector3.zero) || object.Equals(character.transform.position, position))
                {
                    return netuser;
                }
                if (character.transform.position == position)
                {
                    return netuser;
                }
                float num = Vector3.Distance(character.transform.position, position);
                float y = Mathf.Round(num / 1000f);
                if (y > 5f)
                {
                    y = 5f;
                }
                position += new Vector3(0f, y, 0f);
                Helper.LogChat(string.Concat(new object[] { "User [", netuser.displayName, ":", netuser.userID, "] teleported from ", character.transform.position, " to ", position, " (distance: ", num, "m, lifted: ", y, "m)" }), false);
                RustServerManagement.Get().TeleportPlayerToWorld(netuser.networkPlayer, position);
                netuser.truthDetector.NoteTeleported(position, 0.0);
            }
            return netuser;
        }
    }
}

