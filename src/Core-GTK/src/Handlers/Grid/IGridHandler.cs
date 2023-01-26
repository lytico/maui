namespace Microsoft.Maui
{
	public interface IGridHandler : IViewHandler
	{
		new IGridLayout VirtualView { get; }
		new System.Object PlatformView { get; }

		void Add(IView view);
		void Remove(IView view);
		void Clear();
		void Insert(int index, IView view);
		void Update(int index, IView view);
		void UpdateZIndex(IView view);
	}
}
