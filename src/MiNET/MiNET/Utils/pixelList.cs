using System.Collections.Generic;

namespace MiNET.Utils;

public class pixelList
{
	public List<pixelsData> mapData = new();
}

public class pixelsData
{
	public short index;
	public uint pixel;
}