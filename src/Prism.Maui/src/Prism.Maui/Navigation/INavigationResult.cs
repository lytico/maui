namespace Prism.Navigation;

public interface INavigationResult
{
    bool Success { get; }

    bool Cancelled { get; }

    Exception Exception { get; }
}
