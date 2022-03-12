using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaMineController : MonoBehaviour
{
    private Animator _anim;
    private void Awake() {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().ReduceLife();
            _anim.Play("Explode");
        }
    }

    public void DestroyObject() {
        Destroy(gameObject);
    }
}
