using System;

namespace RustExtended
{
	[Serializable]
	public class UserAnswer
	{
		public string Text
		{
			get;
			private set;
		}

		public string Func
		{
			get;
			private set;
		}

		public object[] Args
		{
			get;
			private set;
		}

		public UserAnswer(string text, string func, object[] args)
		{
			this.Text = text;
			this.Func = func;
			this.Args = args;
		}
	}
}
