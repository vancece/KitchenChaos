using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    private const string IS_WALKING = "IsWalking";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        animator.SetBool(IS_WALKING, player.GetIsWalking());
    }
}