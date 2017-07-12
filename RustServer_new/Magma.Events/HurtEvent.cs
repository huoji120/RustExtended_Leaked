using System;

namespace Magma.Events
{
	public class HurtEvent
	{
		private object _attacker;

		private DamageEvent _de;

		private bool _decay;

		private Entity _ent;

		private object _victim;

		private string _weapon;

		private WeaponImpact _wi;

		public object Attacker
		{
			get
			{
				return this._attacker;
			}
			set
			{
				this._attacker = value;
			}
		}

		public float DamageAmount
		{
			get
			{
				return this._de.amount;
			}
			set
			{
				this._de.amount = value;
			}
		}

		public DamageEvent DamageEvent
		{
			get
			{
				return this._de;
			}
			set
			{
				this._de = value;
			}
		}

		public string DamageType
		{
			get
			{
				string text = "Unknown";
				int damageTypes = (int)this.DamageEvent.damageTypes;
				string result;
				switch (damageTypes)
				{
				case 0:
					result = "Bleeding";
					return result;
				case 1:
					result = "Generic";
					return result;
				case 2:
					result = "Bullet";
					return result;
				case 3:
				case 5:
				case 6:
				case 7:
					result = text;
					return result;
				case 4:
					result = "Melee";
					return result;
				case 8:
					result = "Explosion";
					return result;
				case 9:
				case 10:
				case 11:
				case 12:
				case 13:
				case 14:
				case 15:
					break;
				case 16:
					result = "Radiation";
					return result;
				default:
					if (damageTypes == 32)
					{
						result = "Cold";
						return result;
					}
					break;
				}
				result = text;
				return result;
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

		public bool IsDecay
		{
			get
			{
				return this._decay;
			}
			set
			{
				this._decay = value;
			}
		}

		public object Victim
		{
			get
			{
				return this._victim;
			}
			set
			{
				this._victim = value;
			}
		}

		public WeaponImpact WeaponData
		{
			get
			{
				return this._wi;
			}
			set
			{
				this._wi = value;
			}
		}

		public string WeaponName
		{
			get
			{
				return this._weapon;
			}
			set
			{
				this._weapon = value;
			}
		}

		public HurtEvent(ref DamageEvent d)
		{
			Magma.Player player = Magma.Player.FindByPlayerClient(d.attacker.client);
			if (player != null)
			{
				this.Attacker = player;
			}
			else
			{
				this.Attacker = new NPC(d.attacker.character);
			}
			Magma.Player player2 = Magma.Player.FindByPlayerClient(d.victim.client);
			if (player2 != null)
			{
				this.Victim = player2;
			}
			else
			{
				this.Victim = new NPC(d.victim.character);
			}
			this.DamageEvent = d;
			this.WeaponData = null;
			this.IsDecay = false;
			if (d.extraData != null)
			{
				WeaponImpact weaponImpact = d.extraData as WeaponImpact;
				this.WeaponData = weaponImpact;
				string weaponName = "";
				if (weaponImpact.dataBlock != null)
				{
					weaponName = weaponImpact.dataBlock.name;
				}
				this.WeaponName = weaponName;
			}
		}

		public HurtEvent(ref DamageEvent d, Entity en) : this(ref d)
		{
			this.Entity = en;
		}
	}
}
