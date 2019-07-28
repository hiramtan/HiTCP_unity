﻿using HiFramework;
using HiSocket.Tcp;
using System;

namespace HiSocket.Test
{
    public class Package : PackageBase
    {
        protected override void Pack(BlockBuffer<byte> bytes, Action<byte[]> onPacked)
        {
            int length = bytes.WritePosition;
            var header = BitConverter.GetBytes(length);
            var newBytes = new BlockBuffer<byte>(length + header.Length);
            newBytes.Write(header);
            newBytes.Write(bytes.Buffer);
            onPacked(newBytes.Buffer);
        }

        protected override void Unpack(BlockBuffer<byte> bytes, Action<byte[]> onUnpacked)
        {
            while (bytes.WritePosition > 4)
            {
                int length = BitConverter.ToInt32(bytes.Buffer, 0);
                if (bytes.WritePosition >= 4 + length)
                {
                    bytes.MoveReadPostion(4);
                    var data = bytes.Read(length);
                    onUnpacked(data);
                    bytes.ResetIndex();
                }
            }
        }
    }
}
