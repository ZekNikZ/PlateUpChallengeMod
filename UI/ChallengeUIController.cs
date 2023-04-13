using ChallengeMod.Data;
using KitchenLib.Utils;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ChallengeMod.UI
{
    internal class ChallengeUIController : MonoBehaviour
    {
        public Transform TabParent;
        public Transform ContentParent;
        public TextMeshProUGUI PageCounter;
        public GameObject Panel;

        private List<(ChallengeCategory, List<BaseChallenge>)> _data;
        private List<TabUIController> _tabs = new();
        private int _tabIndex = 0;
        private int _pageIndex = 0;

        private void Awake()
        {
            TabParent = gameObject.GetChild("Panel/Layout/Tab Layout").transform;
            ContentParent = gameObject.GetChild("Panel/Layout/Content Layout").transform;
            PageCounter = gameObject.GetChild("Panel/Layout/Info Row/Page Count").GetComponent<TextMeshProUGUI>();
            Panel = gameObject.GetChild("Panel");

            // Load data
            _data = Challenges.AllChallenges().Select(el => (el.Key, el.Value)).OrderBy(el => el.Key.Order).ThenBy(el => el.Key.Id).ToList();

            SetupTabs();
            SetupContent();
        }

        private void Update()
        {
            if (!UIManager.ChallengeUIVisible)
            {
                if (Panel.activeSelf)
                {
                    Panel.SetActive(false);
                }
                return;
            }
            else if (UIManager.ChallengeUIVisible && !Panel.activeSelf)
            {
                Panel.SetActive(true);
            }

            var up = false;
            var down = false;
            var left = false;
            var right = false;
            var back = false;
            var tabChanged = false;
            var contentChanged = false;

            // Detect keybinds
            foreach (var device in InputSystem.devices)
            {
                if (device is Keyboard keyboard)
                {
                    up |= keyboard.upArrowKey.wasPressedThisFrame || keyboard.wKey.wasPressedThisFrame;
                    down |= keyboard.downArrowKey.wasPressedThisFrame || keyboard.sKey.wasPressedThisFrame;
                    left |= keyboard.leftArrowKey.wasPressedThisFrame || keyboard.aKey.wasPressedThisFrame;
                    right |= keyboard.rightArrowKey.wasPressedThisFrame || keyboard.dKey.wasPressedThisFrame;
                    back |= keyboard.escapeKey.wasPressedThisFrame;
                }
                else if (device is Gamepad gamepad)
                {
                    up |= gamepad.dpad.up.wasPressedThisFrame || gamepad.leftStick.up.wasPressedThisFrame;
                    down |= gamepad.dpad.down.wasPressedThisFrame || gamepad.leftStick.down.wasPressedThisFrame;
                    left |= gamepad.dpad.left.wasPressedThisFrame || gamepad.leftStick.left.wasPressedThisFrame || gamepad.leftTrigger.wasPressedThisFrame || gamepad.leftShoulder.wasPressedThisFrame;
                    right |= gamepad.dpad.right.wasPressedThisFrame || gamepad.leftStick.right.wasPressedThisFrame || gamepad.rightTrigger.wasPressedThisFrame || gamepad.rightShoulder.wasPressedThisFrame;
                    back |= gamepad.bButton.wasPressedThisFrame || gamepad.circleButton.wasPressedThisFrame;
                }
            }

            // Apply actions
            if (up && !down)
            {
                _pageIndex = Mathf.Max(0, _pageIndex - 1);
                contentChanged = true;
            }
            else if (down && !up)
            {
                _pageIndex = Mathf.Min(_data[_tabIndex].Item2.Count / 6, _pageIndex + 1);
                contentChanged = true;
            }
            if (left && !right)
            {
                _tabIndex = Mathf.Max(0, _tabIndex - 1);
                tabChanged = true;
            }
            else if (right && !left)
            {
                _tabIndex = Mathf.Min(_data.Count - 1, _tabIndex + 1);
                tabChanged = true;
            }
            if (back)
            {
                UIManager.CloseChallengeUI();
            }

            // Redraw
            if (tabChanged)
            {
                RedrawTabs();
            }
            if (tabChanged || contentChanged)
            {
                RedrawContent();
            }
        }

        public void SetupTabs()
        {
            // Create the correct number of tabs
            foreach (Transform child in TabParent)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < _data.Count; i++)
            {
                var tab = Instantiate(Mod.Bundle.LoadAsset<GameObject>("TabButton"), TabParent);
                var tabUIController = tab.AddComponent<TabUIController>();
                tabUIController.IconText = tab.GetChild("Text").GetComponent<TextMeshProUGUI>();
                _tabs.Add(tabUIController);
            }

            RedrawTabs();
        }

        public void SetupContent()
        {
            // Create the correct number of content blocks
            foreach (Transform child in ContentParent)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < 6; i++)
            {
                var challengeCard = Instantiate(Mod.Bundle.LoadAsset<GameObject>("ChallengeCard"), ContentParent);
                var challengeCardUIController = challengeCard.AddComponent<ChallengeCardUIController>();
                challengeCardUIController.TitleText = challengeCard.GetChild("Title").GetComponent<TextMeshProUGUI>();
                challengeCardUIController.DescriptionText = challengeCard.GetChild("Description").GetComponent<TextMeshProUGUI>();
            }

            RedrawContent();
        }

        public void RedrawTabs()
        {
            // Setup each tab
            for (int i = 0; i < _data.Count; i++)
            {
                var tabController = _tabs[i];
                tabController.Category = _data[i].Item1;
                tabController.Selected = i == _tabIndex;
            }
        }

        public void RedrawContent()
        {
            var challenges = _data[_tabIndex].Item2.Skip(6 * _pageIndex).Take(6).ToList();

            // Setup each challenge card
            for (int i = 0; i < 6; i++)
            {
                var child = ContentParent.GetChild(i).gameObject;
                if (i >= challenges.Count)
                {
                    child.SetActive(false);
                }
                else
                {
                    child.SetActive(true);
                    var challengeCard = child.GetComponent<ChallengeCardUIController>();
                    challengeCard.Challenge = challenges[i];
                }
            }

            // Setup page counter
            PageCounter.text = $"Page {_pageIndex + 1} / {(_data[_tabIndex].Item2.Count + 5) / 6}";
        }
    }
}
