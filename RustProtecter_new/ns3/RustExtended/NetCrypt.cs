namespace RustExtended
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Security.Cryptography;
    using UnityEngine;

    public class NetCrypt
    {
        private byte[] byte_0;
        public bool CryptPackets;
        private EndPoint endPoint_0;
        private System.Net.Sockets.Socket socket_0;
        public RustExtended.UserData UserData;

        private NetCrypt()
        {
        }

        public NetCrypt(System.Net.Sockets.Socket socket, EndPoint ip, [Optional, DefaultParameterValue(null)] byte[] cryptKey)
        {
            this.socket_0 = socket;
            this.endPoint_0 = ip;
            this.CryptPackets = false;
            this.UserData = null;
            if ((cryptKey != null) && (cryptKey.Length > 0))
            {
                this.byte_0 = cryptKey;
            }
            else
            {
                HMACSHA512 hmacsha = new HMACSHA512();
                using (StreamWriter writer = new StreamWriter(new MemoryStream()))
                {
                    writer.Write(RustExtended.Method.Invoke("RustExtended.Hardware.CalculateID").AsBytes);
                    writer.Write(DateTime.Now.Ticks);
                    writer.Write(server.hostname);
                    writer.Write(server.ip);
                    writer.Write(server.port);
                    writer.Write(server.map);
                    writer.BaseStream.Position = 0L;
                    this.byte_0 = hmacsha.ComputeHash(writer.BaseStream);
                    if (this.byte_0.Length > 0x40)
                    {
                        Array.Resize<byte>(ref this.byte_0, 0x40);
                    }
                }
            }
        }

        public int Decrypt(ref byte[] buffer, int length)
        {
            if ((this.CryptPackets && (this.byte_0 != null)) && (this.byte_0.Length > 0))
            {
                int num = BitConverter.ToInt32(buffer, length - 4);
                int num2 = buffer[length - 5];
                for (int i = 0; i < length; i++)
                {
                    if (num2 >= this.byte_0.Length)
                    {
                        num2 = 0;
                    }
                    buffer[i] = (byte) (buffer[i] ^ this.byte_0[num2++]);
                }
                if (BitConverter.ToInt32(new CRC32().ComputeHash(buffer, 0, length - 5), 0) == num)
                {
                    length -= 5;
                    return length;
                }
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
            return length;
        }

        public int Encrypt(ref byte[] buffer, int length)
        {
            if ((this.CryptPackets && (this.byte_0 != null)) && (this.byte_0.Length > 0))
            {
                byte[] src = new CRC32().ComputeHash(buffer, 0, length);
                buffer[length] = (byte) new System.Random().Next(0, this.byte_0.Length - 1);
                int num = buffer[length];
                for (int i = 0; i < length; i++)
                {
                    if (num >= this.byte_0.Length)
                    {
                        num = 0;
                    }
                    buffer[i] = (byte) (buffer[i] ^ this.byte_0[num++]);
                }
                Buffer.BlockCopy(src, 0, buffer, length + 1, src.Length);
                length += src.Length + 1;
            }
            return length;
        }

        public bool SendCryptKey()
        {
            byte[] buffer = new byte[] { 7, 0, 0, 0x25, 10, 0x45, 110, 0x63, 0x72, 0x79, 0x70, 0x74, 0x69, 0x6f, 110, 0xff };
            RustProtectFlag disabled = RustProtectFlag.Disabled;
            if (Truth.RustProtect)
            {
                disabled = RustProtectFlag.Enabled;
                if (Truth.RustProtectSteamHWID)
                {
                    disabled = (RustProtectFlag) ((byte) (disabled | RustProtectFlag.UserHWID));
                }
                if (Truth.RustProtectSnapshots)
                {
                    disabled = (RustProtectFlag) ((byte) (disabled | RustProtectFlag.Snapshot));
                }
            }
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(buffer, 0, buffer.Length);
                    writer.Write((byte) disabled);
                    writer.Write(Truth.RustProtectSnapshotsPacketSize);
                    writer.Write(this.byte_0);
                    if (((this.socket_0 != null) && (this.endPoint_0 != null)) && this.socket_0.Connected)
                    {
                        if ((server.log >= 2) && Core.Debug)
                        {
                            Helper.Log("RustProtect: Packet with crypt key sended for '" + this.endPoint_0 + "'", true);
                        }
                        return (this.socket_0.SendTo(stream.GetBuffer(), 0, (int) stream.Length, SocketFlags.None, this.endPoint_0) == stream.Length);
                    }
                }
            }
            return false;
        }
    }
}

