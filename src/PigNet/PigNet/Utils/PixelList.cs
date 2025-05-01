using System.Collections.Generic;

namespace PigNet.Utils;

public class PixelList
{
	public List<PixelData> mapData = [];
}

public class PixelData
{
	public short index;
	public uint pixel;
}