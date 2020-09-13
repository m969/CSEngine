using System;
using LiteNetLib;
using LiteNetLib.Utils;
using MessageLengthEx = System.UInt32;

namespace CSEngine.Shared
{
	public class CSEngineArgs
	{
		// 登录ip和端口
		public string ip = "127.0.0.1";
		public int port = 20013;
		public INetEventListener netEventListener;

		//// 客户端类型
		//// Reference: http://www.kbengine.org/docs/programming/clientsdkprogramming.html, client types
		//public KBEngineApp.CLIENT_TYPE clientType = KBEngineApp.CLIENT_TYPE.CLIENT_TYPE_MINI;

		////加密通信类型
		//public KBEngineApp.NETWORK_ENCRYPT_TYPE networkEncryptType = KBEngineApp.NETWORK_ENCRYPT_TYPE.ENCRYPT_TYPE_NONE;

		// Allow synchronization role position information to the server
		// 是否开启自动同步玩家信息到服务端，信息包括位置与方向，毫秒
		// 非高实时类游戏不需要开放这个选项
		public int syncPlayerMS = 100;

		// 是否使用别名机制
		// 这个参数的选择必须与kbengine_defs.xml::cellapp/aliasEntityID的参数保持一致
		public bool useAliasEntityID = true;

		// 在Entity初始化时是否触发属性的set_*事件(callPropertysSetMethods)
		public bool isOnInitCallPropertysSetMethods = true;

		// 发送缓冲大小
		public MessageLengthEx TCP_SEND_BUFFER_MAX = 1460;
		public MessageLengthEx UDP_SEND_BUFFER_MAX = 128;

		// 接收缓冲区大小
		public MessageLengthEx TCP_RECV_BUFFER_MAX = 1460;
		public MessageLengthEx UDP_RECV_BUFFER_MAX = 128;

		// 是否多线程启动
		public bool isMultiThreads = false;

		// 只在多线程模式启用
		// 线程主循环处理频率
		public int threadUpdateHZ = 10;

		// 强制禁用UDP通讯
		public bool forceDisableUDP = false;

		// 心跳频率（tick数）
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