using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace PigNet.Utils;

public static class ResourceUtil
{
	public static T ReadResource<T>(string filename, Type namespaceProvider = null, string subFolder = null)
	{
		if (namespaceProvider == null)
			namespaceProvider = typeof(T);

		var assembly = namespaceProvider.Assembly;
		string ns = namespaceProvider.Namespace;

		if (!string.IsNullOrWhiteSpace(subFolder))
		{
			ns += $".{subFolder}";
		}

		string resourcePath = $"{ns}.{filename}";

		var stream = assembly.GetManifestResourceStream(resourcePath);
		if (stream == null)
			throw new FileNotFoundException($"Embedded resource not found: '{resourcePath}' in assembly '{assembly.FullName}'");

		using var reader = new StreamReader(stream);
		return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
	}

}