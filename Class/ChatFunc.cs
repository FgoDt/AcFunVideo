using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;
using Windows;
using System.Threading;
using Windows.Networking.Sockets;
using Windows.Networking;
using System.IO;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.Security.Cryptography;
using System.Runtime.CompilerServices;

namespace AcFunVideo.Class
{
    class ChatFunc
    {
        private CancellationTokenSource roomToken;
        private CancellationTokenSource danmuToken;
        private CancellationTokenSource tickToken;

        private StreamSocket roomSocket;

        public static string GetMD5String(string src)
        {
            string strAlgName = HashAlgorithmNames.Md5;

            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(strAlgName);
            CryptographicHash objHash = objAlgProv.CreateHash();
            IBuffer buffMsg = CryptographicBuffer.ConvertStringToBinary(src, BinaryStringEncoding.Utf8);
            objHash.Append(buffMsg);
            IBuffer buffHash = objHash.GetValueAndReset();
            string strHash = CryptographicBuffer.EncodeToHexString(buffHash);
            return strHash;

        }

        public async Task<object> ReceiveAsync(Stream stream,CancellationToken token)
        {
            object result;

            try
            {
                int receiveBytes = BitConverter.ToInt32(await stream.ReadBlocksAsync(4, token), 0);
                await stream.ReadBlocksAsync(8, token);
                byte[] array = await stream.ReadBlocksAsync(receiveBytes - 10, token);
                await stream.ReadBlocksAsync(2, token);
                string @string = Encoding.UTF8.GetString(array, 0, array.Length);
                System.Diagnostics.Debug.WriteLine(@string);
                var type = @string.IndexOf("type");
                SttDecoder stt = new SttDecoder();
                stt.Parse(@string);
                if (type>0)
                {
                    result = @string;
                    return result;
                }
                else
                {
                    result = null;

                }
            }
            catch 
            {
                result = null;
            }
            return result;
        }

        private async void Send(Stream stream,String msg,CancellationToken token)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                byte[] headerlBytes = BitConverter.GetBytes(msgBytes.Length + 9);
                byte[] singleBytes = BitConverter.GetBytes(689);

