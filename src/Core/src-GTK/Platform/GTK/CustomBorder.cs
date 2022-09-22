using System;
using System.Linq;
using Gtk;
using Windows.ApplicationModel.VoiceCommands;

namespace Microsoft.Maui.Platform
{
	public class CustomBorder : CustomAltView// Fixed
	{
		Fixed _fixed = null!;

		public CustomBorder()
		{
			_fixed = new Fixed();
			Add(_fixed);
		}

		public void UpdateBackground(IBorderView handler)
		{
		}

		public void UpdateStrokeShape(IBorderView handler)
		{
		}

		public void UpdateStroke(IBorderView handler)
		{
		}

		public void UpdateStrokeThickness(IBorderView handler)
		{
		}

		public void UpdateStrokeLineCap(IBorderView handler)
		{
		}

		public void UpdateStrokeLineJoin(IBorderView handler)
		{
		}

		public void UpdateStrokeDashPattern(IBorderView handler)
		{
		}

		public void UpdateStrokeDashOffset(IBorderView handler)
		{
		}

		public void UpdateStrokeMiterLimit(IBorderView handler)
		{
		}
	}
}
