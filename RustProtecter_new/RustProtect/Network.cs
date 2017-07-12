using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace RustProtect
{
	public class Network
	{
		public class NetEncryption
		{
			private byte[] byte_0;

			private byte byte_1;

			public NetEncryption(byte[] key)
			{
				this.byte_0 = new byte[256];
				Buffer.BlockCopy(key, 0, this.byte_0, 0, 64);
				Buffer.BlockCopy(key, 0, this.byte_0, 64, 64);
				Buffer.BlockCopy(key, 0, this.byte_0, 128, 64);
				Buffer.BlockCopy(key, 0, this.byte_0, 192, 64);
				this.byte_1 = 0;
			}

			public uint GetI32(byte[] bytes, int offset)
			{
				if (offset + 4 > bytes.Length)
				{
					return 0u;
				}
				return (uint)((int)bytes[offset++] | (int)bytes[offset++] << 8 | (int)bytes[offset++] << 16 | (int)bytes[offset] << 24);
			}

			public uint GetCRC(byte[] bytes, int length)
			{
				return this.GetI32(new CRC32().ComputeHash(bytes, 0, length), 0);
			}

			public int Encrypt(ref byte[] bytes, int length)
			{
				if (bytes != null && length > 0)
				{
					byte[] array = new CRC32().ComputeHash(bytes, 0, length);
					this.byte_1 = array[0];
					for (int i = 0; i < length; i++)
					{
						byte[] expr_2F_cp_0 = bytes;
						int expr_2F_cp_1 = i;
						byte arg_4E_0 = expr_2F_cp_0[expr_2F_cp_1];
						byte[] arg_4D_0 = this.byte_0;
						byte b;
						this.byte_1 = (byte)((b = this.byte_1) + 1);
						expr_2F_cp_0[expr_2F_cp_1] = (byte)((arg_4E_0 ^ arg_4D_0[b]));
					}
					byte[] bytes2 = BitConverter.GetBytes(4294967295u ^ this.GetI32(array, 0));
					Buffer.BlockCopy(array, 0, bytes, length, 4);
					Buffer.BlockCopy(bytes2, 0, bytes, length + 4, 4);
					length += 8;
				}
				return length;
			}

			public int Decrypt(ref byte[] bytes, int length)
			{
				if (bytes != null && length > 0)
				{
					length -= 8;
					uint i = this.GetI32(bytes, length);
					uint i2 = this.GetI32(bytes, length + 4);
					if ((i ^ i2) == 4294967295u)
					{
						this.byte_1 = bytes[length];
						for (int j = 0; j < length; j++)
						{
							byte[] expr_45_cp_0 = bytes;
							int expr_45_cp_1 = j;
							byte arg_64_0 = expr_45_cp_0[expr_45_cp_1];
							byte[] arg_63_0 = this.byte_0;
							byte b;
							this.byte_1 = (byte)((b = this.byte_1) + 1);
							expr_45_cp_0[expr_45_cp_1] = (byte)((arg_64_0 ^ arg_63_0[(int)b]));
						}
						i2 = this.GetI32(new CRC32().ComputeHash(bytes, 0, length), 0);
						if (i == i2)
						{
							return length;
						}
					}
					if (ProtectLoader.Netlog)
					{
						Debug.LogWarning("Received " + length + " of invalid packet.");
					}
					return 0;
				}
				return length;
			}

            public static explicit operator ulong(NetEncryption v)
            {
                throw new NotImplementedException();
            }
        }

		public static int uLink_DoNetworkSend(System.Net.Sockets.Socket socket, byte[] buffer, int length, EndPoint ip)
		{
			try
			{
				int result = length;
				if (ProtectLoader.Netlog && length > 0)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Network.Send(",
						ip,
						").Packet(",
						length,
						"): ",
						BitConverter.ToString(buffer, 0, length).Replace("-", "")
					}));
				}
				if (ProtectLoader.NetCrypt != null)
				{
					length = ProtectLoader.NetCrypt.Encrypt(ref buffer, length);

                }
				socket.SendTo(buffer, length, SocketFlags.None, ip);
               // UnityEngine.Debug.Log(length.ToString("X16"));
				return result;
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
				socket.Close();
			}
			return 0;
		}

        public static int uLink_DoNetworkRecv(System.Net.Sockets.Socket socket, ref byte[] buffer, ref EndPoint ip)
        {
            try
            {
                int length = socket.ReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref ip);
                if (ProtectLoader.NetCrypt != null)
                {
                    length = ProtectLoader.NetCrypt.Decrypt(ref buffer, length);
                }
                if (ProtectLoader.Netlog && (length > 0))
                {
                    Debug.Log(string.Concat(new object[] { "Network.Recv(", ip, ").Packet(", length, "): ", BitConverter.ToString(buffer, 0, length).Replace("-", "") }));
                }
                return length;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                socket.Close();
            }
            return 0;
        }
    }
}
