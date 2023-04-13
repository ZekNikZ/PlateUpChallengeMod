using ChallengeMod.Data;
using TMPro;
using UnityEngine;

namespace ChallengeMod.UI
{
    internal class ChallengeCardUIController : MonoBehaviour
    {
        public TextMeshProUGUI TitleText;
        public TextMeshProUGUI DescriptionText;

        public BaseChallenge Challenge;

        private void Update()
        {
            if (Challenge == null) return;

            var localisation = Challenges.GetLocalisation(Challenge);

            TitleText.text = localisation.Name;
            DescriptionText.text = localisation.Description;
        }
    }
}
