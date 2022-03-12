using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool 
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] private LevelData[] _levelData;
    [SerializeField] private GameObject[] _levelObjects; // Objects that will be enabled as game advances
    [SerializeField] private GameObject _treasurePrefab;
    [SerializeField] private CameraZoom _zoomCamera;
    private int _currentLevel;
    private float _timeRemaining;
    private float _cumulativeTime;
    private int _currentPickups;

    public static GameManager instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    
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
       // Object Pool creation
       poolDictionary = new Dictionary<string, Queue<GameObject>>();
       foreach (Pool pool in pools)
       {
           GameObject parent = new GameObject(pool.tag + "s");
           Queue<GameObject> objectPool = new Queue<GameObject>();
           for (int i = 0; i < pool.size; i++)
           {
               GameObject obj = Instantiate(pool.prefab);
               obj.SetActive(false);
               obj.transform.SetParent(parent.transform);
               objectPool.Enqueue(obj);
           }

           poolDictionary.Add(pool.tag, objectPool);
       }
    }

    public void AddPickup() {
        _currentPickups++;
        if(_currentPickups == _levelData[_currentLevel].pickupsToAdvance){
            _currentLevel++;
            _currentPickups = 0;
            _timeRemaining = _levelData[_currentLevel].secondsToLose;
            _zoomCamera.SetNewZoomSize(_levelData[_currentLevel].cameraSize);
            _levelObjects[_currentLevel -1].SetActive(true);
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
            ResetScene();
        }
        _timeRemaining -= Time.deltaTime;
        _cumulativeTime += Time.deltaTime;
    }

    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGUI()
    {
        GUILayout.Label($"<color='black'><size=40>{(int)_timeRemaining} Seconds Left </size></color>");
    }
}
