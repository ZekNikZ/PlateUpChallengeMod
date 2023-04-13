using ChallengeMod.Data;
using TMPro;
using UnityEngine;

namespace ChallengeMod.UI
{
    internal class TabUIController : MonoBehaviour
    {
        public TextMeshProUGUI IconText;

        public ChallengeCategory Category = BaseCategories.Food;
        public bool Selected = false;

        private void Update()
        {
            if (Category.Id == null) return;

            IconText.text = Category.Icon ?? "X";
        }
    }
}
