using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int[] _pickupsToAdvance; // Amount of pickups needed to proceed to next "level"
    [SerializeField] private GameObject[] _levelObjects; // Objects that will be enabled as game advances
    [SerializeField] private float [] _timeToLose; // Objects that will be enabled as game advances
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
    }

    public void AddPickup() {
        _currentPickups++;
        if(_currentPickups == _pickupsToAdvance[_currentLevel]){
            _currentLevel++;
            _timeRemaining = _timeToLose[_currentLevel];
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
