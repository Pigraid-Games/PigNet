using System.Collections.Generic;

namespace MiNET.Net;

public class PropertySyncData
{
	public Dictionary<uint, int> intProperties = new();
	public Dictionary<uint, float> floatProperties = new();
}