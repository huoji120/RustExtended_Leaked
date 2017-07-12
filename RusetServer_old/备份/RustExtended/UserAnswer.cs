namespace RustExtended
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class UserAnswer
    {
        public UserAnswer(string text, string func, object[] args)
        {
            this.Text = text;
            this.Func = func;
            this.Args = args;
        }

        public object[] Args { get; private set; }

        public string Func { get; private set; }

        public string Text { get; private set; }
    }
}

