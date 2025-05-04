using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour { // タイトル画面のコントローラークラス
    public GameObject titleImage; // タイトル画像のGameObject
    public GameObject pressSpaceImage; // "Press Space"の画像のGameObject
    [SerializeField] private float moveSpeed= 2f;
    [SerializeField] private float targetY = 0f; // タイトル画像が止まるY座標

    private bool isTitleInPosition = false; // タイトルが位置に到達したかどうかのフラグ

    void Start() { // スタートメソッド
        pressSpaceImage.SetActive(false); // 初期状態では"Press Space"の画像を非表示
    }

    void Update() { // 更新メソッド
        if (!isTitleInPosition) { // タイトルが位置に到達していない場合
            Vector3 currentPosition = titleImage.transform.position; // 現在のタイトル画像の位置を取得
            if (currentPosition.y > targetY) { // 現在のY座標が目標Y座標より大きい場合
                currentPosition.y -= moveSpeed * Time.deltaTime; // Y座標を移動速度に基づいて減少
                titleImage.transform.position = currentPosition; // タイトル画像の位置を更新
            } else { // 目標位置に到達した場合
                currentPosition.y = targetY; // Y座標を目標位置に設定
                titleImage.transform.position = currentPosition; // タイトル画像の位置を更新
                isTitleInPosition = true; // タイトルが位置に到達したフラグを設定
                StartCoroutine(BlinkPressSpaceImage()); // "Press Space"の画像を点滅させるコルーチンを開始
            }
        } else { // タイトルが位置に到達した場合
            if (Input.GetKeyDown(KeyCode.Space)) { // スペースキーが押された場合
                LoadGameScene(); // ゲームシーンを読み込む
            }
        }
    }

    void LoadGameScene() { // ゲームシーンを読み込むメソッド
        SceneManager.LoadScene("GameScene"); // ゲームシーンに遷移
    }

    IEnumerator BlinkPressSpaceImage() { // "Press Space"の画像を点滅させるコルーチン
        while (true) { // 無限ループ
            pressSpaceImage.SetActive(true); // 画像を表示
            yield return new WaitForSeconds(0.5f); // 0.5秒待機
            pressSpaceImage.SetActive(false); // 画像を非表示
            yield return new WaitForSeconds(0.5f); // 0.5秒待機
        }
    }
}
