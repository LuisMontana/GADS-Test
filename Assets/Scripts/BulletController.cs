using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 _target;
    private const float BULLET_SPEED = 4f;

    public void SetTarget(Vector3 target) {
        _target = (target - transform.position).normalized;
    }

    private void Update() {
        transform.Translate(_target * Time.deltaTime * BULLET_SPEED);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<PlayerController>().ReduceLife();
            Destroy(gameObject);
        }
    }
}
