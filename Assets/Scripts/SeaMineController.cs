using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().ReduceLife();
            Destroy(gameObject);
        }
    }
}
