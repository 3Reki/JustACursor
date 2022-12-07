using Scene;
using UnityEngine;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField, Scene] private string mainMenuScene; 
        [SerializeField, Scene] private string gameScene; 
        
        public void OnClickPlay()
        {
            SceneController.Instance.UnloadScene(mainMenuScene);
            SceneController.Instance.LoadScene(gameScene);
            SceneController.Instance.Process();
        }

        public void OnClickContinue()
        {
            
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }
    }
}