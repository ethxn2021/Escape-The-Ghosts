using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
    }

    public void Remove()
    {
        gameObject.SetActive(false);
        
    }
}
