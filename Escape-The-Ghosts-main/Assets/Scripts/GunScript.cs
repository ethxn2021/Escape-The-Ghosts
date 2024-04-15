using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GunScript : MonoBehaviour
{
    public UnityEvent OnGunShoot;
    public GameObject bulletPrefab; // Reference to your bullet prefab
    public Transform muzzleTransform; // Reference to the transform where bullets will be instantiated
    
    private float bulletSpeed = 40.0f;
    
    public float FireCooldown;
    public bool Automatic;
    private float CurrentCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        CurrentCoolDown = FireCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (Automatic)
        {
            if (Input.GetMouseButton(0))
            {
                if (CurrentCoolDown <= 0f)
                {
                    Shoot();
                    CurrentCoolDown = FireCooldown;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (CurrentCoolDown <= 0f)
                {
                    Shoot();
                    CurrentCoolDown = FireCooldown;
                }
            }
        }

        CurrentCoolDown -= Time.deltaTime;
    }

    void Shoot()
    {
        OnGunShoot?.Invoke();

        // Instantiate the bullet at the muzzle position and rotation
        GameObject bullet = Instantiate(bulletPrefab, muzzleTransform.position, muzzleTransform.rotation);

        // Optionally, you can apply force to the bullet here if it has a Rigidbody component
        bullet.GetComponent<Rigidbody>().velocity = muzzleTransform.forward * bulletSpeed;    }
}
