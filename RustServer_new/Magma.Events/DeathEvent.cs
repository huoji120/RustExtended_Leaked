using System;

namespace Magma.Events
{
	public class DeathEvent : HurtEvent
	{
		private bool _drop;

		public bool DropItems
		{
			get
			{
				return this._drop;
			}
			set
			{
				this._drop = value;
			}
		}

		public DeathEvent(ref DamageEvent d) : base(ref d)
		{
			this.DropItems = true;
		}
	}
}
