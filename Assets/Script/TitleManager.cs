using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {
    public RectTransform titleImage; // タイトル画像のRectTransform
    //public GameObject titleImage; // タイトル画像のRectTransform
    public GameObject pressSpaceImage; // "Press Space"の画像オブジェクト
    public float moveSpeed = 200f; // タイトル画像の移動速度
    public float targetY = 0f; // タイトル画像が止まるY座標

    private bool isTitleInPosition = false;

    void Start() {
        // 初期状態では"Press Space"の画像を非表示
        pressSpaceImage.SetActive(false);
    }

    void Update() {
        if (!isTitleInPosition) {
            // タイトル画像を中央に移動
            Vector3 currentPosition = titleImage.anchoredPosition;
            if (currentPosition.y > targetY) {
                currentPosition.y -= moveSpeed * Time.deltaTime;
                titleImage.anchoredPosition = currentPosition;
            } else {
                // 目標位置に到達したら位置を固定し、"Press Space"を表示
                currentPosition.y = targetY;
                titleImage.anchoredPosition = currentPosition;
                isTitleInPosition = true;
                pressSpaceImage.SetActive(true);
            }
        } else {
            // スペースキーが押されたらゲームシーンに遷移
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadGameScene();
            }
        }
    }

    void LoadGameScene() {
        // ゲームシーンに遷移 (シーン名を"GameScene"に変更してください)
        SceneManager.LoadScene("GameScene");
    }
}
