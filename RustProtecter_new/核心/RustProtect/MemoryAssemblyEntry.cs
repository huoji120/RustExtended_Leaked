using System;

namespace RustProtect
{
	public struct MemoryAssemblyEntry
	{
		public long Pointer;

		public uint Filesize;

		public string Filepath;

		public string TargetRuntime;
	}
}
