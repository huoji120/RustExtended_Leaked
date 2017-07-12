using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using UnityEngine;

namespace RustExtended
{
	public class NetCrypt
	{
		private System.Net.Sockets.Socket socket_0;

		private EndPoint endPoint_0;

		private byte[] byte_0;

		public UserData UserData;

		public bool CryptPackets;

		private NetCrypt()
		{
		}

		public NetCrypt(System.Net.Sockets.Socket socket, EndPoint ip, byte[] cryptKey = null)
		{
			this.socket_0 = socket;
			this.endPoint_0 = ip;
			this.CryptPackets = false;
			this.UserData = null;
			if (cryptKey != null && cryptKey.Length > 0)
			{
				this.byte_0 = cryptKey;
			}
			else
			{
				HMACSHA512 hMACSHA = new HMACSHA512();
				using (StreamWriter streamWriter = new StreamWriter(new MemoryStream()))
				{
					streamWriter.Write(Method.Invoke("RustExtended.Hardware.CalculateID").AsBytes);
					streamWriter.Write(DateTime.Now.Ticks);
					streamWriter.Write(server.hostname);
					streamWriter.Write(server.ip);
					streamWriter.Write(server.port);
					streamWriter.Write(server.map);
					streamWriter.BaseStream.Position = 0L;
					this.byte_0 = hMACSHA.ComputeHash(streamWriter.BaseStream);
					if (this.byte_0.Length > 64)
					{
						Array.Resize<byte>(ref this.byte_0, 64);
					}
				}
			}
		}

		public bool SendCryptKey()
		{
			byte[] array = new byte[]
			{
				7,
				0,
				0,
				37,
				10,
				69,
				110,
				99,
				114,
				121,
				112,
				116,
				105,
				111,
				110,
				255
			};
			RustProtectFlag rustProtectFlag = RustProtectFlag.Disabled;
			if (Truth.RustProtect)
			{
				rustProtectFlag = RustProtectFlag.Enabled;
				if (Truth.RustProtectSteamHWID)
				{
					rustProtectFlag |= RustProtectFlag.UserHWID;
				}
				if (Truth.RustProtectSnapshots)
				{
					rustProtectFlag |= RustProtectFlag.Snapshot;
				}
			}
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
				{
					binaryWriter.Write(array, 0, array.Length);
					binaryWriter.Write((byte)rustProtectFlag);
					binaryWriter.Write(Truth.RustProtectSnapshotsPacketSize);
					binaryWriter.Write(this.byte_0);
					if (this.socket_0 != null && this.endPoint_0 != null && this.socket_0.Connected)
					{
						if (server.log >= 2 && Core.Debug)
						{
							Helper.Log("RustProtect: Packet with crypt key sended for '" + this.endPoint_0 + "'", true);
						}
						result = ((long)this.socket_0.SendTo(memoryStream.GetBuffer(), 0, (int)memoryStream.Length, SocketFlags.None, this.endPoint_0) == memoryStream.Length);
						return result;
					}
				}
			}
			result = false;
			return result;
		}

		public int Encrypt(ref byte[] buffer, int length)
		{
			if (this.CryptPackets && this.byte_0 != null && this.byte_0.Length > 0)
			{
				byte[] array = new CRC32().ComputeHash(buffer, 0, length);
				buffer[length] = (byte)new System.Random().Next(0, this.byte_0.Length - 1);
				int num = (int)buffer[length];
				for (int i = 0; i < length; i++)
				{
					if (num >= this.byte_0.Length)
					{
						num = 0;
					}
					byte[] array2 = buffer;
					int num2 = i;
					byte[] expr_82_cp_0 = array2;
					int expr_82_cp_1 = num2;
					expr_82_cp_0[expr_82_cp_1] ^= this.byte_0[num++];
				}
				Buffer.BlockCopy(array, 0, buffer, length + 1, array.Length);
				length += array.Length + 1;
			}
			return length;
		}

		public int Decrypt(ref byte[] buffer, int length)
		{
			if (this.CryptPackets && this.byte_0 != null && this.byte_0.Length > 0)
			{
				int num = BitConverter.ToInt32(buffer, length - 4);
				int num2 = (int)buffer[length - 5];
				for (int i = 0; i < length; i++)
				{
					if (num2 >= this.byte_0.Length)
					{
						num2 = 0;
					}
					byte[] array = buffer;
					int num3 = i;
					byte[] expr_66_cp_0 = array;
					int expr_66_cp_1 = num3;
					expr_66_cp_0[expr_66_cp_1] ^= this.byte_0[num2++];
				}
				int num4 = BitConverter.ToInt32(new CRC32().ComputeHash(buffer, 0, length - 5), 0);
				if (num4 == num)
				{
					length -= 5;
				}
				else
				{
					if (Core.Debug)
					{
						Debug.LogWarning("RustProtect: Invalid Packet.");
					}
					for (int j = 0; j < length; j++)
					{
						buffer[j] = 0;
					}
					length = 8;
				}
			}
			return length;
		}
	}
}
