using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GoalArea : MonoBehaviour
{
    public CountTimer timer; // タイマースクリプトへの参照
    public CanvasGroup fadeCanvasGroup; // フェード用のCanvasGroup

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがゴールエリアに到達したらタイマーを停止
            timer.StopTimer();

            // ゴールエリアに到達したことをデバッグログで表示
            Debug.Log("Player reached the goal area. Timer stopped.");

            // クリアタイムを表示
            float clearTime = timer.GetTimer();
            
        }
    }
}