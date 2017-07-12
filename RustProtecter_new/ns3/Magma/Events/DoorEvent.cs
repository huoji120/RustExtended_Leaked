namespace Magma.Events
{
    using Magma;
    using System;

    public class DoorEvent
    {
        private Magma.Entity _ent;
        private bool _open;

        public DoorEvent(Magma.Entity e)
        {
            this.Open = false;
            this.Entity = e;
        }

        public Magma.Entity Entity
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
    }
}

