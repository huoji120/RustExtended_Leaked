using System;
using System.Collections.Generic;
using UnityEngine;

namespace Magma
{
	public class Entity
	{
		private object _obj;

		public Player Creator
		{
			get
			{
				return Player.FindByGameID(this.CreatorID.ToString());
			}
		}

		public ulong CreatorID
		{
			get
			{
				ulong result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().creatorID;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>()._master.creatorID;
				}
				else
				{
					result = 0uL;
				}
				return result;
			}
		}

		public float Health
		{
			get
			{
				float result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().GetComponent<TakeDamage>().health;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().GetComponent<TakeDamage>().health;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
			set
			{
				if (this.IsDeployableObject())
				{
					this.GetObject<DeployableObject>().GetComponent<TakeDamage>().health = value;
				}
				else if (this.IsStructure())
				{
					this.GetObject<StructureComponent>().GetComponent<TakeDamage>().health = value;
				}
			}
		}

		public int InstanceID
		{
			get
			{
				int result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().GetInstanceID();
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().GetInstanceID();
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}

		public string Name
		{
			get
			{
				string result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().gameObject.name.Replace("(Clone)", "");
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().name.Replace("(Clone)", "");
				}
				else
				{
					result = "";
				}
				return result;
			}
		}

		public object Object
		{
			get
			{
				return this._obj;
			}
			set
			{
				this._obj = value;
			}
		}

		public Player Owner
		{
			get
			{
				return Player.FindByGameID(this.OwnerID.ToString());
			}
		}

		public ulong OwnerID
		{
			get
			{
				ulong result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().ownerID;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>()._master.ownerID;
				}
				else
				{
					result = 0uL;
				}
				return result;
			}
		}

		public float X
		{
			get
			{
				float result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().gameObject.transform.position.x;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().gameObject.transform.position.x;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
			set
			{
				if (this.IsDeployableObject())
				{
					this.GetObject<DeployableObject>().gameObject.transform.position = new Vector3(value, this.Y, this.Z);
				}
				else if (this.IsStructure())
				{
					this.GetObject<StructureComponent>().gameObject.transform.position = new Vector3(value, this.Y, this.Z);
				}
			}
		}

		public float Y
		{
			get
			{
				float result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().gameObject.transform.position.y;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().gameObject.transform.position.y;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
			set
			{
				if (this.IsDeployableObject())
				{
					this.GetObject<DeployableObject>().gameObject.transform.position = new Vector3(this.X, value, this.Z);
				}
				else if (this.IsStructure())
				{
					this.GetObject<StructureComponent>().gameObject.transform.position = new Vector3(this.X, value, this.Z);
				}
			}
		}

		public float Z
		{
			get
			{
				float result;
				if (this.IsDeployableObject())
				{
					result = this.GetObject<DeployableObject>().gameObject.transform.position.z;
				}
				else if (this.IsStructure())
				{
					result = this.GetObject<StructureComponent>().gameObject.transform.position.z;
				}
				else
				{
					result = 0f;
				}
				return result;
			}
			set
			{
				if (this.IsDeployableObject())
				{
					this.GetObject<DeployableObject>().gameObject.transform.position = new Vector3(this.X, this.Y, value);
				}
				else if (this.IsStructure())
				{
					this.GetObject<StructureComponent>().gameObject.transform.position = new Vector3(this.X, this.Y, value);
				}
			}
		}

		public Entity(object Obj)
		{
			this.Object = Obj;
		}

		public void ChangeOwner(Player p)
		{
			if (this.IsDeployableObject())
			{
				this.GetObject<DeployableObject>().SetupCreator(p.PlayerClient.controllable);
			}
		}

		public void Destroy()
		{
			try
			{
				if (this.IsDeployableObject())
				{
					this.GetObject<DeployableObject>().OnKilled();
				}
				else if (this.IsStructure())
				{
					StructureComponent @object = this.GetObject<StructureComponent>();
					@object._master.RemoveComponent(@object);
					@object._master = null;
					this.GetObject<StructureComponent>().StartCoroutine("DelayedKill");
				}
			}
			catch (Exception)
			{
				if (this.IsDeployableObject())
				{
					NetCull.Destroy(this.GetObject<DeployableObject>().networkViewID);
				}
				else if (this.IsStructure())
				{
					NetCull.Destroy(this.GetObject<StructureComponent>().networkViewID);
				}
			}
		}

		public List<Entity> GetLinkedStructs()
		{
			List<Entity> list = new List<Entity>();
			foreach (StructureComponent current in (this.Object as StructureComponent)._master._structureComponents)
			{
				if (!current.Equals(this.Object))
				{
					list.Add(new Entity(current));
				}
			}
			return list;
		}

		private T GetObject<T>()
		{
			return (T)((object)this.Object);
		}

		public TakeDamage GetTakeDamage()
		{
			TakeDamage result;
			if (this.IsDeployableObject())
			{
				result = this.GetObject<DeployableObject>().GetComponent<TakeDamage>();
			}
			else if (this.IsStructure())
			{
				result = this.GetObject<StructureComponent>().GetComponent<TakeDamage>();
			}
			else
			{
				result = null;
			}
			return result;
		}

		public bool IsDeployableObject()
		{
			return this.Object is DeployableObject;
		}

		public bool IsStructure()
		{
			return this.Object is StructureComponent;
		}

		public void SetDecayEnabled(bool c)
		{
			if (this.IsDeployableObject())
			{
				this.GetObject<DeployableObject>().SetDecayEnabled(c);
			}
		}

		public void UpdateHealth()
		{
			if (this.IsDeployableObject())
			{
				this.GetObject<DeployableObject>().UpdateClientHealth();
			}
			else if (this.IsStructure())
			{
				this.GetObject<StructureComponent>().UpdateClientHealth();
			}
		}
	}
}
