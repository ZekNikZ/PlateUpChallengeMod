using System;
using System.Collections.Generic;
using System.Linq;
using ChallengeMod.Utils;
using KitchenData;
using UnityEngine;

namespace ChallengeMod.Data
{
    internal class Challenges
    {
        private static readonly string DATA_FILE_PATH = Application.persistentDataPath + "/UserData/challenges.dat";

        internal Dictionary<string, ChallengeCategory> _categories = new();
        internal Dictionary<string, BaseChallenge> _challenges = new();
        internal Dictionary<string, Dictionary<Locale, ChallengeLocalisation>> _localisation = new();

        internal static Challenges Instance = new();

        internal BaseChallenge GetChallengeById(string id)
        {
            return _challenges.GetSafe(id);
        }

        internal ChallengeCategory GetCategoryById(string id)
        {
            return _categories.GetSafe(id);
        }

        public static BaseChallenge GetChallenge(string id)
        {
            return Instance.GetChallengeById(id);
        }

        public static ChallengeCategory GetCategory(string id)
        {
            return Instance.GetCategoryById(id);
        }

        public static T GetChallenge<T>(string id) where T : BaseChallenge
        {
            return GetChallenge(id) as T;
        }

        public static void CompleteSimple(string challengeId)
        {
            GetChallenge<SimpleChallenge>(challengeId)?.Complete();
        }

        public static void AddProgress(string challengeId, float progress)
        {
            GetChallenge<ProgressChallenge>(challengeId)?.AddProgress(progress);
        }

        public static void CompleteSubChallenge(string challengeId, string subChallengeId)
        {
            GetChallenge<MultipartChallenge>(challengeId)?.GetSubChallenge<SimpleChallenge>(subChallengeId)?.Complete();
        }

        public static void AddProgressToSubChallenge(string challengeId, string subChallengeId, float progress)
        {
            GetChallenge<MultipartChallenge>(challengeId)?.GetSubChallenge<ProgressChallenge>(subChallengeId)?.AddProgress(progress);
        }

        internal ChallengeCategory AddCategory(ChallengeCategory category)
        {
            if (_categories.ContainsKey(category.Id))
            {
                Mod.LogError($"Category with ID '{category.Id}' has already been registered.");
                return default;
            }

            _categories.Add(category.Id, category);

            return category;
        }

        public static ChallengeCategory RegisterCategory(ChallengeCategory category)
        {
            return Instance.AddCategory(category);
        }

        internal BaseChallenge AddChallenge(BaseChallenge challenge)
        {
            if (_challenges.ContainsKey(challenge.Id))
            {
                Mod.LogError($"Challenge with ID '{challenge.Id}' has already been registered.");
                return null;
            }

            if (challenge.Category.Id == null)
            {
                Mod.LogError($"Challenge with ID '{challenge.Id}' does not have a category set.");
                return null;
            }

            _challenges.Add(challenge.Id, challenge);

            return challenge;
        }

        public static BaseChallenge RegisterChallenge(BaseChallenge challenge)
        {
            return Instance.AddChallenge(challenge);
        }

        internal void AddLocalisation(string challengeId, Locale locale, ChallengeLocalisation localisation)
        {
            if (!_localisation.ContainsKey(challengeId))
            {
                _localisation.Add(challengeId, new());
            }

            _localisation[challengeId].Add(locale, localisation);
        }

        public static void RegisterLocalisation(string challengeId, Locale locale, ChallengeLocalisation localisation)
        {
            Instance.AddLocalisation(challengeId, locale, localisation);
        }

        public static Dictionary<ChallengeCategory, List<BaseChallenge>> AllChallenges()
        {
            var res = MiscUtils.CreatePrepopulatedDictionary(Instance._categories.Values, () => new List<BaseChallenge>());

            foreach (var challenge in Instance._challenges.Values)
            {
                res[challenge.Category].Add(challenge);
            }

            return res.ToDictionary(el => el.Key, el => el.Value.OrderBy(ch => ch.Order).ThenBy(ch => ch.Id).ToList());
        }

        internal static ChallengeLocalisation GetLocalisation(string challengeId)
        {
            if (Instance._localisation.TryGetValue(challengeId, out var localisation))
            {
                if (localisation.ContainsKey(Localisation.CurrentLocale))
                {
                    return localisation[Localisation.CurrentLocale];
                }
                else if (localisation.ContainsKey(Locale.Default))
                {
                    return localisation[Locale.Default];
                }
                else if (localisation.ContainsKey(Locale.English))
                {
                    return localisation[Locale.English];
                }
                else
                {
                    return localisation.Values.First();
                }
            }
            else
            {
                return default;
            }
        }

        internal static ChallengeLocalisation GetLocalisation(BaseChallenge challenge)
        {
            return GetLocalisation(challenge.Id);
        }

        internal void Merge(Challenges other, bool addMissingChallenges = false)
        {
            foreach (var (key, value) in other._challenges)
            {
                if (_challenges.ContainsKey(key))
                {
                    _challenges[key].MergeProgress(value);
                }
                else if (addMissingChallenges)
                {
                    _challenges.Add(key, value);
                }
            }
        }

        internal static void Load()
        {
            if (ChallengeDataSerializer.TryLoad(DATA_FILE_PATH, out var challenges))
            {
                Instance.Merge(challenges);
                Mod.LogInfo($"Sucessfully loaded challenge data from {DATA_FILE_PATH}");
            }
            else
            {
                Mod.LogError($"Could not load existing challenge data from {DATA_FILE_PATH}");
            }
        }

        internal static void Save()
        {
            if (ChallengeDataSerializer.TrySave(DATA_FILE_PATH, Instance))
            {
                Mod.LogInfo($"Sucessfully saved challenge data to {DATA_FILE_PATH}");
            }
            else
            {
                Mod.LogError($"Could not save challenge data to {DATA_FILE_PATH}");
            }
        }
    }
}
