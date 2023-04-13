namespace ChallengeMod.Data
{
    public static class BaseCategories
    {
        public static readonly ChallengeCategory Food = Challenges.RegisterCategory(new ChallengeCategory("food", "Master Chef", "Dish and food challenges.", "<sprite name=\"affordable\">"));
        public static readonly ChallengeCategory Automation = Challenges.RegisterCategory(new ChallengeCategory("automation", "Engineer", "Automation challenges.", "<sprite name=\"flask\">"));
        public static readonly ChallengeCategory Customer = Challenges.RegisterCategory(new ChallengeCategory("customer", "Crowd Control", "Customer challenges.", "<sprite name=\"queue\">"));
        public static readonly ChallengeCategory Layout = Challenges.RegisterCategory(new ChallengeCategory("layout", "Map Master", "Map and layout challenges.", "<sprite name=\"seating\">"));
        public static readonly ChallengeCategory Themes = Challenges.RegisterCategory(new ChallengeCategory("themes", "Thematic", "Theme and decoration challenges.", "<sprite name=\"flask\">"));
        public static readonly ChallengeCategory Franchise = Challenges.RegisterCategory(new ChallengeCategory("franchise", "Completionist", "Speed and franchise challenges.", "<sprite name=\"franchise_tier\">"));
        public static readonly ChallengeCategory Multiplayer = Challenges.RegisterCategory(new ChallengeCategory("multiplayer", "Friendly", "Multiplayer challenges.", "<sprite name=\"multiplayer_4\">"));
        public static readonly ChallengeCategory Streaming = Challenges.RegisterCategory(new ChallengeCategory("streaming", "Streamer", "Challenges to be completed between a streamer and their viewers.", "<sprite name=\"upgrade\">"));
        public static readonly ChallengeCategory Seasonal = Challenges.RegisterCategory(new ChallengeCategory("seasonal", "Seasonal",  "Challenges based on the current update.", "<sprite name=\"snow\">"));
        public static readonly ChallengeCategory Hidden = Challenges.RegisterCategory(new ChallengeCategory("hidden", "Mysterious", "Challenges with hidden criteria. A new challenge will appear every week.", "<sprite name=\"thinking\">"));
    }
}
