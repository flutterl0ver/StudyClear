using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LayoutRebuild : MonoBehaviour {
    [SerializeField]
    private Transform _updatingLayoutsContainer;
    
    private IEnumerator SkipFrameCoroutine(Action callback) {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        
        callback();
    }
    
    public void Rebuild() {
        if (_updatingLayoutsContainer != null) {
            foreach (Transform layout in _updatingLayoutsContainer) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layout as RectTransform);
            }
        }
        
        StartCoroutine(SkipFrameCoroutine(() => {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }));
    }
}