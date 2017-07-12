namespace RustExtended
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct HistoryRecord
    {
        public string Name;
        public string Text;
        public HistoryRecord Init(string name, string text)
        {
            this.Name = name;
            this.Text = text;
            return this;
        }
    }
}

