using ChallengeMod.Data;
using KitchenData;
using KitchenLib;

namespace ChallengeMod.Utils
{
    public static class RegistrationUtils
    {
        public static ChallengeCategory RegisterChallengeCategory(ChallengeCategory category)
        {
            return Challenges.RegisterCategory(category);
        }

        public static ChallengeCategory RegisterChallengeCategory(this BaseMod mod, ChallengeCategory category)
        {
            return RegisterChallengeCategory(category);
        }

        public static BaseChallenge RegisterChallenge(BaseChallenge challenge, params (Locale, ChallengeLocalisation)[] localisation)
        {
            var ch = Challenges.RegisterChallenge(challenge);

            foreach (var (locale, localisationData) in localisation)
            {
                RegisterChallengeLocalisation(challenge.Id, locale, localisationData);
            }

            return ch;
        }

        public static BaseChallenge RegisterChallenge(this BaseMod mod, BaseChallenge challenge, params (Locale, ChallengeLocalisation)[] localisation)
        {
            return RegisterChallenge(challenge, localisation);
        }

        public static void RegisterChallengeLocalisation(string challengeId, Locale locale, ChallengeLocalisation localisation)
        {
            Challenges.RegisterLocalisation(challengeId, locale, localisation);
        }

        public static void RegisterChallengeLocalisation(this BaseMod mod, string challengeId, Locale locale, ChallengeLocalisation localisation)
        {
            RegisterChallengeLocalisation(challengeId, locale, localisation);
        }
    }
}
