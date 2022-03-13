using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelData[] _levelData;
    [SerializeField] private GameObject[] _levelObjects; // Objects that will be enabled as game advances
    [SerializeField] private GameObject _treasurePrefab;
    [SerializeField] private CameraZoom _zoomCamera;
    [SerializeField] private Transform _player;
    private int _currentLevel;
    private float _timeRemaining;
    private float _cumulativeTime;
    private int _currentPickups;

    // Game Pool variables
    [System.Serializable]
    public class Pool 
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static GameManager instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // UI Variables
    [SerializeField] private RectTransform _healthObjects;
    [SerializeField] private Sprite _emptyHeart;
    [SerializeField] private Text _secondsLeftText;
    [SerializeField] private GameObject _winText;
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
            if(_currentLevel == 4) {
                EnableWinText();
                Time.timeScale = 0;
                return;
            }
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
            float distance = Vector2.Distance(spawnPoint, _player.position);
            // only overlap if distance is right one
            if(distance >= _levelData[_currentLevel].pickupMinDistance && distance <= _levelData[_currentLevel].pickupMaxDistance ){
                Collider2D collidingObject = Physics2D.OverlapCircle(spawnPoint, 1.5f);
                if(collidingObject == false) {
                    GameObject treasure = poolDictionary["treasures"].Dequeue();
                    treasure.transform.position = spawnPoint;
                    treasure.SetActive(true);
                    treasure.GetComponent<TreasureController>().Initialize();
                    GameManager.instance.poolDictionary["treasures"].Enqueue(treasure);
                    spawned = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _secondsLeftText.text = $"{(int)_timeRemaining} SECONDS LEFT";
        if(_timeRemaining < 0) {
            ResetScene();
        }
        _timeRemaining -= Time.deltaTime;
        _cumulativeTime += Time.deltaTime;
    }

    public void ResetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateHealthUI (int healthRemaining) {
        _healthObjects.GetChild(healthRemaining).GetComponent<Image>().sprite = _emptyHeart;
    }

    public void EnableWinText() {
        _winText.SetActive(true);
    }
}
