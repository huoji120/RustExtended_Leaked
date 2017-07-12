namespace RustExtended
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [Serializable]
    public class UserQuery
    {
        public List<UserAnswer> Answer;

        public UserQuery(UserData userdata, string query,  uint lifetime)
        {
            this.Query = query;
            this.Userdata = userdata;
            this.Answer = new List<UserAnswer>();
            this.Timeout = ((uint) Environment.TickCount) + (lifetime * 0x3e8);
        }

        public bool Answered(string text)
        {
            bool flag = false;
            foreach (UserAnswer answer in this.Answer)
            {
                string str = answer.Text.Replace("*", "");
                if (str != "")
                {
                    if (answer.Text.Equals(text, StringComparison.OrdinalIgnoreCase))
                    {
                        Method.Invoke(answer.Func, answer.Args);
                        flag = true;
                    }
                    else if (answer.Text.StartsWith("*") && text.EndsWith(str, StringComparison.OrdinalIgnoreCase))
                    {
                        Method.Invoke(answer.Func, answer.Args);
                        flag = true;
                    }
                    else if (answer.Text.EndsWith("*") && text.StartsWith(str, StringComparison.OrdinalIgnoreCase))
                    {
                        Method.Invoke(answer.Func, answer.Args);
                        flag = true;
                    }
                }
            }
            if (!flag)
            {
                foreach (UserAnswer answer2 in this.Answer)
                {
                    if (answer2.Text.Replace("*", "") == "")
                    {
                        Method.Invoke(answer2.Func, answer2.Args);
                        flag = true;
                    }
                }
            }
            return flag;
        }

        public string Query { get; private set; }

        public uint Timeout { get; private set; }

        public UserData Userdata { get; private set; }
    }
}

