using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using ModelBase.Base.Utils;

namespace ModelBase.Models.Socket
{
    /// <summary>
    /// 写入类
    /// </summary>
    public class SocketBufferWriter
    {
        private string Header => "f3";
        private MemoryStream m_stream = null;
        private BinaryWriter m_writer = null;
        private int _mFinishLength;
        public int finishLength => _mFinishLength;

        public SocketBufferWriter()
        {
            _mFinishLength = 0;
            m_stream = new MemoryStream();
            m_writer = new BinaryWriter(m_stream);
        }

        public void WriteByte(byte v)
        {
            m_writer.Write(v);
        }

        public void WriteInt(int v)
        {
            m_writer.Write(v);
        }

        public void WriteUInt(uint v)
        {
            m_writer.Write(v);
        }

        public void WriteShort(short v)
        {
            m_writer.Write(v);
        }

        public void WriteUShort(ushort v)
        {
            m_writer.Write(v);
        }

        public void WriteLong(long v)
        {
            m_writer.Write(v);
        }

        public void WriteULong(ulong v)
        {
            m_writer.Write(v);
        }

        public void WriteFloat(float v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToSingle(temp, 0));
        }

        public void WriteDouble(double v)
        {
            byte[] temp = BitConverter.GetBytes(v);
            Array.Reverse(temp);
            m_writer.Write(BitConverter.ToDouble(temp, 0));
        }

        public void WriteString(string v)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(v);
            m_writer.Write(bytes.Length);
            m_writer.Write(bytes);
        }
        public void WriteString(NpcSocketMsg v)
        {
            WriteString(JsonConvert.SerializeObject(v));
        }

        public void WriteBytes(byte[] v)
        {
            m_writer.Write(v.Length);
            m_writer.Write(v);
        }

        public byte[] ToBytes()
        {
            m_writer.Flush();
            return m_stream.ToArray();
        }

        public void Close()
        {
            m_writer.Close();
            m_stream.Close();
            m_writer = null;
            m_stream = null;
        }

        /// <summary>
        /// 将已写入的数据流，封装成一个新的数据流（现有数据长度+现有数据）
        /// 数据转换，网络发送需要两部分数据，一是数据长度，二是主体数据
        /// </summary>
        public byte[] Finish()
        {
            var message = ToBytes();
            var ms = new MemoryStream { Position = 0 };
            var writer = new BinaryWriter(ms);
            writer.Write(Encoding.UTF8.GetBytes(Header));
            writer.Write(message.Length);
            writer.Write(message);
            writer.Flush();
            var result = ms.ToArray();
            _mFinishLength = result.Length;
            return result;
        }
    }
}