using LD;
using UnityEngine;

namespace Bosses.Instructions.Patterns
{
    [CreateAssetMenu(fileName = "Pat_CornerAoE", menuName = "Just A Cursor/Pattern/Corner AoE", order = 0)]
    public class Pat_CornerAoE : Pat_AoE<Boss>
    {
        [SerializeField] private Room.Quarter corner;
        [SerializeField] private Vector3 size = Vector3.one;
        
        protected override GameObject InstantiateAoE(GameObject prefab)
        {
            GameObject go = Instantiate(prefab, linkedEntity.mover.GetCorner((int) corner), Quaternion.identity,
                linkedEntity.transform);
            go.transform.localScale = size;

            return go;
        }
    }
}