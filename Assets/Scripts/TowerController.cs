using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _timeToSpawn;
    [SerializeField] private int _spawnWindow;
    [SerializeField] private Transform _playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBullet", _timeToSpawn, _spawnWindow);
    }

    void SpawnBullet() {
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<BulletController>().SetTarget(_playerTransform.position);
    }
}
