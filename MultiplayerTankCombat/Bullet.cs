using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour {

    private Player _player;

    public void Initialize(Player player, Transform TurretTransform, float speed) {

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = TurretTransform.up * speed;
        _player = player;

        Destroy(gameObject, 2);
    }

    public void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Player") && other.GetComponent<Player>() != _player) {

            if (NetworkManager.Singleton.IsServer) {
                other.GetComponent<Player>().ReduceHealthServerRpc(10);
            }

            Destroy(gameObject);
        }
    }
}
