namespace LegacyBosses.Conditions
{
    public interface ICondition
    {
        public bool Check(Boss boss);
    }

    public enum Entity
    {
        Player,
        Boss
    }
        
    public enum RelativePosition
    {
        Inside,
        Outside
    }
}