                await stream.WriteAsync(headerlBytes, 0, headerlBytes.Length, token);
                await stream.WriteAsync(headerlBytes, 0, headerlBytes.Length, token);
                await stream.WriteAsync(singleBytes, 0, singleBytes.Length, token);
                await stream.WriteAsync(new byte[2], 0, 2, token);
                await stream.WriteAsync(msgBytes, 0, msgBytes.Length, token);
                await stream.WriteAsync(new byte[1], 0, 1, token);
                await stream.FlushAsync(token);

            }
            catch 
            {
            }
        }

        public async void Open()
        {
           
            string guid = System.Guid.NewGuid().ToString().Replace("-", "");

            try
            {
                roomToken = new CancellationTokenSource();
                danmuToken = new CancellationTokenSource();
                tickToken = new CancellationTokenSource();
                roomSocket = new StreamSocket();
                await roomSocket.ConnectAsync(new HostName("119.90.49.95"), "8072");
                Stream Writestream = roomSocket.OutputStream.AsStreamForWrite();
                Stream ReadStream = roomSocket.InputStream.AsStreamForRead();
                long time = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var vk = GetMD5String(time + "lAoZhAnGKao216Yu" + guid);
                SttEncoder sttencoder = new SttEncoder();
                sttencoder.AddItem("type", "loginreq");
                sttencoder.AddItem("username", "");
                sttencoder.AddItem("password", "");
                sttencoder.AddItem("roomid", "58428");
                sttencoder.AddItem("ct", 6);
                sttencoder.AddItem("rt", time);
                sttencoder.AddItem("vk", vk);
                sttencoder.AddItem("ver", 20150326);
                sttencoder.AddItem("devid", guid);
                Send(Writestream, sttencoder.GetResult(), roomToken.Token);
                object obj = new object();
                while (obj!=null)
                {
                    await Task.Delay(100);
                    obj = await ReceiveAsync(ReadStream, roomToken.Token);
                  
                }
                if (true)
                {


                }

            }
            catch
            {

                throw;
            }

        }
    }

    public class SttEncoder
    {
        private StringBuilder sb = new StringBuilder();

        public string GetResult()
        {
            return this.sb.ToString();
        }

        public void Clear()
        {
            this.sb.Clear();
        }

        public void AddItem(string value)
        {
            int i = 0;
            char[] array = value.ToCharArray();
            while (i < array.Length)
            {
                if (array[i] == '/')
                {
                    this.sb.Append("@S");
                }
                else if (array[i] == '@')
                {
                    this.sb.Append("@A");
                }
                else
                {
                    this.sb.Append(array[i]);
                }
                i++;
            }
            this.sb.Append('/');
        }

        public void AddItem(string key, string value)
        {
            int i = 0;
            char[] array = key.ToCharArray();
            while (i < array.Length)
            {
                if (array[i] == '/')
                {
                    this.sb.Append("@S");
                }
                else if (array[i] == '@')
                {
                    this.sb.Append("@A");
                }
                else
                {
                    this.sb.Append(array[i]);
                }
                i++;
            }
            this.sb.Append("@=");
            i = 0;
            array = value.ToCharArray();
            while (i < array.Length)
            {
                if (array[i] == '/')
                {
                    this.sb.Append("@S");
                }
                else if (array[i] == '@')
                {
                    this.sb.Append("@A");
                }
                else
                {
                    this.sb.Append(array[i]);
                }
                i++;
            }
            this.sb.Append('/');
        }

        public void AddItem(int value)
        {
            this.AddItem(value.ToString());
        }

        public void AddItem(string key, int value)
        {
            this.AddItem(key, value.ToString());
        }

        public void AddItem(long value)
        {
            this.AddItem(value.ToString());
        }

        public void AddItem(string key, long value)
        {
            this.AddItem(key, value.ToString());
        }

        public void AddItem(double value)
        {
            this.AddItem(value.ToString());
        }

        public void AddItem(string key, double value)
        {
            this.AddItem(key, value.ToString());
        }
    }

    public static class Extensions
    {
        public static async Task<byte[]> ReadBlocksAsync(this Stream stream,int count)
        {
            byte[] array = new byte[count];
            int i = 0;
            int num = 0;

            while (i<count)
            {
                try
                {
                    int readNum = await stream.ReadAsync(array, i, count - i);
                    if (readNum==0)
                    {
                        num++;
                    }
                    else
                    {
                        i += readNum;
                    }
                }
                catch 
                {
                    num++;
                }
                if (num==3)
               {
                    throw new EndOfStreamException();
                }
            }
            return array;
        }

        public static async Task<byte[]> ReadBlocksAsync(this Stream stream, int count, CancellationToken token)
        {
            byte[] array = new byte[count];
            int i = 0;
            int num = 0;
            while (i < count)
            {
                try
                {
                    int num2 = await stream.ReadAsync(array, i, count - i, token);
                    IL_122:
                    if (num2 == 0)
                    {
                        num++;
                    }
                    else
                    {
                        i += num2;
                    }
                    goto IL_160;
                    TaskAwaiter<int> taskAwaiter;
                    TaskAwaiter<int> var_8_F0 = taskAwaiter;
                    taskAwaiter = default(TaskAwaiter<int>);
                    int arg_118_0 = var_8_F0.GetResult();
                    var_8_F0 = default(TaskAwaiter<int>);
                    num2 = arg_118_0;
                    goto IL_122;
                }
                catch
                {
                    num++;
                }
                IL_160:
                if (num == 3)
                {
                    throw new EndOfStreamException();
                }
            }
            return array;
        }
    }

    public class SttDecoder
    {
        public struct SttEncodingItem
        {
            public string Key;

            public string Value;
        }

        public List<SttDecoder.SttEncodingItem> items = new List<SttDecoder.SttEncodingItem>();

        ~SttDecoder()
        {
        }

        public int Parse(string str)
        {
            SttDecoder.SttEncodingItem item = default(SttDecoder.SttEncodingItem);
            item.Key = null;
            item.Value = null;
            int i = 0;
            char[] array = str.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            while (i < array.Length)
            {
                if (array[i] == '/')
                {
                    if (item.Key == null)
                    {
                        item.Key = "";
                    }
                    item.Value = stringBuilder.ToString();
                    stringBuilder.Clear();
                    this.items.Add(item);
                    item.Key = null;
                    item.Value = null;
                }
                else if (array[i] == '@')
                {
                    i++;
                    if (i < array.Length)
                    {
                        if (array[i] == 'A')
                        {
                            stringBuilder.Append('@');
                        }
                        else if (array[i] == 'S')
                        {
                            stringBuilder.Append('/');
                        }
                        else if (array[i] == '=')
                        {
                            item.Key = stringBuilder.ToString();
                            stringBuilder.Clear();
                        }
                    }
                }
                else
                {
                    stringBuilder.Append(array[i]);
                }
                i++;
            }
            if (i > 0 && i == array.Length && array[i - 1] != '/')
            {
                if (item.Key == null)
                {
                    item.Key = "";
                }
                item.Value = stringBuilder.ToString();
                stringBuilder.Clear();
                this.items.Add(item);
                item.Key = null;
                item.Value = null;
            }
            return this.items.Count<SttDecoder.SttEncodingItem>();
        }

        public void Clear()
        {
            this.items.Clear();
        }

        public string GetItem(int index)
        {
            if (index < 0 || index > this.items.Count<SttDecoder.SttEncodingItem>())
            {
                return null;
            }
            return this.items[index].Value;
        }

    }
}
