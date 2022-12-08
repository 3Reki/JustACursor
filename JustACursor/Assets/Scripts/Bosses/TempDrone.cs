using UnityEngine;

namespace Bosses
{
    public class TempDrone : MonoBehaviour // TODO : Replace by Jolan's ones
    {
        [SerializeField] private Transform myTransform;

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            myTransform.SetPositionAndRotation(position, rotation);
        }
    }
}