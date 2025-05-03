using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleMenuManager : MonoBehaviour
{
    // PLAYボタン
    [SerializeField] private GameObject play;
    // OPTIONSボタン
    [SerializeField] private GameObject options;
    // EXITボタン
    [SerializeField] private GameObject exit;
    
    void Start()
    {
        //開始時にPLAYボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(play);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Select中のボタンが押されたときの処理
    public void OnClick()
    {
        // 選択中のボタンを取得
        GameObject selectedButton = EventSystem.current.currentSelectedGameObject;

        // 選択中のボタンに応じて処理を分岐
        if (selectedButton == play)
        {
            Debug.Log("Play button clicked");
            // Playボタンが押されたときの処理
            // Stage1に遷移する
            // ここにStage1に遷移する処理を書く
            SceneManager.LoadScene("Stage1");
            
            // ここにPlayボタンが押されたときの処理を書く
        }
        else if (selectedButton == options)
        {
            Debug.Log("Options button clicked");
            // Optionsボタンが押されたときの処理
            // ここにOptionsボタンが押されたときの処理を書く
        }
        else if (selectedButton == exit)
        {
            Debug.Log("Exit button clicked");
            // Exitボタンが押されたときの処理
            // ここにExitボタンが押されたときの処理を書く
            #if UNITY_EDITOR
            // Unityエディターでの動作
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            // 実際のゲーム終了処理
            Application.Quit();
            #endif
        }
    }
}
