namespace Magma.Events
{
    using Magma;
    using System;

    public class DecayEvent
    {
        private float _dmg;
        private Magma.Entity _ent;

        public DecayEvent(Magma.Entity en, ref float dmg)
        {
            this.Entity = en;
            this.DamageAmount = dmg;
        }

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
    }
}

