using Microsoft.Maui.Animations;
using Microsoft.Maui.Controls.Internals;

namespace Microsoft.Maui.Controls.Compatibility.Platform.GTK
{
	public class GtkTicker : Ticker
	{
		private uint _timerId;

		public override void Stop()
		{
			base.Stop();
			GLib.Source.Remove(_timerId);
		}

		public override void Start()
		{
			base.Start();
			_timerId = GLib.Timeout.Add(15, new GLib.TimeoutHandler(OnSendSignals));
		}

		//protected override void DisableTimer()
		//{
		//	GLib.Source.Remove(_timerId);
		//}

		//protected override void EnableTimer()
		//{
		//	_timerId = GLib.Timeout.Add(15, new GLib.TimeoutHandler(OnSendSignals));
		//}

		private bool OnSendSignals()
		{
			// SendSignals();

			return true;
		}
	}
}
