using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Magma
{
	public class Zone3D
	{
		private List<Vector2> _points;

		private bool _protected;

		private bool _pvp;

		private List<Entity> tmpPoints;

		public List<Entity> Entities
		{
			get
			{
				List<Entity> list = new List<Entity>();
				foreach (Entity current in World.GetWorld().Entities)
				{
					if (this.Contains(current))
					{
						list.Add(current);
					}
				}
				return list;
			}
		}

		public List<Vector2> Points
		{
			get
			{
				return this._points;
			}
			set
			{
				this._points = value;
			}
		}

		public bool Protected
		{
			get
			{
				return this._protected;
			}
			set
			{
				this._protected = value;
			}
		}

		public bool PVP
		{
			get
			{
				return this._pvp;
			}
			set
			{
				this._pvp = value;
			}
		}

		public Zone3D(string name)
		{
			this.PVP = true;
			this.Protected = false;
			this.tmpPoints = new List<Entity>();
			this.Points = new List<Vector2>();
			DataStore.GetInstance().Add("3DZonesList", name, this);
		}

		public bool Contains(Entity en)
		{
			return this.Contains(new Vector3(en.X, en.Y, en.Z));
		}

		public bool Contains(Player p)
		{
			return this.Contains(p.Location);
		}

		public bool Contains(Vector3 v)
		{
			Vector2 vector = new Vector2(v.x, v.z);
			int index = this.Points.Count - 1;
			bool flag = false;
			int i = 0;
			while (i < this.Points.Count)
			{
				if (((this.Points[i].y <= vector.y && vector.y < this.Points[index].y) || (this.Points[index].y <= vector.y && vector.y < this.Points[i].y)) && vector.x < (this.Points[index].x - this.Points[i].x) * (vector.y - this.Points[i].y) / (this.Points[index].y - this.Points[i].y) + this.Points[i].x)
				{
					flag = !flag;
				}
				index = i++;
			}
			return flag;
		}

		public static Zone3D Get(string name)
		{
			return DataStore.GetInstance().Get("3DZonesList", name) as Zone3D;
		}

		public static Zone3D GlobalContains(Entity e)
		{
			Hashtable table = DataStore.GetInstance().GetTable("3DZonesList");
			Zone3D result;
			if (table != null)
			{
				foreach (object current in table.Values)
				{
					Zone3D zone3D = current as Zone3D;
					if (zone3D.Contains(e))
					{
						Zone3D zone3D2 = zone3D;
						result = zone3D2;
						return result;
					}
				}
			}
			result = null;
			return result;
		}

		public static Zone3D GlobalContains(Player p)
		{
			Hashtable table = DataStore.GetInstance().GetTable("3DZonesList");
			Zone3D result;
			if (table != null)
			{
				foreach (object current in table.Values)
				{
					Zone3D zone3D = current as Zone3D;
					if (zone3D.Contains(p))
					{
						Zone3D zone3D2 = zone3D;
						result = zone3D2;
						return result;
					}
				}
			}
			result = null;
			return result;
		}

		public void HideMarkers()
		{
			foreach (Entity current in this.tmpPoints)
			{
				Util.GetUtil().DestroyObject((current.Object as StructureComponent).gameObject);
			}
			this.tmpPoints.Clear();
		}

		public void Mark(Vector2 v)
		{
			this.Points.Add(v);
		}

		public void Mark(float x, float y)
		{
			this.Points.Add(new Vector2(x, y));
		}

		public void ShowMarkers()
		{
			this.HideMarkers();
			foreach (Vector2 current in this.Points)
			{
				float ground = World.GetWorld().GetGround(current.x, current.y);
				Vector3 location = new Vector3(current.x, ground, current.y);
				Entity item = World.GetWorld().Spawn(";struct_metal_pillar", location) as Entity;
				this.tmpPoints.Add(item);
			}
		}
	}
}
