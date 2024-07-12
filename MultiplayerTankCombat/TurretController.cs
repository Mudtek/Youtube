using Unity.Netcode;
using UnityEngine;

public class TurretController : NetworkBehaviour {

    public Transform TurretPivot;
    public Transform Turret;
    public GameObject Bullet;

    public NetworkVariable<float> CurrentAngle = new (
        90,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );


    private void Update() {

        if (IsOwner) {
            FireBulletCheck();
            UpdateTurretRotation();
        }

        else {
            UpdateClientTurretRotation();
        }
    }

    private void UpdateClientTurretRotation() {

        TurretPivot.rotation = Quaternion.Euler(new Vector3(0, 0, CurrentAngle.Value));
    }

    private void UpdateTurretRotation() {

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = mousePosition - TurretPivot.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        CurrentAngle.Value = angle;
        TurretPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void FireBulletCheck() {

        if (Input.GetMouseButtonDown(0)) {

            SpawnBulletServerRpc();
        }
    }

    [ServerRpc]
    public void SpawnBulletServerRpc() {

        SpawnBulletClientRpc();
    }

    [ClientRpc]
    public void SpawnBulletClientRpc() {

        GameObject bullet = Instantiate(Bullet, TurretPivot.position, TurretPivot.rotation);
        bullet.GetComponent<Bullet>().Initialize(GetComponent<Player>(), TurretPivot, 10);
    }
}
