using System;
using System.Collections.Generic;

public class ParamsList
{
	private List<object> objs = new List<object>();

	public int Length
	{
		get
		{
			return this.objs.Count;
		}
	}

	public void Add(object o)
	{
		this.objs.Add(o);
	}

	public object Get(int index)
	{
		return this.objs[index];
	}

	public void Remove(object o)
	{
		this.objs.Remove(o);
	}

	public object[] ToArray()
	{
		return this.objs.ToArray();
	}
}
