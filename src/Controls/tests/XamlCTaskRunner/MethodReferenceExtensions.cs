using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Mono.Cecil;
using Mono.Cecil.Rocks;

namespace XamlCTaskRunner
{
	static class MethodReferenceExtensions
	{
		public static MethodReference ResolveGenericParameters(this MethodReference self, TypeReference declaringTypeRef,
			ModuleDefinition module)
		{
			if (self == null)
				throw new ArgumentNullException(nameof(self));
			if (declaringTypeRef == null)
				throw new ArgumentNullException(nameof(declaringTypeRef));

			var reference = new MethodReference(self.Name, ImportUnresolvedType(self.ReturnType, module))
			{
				DeclaringType = declaringTypeRef,
				HasThis = self.HasThis,
				ExplicitThis = self.ExplicitThis,
				CallingConvention = self.CallingConvention
			};

			foreach (var parameter in self.Parameters)
			{
				var definition = new ParameterDefinition(ImportUnresolvedType(parameter.ParameterType, module));

				foreach (var attribute in parameter.CustomAttributes)
					definition.CustomAttributes.Add(attribute);

				reference.Parameters.Add(definition);
			}

			foreach (var generic_parameter in self.GenericParameters)
				reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

			return reference;
		}

		static TypeReference ImportUnresolvedType(TypeReference type, ModuleDefinition module)
		{
			if (type.IsGenericParameter)
				return type;

			var generictype = type as GenericInstanceType;
			if (generictype == null)
				return module.ImportReference(type);

			var imported = new GenericInstanceType(module.ImportReference(generictype.ElementType));

			foreach (var argument in generictype.GenericArguments)
				imported.GenericArguments.Add(ImportUnresolvedType(argument, module));

			return imported;
		}

		public static void ImportTypes(this MethodReference self, ModuleDefinition module)
		{
			if (!self.HasParameters)
				return;

			for (var i = 0; i < self.Parameters.Count; i++)
				self.Parameters[i].ParameterType = module.ImportReference(self.Parameters[i].ParameterType);
		}

		public static MethodReference MakeGeneric(this MethodReference self, TypeReference declaringType, params TypeReference[] arguments)
		{
			var reference = new MethodReference(self.Name, self.ReturnType)
			{
				DeclaringType = declaringType,
				HasThis = self.HasThis,
				ExplicitThis = self.ExplicitThis,
				CallingConvention = self.CallingConvention,
			};

			foreach (var parameter in self.Parameters)
				reference.Parameters.Add(new ParameterDefinition(parameter.ParameterType));

			foreach (var generic_parameter in self.GenericParameters)
				reference.GenericParameters.Add(new GenericParameter(generic_parameter.Name, reference));

			return reference;
		}
	}

	static partial class XamlParser
	{
		public const string MauiUri = "http://schemas.microsoft.com/dotnet/2021/maui";
		public const string MauiDesignUri = "http://schemas.microsoft.com/dotnet/2021/maui/design";
		public const string X2006Uri = "http://schemas.microsoft.com/winfx/2006/xaml";
		public const string X2009Uri = "http://schemas.microsoft.com/winfx/2009/xaml";
		public const string McUri = "http://schemas.openxmlformats.org/markup-compatibility/2006";
	}

	static class CecilExtensions
	{
		public static bool IsXaml(this EmbeddedResource resource, ModuleDefinition module, out string classname)
		{
			classname = null!;
			if (!resource.Name.EndsWith(".xaml", StringComparison.InvariantCulture))
				return false;

			using (var resourceStream = resource.GetResourceStream())
			using (var reader = XmlReader.Create(resourceStream))
			{
				// Read to the first Element
				while (reader.Read() && reader.NodeType != XmlNodeType.Element)
					;

				if (reader.NodeType != XmlNodeType.Element)
					return false;

				classname = reader.GetAttribute("Class", XamlParser.X2009Uri) ??
							reader.GetAttribute("Class", XamlParser.X2006Uri);
				if (classname != null)
					return true;

				//no x:Class, but it might be a RD without x:Class and with <?xaml-comp compile="true" ?>
				//in that case, it has a XamlResourceIdAttribute
				var typeRef = GetTypeForResourceId(module, resource.Name);
				if (typeRef != null)
				{
					classname = typeRef.FullName;
					return true;
				}

				return false;
			}
		}

		static TypeReference GetTypeForResourceId(ModuleDefinition module, string resourceId)
		{
			foreach (var ca in module.GetCustomAttributes())
			{
				if (!TypeRefComparer.Default.Equals(ca.AttributeType, module.ImportReference(("Microsoft.Maui.Controls", "Microsoft.Maui.Controls.Xaml", "XamlResourceIdAttribute"))))
					continue;
				if (ca.ConstructorArguments[0].Value as string != resourceId)
					continue;
				return ca.ConstructorArguments[2].Value as TypeReference;
			}
			return null;
		}
	}

