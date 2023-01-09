using UnityEngine;

namespace Bosses.Patterns
{
    public abstract class Pat_AreaOfEffect<T> : Pattern<T>
    {
        [SerializeField] private GameObject aoePrefab;
        [Min(0)]
        [SerializeField] private float previewDuration;

        private float previewProgress;
        private GameObject aoeGameObject;

        private bool statePreview;

        public override void Play(T entity)
        {
            base.Play(entity);
            aoeGameObject = InstantiateAoE(aoePrefab);
            previewProgress = previewDuration;
            aoeGameObject.transform.GetChild(0).gameObject.SetActive(true);
            statePreview = true;
        }

        public override void Update()
        {
            base.Update();
            if (!statePreview) return;
            
            previewProgress -= Time.deltaTime * Energy.GameSpeed;

            if (!(previewProgress <= 0)) return;
            
            statePreview = false;
            aoeGameObject.transform.GetChild(0).gameObject.SetActive(false);
            aoeGameObject.transform.GetChild(1).gameObject.SetActive(true);
        }

        public override void Stop()
        {
            base.Stop();
            Destroy(aoeGameObject);
        }

        protected abstract GameObject InstantiateAoE(GameObject prefab);
    }
}