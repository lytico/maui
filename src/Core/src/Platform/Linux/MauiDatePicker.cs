using System;
using System.Runtime.Serialization;

namespace Microsoft.Maui
{
	public class MauiDatePicker : Gtk.Label
	{

		public MauiDatePicker() : base()
		{
			Format = string.Empty;
		}
		
		DateTime _time;

		public DateTime Date
		{
			get => _time;
			set
			{
				_time = value;
				Text = _time.ToString();
			}
		}

		public string Format { get; set; }

	}
}