	class TypeRefComparer : IEqualityComparer<TypeReference>
	{
		static string GetAssembly(TypeReference typeRef)
		{
			if (typeRef.Scope is ModuleDefinition md)
				return md.Assembly.FullName;
			if (typeRef.Scope is AssemblyNameReference anr)
				return anr.FullName;
			throw new ArgumentOutOfRangeException(nameof(typeRef));
		}

		public bool Equals(TypeReference x, TypeReference y)
		{
			if (x == null)
				return y == null;
			if (y == null)
				return x == null;

			//strip the leading `&` as byref typered fullnames have a `&`
			var xname = x.FullName.EndsWith("&", StringComparison.InvariantCulture) ? x.FullName.Substring(0, x.FullName.Length - 1) : x.FullName;
			var yname = y.FullName.EndsWith("&", StringComparison.InvariantCulture) ? y.FullName.Substring(0, y.FullName.Length - 1) : y.FullName;
			if (xname != yname)
				return false;
			var xasm = GetAssembly(x);
			var yasm = GetAssembly(y);

			//standard types comes from either mscorlib. System.Runtime or netstandard. Assume they are equivalent
			if ((xasm.StartsWith("System.Runtime", StringComparison.Ordinal)
					|| xasm.StartsWith("System", StringComparison.Ordinal)
					|| xasm.StartsWith("mscorlib", StringComparison.Ordinal)
					|| xasm.StartsWith("netstandard", StringComparison.Ordinal)
					|| xasm.StartsWith("System.Xml", StringComparison.Ordinal))
				&& (yasm.StartsWith("System.Runtime", StringComparison.Ordinal)
					|| yasm.StartsWith("System", StringComparison.Ordinal)
					|| yasm.StartsWith("mscorlib", StringComparison.Ordinal)
					|| yasm.StartsWith("netstandard", StringComparison.Ordinal)
					|| yasm.StartsWith("System.Xml", StringComparison.Ordinal)))
				return true;
			return xasm == yasm;
		}

		public int GetHashCode(TypeReference obj)
		{
			return $"{GetAssembly(obj)}//{obj.FullName}".GetHashCode(StringComparison.Ordinal);
		}

		static TypeRefComparer s_default;
		public static TypeRefComparer Default => s_default ?? (s_default = new TypeRefComparer());
	}

	static class TypeReferenceExtensions
	{
		public static PropertyDefinition GetProperty(this TypeReference typeRef, Func<PropertyDefinition, bool> predicate,
			out TypeReference declaringTypeRef)
		{
			declaringTypeRef = typeRef;
			var typeDef = typeRef.ResolveCached();
			var properties = typeDef.Properties.Where(predicate);
			if (properties.Any())
				return properties.Single();
			if (typeDef.IsInterface)
			{
				foreach (var face in typeDef.Interfaces)
				{
					var p = face.InterfaceType.ResolveGenericParameters(typeRef).GetProperty(predicate, out var interfaceDeclaringTypeRef);
					if (p != null)
					{
						declaringTypeRef = interfaceDeclaringTypeRef;
						return p;
					}
				}
			}
			if (typeDef.BaseType == null || typeDef.BaseType.FullName == "System.Object")
				return null;
			return typeDef.BaseType.ResolveGenericParameters(typeRef).GetProperty(predicate, out declaringTypeRef);
		}

		public static EventDefinition GetEvent(this TypeReference typeRef, Func<EventDefinition, bool> predicate,
			out TypeReference declaringTypeRef)
		{
			declaringTypeRef = typeRef;
			var typeDef = typeRef.ResolveCached();
			var events = typeDef.Events.Where(predicate);
			if (events.Any())
			{
				var ev = events.Single();
				return ev.ResolveGenericEvent(declaringTypeRef);
			}
			if (typeDef.BaseType == null || typeDef.BaseType.FullName == "System.Object")
				return null;
			return typeDef.BaseType.ResolveGenericParameters(typeRef).GetEvent(predicate, out declaringTypeRef);
		}

		//this resolves generic eventargs (https://bugzilla.xamarin.com/show_bug.cgi?id=57574)
		static EventDefinition ResolveGenericEvent(this EventDefinition eventDef, TypeReference declaringTypeRef)
		{
			if (eventDef == null)
				throw new ArgumentNullException(nameof(eventDef));
			if (declaringTypeRef == null)
				throw new ArgumentNullException(nameof(declaringTypeRef));
			if (!eventDef.EventType.IsGenericInstance)
				return eventDef;
			if (eventDef.EventType.ResolveCached().FullName != "System.EventHandler`1")
				return eventDef;

			var git = eventDef.EventType as GenericInstanceType;
			var ga = git.GenericArguments.First();
			ga = ga.ResolveGenericParameters(declaringTypeRef);
			git.GenericArguments[0] = ga;
			eventDef.EventType = git;

			return eventDef;

		}
		public static FieldDefinition GetField(this TypeReference typeRef, Func<FieldDefinition, bool> predicate,
			out TypeReference declaringTypeRef)
		{
			declaringTypeRef = typeRef;
			var typeDef = typeRef.ResolveCached();
			var bp = typeDef.Fields.Where
				(predicate);
			if (bp.Any())
				return bp.Single();
			if (typeDef.BaseType == null || typeDef.BaseType.FullName == "System.Object")
				return null;
			return typeDef.BaseType.ResolveGenericParameters(typeRef).GetField(predicate, out declaringTypeRef);
		}

