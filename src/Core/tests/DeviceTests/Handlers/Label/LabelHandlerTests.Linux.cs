using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.DeviceTests.Stubs;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Handlers;
using Xunit;

namespace Microsoft.Maui.DeviceTests
{
	public partial class LabelHandlerTests
	{
		[Theory(DisplayName = "Font Family Initializes Correctly")]
		[InlineData(null)]
		[InlineData("Times New Roman")]
		[InlineData("Dokdo")]
		public async Task FontFamilyInitializesCorrectly(string family)
		{
			var label = new LabelStub()
			{
				Text = "Test",
				Font = Font.OfSize(family, 10)
			};

			await Task.CompletedTask;
		}

		[Fact(DisplayName = "Horizontal TextAlignment Updates Correctly")]
		public async Task HorizontalTextAlignmentInitializesCorrectly()
		{
			var xplatHorizontalTextAlignment = TextAlignment.End;

			var labelStub = new LabelStub()
			{
				Text = "Test",
				HorizontalTextAlignment = xplatHorizontalTextAlignment
			};

			await Task.CompletedTask;
		}

		[Fact(DisplayName = "Padding Initializes Correctly")]
		public async Task PaddingInitializesCorrectly()
		{
			var label = new LabelStub()
			{
				Text = "Test",
				Padding = new Thickness(5, 10, 15, 20)
			};

			var handler = await CreateHandlerAsync(label);
			await Task.CompletedTask;
		}

		[Fact(DisplayName = "TextDecorations Initializes Correctly")]
		public async Task TextDecorationsInitializesCorrectly()
		{
			var xplatTextDecorations = TextDecorations.Underline;

			var labelHandler = new LabelStub()
			{
				Text = "Test", // Native values won't actually apply unless there's text
				TextDecorations = xplatTextDecorations
			};

			await Task.CompletedTask;
		}

		[Fact]
		[Category(TestCategory.TextFormatting)]
		public async Task CanSetAlignmentAndLineHeight()
		{
			// Verifying that setting LineHeight (which requires an attributed string on iOS)
			// doesn't cancel out the text alignment value (which can be set without an attributed string)

			var xplatHorizontalTextAlignment = TextAlignment.End;
			double xplatLineHeight = 2;

			var label = new LabelStub()
			{
				Text = "Test",
				HorizontalTextAlignment = xplatHorizontalTextAlignment,
				LineHeight = xplatLineHeight
			};

			await Task.CompletedTask;
		}

		[Fact]
		[Category(TestCategory.TextFormatting)]
		public async Task TextDecorationsAppliedWhenTextAdded()
		{
			TextDecorations xplatTextDecorations = TextDecorations.Underline;

			var label = new LabelStub() { TextDecorations = xplatTextDecorations }; // No text set

			var handler = await CreateHandlerAsync(label);

			label.Text = "Now we have text";
			await Task.CompletedTask;
		}

		[Fact]
		[Category(TestCategory.TextFormatting)]
		public async Task LineHeightSurvivesTextDecorations()
		{
			TextDecorations xplatTextDecorations = TextDecorations.Underline;
			double xplatLineHeight = 2;
			var expectedLineHeight = xplatLineHeight;

			var label = new LabelStub() { Text = "test", LineHeight = xplatLineHeight };

			var handler = await CreateHandlerAsync(label);

			label.TextDecorations = xplatTextDecorations;
			await InvokeOnMainThreadAsync(() => handler.UpdateValue(nameof(label.TextDecorations)));
			
			await Task.CompletedTask;
		}

		double GetNativeCharacterSpacing(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		Gtk.Align GetNativeHorizontalTextAlignment(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		async Task ValidateNativeBackgroundColor(LabelStub label, Color blue)
		{
			await Task.CompletedTask;
		}

		string GetNativeText(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		Color GetNativeTextColor(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		double GetNativeUnscaledFontSize(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		bool GetNativeIsBold(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

		bool GetNativeIsItalic(LabelHandler arg)
		{
			throw new System.NotImplementedException();
		}

	}
}