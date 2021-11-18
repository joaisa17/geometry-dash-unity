using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game
{
    public class GameHandler : MonoBehaviour
    {
        public UnityEvent startEvent;

        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(Init());
        }

        // We use a coroutine to be able to use wait time
        private IEnumerator Init()
        {
            yield return new WaitForSeconds(0.25f);

            startEvent.Invoke();
        }

        public void OnPlayerWin()
        {
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var currentScene = SceneManager.GetActiveScene().buildIndex;
            var nextScene = currentScene == sceneCount - 1 ? 0 : currentScene + 1;

            SceneManager.LoadScene(nextScene);
        }

        public void OnPlayerDied()
        {
            StartCoroutine(PlayerDied());
        }

        public IEnumerator PlayerDied()
        {
            GetComponent<AudioSource>().Stop();

            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}