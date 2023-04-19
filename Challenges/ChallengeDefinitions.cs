using ChallengeMod.Data;
using ChallengeMod.Utils;
using KitchenData;
using KitchenLib;
using System.Collections.Generic;
using static ChallengeMod.Data.BaseCategories;

namespace ChallengeMod
{
    internal partial class Mod : BaseMod
    {
        private (Locale, ChallengeLocalisation) Loc(string name, string description)
        {
            return (Locale.Default, new ChallengeLocalisation { Name = name, Description = description });
        }

        private void RegisterChallenges()
        {
            this.RegisterChallenge(new SimpleChallenge("test1", Food), Loc("The most important meal", "Serve a full plate of breakfast."));
            this.RegisterChallenge(new ProgressChallenge("test2", Food, 1000), Loc("Pizza chef", "Serve 1000 plates of pizza."));
            this.RegisterChallenge(new MultipartChallenge("test3", Food, new List<BaseChallenge>()
            {
                new SimpleChallenge("ketchup"),
                new SimpleChallenge("mustard"),
                new SimpleChallenge("soy_sauce")
            }), Loc("Condiment connoisseur", "Serve each type of condiment."));

            this.RegisterChallenge(new SimpleChallenge("test4", Food), Loc("The most important meal", "Serve a full plate of breakfast."));
            this.RegisterChallenge(new ProgressChallenge("test5", Food, 1000), Loc("Pizza chef", "Serve 1000 plates of pizza."));
        }
    }
}
