using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ModelBase.Models.Socket
{
    /// <summary>
    /// 读取类
    /// </summary>
    public class SocketBufferReader
    {
        public static string Header => "f3";
        /// <summary>
        /// 功能码
        /// </summary>
        public int FunctionCode { get; set; }
        /// <summary>
        /// 子功能码
        /// </summary>
        public int SubFunctionCode { get; set; }
        private MemoryStream _mStream;
        private BinaryReader _mReader;
        public int MDataLength;
        public int TDataLength => _data.Count;
        private List<byte> _data;

        public SocketBufferReader()
        {
            MDataLength = 0;
            _data = new List<byte>();
        }

        public IEnumerable<byte> Data => _data;
        public bool IsValid => MDataLength == _data.Count;
        public void SocketBufferReaderAdd(IEnumerable<byte> data)
        {
            if (data == null || !data.Any())
            {
                return;
            }

            _data.AddRange(data);
            if (IsValid)
            {
                _mStream = new MemoryStream(_data.ToArray());
                _mReader = new BinaryReader(_mStream);
            }
        }
        public byte ReadByte()
        {
            return _mReader.ReadByte();
        }

        public int ReadInt()
        {
            return _mReader.ReadInt32();
        }

        public uint ReadUInt()
        {
            return _mReader.ReadUInt32();
        }

        public short ReadShort()
        {
            return _mReader.ReadInt16();
        }

        public ushort ReadUShort()
        {
            return _mReader.ReadUInt16();
        }

        public long ReadLong()
        {
            return _mReader.ReadInt64();
        }

        public ulong ReadULong()
        {
            return _mReader.ReadUInt64();
        }

        public float ReadFloat()
        {
            byte[] temp = BitConverter.GetBytes(_mReader.ReadSingle());
            Array.Reverse(temp);
            return BitConverter.ToSingle(temp, 0);
        }

        public double ReadDouble()
        {
            byte[] temp = BitConverter.GetBytes(_mReader.ReadDouble());
            Array.Reverse(temp);
            return BitConverter.ToDouble(temp, 0);
        }

        public string ReadString()
        {
            var len = ReadInt();
            var buffer = _mReader.ReadBytes(len);
            return Encoding.UTF8.GetString(buffer);
        }

        public byte[] ReadBytes(int len = 0)
        {
            return _mReader.ReadBytes(len == 0 ? ReadInt() : len);
        }

        public void Close()
        {
            _mReader?.Close();
            _mStream?.Close();
            _mReader = null;
            _mStream = null;
        }
    }
}