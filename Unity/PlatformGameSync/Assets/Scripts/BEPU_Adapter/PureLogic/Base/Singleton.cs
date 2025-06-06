public abstract class Singleton<T> where T : new() {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                T t = new();
                (t as Singleton<T>)!.Init();
                _instance = t;
            }
            return _instance;
        }
    }

    public abstract void Init();
}