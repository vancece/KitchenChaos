using UnityEngine;

public class GameInput : MonoBehaviour {
    private PlayerInputAction playerInputAction;

    private void Awake() {
        playerInputAction = new PlayerInputAction();
        playerInputAction.Enable();
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputAction.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }
}