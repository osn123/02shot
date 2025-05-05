using UnityEngine;

// ƒvƒŒƒCƒ„[‚Ì’e‚ğ§Œä‚·‚éƒNƒ‰ƒX
public class PlayerBullet : MonoBehaviour {
    [Header("Bullet Settings")]
    public float speed = 10f; // ’e‚ÌˆÚ“®‘¬“x
    public float lifetime = 5f; // ’e‚ªÁ–Å‚·‚é‚Ü‚Å‚ÌŠÔ

    void Start() {
        // ˆê’èŠÔŒã‚É’e‚ğ©“®“I‚É”jŠü
        Destroy(gameObject,lifetime);
    }

    void Update() {
        // ’e‚ğã•ûŒü‚ÉˆÚ“®
        MoveBullet();
    }

    private void MoveBullet() {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // “G‚ÆÕ“Ë‚µ‚½ê‡‚Ìˆ—
        if (collision.CompareTag("Enemy")) {
            Destroy(gameObject); // ’e‚ğ”jŠü
            Destroy(collision.gameObject); // “G‚ğ”jŠü
        }
    }
}
