using System.Collections.Generic;
using PigNet.Net;

namespace PigNet.Utils;

public class ResourcePackInfos : List<ResourcePackInfo>
{
}

public class ResourcePackInfo
{
	/// <summary>
	///     The unique identifier for the pack
	/// </summary>
	public UUID UUID { get; set; }

	/// <summary>
	///     Version is the version of the pack. The client will cache packs sent by the server as
	///     long as they carry the same version. Sending a pack with a different version than previously
	/// </summary>
	public string Version { get; set; }

	/// <summary>
	///     Size is the total size in bytes that the texture pack occupies. This is the size of the compressed
	///     archive (zip) of the texture pack.
	/// </summary>
	public ulong Size { get; set; }

	/// <summary>
	///     ContentKey is the key used to decrypt the behaviour pack if it is encrypted. This is generally the case
	///     for marketplace texture packs.
	/// </summary>
	public string ContentKey { get; set; }

	public string SubPackName { get; set; }

	/// <summary>
	///     Size is the total size in bytes that the texture pack occupies. This is the size of the compressed archive (zip) of
	///     the texture pack.
	/// </summary>
	public string ContentIdentity { get; set; }

	/// <summary>
	///     HasScripts specifies if the texture packs has any scripts in it. A client will only download the behaviour pack if
	///     it supports scripts, which, up to 1.11, only includes Windows 10.
	/// </summary>
	public bool HasScripts { get; set; }

	public bool isAddon { get; set; }
	public string cndUrls { get; set; }
}

public class TexturePackInfos : List<TexturePackInfo>
{
}

public class TexturePackInfo : ResourcePackInfo
{
	/// <summary>
	///     RTXEnabled specifies if the texture pack uses the raytracing technology introduced in 1.16.200.
	/// </summary>
	public bool RtxEnabled { get; set; }
}

public class ResourcePackIdVersions : List<PackIdVersion>
{
}

public class PackIdVersion
{
	public string Id { get; set; }
	public string Version { get; set; }
	public string SubPackName { get; set; }
}

public class ResourcePackIds : List<string>
{
}

public enum ResourcePackType : byte
{
	Addon = 1,
	Cached = 2,
	CopyProtected = 3,
	Behaviour = 4,
	PersonaPiece = 5,
	Resources = 6,
	Skins = 7,
	WorldTemplate = 8
}

public class Header
{
	public string Description { get; set; }
	public string Name { get; set; }
	public string Uuid { get; set; }
	public List<int> Version { get; set; }
	public List<int> MinEngineVersion { get; set; }
}

public class Module
{
	public string Type { get; set; }
	public string Uuid { get; set; }
	public List<int> Version { get; set; }
}

public class manifestStructure
{
	public int FormatVersion { get; set; }
	public Header Header { get; set; }
	public List<Module> Modules { get; set; }
}

public class PlayerPackMapData
{
	public string pack { get; set; }
	public ResourcePackType type { get; set; }
}