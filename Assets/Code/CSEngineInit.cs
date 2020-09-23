using CSEngine.Server;
using CSEngine.Shared;
using LiteNetLib;
using UnityEngine;
using UnityEngine.UI;
using ET;
using System.Threading;

namespace CSEngine.Client
{
    public class CSEngineInit : MonoBehaviour
    {
        [SerializeField] private GameObject _uiObject;
        [SerializeField] private ClientLogic _clientLogic;
        [SerializeField] private ServerLogic _serverLogic;
        [SerializeField] private InputField _ipField;
        [SerializeField] private Text _disconnectInfoField;

        private void Awake()
        {
            _ipField.text = NetUtils.GetLocalIp(LocalAddrType.IPv4);

            SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

            var args = new CSEngineArgs();
            args.netEventListener = _clientLogic;
            args.ip = "127.0.0.1";
            G.CSEngineApp = new CSEngineApp(args);
            G.ClientLogic = _clientLogic;
            G.ClientPlayerInput = GetComponent<ClientPlayerInput>();
            _clientLogic.Initialize();

            G.CSEngineApp.LiteNet._netManager.MaxConnectAttempts = 2;
            G.CSEngineApp.LiteNet._netManager.DisconnectTimeout = 500;
            OnConnectClick();
        }

        public void OnHostClick()
        {
            //_serverLogic.StartServer();
            //OnConnectClick();
            //_uiObject.SetActive(false);
            //_clientLogic.Connect("localhost", OnDisconnected);
        }

        private void OnDisconnected(DisconnectInfo info)
        {
            _uiObject.SetActive(true);
            _disconnectInfoField.text = info.Reason.ToString();

            if (Server.G.CSEngineApp == null)
            {
                var args = new CSEngineArgs();
                args.netEventListener = _serverLogic;
                args.ip = "127.0.0.1";
                Server.G.CSEngineApp = new CSEngineApp(args);
                _serverLogic.StartServer();

                OnConnectClick();
            }
        }

        public void OnConnectClick()
        {
            _uiObject.SetActive(false);
            _clientLogic.Connect(_ipField.text, OnDisconnected);
        }
    }
}
