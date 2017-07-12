using System;
using UnityEngine;

namespace Magma
{
	public class NPC
	{
		private Character _char;

		public Character Character
		{
			get
			{
				return this._char;
			}
			set
			{
				this._char = value;
			}
		}

		public float Health
		{
			get
			{
				return this._char.health;
			}
			set
			{
				this._char.takeDamage.health = value;
			}
		}

		public string Name
		{
			get
			{
				return this._char.name.Replace("(Clone)", "");
			}
			set
			{
				this._char.name = value;
			}
		}

		public NPC(Character c)
		{
			this._char = c;
		}

		public void Kill()
		{
			this.Character.Signal_ServerCharacterDeath();
			this.Character.SendMessage("OnKilled", default(DamageEvent), SendMessageOptions.DontRequireReceiver);
		}
	}
}
