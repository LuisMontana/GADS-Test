using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private PlayerInput platformerInputs;
    private InputAction movementAction;
    [SerializeField] private float speed;

    private bool isFacingRight = true;

    private void Awake() {
        platformerInputs = new PlayerInput();
        rb2D = GetComponent<Rigidbody2D>();
        
    }

    private void OnEnable() {
        movementAction = platformerInputs.Player.Move;
        movementAction.Enable();
    }

    private void OnDisable() {
        movementAction.Disable();
    }

    private void FixedUpdate() {
        Vector2 input = movementAction.ReadValue<Vector2>();
        rb2D.velocity = new Vector2(input.x * speed, input.y * speed);
    }

}


