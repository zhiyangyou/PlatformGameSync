using System;

public static class BEPU_Logger {
    public static Action<object> OnLog;
    public static Action<object> OnLogError;
    public static Action<Exception> OnLogException;

    public static void LogException(Exception e) {
        OnLogException?.Invoke(e);
    }

    public static void LogError(object msg) {
        OnLogError?.Invoke(msg);
    }

    public static void Log(object msg) {
        OnLog?.Invoke(msg);
    }
}