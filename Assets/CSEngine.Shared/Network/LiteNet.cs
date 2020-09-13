using System;
using LiteNetLib;
using LiteNetLib.Utils;

namespace CSEngine.Shared
{ 
    public class LiteNet
    {
        public NetManager _netManager;
        public NetPacketProcessor _packetProcessor;
        public NetDataWriter _cachedWriter = new NetDataWriter();


        public LiteNet(INetEventListener netEventListener)
        {
            _packetProcessor = new NetPacketProcessor();
            _netManager = new NetManager(netEventListener)
            {
                AutoRecycle = true,
                IPv6Enabled = IPv6Mode.Disabled
            };
        }

        public NetDataWriter WriteSerializable<T>(PacketType type, T packet) where T : INetSerializable
        {
            _cachedWriter.Reset();
            _cachedWriter.Put((byte)type);
            packet.Serialize(_cachedWriter);
            return _cachedWriter;
        }

        public NetDataWriter WritePacket<T>(T packet) where T : class, new()
        {
            _cachedWriter.Reset();
            _cachedWriter.Put((byte)PacketType.Serialized);
            _packetProcessor.Write(_cachedWriter, packet);
            return _cachedWriter;
        }
    }
}