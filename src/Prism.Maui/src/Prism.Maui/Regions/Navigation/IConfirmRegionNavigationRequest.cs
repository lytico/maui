﻿namespace Prism.Regions.Navigation;

/// <summary>
/// Provides a way for objects involved in navigation to determine if a navigation request should continue.
/// </summary>
public interface IConfirmRegionNavigationRequest : IRegionAware
{
    /// <summary>
    /// Determines whether this instance accepts being navigated away from.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    /// <param name="continuationCallback">The callback to indicate when navigation can proceed.</param>
    /// <remarks>
    /// Implementors of this method do not need to invoke the callback before this method is completed,
    /// but they must ensure the callback is eventually invoked.
    /// </remarks>
    void ConfirmNavigationRequest(INavigationContext navigationContext, Action<bool> continuationCallback);
}
