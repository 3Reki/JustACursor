using UnityEngine;

namespace LD
{
    public enum Plan {Background, Foreground};
    
    public class RoomElement : MonoBehaviour
    {
        [field:SerializeField] public Plan RenderPlan;
    }
}
