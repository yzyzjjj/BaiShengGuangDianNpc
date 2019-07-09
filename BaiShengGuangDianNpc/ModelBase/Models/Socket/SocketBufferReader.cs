using ServiceStack;
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
        private MemoryStream _mStream;
        private BinaryReader _mReader;
        private string _mHeader;
        public int _mDataLength;
        private List<byte> _data;

        public SocketBufferReader()
        {
            _mDataLength = 0;
            _data = new List<byte>();
        }

        public bool IsValid => _mDataLength + 4 == _data.Count;
        public void SocketBufferReaderAdd(byte[] data)
        {
            if (data == null)
            {
                return;
            }

            if (_mDataLength != 0)
            {
                var valid = data.Take(_mDataLength + 4 - _data.Count);
                _data.AddRange(valid);
            }
            else
            {
                _data.AddRange(data);
            }
            if (_mDataLength == 0)
            {
                _mStream = new MemoryStream(data);
                _mReader = new BinaryReader(_mStream);
                //_mHeader = ReadInt();

                _mDataLength = ReadInt();
            }
            else
            {
                if (_mDataLength + 4 == _data.Count)
                {
                    _mStream = new MemoryStream(_data.ToArray());
                    _mReader = new BinaryReader(_mStream);
                    _mDataLength = ReadInt();
                }
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