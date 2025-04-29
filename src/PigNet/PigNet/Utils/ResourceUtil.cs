using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace PigNet.Utils;

public static class ResourceUtil
{
	public static T ReadResource<T>(string filename, Type namespaceProvider = null, string subFolder = null)
	{
		namespaceProvider ??= typeof(T);
			
		var assembly = Assembly.GetAssembly(namespaceProvider);
		string ns = namespaceProvider.Namespace;

		if (!string.IsNullOrWhiteSpace(subFolder)) ns += $".{subFolder}";
		using Stream stream = assembly.GetManifestResourceStream( ns + $".{filename}");
		using var reader = new StreamReader(stream);
		return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
	}
}