using System.Collections.Generic;

namespace MiNET.Net;

public class PropertySyncData
{
	public Dictionary<uint, float> floatProperties = new();
	public Dictionary<uint, int> intProperties = new();
}