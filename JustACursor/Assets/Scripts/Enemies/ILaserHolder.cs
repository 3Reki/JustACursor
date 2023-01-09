namespace Enemies
{
    public interface ILaserHolder
    {
        public void StartFire(float previewDuration, float laserDuration, float laserWidth, float laserLength);
        public void CeaseFire();
    }
}