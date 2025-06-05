using PlasticGui.WebApi.Responses;
using UnityEngine;


public abstract class GoSingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject go = null;
                T t = default;
                var name = $"[Singleton]-({typeof(T).Name})";

                go = GameObject.Find(name);
                if (go == null) {
                    go = new GameObject();
                    t = go.AddComponent<T>();
                }
                else {
                    t = go.GetComponent<T>();
                }
                (t as GoSingleton<T>)!.Init();
                go.name = name;
                go.hideFlags &= HideFlags.HideAndDontSave;
                if (Application.isPlaying) {
                    GameObject.DontDestroyOnLoad(go);
                }
                _instance = t;
            }
            return _instance;
        }
    }

    public abstract void Init();
}