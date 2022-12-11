using UnityEngine;

namespace LD
{
    public enum Plan {Background, Foreground};
    
    public class FloorSort : MonoBehaviour
    {
        [field:SerializeField] public Plan RenderPlan;
    }
}
