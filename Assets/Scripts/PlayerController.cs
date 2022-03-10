using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb2D;
    private PlayerInput _platformerInputs;
    private InputAction _movementAction;
    private bool _isFacingRight = true;

    private void Awake() {
        _platformerInputs = new PlayerInput();
        _rb2D = GetComponent<Rigidbody2D>();        
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
        _rb2D.velocity = new Vector2(input.x * _speed, input.y * _speed);
    }
}


