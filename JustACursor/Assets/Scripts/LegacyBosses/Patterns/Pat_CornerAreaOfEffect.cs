using LD;
using UnityEngine;

namespace LegacyBosses.Patterns
{
    public class Pat_CornerAreaOfEffect : Pat_AreaOfEffect<Boss>
    {
        [SerializeField] private Room.Quarter corner;
        [SerializeField] private float sizeMultiplier = 1f;
        
        protected override GameObject InstantiateAoE(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, linkedEntity.mover.Room.GetCorner(corner), Quaternion.identity,
                linkedEntity.transform);
            go.transform.localScale *= sizeMultiplier;

            return go;
        }
    }
}