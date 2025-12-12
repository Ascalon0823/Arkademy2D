using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Utp;

namespace Arkademy2D.Tests.Relay
{
    public class TestRelayUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField connectionCodeText;

        public void Host()
        {
            (RelayNetworkManager.singleton as RelayNetworkManager).StartRelayHost(5);
        }

        protected void Update()
        {
            var joinCode =  (RelayNetworkManager.singleton as RelayNetworkManager).relayJoinCode;
            if (!string.IsNullOrEmpty(joinCode))
            {
                connectionCodeText.text = joinCode;
            }
        }

        public void Join()
        {
            var manager =
                RelayNetworkManager.singleton as RelayNetworkManager;
            manager.relayJoinCode = connectionCodeText.text;
            manager.JoinRelayServer();
        }
    }
}