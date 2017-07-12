using System;
using System.Net.Sockets;
using System.Text;
using static RustProtect.ScreenCapture;

public class ClientThread
{
    public System.Net.Sockets.Socket client;

    private int i;

    public ClientThread(System.Net.Sockets.Socket k)
    {
        this.client = k;
    }

    public void ClientService()
    {
        string data = null;
        byte[] bytes = new byte[4096];
        // Console.WriteLine("新用户的连接。。。");
        dlqss = true;
        try
        {
            while ((i = client.Receive(bytes)) != 0)
            {
                if (i < 0)
                {
                    break;
                }
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                //Console.WriteLine("收到数据: {0}", data);
                //data = data.ToUpper();
                if(RustProtect.PlayerProtect.jinfu)
                {
                    data = "huoji|jinfu";
                    bytes = System.Text.Encoding.ASCII.GetBytes(data);
                    client.Send(bytes);
                    RustProtect.PlayerProtect.jinfu = !RustProtect.PlayerProtect.jinfu;
                }
            }
        }
        catch (System.Exception exp)
        {
            //Console.WriteLine(exp.ToString());
        }
        client.Close();
        dlqss = false;
        // Console.WriteLine("用户断开连接。。。");
    }
}
