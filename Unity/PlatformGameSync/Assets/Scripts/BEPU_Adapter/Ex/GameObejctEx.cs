using UnityEngine;

public static class GameObejctEx {
    public static T GetOrAddComponnet<T>(this GameObject go) where T : Component {
        if (go == null) {
            Debug.LogError($"GetOrAddComponnet argument is null ");
            return null;
        }
        T t = go.GetComponent<T>();
        if (t == null) {
            t = go.AddComponent<T>();
        }
        return t;

    }
}