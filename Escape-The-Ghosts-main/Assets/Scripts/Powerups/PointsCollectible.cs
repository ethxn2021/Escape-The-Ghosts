using System.Collections;
using UnityEngine;

public class PointsCollectible : MonoBehaviour
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

}
