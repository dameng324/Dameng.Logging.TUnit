namespace Dameng.Logging.TUnit;

internal class TUnitLoggerScope : IDisposable
{
    private static readonly AsyncLocal<TUnitLoggerScope?> _currentScope = new();

    public object State { get; }
    public TUnitLoggerScope? Parent { get; }

    private TUnitLoggerScope(object state)
    {
        State = state;
        Parent = _currentScope.Value;
        _currentScope.Value = this;
    }

    public static IDisposable Push(object state)
    {
        return new TUnitLoggerScope(state);
    }

    public static List<object> GetCurrentScopeChain()
    {
        var list = new List<object>();
        var scope = _currentScope.Value;
        while (scope != null)
        {
            list.Add(scope.State);
            scope = scope.Parent;
        }
        list.Reverse(); // 从最外层到最内层
        return list;
    }

    public void Dispose()
    {
        _currentScope.Value = Parent;
    }
}