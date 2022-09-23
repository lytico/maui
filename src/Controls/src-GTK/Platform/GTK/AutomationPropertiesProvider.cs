using System;
using System.ComponentModel;

namespace Microsoft.Maui.Controls.Platform
{
	internal static class AutomationPropertiesProvider
	{
#if !__GTK__
		static readonly string s_defaultDrawerId = "drawer";
		static readonly string s_defaultDrawerIdOpenSuffix = "_open";
		static readonly string s_defaultDrawerIdCloseSuffix = "_close";
#endif

		static string ConcatenateNameAndHint(Element Element)
		{
			string separator;

			var name = (string)Element.GetValue(AutomationProperties.NameProperty);
			var hint = (string)Element.GetValue(AutomationProperties.HelpTextProperty);

			if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(hint))
				separator = "";
			else
				separator = ". ";

			return string.Join(separator, name, hint);
		}

		internal static void SetImportantForAccessibility(Gtk.EventBox control, Element element)
		{
			//if (element == null || control == null)
			//{
			//	return;
			//}

			//bool? isInAccessibleTree = (bool?)element.GetValue(AutomationProperties.IsInAccessibleTreeProperty);
			//bool? excludedWithChildren = (bool?)element.GetValue(AutomationProperties.ExcludedWithChildrenProperty);
			//if (excludedWithChildren == true)
			//	control.ImportantForAccessibility = ImportantForAccessibility.NoHideDescendants;
			//else if (isInAccessibleTree.HasValue)
			//	control.ImportantForAccessibility = isInAccessibleTree.Value ? ImportantForAccessibility.Yes : ImportantForAccessibility.No;
		}

		internal static void AccessibilitySettingsChanged(Gtk.EventBox control, Element element, string _defaultHint, string _defaultContentDescription)
		{
			//SetHint(control, element, _defaultHint);
			//SetAutomationId(control, element);
			//SetContentDescription(control, element, _defaultContentDescription, _defaultHint);
			//SetImportantForAccessibility(control, element);
			//SetLabeledBy(control, element);
		}

		internal static void AccessibilitySettingsChanged(Gtk.EventBox control, Element element)
		{
			string _defaultHint = String.Empty;
			string _defaultContentDescription = String.Empty;
			AccessibilitySettingsChanged(control, element, _defaultHint, _defaultContentDescription);
		}


		internal static string ConcatenateNameAndHelpText(BindableObject Element)
		{
			var name = (string)Element.GetValue(AutomationProperties.NameProperty);
			var helpText = (string)Element.GetValue(AutomationProperties.HelpTextProperty);

			if (string.IsNullOrWhiteSpace(name))
				return helpText;
			if (string.IsNullOrWhiteSpace(helpText))
				return name;

			return $"{name}. {helpText}";
		}

		internal static void SetupDefaults(Gtk.EventBox control, ref string defaultContentDescription)
		{
			string hint = null;
			SetupDefaults(control, ref defaultContentDescription, ref hint);
		}

		internal static void SetupDefaults(Gtk.EventBox control, ref string defaultContentDescription, ref string defaultHint)
		{
			//if (defaultContentDescription == null)
			//	defaultContentDescription = control.ContentDescription;

			//if (control is TextView textView && defaultHint == null)
			//{
			//	defaultHint = textView.Hint;
			//}
		}
	}
}