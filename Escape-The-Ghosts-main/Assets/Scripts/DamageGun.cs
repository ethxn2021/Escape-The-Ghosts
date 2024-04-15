using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageGun : MonoBehaviour
{
    public float damage;
    public float BulletRange;
    private Transform PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main.transform;
    }

    public void Shoot()
    {
        Ray gunRay = new Ray(PlayerCamera.position, PlayerCamera.forward);
        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            Debug.Log(hitInfo.collider);
            if (hitInfo.collider.gameObject.TryGetComponent(out enemyAI enemy))
            {
                
                enemy.currentEnemyHealth -= damage;
            }
        }
    }

}
