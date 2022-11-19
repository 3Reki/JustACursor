using UnityEngine;

public class CanvasFocus : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Update()
    {
        transform.position = target.position;
    }
}