		public static bool ImplementsInterface(this TypeReference typeRef, TypeReference @interface)
		{
			var typeDef = typeRef.ResolveCached();
			if (typeDef.Interfaces.Any(tr => tr.InterfaceType.FullName == @interface.FullName))
				return true;
			var baseTypeRef = typeDef.BaseType;
			if (baseTypeRef != null && baseTypeRef.FullName != "System.Object")
				return baseTypeRef.ImplementsInterface(@interface);
			return false;
		}

		public static bool ImplementsGenericInterface(this TypeReference typeRef, string @interface,
			out GenericInstanceType interfaceReference, out IList<TypeReference> genericArguments)
		{
			interfaceReference = null;
			genericArguments = null;
			var typeDef = typeRef.ResolveCached();
			InterfaceImplementation iface;
			if ((iface = typeDef.Interfaces.FirstOrDefault(tr =>
							tr.InterfaceType.FullName.StartsWith(@interface, StringComparison.Ordinal) &&
							tr.InterfaceType.IsGenericInstance && (tr.InterfaceType as GenericInstanceType).HasGenericArguments)) != null)
			{
				interfaceReference = (iface.InterfaceType as GenericInstanceType).ResolveGenericParameters(typeRef);
				genericArguments = interfaceReference.GenericArguments;
				return true;
			}
			var baseTypeRef = typeDef.BaseType;
			if (baseTypeRef != null && baseTypeRef.FullName != "System.Object")
				return baseTypeRef.ResolveGenericParameters(typeRef).ImplementsGenericInterface(@interface, out interfaceReference, out genericArguments);
			return false;
		}

		static readonly string[] arrayInterfaces = {
			"System.ICloneable",
			"System.Collections.IEnumerable",
			"System.Collections.IList",
			"System.Collections.ICollection",
			"System.Collections.IStructuralComparable",
			"System.Collections.IStructuralEquatable",
		};

		static readonly string[] arrayGenericInterfaces = {
			"System.Collections.Generic.IEnumerable`1",
			"System.Collections.Generic.IList`1",
			"System.Collections.Generic.ICollection`1",
			"System.Collections.Generic.IReadOnlyCollection`1",
			"System.Collections.Generic.IReadOnlyList`1",
		};

		public static bool InheritsFromOrImplements(this TypeReference typeRef, TypeReference baseClass)
		{
			if (typeRef is GenericInstanceType genericInstance)
			{
				if (baseClass is GenericInstanceType genericInstanceBaseClass &&
						TypeRefComparer.Default.Equals(genericInstance.ElementType, genericInstanceBaseClass.ElementType))
				{
					foreach (var parameter in genericInstanceBaseClass.ElementType.ResolveCached().GenericParameters)
					{
						var argument = genericInstance.GenericArguments[parameter.Position];
						var baseClassArgument = genericInstanceBaseClass.GenericArguments[parameter.Position];

						if (parameter.IsCovariant)
						{
							if (!argument.InheritsFromOrImplements(baseClassArgument))
								return false;
						}
						else if (parameter.IsContravariant)
						{
							if (!baseClassArgument.InheritsFromOrImplements(argument))
								return false;
						}
						else if (!TypeRefComparer.Default.Equals(argument, baseClassArgument))
						{
							return false;
						}
					}

					return true;
				}
			}
			else
			{
				if (TypeRefComparer.Default.Equals(typeRef, baseClass))
					return true;

				if (typeRef.IsArray)
				{
					var array = (ArrayType)typeRef;
					var arrayType = typeRef.ResolveCached();
					if (arrayInterfaces.Contains(baseClass.FullName))
						return true;
					if (array.IsVector &&  //generic interfaces are not implemented on multidimensional arrays
						arrayGenericInterfaces.Contains(baseClass.ResolveCached().FullName) &&
						baseClass.IsGenericInstance &&
						TypeRefComparer.Default.Equals((baseClass as GenericInstanceType).GenericArguments[0], arrayType))
						return true;
					return baseClass.FullName == "System.Object";
				}
			}

			if (typeRef.IsValueType)
				return false;

			if (typeRef.FullName == "System.Object")
				return false;
			var typeDef = typeRef.ResolveCached();
			if (typeDef.Interfaces.Any(ir => ir.InterfaceType.ResolveGenericParameters(typeRef).InheritsFromOrImplements(baseClass)))
				return true;
			if (typeDef.BaseType == null)
				return false;

			typeRef = typeDef.BaseType.ResolveGenericParameters(typeRef);
			return typeRef.InheritsFromOrImplements(baseClass);
		}

