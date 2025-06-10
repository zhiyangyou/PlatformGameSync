using Newtonsoft.Json;

public class ProtoBuffConvert {
    public static string ToJson<T>(T proto) {
        return (JsonConvert.SerializeObject(proto));
    }
}