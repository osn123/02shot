using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenController : MonoBehaviour {
    [Header("タイトル画像")]
    public GameObject titleImage; // タイトル画像のGameObject

    [Header("\"Press Space\"画像")]
    public GameObject pressSpaceImage; // "Press Space"の画像のGameObject

    [Header("設定値")]
    [SerializeField] private float moveSpeed = 2f;         // タイトル画像の移動速度
    [SerializeField] private float targetY = 0f;           // タイトル画像が止まるY座標
    [SerializeField] private float blinkInterval = 0.5f;   // 明滅の間隔

    private AudioSource audioSource;  // AudioSource
    public AudioClip StartClip;   // 敵撃破SE



    // 内部変数
    private float blinkTimer = 0f;             // 明滅のタイマー
    private bool isPressSpaceVisible = true;   // "Press Space"の表示状態
    // ステートの定義
    private enum TitleState {
        MovingTitle, // タイトル画像が移動中
        WaitingForInput, // 入力待ち
    }

    private TitleState currentState = TitleState.MovingTitle; // 現在のステート

    void Start() {
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
        pressSpaceImage.SetActive(false); // 初期状態では"Press Space"の画像を非表示
        Player.isDead = false; // プレイヤーの死亡状態をリセット
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();  // ESCキーでアプリ終了
        }
        switch (currentState) {
            case TitleState.MovingTitle:
                HandleTitleMovement();
                break;
            case TitleState.WaitingForInput:
                HandleBlinking();
                HandleInput();
                break;
            default:
                break;
        }
    }

    private void HandleTitleMovement() {
        Vector3 currentPosition = titleImage.transform.position; // 現在のタイトル画像の位置を取得
        if (currentPosition.y > targetY) { // 現在のY座標が目標Y座標より大きい場合
            currentPosition.y -= moveSpeed * Time.deltaTime; // Y座標を移動速度に基づいて減少
            titleImage.transform.position = currentPosition; // タイトル画像の位置を更新
        } else { // 目標位置に到達した場合
            currentPosition.y = targetY; // Y座標を目標位置に設定
            titleImage.transform.position = currentPosition; // タイトル画像の位置を更新
            pressSpaceImage.SetActive(true); // "Press Space"の画像を表示
            currentState = TitleState.WaitingForInput; // ステートを入力待ちに変更
        }
    }

    private void HandleBlinking() {
        blinkTimer += Time.deltaTime; // タイマーを更新
        if (blinkTimer >= blinkInterval) { // 明滅間隔を超えた場合
            isPressSpaceVisible = !isPressSpaceVisible; // 表示状態を切り替え
            pressSpaceImage.SetActive(isPressSpaceVisible); // 表示/非表示を切り替え
            blinkTimer = 0f; // タイマーをリセット
        }
    }

    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.S)) { // スペースキーが押された場合
            StartCoroutine(LoadGameScene()); // ゲームシーンを読み込む
        }
    }

    private IEnumerator LoadGameScene() {
        audioSource.PlayOneShot(StartClip); // 敵撃破SEを再生
        yield return new WaitForSeconds(StartClip.length); // SEの再生が終わるまで待機
        SceneManager.LoadScene("GameScene"); // ゲームシーンに遷移
    }
}
