using System;

namespace Magma.Events
{
	public class BPUseEvent
	{
		private BlueprintDataBlock _bdb;

		private bool _cancel;

		public bool Cancel
		{
			get
			{
				return this._cancel;
			}
			set
			{
				this._cancel = value;
			}
		}

		public BlueprintDataBlock DataBlock
		{
			get
			{
				return this._bdb;
			}
			set
			{
				this._bdb = value;
			}
		}

		public string ItemName
		{
			get
			{
				return this._bdb.resultItem.name;
			}
		}

		public BPUseEvent(BlueprintDataBlock bdb)
		{
			this.DataBlock = bdb;
			this.Cancel = false;
		}
	}
}
