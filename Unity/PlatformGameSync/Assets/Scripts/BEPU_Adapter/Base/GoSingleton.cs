using PlasticGui.WebApi.Responses;
using UnityEngine;


public abstract class GoSingleton<T> where T : new() {
    private static T _instance;

    public static T Instance {
        get {
            if (_instance == null) {
                T t = new();
                (t as GoSingleton<T>)!.Init();
                _instance = t;
            }
            return _instance;
        }
    }

    public abstract void Init();
}