		static CustomAttribute GetCustomAttribute(this TypeReference typeRef, TypeReference attribute)
		{
			var typeDef = typeRef.ResolveCached();
			//FIXME: avoid string comparison. make sure the attribute TypeRef is the same one
			var attr = typeDef.CustomAttributes.SingleOrDefault(ca => ca.AttributeType.FullName == attribute.FullName);
			if (attr != null)
				return attr;
			var baseTypeRef = typeDef.BaseType;
			if (baseTypeRef != null && baseTypeRef.FullName != "System.Object")
				return baseTypeRef.GetCustomAttribute(attribute);
			return null;
		}

		public static CustomAttribute GetCustomAttribute(this TypeReference typeRef, ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) attributeType)
		{
			return typeRef.GetCustomAttribute(module.ImportReference(attributeType));
		}

		public static IEnumerable<Tuple<MethodDefinition, TypeReference>> GetMethods(this TypeReference typeRef,
			Func<MethodDefinition, bool> predicate, ModuleDefinition module)
		{
			return typeRef.GetMethods((md, tr) => predicate(md), module);
		}

		public static IEnumerable<Tuple<MethodDefinition, TypeReference>> GetMethods(this TypeReference typeRef,
			Func<MethodDefinition, TypeReference, bool> predicate, ModuleDefinition module)
		{
			var typeDef = typeRef.ResolveCached();
			foreach (var method in typeDef.Methods.Where(md => predicate(md, typeRef)))
				yield return new Tuple<MethodDefinition, TypeReference>(method, typeRef);
			if (typeDef.IsInterface)
			{
				foreach (var face in typeDef.Interfaces)
				{
					if (face.InterfaceType.IsGenericInstance && typeRef is GenericInstanceType)
					{
						int i = 0;
						foreach (var arg in ((GenericInstanceType)typeRef).GenericArguments)
							((GenericInstanceType)face.InterfaceType).GenericArguments[i++] = module.ImportReference(arg);
					}
					foreach (var tuple in face.InterfaceType.GetMethods(predicate, module))
						yield return tuple;
				}
				yield break;
			}
			if (typeDef.BaseType == null || typeDef.BaseType.FullName == "System.Object")
				yield break;
			var baseType = typeDef.BaseType.ResolveGenericParameters(typeRef);
			foreach (var tuple in baseType.GetMethods(predicate, module))
				yield return tuple;
		}

		public static MethodReference GetImplicitOperatorTo(this TypeReference fromType, TypeReference toType, ModuleDefinition module)
		{
			if (TypeRefComparer.Default.Equals(fromType, toType))
				return null;

			var implicitOperatorsOnFromType = fromType.GetMethods(md => md.IsPublic
																		&& md.IsStatic
																		&& md.IsSpecialName
																		&& md.Name == "op_Implicit", module);
			var implicitOperatorsOnToType = toType.GetMethods(md => md.IsPublic
																	&& md.IsStatic
																	&& md.IsSpecialName
																	&& md.Name == "op_Implicit", module);
			var implicitOperators = implicitOperatorsOnFromType.Concat(implicitOperatorsOnToType).ToList();

			if (implicitOperators.Any())
			{
				foreach (var op in implicitOperators)
				{
					var cast = op.Item1;
					var opDeclTypeRef = op.Item2;
					var castDef = module.ImportReference(cast).ResolveGenericParameters(opDeclTypeRef, module);
					var returnType = castDef.ReturnType;
					if (returnType.IsGenericParameter)
						returnType = ((GenericInstanceType)opDeclTypeRef).GenericArguments[((GenericParameter)returnType).Position];
					if (!returnType.InheritsFromOrImplements(toType))
						continue;
					var paramType = cast.Parameters[0].ParameterType.ResolveGenericParameters(castDef);
					if (!fromType.InheritsFromOrImplements(paramType))
						continue;
					return castDef;
				}
			}
			return null;
		}

		public static TypeReference ResolveGenericParameters(this TypeReference self, MethodReference declaringMethodReference)
		{
			var genericParameterSelf = self as GenericParameter;
			var genericdeclMethod = declaringMethodReference as GenericInstanceMethod;
			var declaringTypeReference = declaringMethodReference.DeclaringType;
			var genericdeclType = declaringTypeReference as GenericInstanceType;

			if (genericParameterSelf != null)
			{
				switch (genericParameterSelf.Type)
				{
					case GenericParameterType.Method:
						self = genericdeclMethod.GenericArguments[genericParameterSelf.Position];
						break;

					case GenericParameterType.Type:
						self = genericdeclType.GenericArguments[genericParameterSelf.Position];
						break;
				}
			}

			var genericself = self as GenericInstanceType;
			if (genericself == null)
				return self;

			genericself = genericself.ResolveGenericParameters(declaringTypeReference);
			for (var i = 0; i < genericself.GenericArguments.Count; i++)
			{
				var genericParameter = genericself.GenericArguments[i] as GenericParameter;
				if (genericParameter != null)
					genericself.GenericArguments[i] = genericdeclMethod.GenericArguments[genericParameter.Position];
			}
			return genericself;
		}

