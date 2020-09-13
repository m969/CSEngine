using System;
using LiteNetLib;
using LiteNetLib.Utils;
using MessageLengthEx = System.UInt32;

namespace CSEngine.Shared
{
	public class CSEngineArgs
	{
		// ��¼ip�Ͷ˿�
		public string ip = "127.0.0.1";
		public int port = 20013;
		public INetEventListener netEventListener;

		//// �ͻ�������
		//// Reference: http://www.kbengine.org/docs/programming/clientsdkprogramming.html, client types
		//public KBEngineApp.CLIENT_TYPE clientType = KBEngineApp.CLIENT_TYPE.CLIENT_TYPE_MINI;

		////����ͨ������
		//public KBEngineApp.NETWORK_ENCRYPT_TYPE networkEncryptType = KBEngineApp.NETWORK_ENCRYPT_TYPE.ENCRYPT_TYPE_NONE;

		// Allow synchronization role position information to the server
		// �Ƿ����Զ�ͬ�������Ϣ������ˣ���Ϣ����λ���뷽�򣬺���
		// �Ǹ�ʵʱ����Ϸ����Ҫ�������ѡ��
		public int syncPlayerMS = 100;

		// �Ƿ�ʹ�ñ�������
		// ���������ѡ�������kbengine_defs.xml::cellapp/aliasEntityID�Ĳ�������һ��
		public bool useAliasEntityID = true;

		// ��Entity��ʼ��ʱ�Ƿ񴥷����Ե�set_*�¼�(callPropertysSetMethods)
		public bool isOnInitCallPropertysSetMethods = true;

		// ���ͻ����С
		public MessageLengthEx TCP_SEND_BUFFER_MAX = 1460;
		public MessageLengthEx UDP_SEND_BUFFER_MAX = 128;

		// ���ջ�������С
		public MessageLengthEx TCP_RECV_BUFFER_MAX = 1460;
		public MessageLengthEx UDP_RECV_BUFFER_MAX = 128;

		// �Ƿ���߳�����
		public bool isMultiThreads = false;

		// ֻ�ڶ��߳�ģʽ����
		// �߳���ѭ������Ƶ��
		public int threadUpdateHZ = 10;

		// ǿ�ƽ���UDPͨѶ
		public bool forceDisableUDP = false;

		// ����Ƶ�ʣ�tick����
		public int serverHeartbeatTick = 15;
	}

	public class CSEngineApp
    {
		public static CSEngineApp App { get; set; }
        //public NetManager NetManager { get; set; }
		public LiteNet LiteNet { get; set; }


		public CSEngineApp(CSEngineArgs args)
		{
			//if (App != null)
   //         {
			//	throw new Exception("Only one instance of KBEngineApp!");
			//}
			//App = this;
			Initialize(args);
		}

		//public static CSEngineApp GetSingleton()
		//{
		//	if (CSEngineApp.App == null)
		//	{
		//		throw new Exception("Please create KBEngineApp!");
		//	}
		//	return CSEngineApp.App;
		//}

		public virtual bool Initialize(CSEngineArgs args)
		{
			InitNetwork(args);
			InstallEvents();
			return true;
		}

		void InitNetwork(CSEngineArgs args)
		{
			//NetManager = new NetManager(args.netEventListener)
			//{
			//	AutoRecycle = true
			//};
			LiteNet = new LiteNet(args.netEventListener);
		}

		void InstallEvents()
		{

		}

		//public void Destroy()
		//{
		//	CSEngineApp.App = null;
		//}

		public void SendAll(PacketType packetType, INetSerializable packet)
		{
			var netDataWriter = LiteNet.WriteSerializable(packetType, packet);
			LiteNet._netManager.SendToAll(netDataWriter, DeliveryMethod.Unreliable);
		}
	}
}