using System;
using System.Collections.Generic;
using _01_Scripts.Core;
using _01_Scripts.Entities;
using _01_Scripts.Players;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _01_Scripts.UI
{
    public class LevelUpPanel : MonoBehaviour
    {
        [SerializeField] private GameObject statLevelUpPanel;
        [SerializeField] private GameObject activeLevelUpPanel;
        [SerializeField] private TMP_Text levelText;
        
        [Header("names")]
        [SerializeField] private string[] levelNames;
        [SerializeField] private string[] activeNames;
        
        [Header("Icons")]
        [SerializeField] private Sprite[] levelUpIcons;
        [SerializeField] private Sprite[] activeIcons;
        [SerializeField] private Sprite[] itemIcons;
        
        [Header("Description")]
        [SerializeField] private string[] descriptionTexts;
        [SerializeField] private string[] activeDescriptionTexts;
        
        private PlayerStat _playerStat;
        private PlayerSkillSet _playerSkillSet;
        private Entity _entity;
        
        private Dictionary<string, Action> _statIncreases;
        private List<string> _selectedStats;
        private List<int> _selectedIndices;
        private PlayerItems _playerItems;
        private bool _selectionMade;

        private void Start()
        {
            GameManager.Instance.OnLevelUp += EnablePanels;
            GameManager.Instance.OnActiveLevelUp += EnableActivePanels;
            GameManager.Instance.OnExperienceChanged += LevelUpText;
            
            _entity = GameManager.Instance.PlayerFinder.Target;
            _playerStat = GameManager.Instance.PlayerFinder.Target.GetCompo<PlayerStat>();
            _playerSkillSet = GameManager.Instance.PlayerFinder.Target.GetCompo<PlayerSkillSet>();
            _playerItems = _entity.GetComponent<PlayerItems>();
            
            _statIncreases = new Dictionary<string, Action>
            {
                { "Damage", () => _playerStat.Damage *= 1.2f },
                { "AttackSpeed", () => _playerStat.AttackSpeed += -0.08f },
                { "CritPer", () => _playerStat.CritPer += 0.07f },
                { "CritPower", () => _playerStat.CritPower += 0.5f },
                { "Health", () =>
                    {
                        _playerStat._healthCompo.AddMaxHealth(10f);
                        _playerStat._healthCompo.AddCurrentHealth(25f);
                        GameManager.Instance.OnValueChange?.Invoke();
                    }
                }
            };
            
            LevelUpText();
        }
        private void LevelUpText()
        {
            levelText.text = $"Lv.{GameManager.Instance.GetCurLevel()}";
        }
        
        private void ChooseRandomStats()
        {
            List<string> statNames = new List<string>(_statIncreases.Keys);
            if (_playerItems != null)
                foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
                    if (_playerItems.CanUpgrade(item)) statNames.Add("Item:" + item);
            _selectedStats = new List<string>();
            _selectedIndices = new List<int>();
            
            while (_selectedStats.Count < 3)
            {
                int randIndex = Random.Range(0, statNames.Count);
                string stat = statNames[randIndex];

                if (!_selectedStats.Contains(stat))
                {
                    _selectedStats.Add(stat);
                    _selectedIndices.Add(randIndex < _statIncreases.Count ? randIndex : -1);
                }
            }
        }

        private void EnableActivePanels()
        {
            _selectionMade = false;
            Transform trm = activeLevelUpPanel.transform;
            Sequence seq = DOTween.Sequence();
            activeLevelUpPanel.SetActive(true);
            
            for (int i = 0; i < activeLevelUpPanel.transform.childCount; i++)
            {
                var child = activeLevelUpPanel.transform.GetChild(i);
                child.GetComponentInChildren<TMP_Text>().text = activeNames[i];

                var image = child.transform.GetChild(2).GetComponent<Image>();
                if (image != null)
                    image.sprite = activeIcons[i];
                
                var description = child.transform.GetChild(1).GetComponent<TMP_Text>();
                if(description != null)
                    description.text = activeDescriptionTexts[i];
            }
            
            seq.SetUpdate(true);
            Time.timeScale = 0;
            
            seq.Append(trm.GetChild(0).transform.DOMoveY(600, 0.3f));
            seq.Append(trm.GetChild(1).transform.DOMoveY(600, 0.3f));
            seq.Play();
        }
        
        private void DisableActivePanels()
        {
            Transform trm = activeLevelUpPanel.transform;
            Sequence seq = DOTween.Sequence();

            seq.SetUpdate(true);
            seq.Append(trm.GetChild(0).transform.DOMoveY(1400, 0.3f));
            seq.Append(trm.GetChild(1).transform.DOMoveY(1400, 0.3f));
            seq.OnComplete(() =>
            {
                activeLevelUpPanel.SetActive(false);
                Time.timeScale = 1;
                GameManager.Instance.CompleteLevelUp();
            });
        }
        
        private void EnablePanels()
        {
            _selectionMade = false;
            Transform trm = statLevelUpPanel.transform;
            Sequence seq = DOTween.Sequence();
            statLevelUpPanel.SetActive(true);
            
            ChooseRandomStats();
            
            for (int i = 0; i < statLevelUpPanel.transform.childCount; i++)
            {
                var child = statLevelUpPanel.transform.GetChild(i);
                if (_selectedIndices[i] < 0)
                {
                    ItemType item = (ItemType)Enum.Parse(typeof(ItemType), _selectedStats[i].Substring(5));
                    int level = _playerItems.GetLevel(item);
                    child.GetComponentInChildren<TMP_Text>().text = PlayerItems.ItemName(item) +
                        (level == 0 ? " [신규]" : $" Lv.{level + 1}");
                    child.transform.GetChild(1).GetComponent<TMP_Text>().text = _playerItems.Description(item);
                    Image itemImage = child.transform.GetChild(2).GetComponent<Image>();
                    if (itemImage != null)
                    {
                        if (itemIcons != null && (int)item < itemIcons.Length)
                            itemImage.sprite = itemIcons[(int)item];
                        else if (levelUpIcons.Length > 0)
                            itemImage.sprite = levelUpIcons[(int)item % levelUpIcons.Length];
                        itemImage.color = PlayerItems.ItemColor(item);
                    }
                    continue;
                }
                child.GetComponentInChildren<TMP_Text>().text = levelNames[_selectedIndices[i]];

                var image = child.transform.GetChild(2).GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = levelUpIcons[_selectedIndices[i]];
                    image.color = Color.white;
                }
                
                var description = child.transform.GetChild(1).GetComponent<TMP_Text>();
                if(description != null)
                    description.text = descriptionTexts[_selectedIndices[i]];
            }
            seq.SetUpdate(true);
            Time.timeScale = 0;
            
            seq.Append(trm.GetChild(0).transform.DOMoveY(600, 0.3f));
            seq.Append(trm.GetChild(1).transform.DOMoveY(600, 0.3f));
            seq.Append(trm.GetChild(2).transform.DOMoveY(600, 0.3f));
            seq.Play();
        }

        private void DisablePanels()
        {
            Transform trm = statLevelUpPanel.transform;
            Sequence seq = DOTween.Sequence();

            seq.SetUpdate(true);
            seq.Append(trm.GetChild(0).transform.DOMoveY(1400, 0.3f));
            seq.Append(trm.GetChild(1).transform.DOMoveY(1400, 0.3f));
            seq.Append(trm.GetChild(2).transform.DOMoveY(1400, 0.3f));
            seq.OnComplete(() =>
            {
                statLevelUpPanel.SetActive(false);
                Time.timeScale = 1;
                GameManager.Instance.CompleteLevelUp();
            });
        }

        public void SelectActive1()
        {
            if (_selectionMade) return;
            _selectionMade = true;
            activeLevelUpPanel.transform.GetChild(0).transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetUpdate(true).OnComplete(() =>
            {
                _playerSkillSet.radius += 1f;
                DisableActivePanels();
            });
        }

        public void SelectActive2()
        {
            if (_selectionMade) return;
            _selectionMade = true;
            activeLevelUpPanel.transform.GetChild(1).transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetUpdate(true).OnComplete(() =>
            {
                _playerSkillSet.damage += 10f;
                DisableActivePanels();
            });
        }
        
        public void Select1()
        {
            if (_selectionMade) return;
            _selectionMade = true;
            statLevelUpPanel.transform.GetChild(0).transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetUpdate(true).OnComplete(() =>
            {
                ApplyStatIncrease(0);
                DisablePanels();
            });
        }

        public void Select2()
        {
            if (_selectionMade) return;
            _selectionMade = true;
            
            statLevelUpPanel.transform.GetChild(1).transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetUpdate(true).OnComplete(() =>
            {
                ApplyStatIncrease(1);
                DisablePanels();
            });
        }

        public void Select3()
        {
            if (_selectionMade) return;
            _selectionMade = true;
            statLevelUpPanel.transform.GetChild(2).transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).SetUpdate(true).OnComplete(() =>
            {
                ApplyStatIncrease(2);
                DisablePanels();
            });
        }
        
        private void ApplyStatIncrease(int index)
        {
            string statName = _selectedStats[index];
            if (statName.StartsWith("Item:"))
            {
                _playerItems.Upgrade((ItemType)Enum.Parse(typeof(ItemType), statName.Substring(5)));
                return;
            }
            _statIncreases[statName]?.Invoke();
        }

        private void OnDestroy()
        {
            if (GameManager.Instance == null) return;
            GameManager.Instance.OnLevelUp -= EnablePanels;
            GameManager.Instance.OnActiveLevelUp -= EnableActivePanels;
            GameManager.Instance.OnExperienceChanged -= LevelUpText;
        }
    }
}
