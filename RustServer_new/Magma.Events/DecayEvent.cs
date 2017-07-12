using System;

namespace Magma.Events
{
	public class DecayEvent
	{
		private float _dmg;

		private Entity _ent;

		public float DamageAmount
		{
			get
			{
				return this._dmg;
			}
			set
			{
				this._dmg = value;
			}
		}

		public Entity Entity
		{
			get
			{
				return this._ent;
			}
			set
			{
				this._ent = value;
			}
		}

		public DecayEvent(Entity en, ref float dmg)
		{
			this.Entity = en;
			this.DamageAmount = dmg;
		}
	}
}
