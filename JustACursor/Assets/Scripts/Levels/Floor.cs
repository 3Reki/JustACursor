using UnityEngine;

namespace Levels
{
    public class Floor : MonoBehaviour
    {
        [SerializeField] private LevelHandler levelHandler;

        public Transform StartPoint;
        [SerializeField] private BoxCollider2D endPoint;
    }
}