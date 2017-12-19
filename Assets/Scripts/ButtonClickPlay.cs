using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class ButtonClickPlay : MonoBehaviour
{
    public Button yourButtonPlay;
    public Button yourButtonExit;
    private AssetBundle myLoadedAssetBundle;
    private string[] scenePaths;

    void Start()
    {
        Button btnPlay = yourButtonPlay.GetComponent<Button>();
        btnPlay.onClick.AddListener(TaskOnClickPlay);

        Button btnExit = yourButtonExit.GetComponent<Button>();
        btnExit.onClick.AddListener(TaskOnClickExit);
    }

    void TaskOnClickPlay()
    {
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene("Main");
    }

    void TaskOnClickExit()
    {
        Debug.Log("You have clicked the button!");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


}