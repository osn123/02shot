using UnityEngine;

public class PlayerBullet : MonoBehaviour // PlayerBulletクラスを定義
{
    public float speed = 10f; // 弾の移動速度
    public float lifetime = 5f; // 弾が画面外で消滅するまでの時間

    void Start() // スタートメソッド
    {
        // 一定時間後に弾を自動的に破棄
        Destroy(gameObject,lifetime); // lifetime後に弾を破棄
    }

    void Update() // 更新メソッド
    {
        // 弾を上方向に移動
        transform.Translate(Vector3.up * speed * Time.deltaTime); // 弾を上に移動
    }

    private void OnTriggerEnter2D(Collider2D collision) // 衝突時の処理
    {
        // 敵と衝突した場合
        if (collision.CompareTag("Enemy")) // 衝突したオブジェクトが敵か確認
        {
            Destroy(gameObject); // 弾を破棄
            Destroy(collision.gameObject); // 敵を破棄（必要に応じて変更）
        }
    }
}
