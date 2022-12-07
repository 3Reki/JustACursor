using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component {

        public static T Instance;

        public virtual void Awake() {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
        
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}