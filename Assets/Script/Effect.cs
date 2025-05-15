using UnityEngine;

public class Effect : MonoBehaviour
{
    int lifetime = 1; // 弾のライフタイム（秒）
    void Start() // スタートメソッド
    {
        // 一定時間後に弾を自動的に破棄
        Destroy(gameObject,lifetime); // lifetime後に弾を破棄
    }
}
