using LiteNetLib;
using LiteNetLib.Utils;

namespace CSEngine.Shared
{
	public static class PacketHelper
	{
        public static void SendAll(PacketType packetType, INetSerializable packet)
        {
            var netDataWriter = CSEngineApp.App.LiteNet.WriteSerializable(packetType, packet);
            CSEngineApp.App.LiteNet._netManager.SendToAll(netDataWriter, DeliveryMethod.Unreliable);
        }
    }
}