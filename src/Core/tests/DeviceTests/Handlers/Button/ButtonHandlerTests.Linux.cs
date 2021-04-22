using System.Threading.Tasks;
using Gtk;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Handlers;
using Xunit;
using Microsoft.Maui.Graphics;
using Style = Pango.Style;

namespace Microsoft.Maui.DeviceTests
{

	public partial class ButtonHandlerTests
	{

		[Fact(DisplayName = "CharacterSpacing Initializes Correctly")]
		public async Task CharacterSpacingInitializesCorrectly()
		{
			var xplatCharacterSpacing = 4;

			var button = new ButtonStub() { CharacterSpacing = xplatCharacterSpacing, Text = "Test" };

			var expectedValue = button.CharacterSpacing;

			var values = await GetValueAsync(button, (handler) => new { ViewValue = button.CharacterSpacing, NativeViewValue = GetNativeCharacterSpacing(handler) });

			Assert.Equal(xplatCharacterSpacing, values.ViewValue);
		}

		[Theory(DisplayName = "Font Family Initializes Correctly")]
		[InlineData(null)]
		[InlineData("monospace")]
		[InlineData("Dokdo")]
		public async Task FontFamilyInitializesCorrectly(string family)
		{
			var button = new ButtonStub { Text = "Test", Font = Font.OfSize(family, 10) };

			var handler = await CreateHandlerAsync(button);
			var nativeButton = GetNativeButton(handler);

			var fontManager = handler.Services.GetRequiredService<IFontManager>();

			var nativeFont = fontManager.GetFontFamily(Font.OfSize(family, 0.0));

			Assert.Equal(nativeFont, nativeButton.GetPangoFontDescription());

			if (string.IsNullOrEmpty(family))
				Assert.Equal(fontManager.DefaultTypeface, nativeButton.Typeface);
			else
				Assert.NotEqual(fontManager.DefaultTypeface, nativeButton.Typeface);
		}

		[Fact(DisplayName = "Button Padding Initializing")]
		public async Task PaddingInitializesCorrectly()
		{
			var actual = new Thickness(5, 10, 15, 20);
			var button = new ButtonStub() { Text = "Test", Padding = actual };

			var handler = await CreateHandlerAsync(button);
			var widget = handler.WidgetOf<Button>();
			var expected = widget.GetMargin();

			Assert.Equal(expected, actual);
		}

		Button GetNativeButton(ButtonHandler buttonHandler) =>
			buttonHandler.WidgetOf<Button>();

		string GetNativeText(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).Label;

		Color GetNativeTextColor(ButtonHandler buttonHandler)
		{
			var expected = GetNativeButton(buttonHandler)
			   .GetForegroundColor();

			return expected;
		}

		Thickness GetNativePadding(ButtonHandler buttonHandler)
		{
			var buttonWidget = GetNativeButton(buttonHandler);

			return buttonWidget.GetMargin();

		}

		Task PerformClick(IButton button) =>
			InvokeOnMainThreadAsync(() =>
			{
				GetNativeButton(CreateHandler(button)).Click();
			});

		double GetNativeUnscaledFontSize(ButtonHandler buttonHandler) 
			=> GetNativeButton(buttonHandler).GetPangoFontDescription().GetSize();

		bool GetNativeIsBold(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).GetPangoFontDescription().Style == Style.Oblique;

		bool GetNativeIsItalic(ButtonHandler buttonHandler) =>
			GetNativeButton(buttonHandler).GetPangoFontDescription().Style == Style.Italic;

		double GetNativeCharacterSpacing(ButtonHandler buttonHandler)
		{
			var button = GetNativeButton(buttonHandler);

			if (button != null)
			{
				button.GetPangoFontDescription().Stretch.
			}

			return -1;
		}

	}

}