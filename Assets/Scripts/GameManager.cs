using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int[] _pickupsToAdvance; // Amount of pickups needed to proceed to next "level"
    [SerializeField] private GameObject[] _levelObjects; // Objects that will be enabled as game advances
    [SerializeField] private float [] _timeToLose; // Objects that will be enabled as game advances
    [SerializeField] private GameObject _treasurePrefab;
    private int _currentLevel;
    private float _timeRemaining;
    private float _cumulativeTime;
    private int _currentPickups;

    public static GameManager instance;    
    
    void Awake()
    {
        if(instance != null && instance != this) {
            Destroy(this);
        } else {
            instance = this;
        }

       _currentLevel = 0; 
       _cumulativeTime = 0f;
       _timeRemaining = _timeToLose[_currentLevel];
       _currentPickups = 0;
       InvokeRepeating("SpawnTreasures", 0f, 3f);
    }

    public void AddPickup() {
        _currentPickups++;
        if(_currentPickups == _pickupsToAdvance[_currentLevel]){
            _currentLevel++;
            _timeRemaining = _timeToLose[_currentLevel];
        }
    }

    public void SpawnTreasures() {
        bool spawned = false;
        while(!spawned) {
            Vector2 spawnPoint = new Vector2(Random.Range(-6, 6), Random.Range(-4.5f, 4.5f));
            Collider2D collidingObject = Physics2D.OverlapCircle(spawnPoint, 1.5f);
            if(collidingObject == false) {
                Instantiate(_treasurePrefab, spawnPoint, Quaternion.identity);
                spawned = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_timeRemaining < 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        _timeRemaining -= Time.deltaTime;
        _cumulativeTime += Time.deltaTime;
    }

    private void OnGUI()
    {
        GUILayout.Label($"<color='black'><size=40>{_timeRemaining} Seconds Left {_cumulativeTime} Total Seconds</size></color>");
    }
}
