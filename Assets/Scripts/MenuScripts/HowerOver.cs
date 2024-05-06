using UnityEngine;
using UnityEngine.EventSystems;

public class HowerOver : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _panelToOpen;

    public void OnPointerClick(PointerEventData eventData)
    {
        _panelToOpen.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _panelToOpen.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _panelToOpen.SetActive(false);
    }
}
