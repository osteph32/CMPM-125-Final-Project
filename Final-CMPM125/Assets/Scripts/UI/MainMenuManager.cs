    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MainMenuController : MonoBehaviour
    {
        public string gameScene; // The scene to load when Play is clicked
        public GameObject mainMenuPanel;
        public GameObject optionsPanel;

        public void OpenOptions()
        {
            mainMenuPanel.SetActive(false);
            optionsPanel.SetActive(true);
        }

        public void CloseOptions()
        {
            optionsPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
        public void PlayGame()
        {
            SceneManager.LoadScene(gameScene);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }