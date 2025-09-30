using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private ClearCounter clearCounter;
    [SerializeField] private GameObject visibleGameObject;

    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnOnSelectedCounterChanged;
    }

    private void Player_OnOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == clearCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        visibleGameObject.SetActive(true);
    }

    private void Hide() {
        visibleGameObject.SetActive(false);
    }
}