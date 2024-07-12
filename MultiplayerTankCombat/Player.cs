using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {

    public NetworkVariable<float> Health = new (
        100,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public static List<Color> ClientColors = new () {
        Color.blue,
        Color.red,
        Color.yellow,
        Color.cyan
    };


    private void Start() {

        GetComponent<SpriteRenderer>().color = ClientColors[(int)GetComponent<NetworkObject>().NetworkObjectId - 1];
    }

    [ServerRpc (RequireOwnership = false)]
    public void ReduceHealthServerRpc(float damage) {

        Health.Value -= damage;
        IsPlayerDeadCheckClientRpc();
        Debug.Log(Health.Value);
    }

    [ClientRpc]
    public void IsPlayerDeadCheckClientRpc() {

        if (Health.Value <= 0) {

            GetComponent<SpriteRenderer>().color = Color.grey;

            if (IsOwner) {
                NetworkManager.Singleton.Shutdown();
                GameManager.Singleton.OpenMainMenu();
            }
        }
    }
}
