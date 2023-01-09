namespace GameResources.Stats.Scripts
{
    public interface IStatHandler
    {
        public int NextLevelCost { get; }
        public int Value { get; }
        
        public string Description { get; }
        public Stat[] Levels { get; }

        public void AddLevel(int value);
    }
}
