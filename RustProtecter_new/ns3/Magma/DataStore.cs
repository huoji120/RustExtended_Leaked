namespace Magma
{
    using System;
    using System.Collections;
    using System.IO;
    using RustExtended;

    public class DataStore
    {
        public Hashtable datastore = new Hashtable();
        private static DataStore instance;
        public static string PATH = Path.Combine(Core.SavePath, "datastore.magma");

        public void Add(string tablename, object key, object val)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
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
            if (hashtable != null)
            {
                IEnumerator enumerator = hashtable.Keys.GetEnumerator();
                bool result;
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        if (current == key)
                        {
                            result = true;
                            return result;
                        }
                    }
                    return false;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return true;
            }
            return false;
        }


        public bool ContainsValue(string tablename, object val)
        {
            Hashtable hashtable = (Hashtable)this.datastore[tablename];
            if (hashtable != null)
            {
                IEnumerator enumerator = hashtable.Values.GetEnumerator();
                bool result;
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object current = enumerator.Current;
                        if (current == val)
                        {
                            result = true;
                            return result;
                        }
                    }
                    return false;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                return result;
            }
            return false;
        }


        public int Count(string tablename)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable == null)
            {
                return 0;
            }
            return hashtable.Count;
        }

        public void Flush(string tablename)
        {
            if (((Hashtable) this.datastore[tablename]) != null)
            {
                this.datastore.Remove(tablename);
            }
        }

        public object Get(string tablename, object key)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable == null)
            {
                return null;
            }
            return hashtable[key];
        }

        public static DataStore GetInstance()
        {
            if (instance == null)
            {
                instance = new DataStore();
            }
            return instance;
        }

        public Hashtable GetTable(string tablename)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable == null)
            {
                return null;
            }
            return hashtable;
        }

        public object[] Keys(string tablename)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable != null)
            {
                object[] array = new object[hashtable.Keys.Count];
                hashtable.Keys.CopyTo(array, 0);
                return array;
            }
            return null;
        }

        public void Load()
        {
            if (File.Exists(PATH))
            {
                try
                {
                    Hashtable hashtable = Util.HashtableFromFile(PATH);
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
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable != null)
            {
                hashtable.Remove(key);
            }
        }

        public void Save()
        {
            if (this.datastore.Count != 0)
            {
                Util.HashtableToFile(this.datastore, PATH);
                Util.GetUtil().ConsoleLog("Magma DataStore Saved", false);
            }
        }

        public object[] Values(string tablename)
        {
            Hashtable hashtable = (Hashtable) this.datastore[tablename];
            if (hashtable != null)
            {
                object[] array = new object[hashtable.Values.Count];
                hashtable.Values.CopyTo(array, 0);
                return array;
            }
            return null;
        }
    }
}

