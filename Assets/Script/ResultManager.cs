using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResultManager : MonoBehaviour
{
    public Text scoreText; // スコアを表示するテキスト
    public Text scoreText2; // スコアを表示するテキスト
    public GameObject pressStartText; // "PRESS START KEY"を表示するテキスト
    private bool isPressStartVisible = true; // "PRESS START KEY"の明滅制御用フラグ
    private float blinkInterval = 0.5f; // 明滅の間隔
    private float nextBlinkTime; // 次に明滅を切り替える時間
    bool isShow=false;

    void Start()
    {
        scoreText.enabled = false;
        scoreText2.enabled = false;

        // 明滅タイミングの初期化
        nextBlinkTime = Time.time + blinkInterval;
        StartCoroutine(Result());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();  // ESCキーでアプリ終了
        }
        if (isShow)
        {
            HandleBlinking(); // "PRESS START KEY"の明滅制御
            HandleSceneTransition(); // シーン遷移の制御
        }
    }

    private void HandleBlinking()
    {
        // 一定時間ごとに明滅を切り替える
        if (Time.time >= nextBlinkTime)
        {
            isPressStartVisible = !isPressStartVisible; // 表示/非表示を切り替え
            pressStartText.gameObject.SetActive(isPressStartVisible); // テキストの表示状態を切り替え
            nextBlinkTime = Time.time + blinkInterval; // 次の明滅タイミングを設定
        }
    }

    private void HandleSceneTransition()
    {
        if (Input.GetKeyDown(KeyCode.S))
        { 
            SceneManager.LoadScene("TitleScene"); // タイトルシーンに遷移
        }
    }

    private IEnumerator Result()
    {
        yield return new WaitForSeconds(1); // 
        scoreText.text = "あなたの獲得スコアは: " ; // スコアをテキストに反映
        scoreText.enabled = true;

        yield return new WaitForSeconds(1); // 
        scoreText2.text = GameManager.Instance.GetScore() + "点"; // スコアをテキストに反映
        scoreText2.enabled = true;
        isShow = true;

    }

}
