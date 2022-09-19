using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : MonoBehaviour
{
    public static LoadingSceneController sharedInstance = null;
    static string nextScene;

    TSVLoader tsv;
    GoogleSheetManager gs;

    bool bGoodToGo = false;

    [SerializeField]
    Image progressBar;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    private void Start()
    {
        if (sharedInstance != null && sharedInstance != this)
            Destroy(gameObject);
        else
            sharedInstance = this;

        //nextScene = "MainScene";
        nextScene = "testScene";
        DontDestroyOnLoad(this);
        tsv = gameObject.AddComponent<TSVLoader>();
        gs = gameObject.AddComponent<GoogleSheetManager>();

        gs.recall = true;
        StartCoroutine(LoadSceneProgress());
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "testScene" ||
            SceneManager.GetActiveScene().name == "MainScene")
        {
            GameManager.sharedInstance.setTsvData(tsv.getSavedData());
            GameManager.sharedInstance.getPenguriManager().DEBUG__Start();
            Destroy(gameObject);
        }
    }

    public void setGoodToGo(bool _gtg)
    {
        bGoodToGo = _gtg;
    }

    IEnumerator LoadSceneProgress()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return new WaitUntil(() => bGoodToGo == true);

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
