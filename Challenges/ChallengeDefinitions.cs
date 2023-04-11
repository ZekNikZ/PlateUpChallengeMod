using ChallengeMod.Data;
using ChallengeMod.Utils;
using KitchenLib;
using System.Collections.Generic;

namespace ChallengeMod
{
    internal partial class Mod : BaseMod
    {
        private void RegisterChallenges()
        {
            var foodCategory = this.RegisterChallengeCategory(new ChallengeCategory("food", "Master Chef", "Complete food-related tasks", @"<sprite name=""formal"">"));

            this.RegisterChallenge(new SimpleChallenge("test1", foodCategory));
            this.RegisterChallenge(new ProgressChallenge("test2", foodCategory, 5));
            this.RegisterChallenge(new MultipartChallenge("test3", foodCategory, new List<BaseChallenge>()
            {
                new SimpleChallenge("ketchup"),
                new SimpleChallenge("mustard"),
                new SimpleChallenge("soy_sauce")
            }));
        }
    }
}
