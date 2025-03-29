using System.Collections.Generic;

namespace PigNet.Net;

public class PropertySyncData
{
	public Dictionary<uint, float> floatProperties = new();
	public Dictionary<uint, int> intProperties = new();
}