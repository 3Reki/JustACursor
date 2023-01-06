using UnityEngine;

namespace LegacyBosses.Patterns
{
    public class Pat_RandomAreaOfEffect : Pattern<Boss>
    {
        [SerializeField] private GameObject aoePrefab;
        [Min(0)]
        [SerializeField] private float previewDuration;

        private float previewProgress;
        private readonly GameObject[] aoeGameObject = new GameObject[5];

        private bool statePreview;

        public override void Play(Boss entity)
        {
            base.Play(entity);
            for (int i = 0; i < 5; i++)
            {
                aoeGameObject[i] = InstantiateAoE(aoePrefab);
                aoeGameObject[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            
            previewProgress = previewDuration;
            
            statePreview = true;
        }

        public override void Update()
        {
            base.Update();
            if (!statePreview) return;
            
            previewProgress -= Time.deltaTime * Energy.GameSpeed;

            if (!(previewProgress <= 0)) return;
            
            statePreview = false;
            for (int i = 0; i < 5; i++)
            {
                aoeGameObject[i].transform.GetChild(0).gameObject.SetActive(false);
                aoeGameObject[i].transform.GetChild(1).gameObject.SetActive(true);
            }
            
        }

        public override void Stop()
        {
            for (int i = 0; i < 5; i++)
            {
                Destroy(aoeGameObject[i]);
            }
            
        }

        private GameObject InstantiateAoE(GameObject prefab)
        {
            Vector2 topLeft = linkedEntity.mover.Room.topLeft;
            Vector2 bottomRight = linkedEntity.mover.Room.bottomRight;
            GameObject go = Instantiate(prefab, new Vector3(Random.Range(topLeft.x, bottomRight.x), Random.Range(bottomRight.y, topLeft.y)), Quaternion.identity,
                linkedEntity.transform);
            go.transform.localScale *= 3;

            return go;
        }
    }
}