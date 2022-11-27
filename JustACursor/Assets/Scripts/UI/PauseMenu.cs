using UnityEngine;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        public void OnClickConfirmQuit() {
            Application.Quit();
        }
    }
}