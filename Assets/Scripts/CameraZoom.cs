using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float _zoomSpeed;
    private bool _isZooming;
    private float _targetSize;
    private Camera _cam;
    
    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentSize = _cam.orthographicSize;
        if(_isZooming && currentSize < _targetSize) {
            _cam.orthographicSize += _zoomSpeed;
            if(_cam.orthographicSize >= _targetSize) {
                _isZooming = false;
            }

        }
    }

    public void SetNewZoomSize(float newSize) {
        _targetSize = newSize;
        _isZooming = true;
    }
}
