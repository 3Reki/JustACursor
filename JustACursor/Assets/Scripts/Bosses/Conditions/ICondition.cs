namespace Bosses.Conditions
{
    public interface ICondition
    {
        public bool Check(Boss boss);
    }
    
    public enum ConditionType
    {
        None,
        HealthThreshold,
        CornerDistance,
        CenterDistance,
        BossDistance
    }
    
    public enum Entity
    {
        Player,
        Boss
    }
        
    public enum RelativePosition
    {
        CloserThan,
        FartherThan
    }
}