		public static TypeReference ResolveGenericParameters(this TypeReference self, TypeReference declaringTypeReference)
		{
			var genericdeclType = declaringTypeReference as GenericInstanceType;
			var genericParameterSelf = self as GenericParameter;
			var genericself = self as GenericInstanceType;

			if (genericdeclType == null && genericParameterSelf == null && genericself == null)
				return self;

			if (genericdeclType == null && genericParameterSelf != null)
			{
				var typeDef = declaringTypeReference.Resolve();
				if (typeDef.BaseType == null || typeDef.BaseType.FullName == "System.Object")
					return self;
				return self.ResolveGenericParameters(typeDef.BaseType.ResolveGenericParameters(declaringTypeReference));
			}
			if (genericParameterSelf != null)
				return genericdeclType.GenericArguments[genericParameterSelf.Position];

			if (genericself != null)
				return genericself.ResolveGenericParameters(declaringTypeReference);

			return self;
		}

		public static GenericInstanceType ResolveGenericParameters(this GenericInstanceType self, TypeReference declaringTypeReference)
		{
			var genericdeclType = declaringTypeReference as GenericInstanceType;
			if (genericdeclType == null)
				return self;

			return self.ResolveGenericParameters(genericdeclType);
		}

		public static GenericInstanceType ResolveGenericParameters(this GenericInstanceType self, GenericInstanceType declaringTypeReference)
		{
			List<TypeReference> args = new List<TypeReference>();
			for (var i = 0; i < self.GenericArguments.Count; i++)
			{
				var genericParameter = self.GenericArguments[i] as GenericParameter;
				if (genericParameter == null)
					args.Add(self.GenericArguments[i].ResolveGenericParameters(declaringTypeReference));
				else if (genericParameter.Type == GenericParameterType.Type)
					args.Add(declaringTypeReference.GenericArguments[genericParameter.Position]);
			}
			return self.ElementType.MakeGenericInstanceType(args.ToArray());
		}

