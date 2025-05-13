using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerClickHandler

public class TileClickHandler : MonoBehaviour, IPointerClickHandler
{
    private int tileIndex;
    private System.Action<int> onClick;

    public void Initialize(int index, System.Action<int> clickCallback)
    {
        tileIndex = index;
        onClick = clickCallback;
    }

    // This method is called when the tile is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(tileIndex);
    }
}