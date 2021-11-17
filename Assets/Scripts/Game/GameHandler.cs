using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    public UnityEvent startEvent;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }

    // We use a coroutine to be able to use wait time
    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.25f);

        startEvent.Invoke();
    }

    public void OnPlayerWin()
    {
        int sceneCount = SceneManager.sceneCount;
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene == sceneCount ? 0 : currentScene + 1;

        SceneManager.LoadScene(nextScene);
    }

    public void OnPlayerDied()
    {
        StartCoroutine(PlayerDied());
    }

    IEnumerator PlayerDied()
    {
        GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
