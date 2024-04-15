using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject buleltSound;
    private void Awake()
    {
        StartCoroutine(deleteBullet());

    }
    IEnumerator deleteBullet()
    {
        buleltSound.SetActive(true);
        yield return  new WaitForSeconds(1.0f);
        buleltSound.SetActive(false);
        Destroy(gameObject);
    }


}
