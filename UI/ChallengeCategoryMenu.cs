using ChallengeMod.Data;
using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System.Linq;
using UnityEngine;

namespace ChallengeMod.UI
{
    public class ChallengeCategoryMenu : KLMenu<PauseMenuAction>
    {
        private const int CHALLENGES_PER_PAGE = 3;

        public ChallengeCategoryMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private int _categoryIndex;
        private int _currentPage = 0;
        private int _numPages;

        private bool _titleSelected = false;

        public override void Setup(int playerId)
        {
            _categoryIndex = UIManager.Challenges.FindIndex((c) => c.Item1.Id == UIManager.SelectedCategory.Id);
            _numPages = Mathf.Max(1, (UIManager.Challenges[_categoryIndex].Item2.Count + CHALLENGES_PER_PAGE - 1) / CHALLENGES_PER_PAGE);

            ModuleList.Clear();

            var enumerated = Enumerable.Range(0, UIManager.Challenges.Count).ToList();
            var titleOption = new Option<int>(
                enumerated,
                _categoryIndex,
                enumerated.Select(i => $"{UIManager.Challenges[i].Item1.Icon} {UIManager.Challenges[i].Item1.Name}").ToList()
            );
            titleOption.OnChanged += (_, i) =>
            {
                _titleSelected = true;
                SetCategory(i);
            };
            var titleSelect = AddSelect(titleOption);

            AddLabel($"Completed: 10/20").SetSize(DefaultElementSize.x * 1.5f, DefaultElementSize.y);
            var description = AddInfo(UIManager.SelectedCategory.Description);
            description.SetSize(DefaultElementSize.x * 1.5f, description.BoundingBox.size.y);
            New<SpacerElement>();

            foreach (var challenge in UIManager.Challenges[_categoryIndex].Item2.Skip(CHALLENGES_PER_PAGE * _currentPage).Take(CHALLENGES_PER_PAGE))
            {
                var localisation = Challenges.GetLocalisation(challenge);
                AddLabel(localisation.Name).SetSize(DefaultElementSize.x * 1.5f, DefaultElementSize.y);
                var info = AddInfo(localisation.Description);
                info.SetSize(DefaultElementSize.x * 1.5f, info.BoundingBox.size.y);
            }

            enumerated = Enumerable.Range(0, _numPages).ToList();
            var pageSelectorOption = new Option<int>(
                enumerated,
                _currentPage,
                enumerated.Select(n => $"Page {n + 1}/{_numPages}").ToList()
            );
            pageSelectorOption.OnChanged += (_, n) =>
            {
                _titleSelected = false;
                SetPage(n);
            };
            var pageSelect = AddSelect(pageSelectorOption);

            AddActionButton("Back", PauseMenuAction.Back);

            ModuleList.Select(_titleSelected ? titleSelect : pageSelect);
        }

        public void SetCategory(int category)
        {
            UIManager.SelectedCategory = UIManager.Challenges[category].Item1;
            _currentPage = 0;
            RequestSubMenu(typeof(ChallengeCategoryMenu), true);
        }

        public void SetPage(int page)
        {
            _currentPage = page;
            RequestSubMenu(typeof(ChallengeCategoryMenu), true);
        }
    }
}
