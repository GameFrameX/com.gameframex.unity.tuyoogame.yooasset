using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace YooAsset
{
    [UnityEngine.Scripting.Preserve]
    internal class BufferReader
    {
        private readonly byte[] _buffer;
        private int _index = 0;

        [UnityEngine.Scripting.Preserve]
        public BufferReader(byte[] data)
        {
            _buffer = data;
        }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (_buffer == null || _buffer.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 缓冲区容量
        /// </summary>
        public int Capacity
        {
            get { return _buffer.Length; }
        }

        [UnityEngine.Scripting.Preserve]
        public byte[] ReadBytes(int count)
        {
            CheckReaderIndex(count);
            var data = new byte[count];
            Buffer.BlockCopy(_buffer, _index, data, 0, count);
            _index += count;
            return data;
        }

        [UnityEngine.Scripting.Preserve]
        public byte ReadByte()
        {
            CheckReaderIndex(1);
            return _buffer[_index++];
        }

        [UnityEngine.Scripting.Preserve]
        public bool ReadBool()
        {
            CheckReaderIndex(1);
            return _buffer[_index++] == 1;
        }

        [UnityEngine.Scripting.Preserve]
        public short ReadInt16()
        {
            CheckReaderIndex(2);
            if (BitConverter.IsLittleEndian)
            {
                var value = (short)(_buffer[_index] | (_buffer[_index + 1] << 8));
                _index += 2;
                return value;
            }
            else
            {
                var value = (short)((_buffer[_index] << 8) | _buffer[_index + 1]);
                _index += 2;
                return value;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public ushort ReadUInt16()
        {
            return (ushort)ReadInt16();
        }

        [UnityEngine.Scripting.Preserve]
        public int ReadInt32()
        {
            CheckReaderIndex(4);
            if (BitConverter.IsLittleEndian)
            {
                var value = _buffer[_index] | (_buffer[_index + 1] << 8) | (_buffer[_index + 2] << 16) | (_buffer[_index + 3] << 24);
                _index += 4;
                return value;
            }
            else
            {
                var value = (_buffer[_index] << 24) | (_buffer[_index + 1] << 16) | (_buffer[_index + 2] << 8) | _buffer[_index + 3];
                _index += 4;
                return value;
            }
        }

        [UnityEngine.Scripting.Preserve]
        public uint ReadUInt32()
        {
            return (uint)ReadInt32();
        }

        [UnityEngine.Scripting.Preserve]
        public long ReadInt64()
        {
            CheckReaderIndex(8);
            if (BitConverter.IsLittleEndian)
            {
                var i1 = _buffer[_index] | (_buffer[_index + 1] << 8) | (_buffer[_index + 2] << 16) | (_buffer[_index + 3] << 24);
                var i2 = _buffer[_index + 4] | (_buffer[_index + 5] << 8) | (_buffer[_index + 6] << 16) | (_buffer[_index + 7] << 24);
                _index += 8;
                return (uint)i1 | ((long)i2 << 32);
            }
            else
            {
                var i1 = (_buffer[_index] << 24) | (_buffer[_index + 1] << 16) | (_buffer[_index + 2] << 8) | _buffer[_index + 3];
                var i2 = (_buffer[_index + 4] << 24) | (_buffer[_index + 5] << 16) | (_buffer[_index + 6] << 8) | _buffer[_index + 7];
                _index += 8;
                return (uint)i2 | ((long)i1 << 32);
            }
        }

        [UnityEngine.Scripting.Preserve]
        public ulong ReadUInt64()
        {
            return (ulong)ReadInt64();
        }

        [UnityEngine.Scripting.Preserve]
        public string ReadUTF8()
        {
            var count = ReadUInt16();
            if (count == 0)
            {
                return string.Empty;
            }

            CheckReaderIndex(count);
            var value = Encoding.UTF8.GetString(_buffer, _index, count);
            _index += count;
            return value;
        }

        [UnityEngine.Scripting.Preserve]
        public int[] ReadInt32Array()
        {
            var count = ReadUInt16();
            var values = new int[count];
            for (var i = 0; i < count; i++)
            {
                values[i] = ReadInt32();
            }

            return values;
        }

        [UnityEngine.Scripting.Preserve]
        public long[] ReadInt64Array()
        {
            var count = ReadUInt16();
            var values = new long[count];
            for (var i = 0; i < count; i++)
            {
                values[i] = ReadInt64();
            }

            return values;
        }

        [UnityEngine.Scripting.Preserve]
        public string[] ReadUTF8Array()
        {
            var count = ReadUInt16();
            var values = new string[count];
            for (var i = 0; i < count; i++)
            {
                values[i] = ReadUTF8();
            }

            return values;
        }

        [UnityEngine.Scripting.Preserve]
        [Conditional("DEBUG")]
        private void CheckReaderIndex(int length)
        {
            if (_index + length > Capacity)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}