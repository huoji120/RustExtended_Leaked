using System;

namespace Magma.Events
{
	public class DoorEvent
	{
		private Entity _ent;

		private bool _open;

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

		public bool Open
		{
			get
			{
				return this._open;
			}
			set
			{
				this._open = value;
			}
		}

		public DoorEvent(Entity e)
		{
			this.Open = false;
			this.Entity = e;
		}
	}
}
