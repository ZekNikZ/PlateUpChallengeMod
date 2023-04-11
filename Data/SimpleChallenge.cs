namespace ChallengeMod.Data
{
    public class SimpleChallenge : BaseChallenge
    {
        internal bool _complete = false;
        
        internal SimpleChallenge(string id, bool complete) : base(ChallengeType.Simple, id, default) {
            _complete = complete;
        }

        public SimpleChallenge(string id) : base(ChallengeType.Simple, id, default) { }

        public SimpleChallenge(string id, ChallengeCategory category) : base(ChallengeType.Simple, id, category) { }

        public SimpleChallenge(string id, ChallengeCategory category, int order) : base(ChallengeType.Simple, id, category, order) { }

        public void Complete()
        {
            _complete = true;
        }

        public override bool IsComplete()
        {
            return _complete;
        }

        public override void MergeProgress(BaseChallenge other)
        {
            if (other is SimpleChallenge simpleChallenge)
            {
                _complete = _complete || simpleChallenge._complete;
            }
        }

        public override float Progress()
        {
            return _complete ? 1 : 0;
        }
    }
}
