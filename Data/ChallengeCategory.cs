namespace ChallengeMod.Data
{
    public struct ChallengeCategory
    {
        public string Id;
        public string Name;
        public string Description;
        public string Icon;
        public int Order;

        public ChallengeCategory(string id, string name, string description, string icon) : this(id, name, description, icon, 0) { }

        public ChallengeCategory(string id, string name, string description, string icon, int order)
        {
            Id = id;
            Name = name;
            Description = description;
            Icon = icon;
            Order = order;
        }
    }
}
