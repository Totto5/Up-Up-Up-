using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SceneManager))]
public class PauseMenu : MonoBehaviour
{
    // PLAYボタン
    [SerializeField] private GameObject play;
    // OPTIONSボタン
    [SerializeField] private GameObject options;
    // RESTARTボタン
    [SerializeField] private GameObject restart;
    // TITLEボタン
    [SerializeField] private GameObject title;
    // EXITボタン
    [SerializeField] private GameObject exit;
    // PAUSEボタン
    [SerializeField] private GameObject pause;
    // PAUSEManu
    [SerializeField] private GameObject pauseMenu;
    
    private Color defaultColor = Color.black; // デフォルトのテキストカラー
    private Color selectedColor = Color.red; // 選択時のテキストカラー
    
    // Pause状態を管理するフラグ
    private bool _isPaused = false;

    public void OnClick()
    {
        // トグル式にPause状態を切り替える
        _isPaused = !_isPaused;
        
        // 選択中のボタンを取得
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;
        
        // 全てのボタンのテキストカラーをリセット
        ResetButtonColors();
        
        if (selectedButton != null)
        {
            // 選択されたボタンのテキストカラーを変更
            TextMeshProUGUI buttonText = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.color = selectedColor;
            }
        }
        
        
        // 選択中のボタンに応じて処理を分岐
        if (selectedButton == play)
        {
            // Pause画面を解除する
            Debug.Log("Play button clicked");
            // 例えば、PauseMenuを非表示にする(再開)
            pauseMenu.SetActive(false);
            pause.SetActive(true);
            // ゲームを再開する
            Time.timeScale = 1f;
        }
        else if (selectedButton == options)
        {
            // Optionsボタンが押されたときの処理
            Debug.Log("Options button clicked");
            // ここにOptionsボタンが押されたときの処理を書く
            // オプションメニューを表示
            // options.SetActive(true);
            
        }
        else if (selectedButton == restart)
        {
            // Restartボタンが押されたときの処理
            Debug.Log("Restart button clicked");
            // 現在のシーンを再読み込みする
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (selectedButton == title)
        {
            // Title画面に戻る。
            Debug.Log("Title button clicked");
            // Title画面に戻る処理
            SceneManager.LoadScene("TitleScene");
        }
        else if (selectedButton == exit)
        {
            // Exitボタンが押されたときの処理
            Debug.Log("Exit button clicked");
            // Exitボタンが押されたときの処理
            #if UNITY_EDITOR
            // Unityエディターでの動作
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // 実際のゲーム終了処理
            Application.Quit();
            #endif
        }
        else if (selectedButton == pause)
        {
            // Pauseボタンが押されたときの処理
            Debug.Log("Pause button clicked");
            
            if (_isPaused)
            {
                
                // 選択状態をPLAYボタンに設定
                EventSystem.current.SetSelectedGameObject(play);
                // PauseMenuを表示する
                pauseMenu.SetActive(true);
                // Pauseボタンを非表示にする
                pause.SetActive(false);
                // ゲームを一時停止する
                Time.timeScale = 0f;
            }
            else if (!_isPaused)
            {
                // PauseMenuを非表示にする
                pauseMenu.SetActive(false);
                // ゲームを再開する
                Time.timeScale = 1f;
            }
            
        }
    }
    
    private void ResetButtonColors()
    {
        // 全てのボタンのテキストカラーをデフォルトに戻す
        ResetButtonColor(play);
        ResetButtonColor(options);
        ResetButtonColor(restart);
        ResetButtonColor(title);
        ResetButtonColor(exit);
        ResetButtonColor(pause);
    }

    private void ResetButtonColor(GameObject button)
    {
        TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.color = defaultColor;
        }
    }
}
