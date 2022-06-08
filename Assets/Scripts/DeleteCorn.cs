using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCorn : MonoBehaviour
{
    public float timeToDelete = 2f;
    private void Start()
    {
        Destroy(gameObject, timeToDelete);
    }
}
