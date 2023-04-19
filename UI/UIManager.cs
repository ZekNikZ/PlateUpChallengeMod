using ChallengeMod.Data;
using System.Collections.Generic;

namespace ChallengeMod.UI
{
    internal class UIManager
    {
        internal static List<(ChallengeCategory, List<BaseChallenge>)> Challenges;
        internal static ChallengeCategory SelectedCategory;
    }
}
