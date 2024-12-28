using System.Collections.Generic;

namespace MiNET.Utils;

public class fogStack
{
	public List<string> fogList = new();

	public fogStack(params string[] efects)
	{
		fogList.AddRange(efects);
	}
}