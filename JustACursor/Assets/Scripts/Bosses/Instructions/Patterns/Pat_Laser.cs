namespace Bosses.Instructions.Patterns
{
    public class Pat_Laser : Pattern<Boss>
    {
        // public override void Play(Boss boss)
        // {
        //     base.Play(boss);
        // }
        //
        // public IEnumerator CustomFire(float previewDuration, float laserDuration, float customWidth, float customLength)
        // {
        //     InitLineRenderer(customWidth, customLength);
        //     InitNewColliders(customWidth, customLength);
        //
        //     yield return Fire(previewDuration, laserDuration);
        // }
        //
        // private void InitLineRenderer(float width, float length)
        // {
        //     lineRenderer.widthMultiplier = width;
        //     lineRenderer.positionCount = 2;
        //     lineRenderer.SetPosition(0,transform.position);
        //     lineRenderer.SetPosition(1,transform.position+transform.up*length);
        // }
        //
        // private void InitNewColliders(float width, float length)
        // {
        //     float colliderOffset = width/2;
        //     
        //     newColliders[0] = new BulletCollider
        //     {
        //         colliderType = BulletColliderType.Line,
        //         lineStart = new Vector2(-colliderOffset, 0),
        //         lineEnd = new Vector2(-colliderOffset, length)
        //     };
        //     
        //     newColliders[1] = new BulletCollider
        //     {
        //         colliderType = BulletColliderType.Line,
        //         lineEnd = new Vector2(0, length)
        //     };
        //     
        //     newColliders[2] = new BulletCollider
        //     {
        //         colliderType = BulletColliderType.Line,
        //         lineStart = new Vector2(colliderOffset, 0),
        //         lineEnd = new Vector2(colliderOffset, length)
        //     };
        // }
    }
}