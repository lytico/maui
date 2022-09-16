namespace Microsoft.Maui.Animations
{
	public class PlatformTicker : Ticker
	{
		private uint _timerId;

		public override void Start()
		{
			_timerId = GLib.Timeout.Add(15, new GLib.TimeoutHandler(OnSendSignals));
		}

		public override void Stop()
		{
			GLib.Source.Remove(_timerId);
		}

		private bool OnSendSignals()
		{
			Fire?.Invoke();

			return true;
		}
	}
}