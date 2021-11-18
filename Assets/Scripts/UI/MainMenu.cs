using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnPlay()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OnQuit()
        {
            Application.Quit();
        }
    }
}