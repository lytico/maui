using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using NUnit.Framework;

namespace Microsoft.Maui.Controls.Xaml.UnitTests
{
	public partial class Bz30074 : ContentPage
	{
		public Bz30074()
		{
			InitializeComponent();
		}

		public Bz30074(bool useCompiledXaml)
		{
			//this stub will be replaced at compile time
		}

		[TestFixture]
		class Tests
		{
			[TestCase(true)]
			[TestCase(false)]
			public void DataTriggerInTemplates(bool useCompiledXaml)
			{
				var layout = new Bz30074(useCompiledXaml);
				Assert.Null(layout.image.Source);

				layout.BindingContext = new { IsSelected = true };
				Assert.AreEqual("Add.png", ((FileImageSource)layout.image.Source).File);

				layout.BindingContext = new { IsSelected = false };
				Assert.Null(layout.image.Source);
			}
		}
	}
}

