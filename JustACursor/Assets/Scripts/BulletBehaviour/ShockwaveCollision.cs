using BulletPro;
using Player;
using UnityEngine;

// This script is supported by the BulletPro package for Unity.
// Template author : Simon Albou <albou.simon@gmail.com>

// This script is actually a MonoBehaviour for coding advanced things with Bullets.
namespace BulletBehaviour
{
	public class ShockwaveCollision : BaseBulletBehaviour
	{
		private PlayerCollision player;
		private float size;

		// You can access this.bullet to get the parent bullet script.
		// After bullet's death, you can delay this script's death : use this.lifetimeAfterBulletDeath.

		// Use this for initialization (instead of Start)
		public override void OnBulletBirth ()
		{
			base.OnBulletBirth();

			size = bullet.moduleCollision.GetCollider(0).size;
		}
	
		// Update is (still) called once per frame
		public override void Update ()
		{
			
		}

		// This gets called when the bullet dies
		public override void OnBulletDeath()
		{
			
		}

		// This gets called after the bullet has died, it can be delayed.
		public override void OnBehaviourDeath()
		{
			
		}

		// This gets called whenever the bullet collides with a BulletReceiver. The most common callback.
		public override void OnBulletCollision(BulletReceiver br, Vector3 collisionPoint)
		{

		}

		// This gets called whenever the bullet collides with a BulletReceiver AND was not colliding during the previous frame.
		public override void OnBulletCollisionEnter(BulletReceiver br, Vector3 collisionPoint)
		{
			
		}

		// This gets called whenever the bullet stops colliding with any BulletReceiver.
		public override void OnBulletCollisionExit()
		{
			
		}

		// This gets called whenever the bullet shoots a pattern.
		public override void OnBulletShotAnotherBullet(int patternIndex)
		{
			
		}

		public bool CheckCollision(Vector3 collisionPoint)
		{
			float distance = Vector3.Distance(collisionPoint,bullet.self.position) - bullet.moduleCollision.scale * size;
			return distance is < 0f and > -0.5f;
		}
	}
}
