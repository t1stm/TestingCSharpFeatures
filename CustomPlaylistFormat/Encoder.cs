#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CustomPlaylistFormat.Objects;

namespace CustomPlaylistFormat
{
    public class Encoder
    {
        private struct EncodingData
        {
            public byte Type;
            public string Url;
        }

        private readonly BinaryWriter Writer;
        private readonly PlaylistInfo? Info;

        private bool Started;

        public Encoder(Stream backingStream, PlaylistInfo? info = null)
        {
            Info = info;
            Writer = new BinaryWriter(backingStream);
        }
        public void Encode(IEnumerable<string> data)
        {
            if (!Started)
            {
                Started = true;
                AddFileStartHeader();
                AddInfo();
                AddPlaylistBeginHeader();
            }
            foreach (var item in data)
            {
                // This will be changed when used in production. 
                var split = item.Split("://");
                var protocol = split[0];
                var url = string.Join("://", split[1..]);
                
                var encData = new EncodingData
                {
                    Type = protocol switch
                    {
                        "yt" => 01,
                        "spt" => 02,
                        "file" => 03,
                        "vb7" => 05,
                        "onl" => 06,
                        "tts" => 07,
                        "ttv" => 08,
                        "yt-ov" => 09,
                        _ => 255
                    }
                };
                encData.Url = encData.Type != 255 ? url : item;
                EncodePart(encData);
            }
        }

        private void AddFileStartHeader()
        {
            Writer.Write(FormatConstants.FileStartHeader);
        }

        private void AddPlaylistBeginHeader()
        {
            Writer.Write(FormatConstants.PlaylistBeginHeader);
        }

        private void AddInfo()
        {
            if (Info == null) return;
            Queue<byte[]> writeQueue = new();
            Writer.Write(FormatConstants.InfoBeginHeader);
            byte position = 0;
            byte features = 0;
            if (Info?.Maker != null)
            {
                features = SetBit(features, position);
                AddToQueue(writeQueue, Info.Maker);
            }
            position++;
            if (Info?.Name != null)
            {
                features = SetBit(features, position);
                AddToQueue(writeQueue, Info.Name);
            }
            position++;
            if (Info?.Description != null)
            {
                features = SetBit(features, position);
                AddToQueue(writeQueue, Info.Description);
            }
            position++;
            if (Info?.IsPublic ?? false) features = SetBit(features, position);
            AddToQueue(writeQueue, Info?.LastModified ?? 0);
            Writer.Write(features);
            while (writeQueue.Count != 0)
            {
                Writer.Write(writeQueue.Dequeue());
            }
        }

        private void EncodePart(EncodingData data)
        {
            byte[] urlBytes = Encoding.UTF8.GetBytes(data.Url);
            Writer.Write((ushort) urlBytes.Length);
            Writer.Write(data.Type);
            Writer.Write(urlBytes);
        }
        
        private static void AddToQueue(Queue<byte[]> queue, string info)
        {
            var text = Encoding.UTF8.GetBytes(info);
            queue.Enqueue(new[]{(byte) text.Length});
            queue.Enqueue(text.Length > byte.MaxValue ? text[..byte.MaxValue] : text);
        }
        
        private static void AddToQueue(Queue<byte[]> queue, long info)
        {
            queue.Enqueue(BitConverter.GetBytes(info));
        }

        private static byte SetBit(byte startingByte, byte position) => (byte) (startingByte | (1 << position));
    }
}