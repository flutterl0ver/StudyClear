using UnityEngine;
using UnityEngine.Events;

public class InvokeOnEnable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEnable;

    private void OnEnable() {
        _onEnable.Invoke();
    }
}
