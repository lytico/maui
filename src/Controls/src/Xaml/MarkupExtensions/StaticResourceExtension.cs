﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Maui.Controls.Xaml.Internals;

namespace Microsoft.Maui.Controls.Xaml
{
	[ContentProperty(nameof(Key))]
	public sealed class StaticResourceExtension : IMarkupExtension
	{
		public string Key { get; set; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			if (serviceProvider is null)
			{
				throw new ArgumentNullException(nameof(serviceProvider));
			}

			if (Key is null)
			{
				throw new XamlParseException("you must specify a key in {StaticResource}", serviceProvider);
			}

			if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideParentValues valueProvider)
			{
				throw new ArgumentException(null, nameof(serviceProvider));
			}

			if (!TryGetResource(Key, valueProvider.ParentObjects, out var resource, out var resourceDictionary)
				&& !TryGetApplicationLevelResource(Key, out resource, out resourceDictionary))
			{
				var xmlLineInfo = serviceProvider.GetService(typeof(IXmlLineInfoProvider)) is IXmlLineInfoProvider xmlLineInfoProvider ? xmlLineInfoProvider.XmlLineInfo : null;
				throw new XamlParseException($"StaticResource not found for key {Key}", xmlLineInfo);
			}

			Diagnostics.ResourceDictionaryDiagnostics.OnStaticResourceResolved(resourceDictionary, Key, valueProvider.TargetObject, valueProvider.TargetProperty);

			return CastTo(resource, valueProvider.TargetProperty);
		}

		//used by X.HR.F
		internal static object CastTo(object value, object targetProperty)
		{
			var bp = targetProperty as BindableProperty;
			var pi = targetProperty as PropertyInfo;

			Type valueType = value.GetType();
			var propertyType = bp?.ReturnType ?? pi?.PropertyType;
			if (propertyType is null || propertyType.IsAssignableFrom(valueType))
			{
				return value;
			}

			MethodInfo implicit_op;

			//OnPlatform might need double cast
			if (valueType.IsGenericType && valueType.Name == "OnPlatform`1")
			{
				var onPlatType = valueType.GetGenericArguments()[0];
				implicit_op = valueType.GetImplicitConversionOperator(fromType: valueType, toType: onPlatType);
				value = implicit_op.Invoke(value, new[] { value });
				valueType = value.GetType();
			}

			implicit_op = valueType.GetImplicitConversionOperator(fromType: valueType, toType: propertyType)
							?? propertyType.GetImplicitConversionOperator(fromType: valueType, toType: propertyType);
			if (implicit_op != null)
			{
				return implicit_op.Invoke(value, new[] { value });
			}

			return value;
		}

		static bool TryGetResource(string key, IEnumerable<object> parentObjects, out object resource, out ResourceDictionary resourceDictionary)
		{
			resource = null;
			resourceDictionary = null;

			foreach (var p in parentObjects)
			{
				var resDict = p is IResourcesProvider irp && irp.IsResourcesCreated ? irp.Resources : p as ResourceDictionary;
				if (resDict == null)
				{
					continue;
				}

				if (resDict.TryGetValueAndSource(key, out resource, out resourceDictionary))
				{
					return true;
				}
			}
			return false;
		}

		static bool TryGetApplicationLevelResource(string key, out object resource, out ResourceDictionary resourceDictionary)
		{
			resource = null;
			resourceDictionary = null;
			return Application.Current != null
				&& ((IResourcesProvider)Application.Current).IsResourcesCreated
				&& Application.Current.Resources.TryGetValueAndSource(key, out resource, out resourceDictionary);
		}
	}
}