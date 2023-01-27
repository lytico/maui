global using global::Microsoft.Maui;
global using global::Microsoft.Maui.Controls;
global using global::Microsoft.Maui.Graphics;
global using global::Microsoft.Extensions.DependencyInjection;
global using global::Microsoft.Maui.Controls.Xaml;

using DryIoc;
using Prism;
using Prism.DryIoc;
using Microsoft.Maui.Hosting;

namespace Microsoft.Maui;

/// <summary>
/// Application base class using DryIoc
/// </summary>
public static class PrismAppExtensions
{
    public static MauiAppBuilder UsePrism(this MauiAppBuilder builder, Action<PrismAppBuilder> configurePrism)
    {
        return builder.UsePrism(new DryIocContainerExtension(), configurePrism);
    }

    public static MauiAppBuilder UsePrism(this MauiAppBuilder builder, Rules rules, Action<PrismAppBuilder> configurePrism)
    {
        rules = rules.WithTrackingDisposableTransients()
            .With(Made.Of(FactoryMethod.ConstructorWithResolvableArguments))
            .WithFactorySelector(Rules.SelectLastRegisteredFactory());
        return builder.UsePrism(new DryIocContainerExtension(rules), configurePrism);
    }
}