		static Dictionary<TypeReference, TypeDefinition> resolves = new Dictionary<TypeReference, TypeDefinition>();
		public static TypeDefinition ResolveCached(this TypeReference typeReference)
		{
			if (resolves.TryGetValue(typeReference, out var typeDefinition))
				return typeDefinition;
			return (resolves[typeReference] = typeReference.Resolve());
		}
	}

	static class ModuleDefinitionExtensions
	{
		static Dictionary<(ModuleDefinition module, string typeKey), TypeReference> TypeRefCache = new Dictionary<(ModuleDefinition module, string typeKey), TypeReference>();
		public static TypeReference ImportReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type)
		{
			var typeKey = type.ToString();
			if (!TypeRefCache.TryGetValue((module, typeKey), out var typeRef))
				TypeRefCache.Add((module, typeKey), typeRef = module.ImportReference(module.GetTypeDefinition(type)));
			return typeRef;
		}

		public static TypeReference ImportReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, (string assemblyName, string clrNamespace, string typeName)[] classArguments)
		{
			var typeKey = $"{type}<{string.Join(",", classArguments)}>";
			if (!TypeRefCache.TryGetValue((module, typeKey), out var typeRef))
				TypeRefCache.Add((module, typeKey), typeRef = module.ImportReference(module.ImportReference(type).MakeGenericInstanceType(classArguments.Select(gp => module.GetTypeDefinition((gp.assemblyName, gp.clrNamespace, gp.typeName))).ToArray())));
			return typeRef;
		}

		public static TypeReference ImportArrayReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type)
		{
			var typeKey = "${type}[]";
			if (!TypeRefCache.TryGetValue((module, typeKey), out var typeRef))
				TypeRefCache.Add((module, typeKey), typeRef = module.ImportReference(module.ImportReference(type).MakeArrayType()));
			return typeRef;
		}

		static Dictionary<(ModuleDefinition module, string methodRefKey), MethodReference> MethodRefCache = new Dictionary<(ModuleDefinition module, string methodRefKey), MethodReference>();
		static MethodReference ImportCtorReference(this ModuleDefinition module, TypeReference type, TypeReference[] classArguments, Func<MethodDefinition, bool> predicate)
		{
			var ctor = module.ImportReference(type).ResolveCached().Methods.FirstOrDefault(md => !md.IsPrivate && !md.IsStatic && md.IsConstructor && (predicate?.Invoke(md) ?? true));
			if (ctor is null)
				return null;
			var ctorRef = module.ImportReference(ctor);
			if (classArguments == null)
				return ctorRef;
			return module.ImportReference(ctorRef.ResolveGenericParameters(type.MakeGenericInstanceType(classArguments), module));
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, TypeReference type, TypeReference[] parameterTypes)
		{
			var ctorKey = $"{type}.ctor({(parameterTypes == null ? "" : string.Join(",", parameterTypes.Select(SerializeTypeReference)))})";
			if (MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				return ctorRef;
			ctorRef = module.ImportCtorReference(type, classArguments: null, predicate: md =>
			{
				if (md.Parameters.Count != (parameterTypes?.Length ?? 0))
					return false;
				for (var i = 0; i < md.Parameters.Count; i++)
					if (!TypeRefComparer.Default.Equals(md.Parameters[i].ParameterType, module.ImportReference(parameterTypes[i])))
						return false;
				return true;
			});
			MethodRefCache.Add((module, ctorKey), ctorRef);
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, int paramCount)
		{
			var ctorKey = $"{type}.ctor({(string.Join(",", Enumerable.Repeat("_", paramCount)))})";
			if (!MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				MethodRefCache.Add((module, ctorKey), ctorRef = module.ImportCtorReference(module.GetTypeDefinition(type), null, md => md.Parameters.Count == paramCount));
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, TypeReference type, int paramCount)
		{
			var ctorKey = $"{type}.ctor({(string.Join(",", Enumerable.Repeat("_", paramCount)))})";
			if (!MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				MethodRefCache.Add((module, ctorKey), ctorRef = module.ImportCtorReference(type, null, md => md.Parameters.Count == paramCount));
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, int paramCount, (string assemblyName, string clrNamespace, string typeName)[] classArguments)
		{
			var ctorKey = $"{type}<{(string.Join(",", classArguments))}>.ctor({(string.Join(",", Enumerable.Repeat("_", paramCount)))})";
			if (!MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				MethodRefCache.Add((module, ctorKey), ctorRef = module.ImportCtorReference(module.GetTypeDefinition(type), classArguments.Select(module.GetTypeDefinition).ToArray(), md => md.Parameters.Count == paramCount));
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, int paramCount, TypeReference[] classArguments)
		{
			var ctorKey = $"{type}<{string.Join(",", classArguments.Select(SerializeTypeReference))}>.ctor({(string.Join(",", Enumerable.Repeat("_", paramCount)))})";
			if (!MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				MethodRefCache.Add((module, ctorKey), ctorRef = module.ImportCtorReference(module.GetTypeDefinition(type), classArguments, predicate: md => md.Parameters.Count == paramCount));
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, (string assemblyName, string clrNamespace, string typeName)[] parameterTypes, (string assemblyName, string clrNamespace, string typeName)[] classArguments)
		{
			var ctorKey = $"{type}<{(string.Join(",", classArguments))}>.ctor({(parameterTypes == null ? "" : string.Join(",", parameterTypes))})";
			if (MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				return ctorRef;
			ctorRef = module.ImportCtorReference(module.GetTypeDefinition(type), classArguments.Select(module.GetTypeDefinition).ToArray(), md =>
			{
				if (md.Parameters.Count != (parameterTypes?.Length ?? 0))
					return false;
				for (var i = 0; i < md.Parameters.Count; i++)
					if (!TypeRefComparer.Default.Equals(md.Parameters[i].ParameterType, module.ImportReference(parameterTypes[i])))
						return false;
				return true;
			});
			MethodRefCache.Add((module, ctorKey), ctorRef);
			return ctorRef;
		}

		public static MethodReference ImportCtorReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, (string assemblyName, string clrNamespace, string typeName)[] parameterTypes)
		{
			var ctorKey = $"{type}.ctor({(parameterTypes == null ? "" : string.Join(",", parameterTypes))})";
			if (MethodRefCache.TryGetValue((module, ctorKey), out var ctorRef))
				return ctorRef;
			ctorRef = module.ImportCtorReference(module.GetTypeDefinition(type), classArguments: null, predicate: md =>
			{
				if (md.Parameters.Count != (parameterTypes?.Length ?? 0))
					return false;
				for (var i = 0; i < md.Parameters.Count; i++)
					if (!TypeRefComparer.Default.Equals(md.Parameters[i].ParameterType, module.ImportReference(parameterTypes[i])))
						return false;
				return true;
			});
			MethodRefCache.Add((module, ctorKey), ctorRef);
			return ctorRef;
		}

		static MethodReference ImportPropertyGetterReference(this ModuleDefinition module, TypeReference type, string propertyName, Func<PropertyDefinition, bool> predicate = null, bool flatten = false, bool caseSensitive = true)
		{
			var properties = module.ImportReference(type).Resolve().Properties;
			var getter = module
				.ImportReference(type)
				.ResolveCached()
				.Properties(flatten)
				.FirstOrDefault(pd =>
								   string.Equals(pd.Name, propertyName, caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase)
								&& !pd.GetMethod.IsPrivate
								&& (predicate?.Invoke(pd) ?? true))
				?.GetMethod;
			return getter == null ? null : module.ImportReference(getter);
		}

		public static MethodReference ImportPropertyGetterReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, string propertyName, bool isStatic = false, bool flatten = false, bool caseSensitive = true)
		{
			var getterKey = $"{(isStatic ? "static " : "")}{type}.get_{propertyName}{(flatten ? "*" : "")}";
			if (!MethodRefCache.TryGetValue((module, getterKey), out var methodReference))
				MethodRefCache.Add((module, getterKey), methodReference = module.ImportPropertyGetterReference(module.GetTypeDefinition(type), propertyName, pd => pd.GetMethod.IsStatic == isStatic, flatten, caseSensitive: caseSensitive));
			return methodReference;
		}

		static MethodReference ImportPropertySetterReference(this ModuleDefinition module, TypeReference type, string propertyName, Func<PropertyDefinition, bool> predicate = null)
		{
			var setter = module
				.ImportReference(type)
				.ResolveCached()
				.Properties
				.FirstOrDefault(pd =>
								   pd.Name == propertyName
								&& !pd.SetMethod.IsPrivate
								&& (predicate?.Invoke(pd) ?? true))
				?.SetMethod;
			return setter == null ? null : module.ImportReference(setter);
		}

		public static MethodReference ImportPropertySetterReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, string propertyName, bool isStatic = false)
		{
			var setterKey = $"{(isStatic ? "static " : "")}{type}.set{propertyName}";
			if (!MethodRefCache.TryGetValue((module, setterKey), out var methodReference))
				MethodRefCache.Add((module, setterKey), methodReference = module.ImportPropertySetterReference(module.GetTypeDefinition(type), propertyName, pd => pd.SetMethod.IsStatic == isStatic));
			return methodReference;
		}

		static MethodReference ImportMethodReference(this ModuleDefinition module, TypeReference type, string methodName, Func<MethodDefinition, bool> predicate = null, TypeReference[] classArguments = null)
		{
			var method = module
				.ImportReference(type)
				.ResolveCached()
				.Methods
				.FirstOrDefault(md =>
								   !md.IsConstructor
								&& !md.IsPrivate
								&& md.Name == methodName
								&& (predicate?.Invoke(md) ?? true));
			if (method is null)
				return null;
			var methodRef = module.ImportReference(method);
			if (classArguments == null)
				return methodRef;
			return module.ImportReference(methodRef.ResolveGenericParameters(type.MakeGenericInstanceType(classArguments), module));
		}

		public static MethodReference ImportMethodReference(this ModuleDefinition module,
													 TypeReference type,
													 string methodName,
													 TypeReference[] parameterTypes = null,
													 TypeReference[] classArguments = null,
													 bool isStatic = false)
		{
			return module.ImportMethodReference(type,
												methodName: methodName,
												predicate: md =>
												{
													if (md.IsStatic != isStatic)
														return false;
													if (md.Parameters.Count != (parameterTypes?.Length ?? 0))
														return false;
													for (var i = 0; i < md.Parameters.Count; i++)
														if (!TypeRefComparer.Default.Equals(md.Parameters[i].ParameterType, parameterTypes[i]))
															return false;
													return true;
												},
												classArguments: classArguments);
		}

		public static MethodReference ImportMethodReference(this ModuleDefinition module,
															(string assemblyName, string clrNamespace, string typeName) type,
															string methodName,
															(string assemblyName, string clrNamespace, string typeName)[] parameterTypes,
															(string assemblyName, string clrNamespace, string typeName)[] classArguments = null,
															bool isStatic = false)
		{
			var methodKey = $"{(isStatic ? "static " : "")}{type}<{(classArguments == null ? "" : string.Join(",", classArguments))}>.({(parameterTypes == null ? "" : string.Join(",", parameterTypes))})";
			if (MethodRefCache.TryGetValue((module, methodKey), out var methodReference))
				return methodReference;
			methodReference = module.ImportMethodReference(module.GetTypeDefinition(type),
														   methodName: methodName,
														   predicate: md =>
														   {
															   if (md.IsStatic != isStatic)
																   return false;
															   if (md.Parameters.Count != (parameterTypes?.Length ?? 0))
																   return false;
															   for (var i = 0; i < md.Parameters.Count; i++)
																   if (!TypeRefComparer.Default.Equals(md.Parameters[i].ParameterType, module.ImportReference(parameterTypes[i])))
																	   return false;
															   return true;
														   },
														   classArguments: classArguments?.Select(gp => module.GetTypeDefinition((gp.assemblyName, gp.clrNamespace, gp.typeName))).ToArray());
			MethodRefCache.Add((module, methodKey), methodReference);
			return methodReference;
		}

		public static MethodReference ImportMethodReference(this ModuleDefinition module,
													(string assemblyName, string clrNamespace, string typeName) type,
													string methodName,
													int paramCount,
													(string assemblyName, string clrNamespace, string typeName)[] classArguments = null,
													bool isStatic = false)
		{
			var methodKey = $"{(isStatic ? "static " : "")}{type}<{(classArguments == null ? "" : string.Join(",", classArguments))}>.({(string.Join(",", Enumerable.Repeat("_", paramCount)))})";
			if (MethodRefCache.TryGetValue((module, methodKey), out var methodReference))
				return methodReference;
			methodReference = module.ImportMethodReference(module.GetTypeDefinition(type),
														   methodName: methodName,
														   predicate: md =>
														   {
															   if (md.IsStatic != isStatic)
																   return false;
															   if (md.Parameters.Count != paramCount)
																   return false;
															   return true;
														   },
														   classArguments: classArguments?.Select(gp => module.GetTypeDefinition((gp.assemblyName, gp.clrNamespace, gp.typeName))).ToArray());
			MethodRefCache.Add((module, methodKey), methodReference);
			return methodReference;
		}

		static Dictionary<(ModuleDefinition module, string fieldRefKey), FieldReference> FieldRefCache = new Dictionary<(ModuleDefinition module, string fieldRefKey), FieldReference>();
		static FieldReference ImportFieldReference(this ModuleDefinition module, TypeReference type, string fieldName, Func<FieldDefinition, bool> predicate = null, bool caseSensitive = true)
		{
			var field = module
				.ImportReference(type)
				.ResolveCached()
				.Fields
				.FirstOrDefault(fd =>
								   string.Equals(fd.Name, fieldName, caseSensitive ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase)
								&& (predicate?.Invoke(fd) ?? true));
			return field == null ? null : module.ImportReference(field);
		}

		public static FieldReference ImportFieldReference(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type, string fieldName, bool isStatic = false, bool caseSensitive = true)
		{
			var fieldKey = $"{(isStatic ? "static " : "")}{type}.{(caseSensitive ? fieldName : fieldName.ToLowerInvariant())}";
			if (!FieldRefCache.TryGetValue((module, fieldKey), out var fieldReference))
				FieldRefCache.Add((module, fieldKey), fieldReference = module.ImportFieldReference(module.GetTypeDefinition(type), fieldName: fieldName, predicate: fd => fd.IsStatic == isStatic, caseSensitive: caseSensitive));
			return fieldReference;
		}

		static Dictionary<(ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName)), TypeDefinition> typeDefCache
			= new Dictionary<(ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName)), TypeDefinition>();

		public static TypeDefinition GetTypeDefinition(this ModuleDefinition module, (string assemblyName, string clrNamespace, string typeName) type)
		{
			if (typeDefCache.TryGetValue((module, type), out TypeDefinition cachedTypeDefinition))
				return cachedTypeDefinition;

			var asm = module.Assembly.Name.Name == type.assemblyName
							? module.Assembly
							: module.AssemblyResolver.Resolve(AssemblyNameReference.Parse(type.assemblyName));
			var typeDef = asm.MainModule.GetType($"{type.clrNamespace}.{type.typeName}");
			if (typeDef != null)
			{
				typeDefCache.Add((module, type), typeDef);
				return typeDef;
			}
			var exportedType = asm.MainModule.ExportedTypes.FirstOrDefault(
				arg => arg.IsForwarder && arg.Namespace == type.clrNamespace && arg.Name == type.typeName);
			if (exportedType != null)
			{
				typeDef = exportedType.Resolve();
				typeDefCache.Add((module, type), typeDef);
				return typeDef;
			}

			//I hate you, netstandard
			if (type.assemblyName == "mscorlib" && type.clrNamespace == "System.Reflection")
				return module.GetTypeDefinition(("System.Reflection", type.clrNamespace, type.typeName));
			return null;
		}

		static IEnumerable<PropertyDefinition> Properties(this TypeDefinition typedef, bool flatten)
		{
			foreach (var property in typedef.Properties)
				yield return property;
			if (!flatten || typedef.BaseType == null)
				yield break;
			foreach (var property in typedef.BaseType.ResolveCached().Properties(true))
				yield return property;
		}

		static string SerializeTypeReference(TypeReference tr)
		{
			var serialized = $"{tr.Scope.Name},{tr.Namespace},{tr.Name}";
			var gitr = tr as GenericInstanceType;
			return gitr == null ? serialized : $"{serialized}<{string.Join(",", gitr.GenericArguments.Select(SerializeTypeReference))}>";
		}
	}
}