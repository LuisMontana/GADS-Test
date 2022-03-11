using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb2D;
    private PlayerInput _platformerInputs;
    private InputAction _movementAction;
    private bool _isFacingRight = true;
    private int _currentLives;
    private float _extraForceX;
    private float _extraForceY;
    private const int MAX_LIVES = 3;

    private void Awake() {
        _platformerInputs = new PlayerInput();
        _rb2D = GetComponent<Rigidbody2D>();
        _currentLives = MAX_LIVES;
        _extraForceX = 0;
        _extraForceY = 0;
    }

    private void OnEnable() {
        _movementAction = _platformerInputs.Player.Move;
        _movementAction.Enable();
    }

    private void OnDisable() {
        _movementAction.Disable();
    }

    private void FixedUpdate() {
        Vector2 input = _movementAction.ReadValue<Vector2>();
        _rb2D.velocity = new Vector2(input.x * _speed + _extraForceX, input.y * _speed + _extraForceY);
    }

    public void ReduceLife() {
        _currentLives--;
        Debug.Log(_currentLives);
        if(_currentLives < 1) {
            GameManager.instance.ResetScene();
        }
    }

    public void SetExtraForces(float x, float y) {
        _extraForceX = x;
        _extraForceY = y;
    }
}


