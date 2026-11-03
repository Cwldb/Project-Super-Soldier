using UnityEngine;
using UnityEngine.EventSystems;

namespace _01_Scripts.UI
{
    public class OwnedItemSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private OwnedItemsPanel panel;
        [SerializeField] private int slotIndex;

        public void OnPointerEnter(PointerEventData eventData) => panel.ShowDetails(slotIndex);
        public void OnPointerExit(PointerEventData eventData) => panel.HideDetails();
        public void OnPointerClick(PointerEventData eventData) => panel.ShowDetails(slotIndex);
    }
}
