using UnityEngine;
using Utils;

namespace Scene
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField, Scene] private string firstScene;

        void Start() {
            SceneController.Instance.LoadScene(firstScene);
            SceneController.Instance.Process();
        }
    }
}
