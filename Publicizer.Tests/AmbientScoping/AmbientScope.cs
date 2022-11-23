namespace Publicizer.Tests.AmbientScoping;

public abstract class AmbientScope<T> : IDisposable
    where T : AmbientScope<T>
{
    private static readonly AsyncLocal<AmbientScope<T>?> _ambientScope = new ();
    protected AmbientScope<T>? _parentScope { get; }

    protected AmbientScope()
    {
        _parentScope = _ambientScope.Value;
        _ambientScope.Value = this;
    }

    public void Dispose()
    {
        _ambientScope.Value = ParentScope;
    }

    protected T? ParentScope => (T?) _parentScope;

    public static T? Current => (T?) _ambientScope.Value;
}
