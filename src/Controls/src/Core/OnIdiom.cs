#nullable disable
using Microsoft.Maui.Devices;

namespace Microsoft.Maui.Controls
{
	public class OnIdiom<T>
	{
		T _phone;
		T _tablet;
		T _desktop;
		T _tV;
		T _watch;
		T _default;
#if !__GTK__
		bool _isPhoneSet;
		bool _isTabletSet;
#endif
		bool _isDesktopSet;
#if !__GTK__
		bool _isTVSet;
		bool _isWatchSet;
#endif
		bool _isDefaultSet;

		public T Phone
		{
			get => _phone;
			set
			{
				_phone = value;
#if !__GTK__
				_isPhoneSet = true;
#endif
			}
		}
		public T Tablet
		{
			get => _tablet;
			set
			{
				_tablet = value;
#if !__GTK__
				_isTabletSet = true;
#endif
			}
		}
		public T Desktop
		{
			get => _desktop;
			set
			{
				_desktop = value;
				_isDesktopSet = true;
			}
		}
		public T TV
		{
			get => _tV;
			set
			{
				_tV = value;
#if !__GTK__
				_isTVSet = true;
#endif
			}
		}
		public T Watch
		{
			get => _watch;
			set
			{
				_watch = value;
#if !__GTK__
				_isWatchSet = true;
#endif
			}
		}
		public T Default
		{
			get => _default;
			set
			{
				_default = value;
				_isDefaultSet = true;
			}
		}

		public static implicit operator T(OnIdiom<T> onIdiom)
		{
#if !__GTK__
			var idiom = DeviceInfo.Idiom;
			if (idiom == DeviceIdiom.Tablet)
				return onIdiom._isTabletSet ? onIdiom.Tablet : (onIdiom._isDefaultSet ? onIdiom.Default : default(T));
			else if (idiom == DeviceIdiom.Desktop)
#endif
			return onIdiom._isDesktopSet ? onIdiom.Desktop : (onIdiom._isDefaultSet ? onIdiom.Default : default(T));
#if !__GTK__
			else if (idiom == DeviceIdiom.TV)
				return onIdiom._isTVSet ? onIdiom.TV : (onIdiom._isDefaultSet ? onIdiom.Default : default(T));
			else if (idiom == DeviceIdiom.Watch)
				return onIdiom._isWatchSet ? onIdiom.Watch : (onIdiom._isDefaultSet ? onIdiom.Default : default(T));
			else
				return onIdiom._isPhoneSet ? onIdiom.Phone : (onIdiom._isDefaultSet ? onIdiom.Default : default(T));
#endif
		}
	}
}
