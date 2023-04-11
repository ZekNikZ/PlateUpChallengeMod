using ChallengeMod.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ChallengeMod.Data
{
    public class MultipartChallenge : BaseChallenge
    {
        internal readonly Dictionary<string, BaseChallenge> _subChallenges;

        internal MultipartChallenge(string id, List<BaseChallenge> subChallenges) : base(ChallengeType.Multipart, id, default)
        {
            _subChallenges = subChallenges.ToDictionary(c => c.Id, c => c);
        }

        public MultipartChallenge(string id, ChallengeCategory category, List<BaseChallenge> subChallenges) : base(ChallengeType.Multipart, id, category) {
            _subChallenges = subChallenges.ToDictionary(c => c.Id, c => c);
        }

        public MultipartChallenge(string id, ChallengeCategory category, int order, List<BaseChallenge> subChallenges) : base(ChallengeType.Multipart, id, category, order) {
            _subChallenges = subChallenges.ToDictionary(c => c.Id, c => c);
        }

        public BaseChallenge GetSubChallenge(string id)
        {
            return _subChallenges.GetSafe(id);
        }

        public T GetSubChallenge<T>(string id) where T : BaseChallenge
        {
            return GetSubChallenge(id) as T;
        }

        public override bool IsComplete()
        {
            return _subChallenges.Values.All(c => c.IsComplete());
        }

        public override void MergeProgress(BaseChallenge other)
        {
            foreach (var (key, value) in _subChallenges)
            {
                if (key == other.Id)
                {
                    value.MergeProgress(other);
                }
            }
        }

        public override float Progress()
        {
            return (float) _subChallenges.Values.Count(c => c.IsComplete()) / _subChallenges.Count;
        }
    }
}
