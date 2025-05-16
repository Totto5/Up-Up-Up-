using UnityEngine;
using TMPro;

public class CountTimer : MonoBehaviour
{
    [SerializeField]private TMP_Text timerText; // TextMeshProのテキストオブジェクト
    private float _startTime; // タイマー開始時間
    private bool _isTimerRunning; // タイマーが実行中かどうか
    private float _elapsedTime; // 経過時間

    void Start()
    {
        // タイマーを開始
        StartTimer();
    }

    void Update()
    {
        if (_isTimerRunning)
        {
            // 経過時間を更新
            _elapsedTime = Time.time - _startTime;

            // テキストUIに表示する形式にフォーマット
            string hours = ((int)_elapsedTime / 3600).ToString("00");
            string minutes = ((int)_elapsedTime / 60).ToString("00");
            string seconds = (_elapsedTime % 60).ToString("00");
            // string milliseconds = ((int)((_elapsedTime * 1000) % 1000) / 10).ToString("00");

            // テキストUIにタイマーを表示
            timerText.text = hours + ":" + minutes + ":" + seconds;
        }
    }

    public void StartTimer()
    {
        // タイマーを開始
        _startTime = Time.time;
        _isTimerRunning = true;
    }

    public void StopTimer()
    {
        // タイマーを停止
        _isTimerRunning = false;
    }

    public float GetTimer()
    {
        return this._elapsedTime;
    }
}