using UnityEngine;

namespace ChallengeMod.Data
{
    public class ProgressChallenge : BaseChallenge
    {
        internal float _progress = 0;
        internal readonly float _max = 0;

        internal ProgressChallenge(string id, float progress, float max) : base(ChallengeType.Progress, id, default)
        {
            _progress = progress;
            _max = max;
        }

        public ProgressChallenge(string id, float max) : base(ChallengeType.Progress, id, default) {
            _max = max;
        }

        public ProgressChallenge(string id, ChallengeCategory category, float max) : base(ChallengeType.Progress, id, category) {
            _max = max;
        }

        public ProgressChallenge(string id, ChallengeCategory category, int order, float max) : base(ChallengeType.Progress, id, category, order) {
            _max = max;
        }

        public void AddProgress(float progress)
        {
            _progress = Mathf.Min(_max, _progress + progress);
        }

        public override bool IsComplete()
        {
            return _progress >= _max;
        }

        public override void MergeProgress(BaseChallenge other)
        {
            if (other is ProgressChallenge progressChallenge)
            {
                _progress = Mathf.Max(_progress, progressChallenge._progress);
            }
        }

        public override float Progress()
        {
            return Mathf.Clamp(_progress / _max, 0, 1);
        }
    }
}
