using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.Maui.Storage;

namespace Microsoft.Maui.ApplicationModel
{
	public static partial class Permissions
	{
		public abstract partial class BasePlatformPermission : BasePermission
		{
			protected virtual Func<IEnumerable<string>> RequiredDeclarations { get; } = () => Array.Empty<string>();

			public override Task<PermissionStatus> CheckStatusAsync()
			{
				return Task.FromResult(PermissionStatus.Granted);
			}

			public override Task<PermissionStatus> RequestAsync()
				=> CheckStatusAsync();

			public override void EnsureDeclared()
			{
			}

			public override bool ShouldShowRationale() => false;
		}

#if !__GTK__
		public partial class Battery : BasePlatformPermission
		{
		}
#endif

		public partial class CalendarRead : BasePlatformPermission
		{
			protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
				new[] { "appointments" };
		}

		public partial class CalendarWrite : BasePlatformPermission
		{
			protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
				new[] { "appointments" };
		}

		public partial class Camera : BasePlatformPermission
		{
		}

		public partial class LaunchApp : BasePlatformPermission
		{
		}

#if !__GTK__
		public partial class LocationWhenInUse : BasePlatformPermission
		{
			protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
				new[] { "location" };

			public override Task<PermissionStatus> CheckStatusAsync()
			{
				EnsureDeclared();
				return Task.FromResult(PermissionStatus.Granted);
			}
		}

		public partial class LocationAlways : BasePlatformPermission
		{
			protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
				new[] { "location" };

			public override Task<PermissionStatus> CheckStatusAsync()
			{
				EnsureDeclared();
				return Task.FromResult(PermissionStatus.Granted);
			}
		}
#endif

		public partial class Microphone : BasePlatformPermission
		{
			protected override Func<IEnumerable<string>> RequiredDeclarations => () =>
				new[] { "microphone" };
		}

		public partial class NetworkState : BasePlatformPermission
		{
		}

		public partial class Phone : BasePlatformPermission
		{
		}

		public partial class Photos : BasePlatformPermission
		{
		}

		public partial class Reminders : BasePlatformPermission
		{
		}

		public partial class Sensors : BasePlatformPermission
		{
			static readonly Guid activitySensorClassId = new Guid("9D9E0118-1807-4F2E-96E4-2CE57142E196");

			public override Task<PermissionStatus> CheckStatusAsync()
			{
				return Task.FromResult(PermissionStatus.Granted);               
			}
		}

		public partial class Sms : BasePlatformPermission
		{
		}

		public partial class Speech : BasePlatformPermission
		{
		}

		public partial class StorageRead : BasePlatformPermission
		{
		}

		public partial class StorageWrite : BasePlatformPermission
		{
		}

		public partial class Vibrate : BasePlatformPermission
		{
		}
	}
}
