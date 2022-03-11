using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentController : MonoBehaviour
{
    [SerializeField] private Vector2 currentForce;  // -3 seems to be a good value for it
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().SetExtraForces(currentForce.x, currentForce.y);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().SetExtraForces(0, 0);
        }
    }
}
