using System;

namespace PigNet.Utils.Skins;

public class Cape : ICloneable
{
	public Cape()
	{
		Data = new byte[0];
	}

	public string Id { get; set; }
	public int ImageHeight { get; set; }
	public int ImageWidth { get; set; }
	public byte[] Data { get; set; }
	public bool OnClassicSkin { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}
}