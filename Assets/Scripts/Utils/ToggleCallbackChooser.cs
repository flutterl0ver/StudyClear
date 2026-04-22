using UnityEngine;
using UnityEngine.Events;

public class ToggleCallbackChooser : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onCallback, _offCallback;
    
    public void OnValueChanged(bool value) {
        if(value) _onCallback?.Invoke();
        else _offCallback?.Invoke();
    }
}
