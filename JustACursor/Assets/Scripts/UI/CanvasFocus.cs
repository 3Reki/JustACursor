using UnityEngine;

namespace UI
{
    public class CanvasFocus : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Update()
        {
            transform.position = target.position;
        }
    }
}
