using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Arkademy2D.Tests.Relay
{
    public class TestUnityRelay : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Logged into Unity, player ID: " + AuthenticationService.Instance.PlayerId);
            
        }
    }
}
