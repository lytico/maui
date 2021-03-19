namespace Microsoft.Maui.Limaki.Extensions {
    public interface INativeView {

    }

    public interface IViewNativeView: INativeView 
    {

	    void UpdateIsEnabled(IView view);

	    void UpdateBackgroundColor(IView view);

	    void UpdateAutomationId(IView view);

    }

    public interface IProgressBarNativeView : INativeView {
        void UpdateProgress(IProgress progress);
    }

    public interface IButtonNativeView : INativeView { }

    public interface IEditorNativeView : INativeView { }

    public interface IEntryNativeView : INativeView { }

    public interface ILabelNativeView : INativeView { }

    public interface ILayoutNativeView : INativeView { }

    public interface ISearchBarNativeView : INativeView { }

    public interface ISwitchNativeView : INativeView { }

    public interface ISliderNativeView : INativeView { }
}