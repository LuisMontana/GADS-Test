using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasureController : MonoBehaviour
{
    private float _timeToDisable = 1f;
    private bool _playerInTrigger = false;
    [SerializeField] private Slider _slider;
    private const float TRIGGER_DISABLE_TIME = 1f;
    private const float INACTIVE_DISABLE_TIME = 4f;

    private void Awake() {
        _slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f, 1f, 0f));
        _timeToDisable = TRIGGER_DISABLE_TIME;
        Invoke("OnTimeOutDisable", INACTIVE_DISABLE_TIME);
    }
    
    private void Update() {
        if(_playerInTrigger) {
            _timeToDisable -= Time.deltaTime;
            _slider.value = _timeToDisable;
            if(_timeToDisable < 0) {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            _playerInTrigger = true;
            _slider.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            _playerInTrigger = false;
            _timeToDisable = TRIGGER_DISABLE_TIME;
            _slider.value = TRIGGER_DISABLE_TIME;
            _slider.gameObject.SetActive(false);
        }
    }

    private void OnTimeOutDisable() {
        gameObject.SetActive(false);
    }
}
