using Player;
using UnityEngine;

public class Tourniquet : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private float currentAngle;

    private void Update()
    {
        currentAngle += rotationSpeed * PlayerEnergy.GameSpeed * Time.deltaTime;
        if (currentAngle >= 360) currentAngle -= 360;
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
