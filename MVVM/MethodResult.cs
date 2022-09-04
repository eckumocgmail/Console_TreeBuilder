using System;

 
public class MethodResult<T> where T: class
{
    public T Result { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public static MethodResult<T> FromResult(T result)
        => new MethodResult<T>()
        {
            Message = "Операция выполнена успешно",
            Success = true,
            Result = result
        };
    public static MethodResult<T> FromException(Exception ex)
        => new MethodResult<T>()
        {
            Message = "Операция выполнена завершена с ошибкой: " + ex.Message,
            Success = false,
            Result = null
        };
}
 
public class MethodResult: MethodResult<object>
{
    public static new MethodResult FromResult(object result)
        => new MethodResult ()
        {
            Message = "Операция выполнена успешно",
            Success = true,
            Result = result
        };
    public static new MethodResult FromException(Exception ex)
        => new MethodResult ()
        {
            Message = "Операция выполнена завершена с ошибкой: " + ex.Message,
            Success = false,
            Result = null
        };
}
