using System;
using _01_Scripts.Core;
using _01_Scripts.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _01_Scripts.UI
{
    public class OwnedItemsPanel : MonoBehaviour
    {
        [Serializable]
        public class ItemSlot
        {
            public Image icon;
            public TMP_Text levelText;
            public Image[] levelPips;
        }

        [SerializeField] private ItemSlot[] slots;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private GameObject tooltip;
        [SerializeField] private TMP_Text detailsText;
        private PlayerItems _items;
        private int _hovered = -1;

        private void Start()
        {
            _items = GameManager.Instance.PlayerFinder.Target.GetComponent<PlayerItems>();
            if (_items != null) _items.OnItemsChanged += Refresh;
            Refresh();
        }

        private void OnEnable()
        {
            if (_items == null) return;
            _items.OnItemsChanged += Refresh;
            Refresh();
        }

        private void OnDisable()
        {
            if (_items != null) _items.OnItemsChanged -= Refresh;
            HideDetails();
        }

        private void Refresh()
        {
            int owned = 0;
            for (int i = 0; i < slots.Length; i++)
            {
                int level = _items != null ? _items.GetLevel((ItemType)i) : 0;
                if (level > 0) owned++;
                Color color = PlayerItems.ItemColor((ItemType)i);
                slots[i].icon.color = level > 0 ? color : new Color(0.4f, 0.4f, 0.4f, 0.55f);
                slots[i].levelText.text = level == 0 ? "미보유" : level == PlayerItems.MaxLevel ? "Lv.3 MAX" : $"Lv.{level}";
                slots[i].levelText.color = level > 0 ? color : Color.gray;
                for (int pip = 0; pip < PlayerItems.MaxLevel; pip++)
                    slots[i].levelPips[pip].color = pip < level ? color : new Color(0.25f, 0.25f, 0.25f, 1f);
            }
            countText.text = $"보유 아이템  {owned} / {slots.Length}";
            if (_hovered >= 0) ShowDetails(_hovered);
        }

        public void ShowDetails(int index)
        {
            if (_items == null || index < 0 || index >= slots.Length) return;
            _hovered = index;
            ItemType item = (ItemType)index;
            int level = _items.GetLevel(item);
            string heading = PlayerItems.ItemName(item) + (level == 0 ? " · 미보유" : $" · Lv.{level}");
            detailsText.text = "<b>" + heading + "</b>\n\n" +
                (level == 0 ? "획득 효과\n" + _items.Description(item) :
                    "현재 효과\n" + _items.Description(item, false) + "\n\n" +
                    (level == PlayerItems.MaxLevel ? "최대 강화 완료" : "다음 강화\n" + _items.Description(item)));
            tooltip.SetActive(true);
        }

        public void HideDetails()
        {
            _hovered = -1;
            if (tooltip != null) tooltip.SetActive(false);
        }

    }
}
