namespace Microsoft.Maui.Limaki.Extensions {
    public interface INativeView {
        void UpdateIsEnabled(IView view);

        void UpdateBackgroundColor(IView view);

        void UpdateAutomationId(IView view);
    }

    public interface IProgressBarNativeView : INativeView {
        void UpdateProgress(IProgress progress);
    }

    public interface ButtonNativeView : INativeView { }

    public interface EditorNativeView : INativeView { }

    public interface EntryNativeView : INativeView { }

    public interface LabelNativeView : INativeView { }

    public interface LayoutNativeView : INativeView { }

    public interface SearchBarNativeView : INativeView { }

    public interface SwitchNativeView : INativeView { }

    public interface SliderNativeView : INativeView { }
}