using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthPackCollectible : MonoBehaviour
{
    public float rotationSpeed = 150f;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            PowerupController.healthPack = true;
            Destroy(gameObject);
        }
    }
}


