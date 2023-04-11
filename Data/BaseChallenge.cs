namespace ChallengeMod.Data
{
    public abstract class BaseChallenge
    {
        public ChallengeType Type { get; private set; }
        public string Id { get; private set; }
        public ChallengeCategory Category { get; private set; }
        public int Order { get; private set; }

        public BaseChallenge(ChallengeType type, string id, ChallengeCategory category) : this(type, id, category, 0) { }

        public BaseChallenge(ChallengeType type, string id, ChallengeCategory category, int order)
        {
            Type = type;
            Id = id;
            Category = category;
            Order = order;
        }

        public abstract void MergeProgress(BaseChallenge other);

        public abstract bool IsComplete();

        public abstract float Progress();
    }
}
