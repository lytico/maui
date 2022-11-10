using System;
using System.Linq;
using Gtk;

namespace Microsoft.Maui.Platform
{
	public class DateEventArgs : EventArgs
	{
		DateTime _date;

		public DateTime Date
		{
			get
			{
				return _date;
			}
		}

		public DateEventArgs(DateTime date)
		{
			_date = date;
		}
	}

	public partial class DatePickerWindow : Gtk.Window
	{
		Box _datebox = null!;
		RangeCalendar _calendar = null!;
		//private static uint CURRENT_TIME = 0;

		public DatePickerWindow()
			: base(WindowType.Popup)
		{
			BuildDatePickerWindow();

			GrabFocus();

			Grab.Add(this);

			//Gdk.GrabStatus grabbed =
			//	Gdk.Pointer.Grab(GdkWindow, true,
			//	Gdk.EventMask.ButtonPressMask
			//	| Gdk.EventMask.ButtonReleaseMask
			//	| Gdk.EventMask.PointerMotionMask, null, null, CURRENT_TIME);

			//if (grabbed == Gdk.GrabStatus.Success)
			//{
			//	grabbed = Gdk.Keyboard.Grab(GdkWindow, true, CURRENT_TIME);

			//	if (grabbed != Gdk.GrabStatus.Success)
			//	{
			//		Grab.Remove(this);
			//		this.Destroy();
			//	}
			//}
			//else
			//{
			//	Grab.Remove(this);
			//	this.Destroy();
			//}

			SelectedDate = DateTime.Now;
		}

		public DateTime SelectedDate
		{
			get
			{
				return _calendar.Date;
			}

			set
			{
				_calendar.Date = value;
			}
		}

		public DateTime MinimumDate
		{
			get
			{
				return _calendar.MinimumDate;
			}

			set
			{
				_calendar.MinimumDate = value;
			}
		}

		public DateTime MaximumDate
		{
			get
			{
				return _calendar.MaximumDate;
			}

			set
			{
				_calendar.MaximumDate = value;
			}
		}

		public delegate void DateEventHandler(object sender, DateEventArgs args);

		public event DateEventHandler OnDateTimeChanged = null!;

		//protected override bool OnExposeEvent(Gdk.EventExpose args)
		//{
		//	base.OnExposeEvent(args);

		//	int winWidth, winHeight;
		//	GetSize(out winWidth, out winHeight);
		//	GdkWindow.DrawRectangle(
		//		Style.ForegroundGC(StateType.Insensitive), false, 0, 0, winWidth - 1, winHeight - 1);

		//	return false;
		//}

		protected virtual void OnButtonPressEvent(object? o, ButtonPressEventArgs args)
		{
			CloseRemoveGrab();
		}

		protected virtual void OnCalendarButtonPressEvent(object? o, ButtonPressEventArgs args)
		{
			args.RetVal = true;
		}

		protected virtual void OnCalendarDaySelected(object? sender, EventArgs e)
		{
			OnDateTimeChanged?.Invoke(this, new DateEventArgs(SelectedDate));
		}

		protected virtual void OnCalendarDaySelectedDoubleClick(object? sender, EventArgs e)
		{
			OnDateTimeChanged?.Invoke(this, new DateEventArgs(SelectedDate));
			CloseRemoveGrab();
		}

		private void BuildDatePickerWindow()
		{
			Title = "DatePicker";
			TypeHint = Gdk.WindowTypeHint.Desktop;
			WindowPosition = WindowPosition.Mouse;
			BorderWidth = 1;
			Resizable = false;
			// AllowGrow = false;
			Decorated = false;
			DestroyWithParent = true;
			SkipPagerHint = true;
			SkipTaskbarHint = true;

			_datebox = new Box(Gtk.Orientation.Vertical, 0);
			_datebox.Spacing = 6;
			_datebox.BorderWidth = 3;

			_calendar = new RangeCalendar();
			_calendar.CanFocus = true;
			_calendar.DisplayOptions = CalendarDisplayOptions.ShowHeading;
			_datebox.Add(_calendar);
			Box.BoxChild dateBoxChild = ((Box.BoxChild)(_datebox[_calendar]));
			dateBoxChild.Position = 0;

			Add(_datebox);

			if ((Child != null))
			{
				Child.ShowAll();
			}

			Show();

			ButtonPressEvent += new ButtonPressEventHandler(OnButtonPressEvent);
			_calendar.ButtonPressEvent += new ButtonPressEventHandler(OnCalendarButtonPressEvent);
			_calendar.DaySelected += new EventHandler(OnCalendarDaySelected);
			_calendar.DaySelectedDoubleClick += new EventHandler(OnCalendarDaySelectedDoubleClick);
		}

		void CloseRemoveGrab()
		{
			Grab.Remove(this);
			//Gdk.Pointer.Ungrab(CURRENT_TIME);
			//Gdk.Keyboard.Ungrab(CURRENT_TIME);
			Destroy();
		}

		void NotifyDateChanged()
		{
			OnDateTimeChanged?.Invoke(this, new DateEventArgs(SelectedDate));
		}

