using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RefreshOnEnable : MonoBehaviour {
    [SerializeField]
    private List<RefreshItemDto> _refreshItems;

    private void OnEnable() {
        StartCoroutine(RefreshCoroutine());
    }

    private IEnumerator RefreshCoroutine() {
        foreach (RefreshItemDto item in _refreshItems) {
            if (item.RefreshChildren) {
                foreach (RectTransform child in item.Transform) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(child);
                }

                yield return new WaitForEndOfFrame();
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(item.Transform);
            yield return new WaitForEndOfFrame();
        }
    }

    [Serializable]
    public class RefreshItemDto {
        public RectTransform Transform;
        public bool RefreshChildren;
    }
}