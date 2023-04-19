using ChallengeMod.Data;
using ChallengeMod.Utils;
using Kitchen;
using Kitchen.Modules;
using KitchenLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ChallengeMod.UI
{
    public class ChallengesMenu : KLMenu<PauseMenuAction>
    {
        private const int CATEGORIES_PER_PAGE = 6;

        public ChallengesMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private int _currentPage = 0;
        private int _numPages;
        private bool _pageSelectUsed = false;

        public override void Setup(int playerId)
        {
            UIManager.Challenges = Challenges.AllChallenges().Select(el => (el.Key, el.Value)).OrderBy(el => el.Key.Order).ThenBy(el => el.Key.Id).ToList();
            _numPages = (UIManager.Challenges.Count + CATEGORIES_PER_PAGE - 1) / CATEGORIES_PER_PAGE;

            var titleLabel = AddLabel("Challenges");

            foreach (var (category, _) in UIManager.Challenges.Skip(CATEGORIES_PER_PAGE * _currentPage).Take(CATEGORIES_PER_PAGE))
            {
                AddButton($"{category.Icon} {category.Name}", _ =>
                {
                    UIManager.SelectedCategory = category;
                    RequestSubMenu(typeof(ChallengeCategoryMenu));
                });
            }

            New<SpacerElement>();

            var enumerated = Enumerable.Range(0, _numPages).ToList();
            var pageSelectorOption = new Option<int>(
                enumerated,
                _currentPage,
                enumerated.Select(n => $"Page {n + 1}/{_numPages}").ToList()
            );
            pageSelectorOption.OnChanged += (_, n) =>
            {
                _pageSelectUsed = true;
                SetPage(n);
            };
            var pageSelect = AddSelect(pageSelectorOption);

            AddActionButton("Back", PauseMenuAction.Back);

            if (_pageSelectUsed)
            {
                ModuleList.Select(pageSelect);
            }
        }

        public override void CreateSubmenus(ref Dictionary<Type, Menu<PauseMenuAction>> menus)
        {
            menus.Add(typeof(ChallengeCategoryMenu), new ChallengeCategoryMenu(Container, ModuleList));
        }

        public void SetPage(int page)
        {
            _currentPage = page;
            RequestSubMenu(typeof(ChallengesMenu), true);
        }
    }
}