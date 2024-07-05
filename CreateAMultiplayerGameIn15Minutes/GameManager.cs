using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainMenu;

    public void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            _mainMenu.SetActive(true);
        }
    }

    public void ExitMainMenu() {

        _mainMenu.SetActive(false);
    }

    public void StartHost() {

        NetworkManager.Singleton.StartHost();
    }

    public void JoinGame() {

        NetworkManager.Singleton.StartClient();
    }
}
