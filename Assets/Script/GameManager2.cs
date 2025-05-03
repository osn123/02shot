using UnityEngine;

public class GameManager2 : MonoBehaviour {
    public SpriteRenderer background1; // 1–‡–Ú‚Ì”wŒi
    public SpriteRenderer background2; // 2–‡–Ú‚Ì”wŒi
    public float scrollSpeed = 2f; // ƒXƒNƒ[ƒ‹‘¬“x
    private float backgroundHeight; // ”wŒi‰æ‘œ‚Ì‚‚³

    void Start() {
        // ”wŒi‰æ‘œ‚Ì‚‚³‚ğæ“¾
        backgroundHeight = background1.bounds.size.y;
    }

    void Update() {
        // ”wŒi‚ğƒXƒNƒ[ƒ‹
        ScrollBackground(background1);
        ScrollBackground(background2);

        // ”wŒi‚ğƒŠƒZƒbƒg
        ResetPositionIfNeeded(background1, background2);
        ResetPositionIfNeeded(background2, background1);
    }

    void ScrollBackground(SpriteRenderer background) {
        // Œ»İ‚ÌˆÊ’u‚ğæ“¾
        Vector3 position = background.transform.position;

        // ‰º•ûŒü‚ÉˆÚ“®
        position.y -= scrollSpeed * Time.deltaTime;

        // ˆÊ’u‚ğXV
        background.transform.position = position;
    }

    void ResetPositionIfNeeded(SpriteRenderer currentBackground, SpriteRenderer otherBackground) {
        // Œ»İ‚Ì”wŒi‚ª‰æ–ÊŠO‚Éo‚½‚çA‚à‚¤ˆê•û‚Ì”wŒi‚Ìã‚ÉˆÚ“®
        if (currentBackground.transform.position.y <= -backgroundHeight) {
            currentBackground.transform.position = new Vector3(
                currentBackground.transform.position.x,
                otherBackground.transform.position.y + backgroundHeight,
                currentBackground.transform.position.z
            );
        }
    }
}
