using PlasticGui.WebApi.Responses;
using UnityEngine;


public abstract class GoSingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject go = new GameObject();
                if (Application.isPlaying) {
                    GameObject.DontDestroyOnLoad(go);
                }
                go.name = typeof(T).Name;
                go.hideFlags &= HideFlags.DontSaveInEditor;
                if (!Application.isPlaying) {
                    go.hideFlags &= HideFlags.HideInHierarchy;
                  
                }
                T t = go.AddComponent<T>();
                (t as GoSingleton<T>)!.Init();
                _instance = t;
            }
            return _instance;
        }
    }

    public abstract void Init();
}