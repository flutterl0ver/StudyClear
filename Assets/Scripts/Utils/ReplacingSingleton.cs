public class ReplacingSingleton<T> : Singleton<T> where T : class {
    protected void OnEnable() {
        Instance = this as T;
    }
}