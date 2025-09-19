using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour {

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;
    public static Player Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInputOnOnInteractAction;
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    private void GameInputOnOnInteractAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact();
        }
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position,
                lastInteractDir,
                out RaycastHit raycastHit,
                interactDistance,
                countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                if (clearCounter != selectedCounter) {
                    SetSelectedCounter(clearCounter);
                }
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,
            transform.position + Vector3.up * playerHeight,
            playerRadius,
            moveDir,
            moveDistance);

        if (!canMove) {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDirX,
                moveDistance);

            if (canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position,
                    transform.position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    public bool GetIsWalking() {
        return isWalking;
    }

    private void SetSelectedCounter(ClearCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this,
            new OnSelectedCounterChangedEventArgs {
                selectedCounter = selectedCounter,
            });
    }

    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }
}