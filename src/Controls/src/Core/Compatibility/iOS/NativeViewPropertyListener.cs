#nullable disable
using System;
using System.ComponentModel;
using Foundation;

#if __MOBILE__
namespace Microsoft.Maui.Controls.Compatibility.Platform.iOS
#else

namespace Microsoft.Maui.Controls.Compatibility.Platform.MacOS
#endif
{
	class NativeViewPropertyListener : NSObject, INotifyPropertyChanged
	{
		string TargetProperty { get; set; }

		public NativeViewPropertyListener(string targetProperty)
		{
			TargetProperty = targetProperty;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
		{
			if (keyPath.ToString().Equals(TargetProperty, StringComparison.OrdinalIgnoreCase))

/* Unmerged change from project 'Controls.Core(net8.0-maccatalyst)'
Before:
				PropertyChanged?.Invoke(ofObject, new PropertyChangedEventArgs(TargetProperty));
After:
			{
				PropertyChanged?.Invoke(ofObject, new PropertyChangedEventArgs(TargetProperty));
			}
*/
			{
				PropertyChanged?.Invoke(ofObject, new PropertyChangedEventArgs(TargetProperty));
			}
			else
			{
			{
				base.ObserveValue(keyPath, ofObject, change, context);
			}
		}
	}
}