using System;
using System.Collections.Generic;

namespace RustExtended
{
	[Serializable]
	public class UserQuery
	{
		public List<UserAnswer> Answer;

		public string Query
		{
			get;
			private set;
		}

		public UserData Userdata
		{
			get;
			private set;
		}

		public uint Timeout
		{
			get;
			private set;
		}

		public UserQuery(UserData userdata, string query, uint lifetime = 10u)
		{
			this.Query = query;
			this.Userdata = userdata;
			this.Answer = new List<UserAnswer>();
			this.Timeout = (uint)(Environment.TickCount + (int)(lifetime * 1000u));
		}

		public bool Answered(string text)
		{
			bool flag = false;
			foreach (UserAnswer current in this.Answer)
			{
				string text2 = current.Text.Replace("*", "");
				if (!(text2 == ""))
				{
					if (current.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
					{
						Method.Invoke(current.Func, current.Args);
						flag = true;
					}
					else if (current.Text.StartsWith("*") && text.EndsWith(text2, StringComparison.OrdinalIgnoreCase))
					{
						Method.Invoke(current.Func, current.Args);
						flag = true;
					}
					else if (current.Text.EndsWith("*") && text.StartsWith(text2, StringComparison.OrdinalIgnoreCase))
					{
						Method.Invoke(current.Func, current.Args);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				foreach (UserAnswer current2 in this.Answer)
				{
					if (current2.Text.Replace("*", "") == "")
					{
						Method.Invoke(current2.Func, current2.Args);
						flag = true;
					}
				}
			}
			return flag;
		}
	}
}
