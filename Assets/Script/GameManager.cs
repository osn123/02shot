using UnityEngine;

// ゲーム全体の管理を行うクラス
public class GameManager : MonoBehaviour {
    public GameObject background1; // 1枚目の背景画像オブジェクト
    public GameObject background2; // 2枚目の背景画像オブジェクト
    [SerializeField] private float scrollSpeed = 2f; // 背景画像のスクロール速度（Inspectorで調整可能）
    private float backgroundHeight; // 背景画像の高さを格納する変数

    void Start() {
        // 背景画像の高さを取得
        backgroundHeight = background1.GetComponentInChildren<SpriteRenderer>().bounds.size.y * 4; // 1枚目の背景の高さを取得し、4倍する
    }

    void Update() {
        // 毎フレーム、背景画像をスクロールさせる
        ScrollBackground(background1); // 1枚目の背景をスクロール
        ScrollBackground(background2); // 2枚目の背景をスクロール

        // 背景画像が画面外に出た場合、位置をリセットする
        ResetPositionIfNeeded(background1,background2); // 1枚目の背景をリセット
        ResetPositionIfNeeded(background2,background1); // 2枚目の背景をリセット
    }

    void ScrollBackground(GameObject background) {
        // 現在の位置を取得
        Vector3 position = background.transform.position;

        // 下方向に移動（スクロール速度に応じて移動量を計算）
        position.y -= scrollSpeed * Time.deltaTime; // スクロール速度に基づいてY座標を減少させる

        // 計算した位置を背景オブジェクトに適用
        background.transform.position = position; // 新しい位置を背景オブジェクトに設定
    }

    void ResetPositionIfNeeded(GameObject currentBackground,GameObject otherBackground) {
        // 現在の背景が画面外（-backgroundHeight以下）に出た場合
        if (currentBackground.transform.position.y <= -backgroundHeight) {
            // もう一方の背景の上に現在の背景を移動させる
            currentBackground.transform.position = new Vector3(
                currentBackground.transform.position.x, // X座標はそのまま
                otherBackground.transform.position.y + backgroundHeight, // Y座標をもう一方の背景の上に設定
                currentBackground.transform.position.z // Z座標はそのまま
            );
        }
    }
}
