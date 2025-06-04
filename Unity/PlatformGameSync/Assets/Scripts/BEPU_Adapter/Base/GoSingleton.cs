using PlasticGui.WebApi.Responses;
using UnityEngine;


public abstract class GoSingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject();
                GameObject.DontDestroyOnLoad(go);
                T t = go.AddComponent<T>();
                (t as GoSingleton<T>)!.Init();
                go.name = typeof(T).Name;
                _instance = t;
            }
            return _instance;
        }
    }

    public abstract void Init();
}