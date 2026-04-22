using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : class {
    public static T Instance;

    protected void Awake() {
        Instance = this as T;
    }
}
