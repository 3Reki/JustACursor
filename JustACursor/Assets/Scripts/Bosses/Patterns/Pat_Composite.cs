using UnityEngine;
using XNode;

namespace Bosses.Patterns
{
    [CreateAssetMenu(fileName = "Pat_Composite", menuName = "Just A Cursor/Pattern/Composite")]
    public class Pat_Composite : Pattern<Boss>
    {
        [Input(dynamicPortList = true, connectionType = ConnectionType.Override, backingValue = ShowBackingValue.Unconnected)] 
        [SerializeField] private Pattern<Boss>[] patterns;
        
        public override void Play(Boss entity)
        {
            base.Play(entity);

            {
                int i = 0;
                foreach (NodePort dynamicInput in DynamicInputs)
                {
                    patterns[i] = (Pattern<Boss>) dynamicInput.Connection.node;
                    i++;
                }
            }
            
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Play(entity);
            }
        }

        public override void Update()
        {
            foreach (Pattern<Boss> pattern in patterns)
            {
                if (pattern.currentState != State.Update)
                    return;
                
                pattern.Update();
            }

            foreach (Pattern<Boss> pattern in patterns)
            {
                if (pattern.currentState != State.Stop)
                    return;
            }

            currentState = State.Stop;
        }

        public override void Stop()
        {
            base.Stop();
            foreach (Pattern<Boss> pattern in patterns)
            {
                pattern.Stop();
            }
        }
    }
}