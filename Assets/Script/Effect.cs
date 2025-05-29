using UnityEngine;

public class Effect : MonoBehaviour
{
    int lifetime = 1; // 弾のライフタイム（秒）
    void Start() // スタートメソッド
    {        
        Destroy(gameObject,lifetime); // lifetime後に弾を破棄
    }
}
