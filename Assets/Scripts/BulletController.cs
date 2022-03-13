using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 _target;
    private float TIME_TO_LIVE = 5f;
    private const float BULLET_SPEED = 5f;
    
    public void SetTarget(Vector3 target) {
        _target = (target - transform.position).normalized;
        Invoke("Despawn", TIME_TO_LIVE);
    }

    private void Update() {
        transform.Translate(_target * Time.deltaTime * BULLET_SPEED);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().ReduceLife();
            gameObject.SetActive(false);
        }
    }

    private void Despawn() {
        gameObject.SetActive(false);
    }
}
