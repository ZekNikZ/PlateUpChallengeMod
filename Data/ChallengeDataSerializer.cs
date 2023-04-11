using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ChallengeMod.Data
{
    internal abstract class ChallengeDataSerializer
    {
        internal enum Version
        {
            V1_JSON
        }

        private static readonly Dictionary<Version, ChallengeDataSerializer> Serializers = new()
        {
            { Version.V1_JSON, new V1_JSON() }
        };

        private const Version PreferredVersion = Version.V1_JSON;

        protected abstract bool Load(string fileName, out Challenges data);

        protected abstract bool Save(string fileName, Challenges data);

        protected string ReadFileString(string fileName)
        {
            return File.ReadAllText(fileName);
        }
        protected void WriteFileString(string fileName, string contents)
        {
            File.WriteAllText(fileName, contents);
        }

        public static bool TryLoad(string fileName, out Challenges data)
        {
            foreach (var serilizer in Serializers.Values)
            {
                if (serilizer.Load(fileName, out data))
                {
                    return true;
                }
            }

            data = null;
            return false;
        }

        public static bool TrySave(string fileName, Challenges data)
        {
            return Serializers[PreferredVersion].Save(fileName, data);
        }

        private class V1_JSON : ChallengeDataSerializer
        {
            private struct ChallengeData
            {
                public struct ChallengeEntry
                {
                    public string id;
                    public ChallengeType type;
                    public bool? complete;
                    public float? progress;
                    public float? max;
                    public List<ChallengeEntry> subChallenges;
                }

                public List<ChallengeEntry> challenges;

                private static ChallengeEntry MakeEntry(BaseChallenge challenge)
                {
                    var res = new ChallengeEntry
                    {
                        id = challenge.Id,
                        type = challenge.Type
                    };

                    if (challenge is SimpleChallenge simple)
                    {
                        res.complete = simple._complete;
                    }
                    else if (challenge is ProgressChallenge progress)
                    {
                        res.progress = progress._progress;
                        res.max = progress._max;
                    }
                    else if (challenge is MultipartChallenge multipart)
                    {
                        res.subChallenges = multipart._subChallenges.Values.Select(MakeEntry).ToList();
                    }
                    else
                    {
                        Mod.LogError($"Cannot serialize challenge with ID '{challenge.Id}'");
                        return default;
                    }

                    return res;
                }

                private static BaseChallenge MakeChallenge(ChallengeEntry challenge)
                {
                    switch (challenge.type)
                    {
                        case ChallengeType.Simple:
                            return new SimpleChallenge(challenge.id, challenge.complete ?? false);
                        case ChallengeType.Progress:
                            return new ProgressChallenge(challenge.id, challenge.progress ?? 0, challenge.max ?? 0);
                        case ChallengeType.Multipart:
                            return new MultipartChallenge(challenge.id, challenge.subChallenges.Select(MakeChallenge).ToList());
                        default:
                            Mod.LogError($"Cannot deserialize challenge with type '{challenge.type}'");
                            return null;
                    }
                }

                public static implicit operator ChallengeData(Challenges data)
                {
                    return new ChallengeData
                    {
                        challenges = data._challenges.Values
                            .Select(MakeEntry)
                            .Where(x => x.id != null)
                            .ToList()
                    };
                }

                public static implicit operator Challenges(ChallengeData data)
                {
                    return new Challenges()
                    {
                        _challenges = data.challenges.Select(MakeChallenge).ToDictionary(ch => ch.Id, ch => ch)
                    };
                }
            }

            protected override bool Load(string fileName, out Challenges data)
            {
                try
                {
                    var fileContents = ReadFileString(fileName);
                    data = JsonConvert.DeserializeObject<ChallengeData>(fileContents);
                    return true;
                }
                catch
                {
                    data = null;
                    return false;
                }
            }

            protected override bool Save(string fileName, Challenges data)
            {
                try
                {
                    ChallengeData dto = data;
                    var serializedString = JsonConvert.SerializeObject(dto, Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
                    WriteFileString(fileName, serializedString);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
