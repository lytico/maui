using System;
using System.Threading;
using System.Timers;

namespace Microsoft.Maui.Dispatching
{
	public partial class Dispatcher : IDispatcher
	{
		readonly FallbackDispatcher _dispatcherQueue;

		internal Dispatcher(FallbackDispatcher dispatcherQueue)
		{
			_dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
		}

		bool IsDispatchRequiredImplementation() =>
			!_dispatcherQueue.IsDispatchRequired;

		bool DispatchImplementation(Action action) =>
			_dispatcherQueue.BeginInvokeOnMainThread(action);

		bool DispatchDelayedImplementation(TimeSpan delay, Action action)
		{
			var timer = _dispatcherQueue.CreateTimer();
			timer.Interval = delay;
			timer.Tick += OnTimerTick;
			timer.Start();
			return true;

			void OnTimerTick(object? sender, EventArgs e)
			{
				action();
				timer.Tick -= OnTimerTick;
			}
		}

		IDispatcherTimer CreateTimerImplementation()
		{
			return _dispatcherQueue.CreateTimer();
		}
	}

	partial class DispatcherTimer : IDispatcherTimer
	{
		readonly System.Timers.Timer _timer;

		public DispatcherTimer(System.Timers.Timer timer)
		{
			_timer = timer;
		}

		public TimeSpan Interval
		{
			get
			{
				var interval = _timer.Interval;
				return new TimeSpan(0, 0, 0, 0, (int)interval);
			}
			set
			{
				var interval = value.TotalMilliseconds;
				_timer.Interval = interval;
			}
		}

		public bool IsRepeating
		{
			get => _timer.AutoReset;
			set => _timer.AutoReset = value;
		}

		public bool IsRunning { get; private set; }

		public event EventHandler? Tick;

		public void Start()
		{
			if (IsRunning)
				return;

			IsRunning = true;

			_timer.Elapsed += OnTimerTick;

			_timer.Start();
		}

		private void OnTimerTick(object? sender, ElapsedEventArgs e)
		{
			Tick?.Invoke(this, EventArgs.Empty);

			if (!IsRepeating)
				Stop();
		}

		public void Stop()
		{
			if (!IsRunning)
				return;

			IsRunning = false;

			_timer.Elapsed -= OnTimerTick;

			_timer.Stop();
		}

	}

	public partial class DispatcherProvider
	{
		static IDispatcher? GetForCurrentThreadImplementation()
		{
			var q = new FallbackDispatcher();
			if (q == null)
				return null;

			return new Dispatcher(q);
		}
	}

	internal class FallbackDispatcher : IDispatcher
	{
		public bool IsInvokeRequired => Thread.CurrentThread.IsBackground;

		public bool IsDispatchRequired => Thread.CurrentThread.IsBackground;

		public bool BeginInvokeOnMainThread(Action action)
		{
			GLib.Idle.Add(delegate{ action(); return false; });
			return true;
		}

		public IDispatcherTimer CreateTimer()
		{
			var timer = new System.Timers.Timer(500);
			return new DispatcherTimer(timer);
		}

		public bool Dispatch(Action action)
		{
			GLib.Idle.Add(delegate { action(); return false; });
			return true;
		}

		public bool DispatchDelayed(TimeSpan delay, Action action)
		{
			var timer = new System.Timers.Timer(delay.TotalMilliseconds);
			timer.AutoReset = false;
			var dispatchTimer =  new DispatcherTimer(timer);
			dispatchTimer.Tick += DispatchTimer_Tick;
			dispatchTimer.Start();
			
			void DispatchTimer_Tick(object? sender, EventArgs e)
			{
				dispatchTimer.Stop();
				BeginInvokeOnMainThread(action);
			}

			return true;
		}
	}
}