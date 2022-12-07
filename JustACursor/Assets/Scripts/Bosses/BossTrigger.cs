using UnityEngine;

namespace Bosses
{
    public class BossTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject bossGameObject;
        [SerializeField] private GameObject[] invisibleWalls;

        private void OnTriggerExit2D(Collider2D other)
        {
            bossGameObject.SetActive(true);
            foreach (GameObject wall in invisibleWalls)
            {
                wall.SetActive(true);
            }
        }
    }
}