		class RangeCalendar : Calendar
		{
			DateTime _minimumDate;
			DateTime _maximumDate;

			public RangeCalendar()
			{
				_minimumDate = new DateTime(1900, 1, 1);
				_maximumDate = new DateTime(2199, 1, 1);
			}

			public DateTime MinimumDate
			{
				get
				{
					return _minimumDate;
				}

				set
				{
					if (MaximumDate < value)
					{
						throw new InvalidOperationException($"{nameof(MinimumDate)} must be lower than {nameof(MaximumDate)}");
					}

					_minimumDate = value;
				}
			}

			public DateTime MaximumDate
			{
				get
				{
					return _maximumDate;
				}

				set
				{
					if (MinimumDate > value)
					{
						throw new InvalidOperationException($"{nameof(MaximumDate)} must be greater than {nameof(MinimumDate)}");
					}

					_maximumDate = value;
				}
			}

			protected override void OnDaySelected()
			{
				if (Date < MinimumDate)
				{
					Date = MinimumDate;
				}

				if (Date > MaximumDate)
				{
					Date = MaximumDate;
				}
			}
		}
	}

	public partial class DatePicker : MauiView
	{
		MauiComboBox _comboBox = null!;
		Gdk.Color _color;
		DateTime _currentDate;
		DateTime _minDate;
		DateTime _maxDate;
		string _dateFormat = null!;

		public event EventHandler DateChanged = null!;
		public event EventHandler GotFocus = null!;
		public event EventHandler LostFocus = null!;

		public DatePicker()
		{
			BuildDatePicker();

			CurrentDate = DateTime.Now;

			if (_comboBox != null)
			{
				// TextColor = _comboBox.Entry.Style.Text(StateType.Normal);

				_comboBox.Entry.CanDefault = false;
				_comboBox.Entry.CanFocus = false;
				_comboBox.Entry.IsEditable = false;
				// _comboBox.Entry.State = StateType.Normal;
				_comboBox.Entry.FocusGrabbed += new EventHandler(OnEntryFocused);
				_comboBox.PopupButton.Clicked += new EventHandler(OnBtnShowCalendarClicked);
			}
		}

		public DateTime CurrentDate
		{
			get
			{
				return _currentDate;
			}
			set
			{
				_currentDate = value;
				UpdateEntryText();
			}
		}

		public DateTime MinDate
		{
			get
			{
				return _minDate;
			}
			set
			{
				_minDate = value;
			}
		}

		public DateTime MaxDate
		{
			get
			{
				return _maxDate;
			}
			set
			{
				_maxDate = value;
			}
		}

		public Gdk.Color TextColor
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				_comboBox.Color = _color;
			}
		}

		public string DateFormat
		{
			get
			{
				return _dateFormat;
			}
			set
			{
				_dateFormat = value;
				UpdateEntryText();
			}
		}

		public void SetBackgroundColor(Gdk.Color color)
		{
			_comboBox.SetBackgroundColor(color);
		}

		public void OpenPicker()
		{
			ShowPickerWindow();
		}

		public void ClosePicker()
		{
			var windows = Gtk.Window.ListToplevels();
			var window = windows.FirstOrDefault(w => w.GetType() == typeof(DatePickerWindow));

			if (window != null)
			{
				Remove(window);
			}
		}

		void ShowPickerWindow()
		{
			//int x = 0;
			//int y = 0;

			//GdkWindow.GetOrigin(out x, out y);
			//y += Allocation.Height;

			var picker = new DatePickerWindow();
			//picker.Move(x, y);
			picker.SelectedDate = CurrentDate;
			picker.MinimumDate = _minDate;
			picker.MaximumDate = _maxDate;
			picker.OnDateTimeChanged += OnPopupDateChanged;
			picker.Destroyed += OnPickerClosed;
		}

		void OnPopupDateChanged(object sender, DateEventArgs args)
		{
			var date = args.Date;

			if (date < MinDate)
			{
				CurrentDate = MinDate;
				return;
			}

			if (date > MaxDate)
			{
				CurrentDate = MaxDate;
				return;
			}

			CurrentDate = args.Date;
			DateChanged?.Invoke(this, EventArgs.Empty);
		}

		void BuildDatePicker()
		{
			_comboBox = new MauiComboBox();
			Add(_comboBox);

			if ((Child != null))
			{
				Child.ShowAll();
			}

			Show();
		}

		void UpdateEntryText()
		{
			_comboBox.Entry.Text = _currentDate.ToString(string.IsNullOrEmpty(_dateFormat) ? "D" : _dateFormat);
		}

		void OnBtnShowCalendarClicked(object? sender, EventArgs e)
		{
			ShowPickerWindow();
		}

		void OnEntryFocused(object? sender, EventArgs e)
		{
			ShowPickerWindow();
			GotFocus?.Invoke(this, EventArgs.Empty);
		}

		void OnPickerClosed(object? sender, EventArgs e)
		{
			var window = sender as DatePickerWindow;

			if (window != null)
			{
				Remove(window);
				LostFocus?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}
