using RustExtended;
using System;
using System.Collections;
using System.IO;

namespace Magma
{
	public class DataStore
	{
		public Hashtable datastore = new Hashtable();

		private static DataStore instance;

		public static string PATH = Path.Combine(Core.SavePath, "datastore.magma");

		public void Add(string tablename, object key, object val)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this.datastore.Add(tablename, hashtable);
			}
			if (hashtable.ContainsKey(key))
			{
				hashtable[key] = val;
			}
			else
			{
				hashtable.Add(key, val);
			}
		}

		public bool ContainsKey(string tablename, object key)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			bool result;
			if (hashtable != null)
			{
				IEnumerator enumerator = hashtable.Keys.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (current == key)
						{
							bool flag = true;
							result = flag;
							return result;
						}
					}
					result = false;
					return result;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			result = false;
			return result;
		}

		public bool ContainsValue(string tablename, object val)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			bool result;
			if (hashtable != null)
			{
				IEnumerator enumerator = hashtable.Values.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object current = enumerator.Current;
						if (current == val)
						{
							bool flag = true;
							result = flag;
							return result;
						}
					}
					result = false;
					return result;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
			}
			result = false;
			return result;
		}

		public int Count(string tablename)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			int result;
			if (hashtable == null)
			{
				result = 0;
			}
			else
			{
				result = hashtable.Count;
			}
			return result;
		}

		public void Flush(string tablename)
		{
			if ((Hashtable)this.datastore[tablename] != null)
			{
				this.datastore.Remove(tablename);
			}
		}

		public object Get(string tablename, object key)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			object result;
			if (hashtable == null)
			{
				result = null;
			}
			else
			{
				result = hashtable[key];
			}
			return result;
		}

		public static DataStore GetInstance()
		{
			if (DataStore.instance == null)
			{
				DataStore.instance = new DataStore();
			}
			return DataStore.instance;
		}

		public Hashtable GetTable(string tablename)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			Hashtable result;
			if (hashtable == null)
			{
				result = null;
			}
			else
			{
				result = hashtable;
			}
			return result;
		}

		public object[] Keys(string tablename)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			object[] result;
			if (hashtable != null)
			{
				object[] array = new object[hashtable.Keys.Count];
				hashtable.Keys.CopyTo(array, 0);
				result = array;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public void Load()
		{
			if (File.Exists(DataStore.PATH))
			{
				try
				{
					Hashtable hashtable = Util.HashtableFromFile(DataStore.PATH);
					this.datastore = hashtable;
					Util.GetUtil().ConsoleLog("Magma DataStore Loaded", false);
				}
				catch (Exception)
				{
				}
			}
		}

		public void Remove(string tablename, object key)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			if (hashtable != null)
			{
				hashtable.Remove(key);
			}
		}

		public void Save()
		{
			if (this.datastore.Count != 0)
			{
				Util.HashtableToFile(this.datastore, DataStore.PATH);
				Util.GetUtil().ConsoleLog("Magma DataStore Saved", false);
			}
		}

		public object[] Values(string tablename)
		{
			Hashtable hashtable = (Hashtable)this.datastore[tablename];
			object[] result;
			if (hashtable != null)
			{
				object[] array = new object[hashtable.Values.Count];
				hashtable.Values.CopyTo(array, 0);
				result = array;
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
