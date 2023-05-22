using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseIncreaser : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(2.2f, 2.2f, 1f), 0.5f).setEaseOutCubic().setOnComplete(
            delegate () {
                transform.LeanMoveLocal(new Vector3(transform.localPosition.x, transform.localPosition.y, -20f), 0);
            });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(1f, 1f, 1f), 0.5f).setEaseOutCubic().setOnComplete(
            delegate ()
            {
                transform.LeanMoveLocal(new Vector3(transform.localPosition.x, transform.localPosition.y, -19f), 0);
            });
    }
}
