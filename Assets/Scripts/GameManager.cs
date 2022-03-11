using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData[] _levelData;
    [SerializeField] private GameObject[] _levelObjects; // Objects that will be enabled as game advances
    [SerializeField] private GameObject _treasurePrefab;
    [SerializeField] private CameraZoom _zoomCamera;
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
       _timeRemaining = _levelData[_currentLevel].secondsToLose;
       _currentPickups = 0;
       InvokeRepeating("SpawnTreasures", 0f, 3f);
    }

    public void AddPickup() {
        _currentPickups++;
        if(_currentPickups == _levelData[_currentLevel].pickupsToAdvance){
            _currentLevel++;
            _currentPickups = 0;
            _timeRemaining = _levelData[_currentLevel].secondsToLose;
            _zoomCamera.SetNewZoomSize(_levelData[_currentLevel].cameraSize);
        }
    }

    public void SpawnTreasures() {
        bool spawned = false;
        while(!spawned) {
            float boundX = _levelData[_currentLevel].spawnBoundaries.x;
            float boundY = _levelData[_currentLevel].spawnBoundaries.y;
            Vector2 spawnPoint = new Vector2(Random.Range(boundX * -1, boundX), Random.Range(boundY * -1, boundY));
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
        GUILayout.Label($"<color='black'><size=40>{_timeRemaining} Seconds Left</size></color>");
    }
}
