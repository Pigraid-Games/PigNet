#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/PigNet/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is PigNet.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using fNbt;
using log4net;
using PigNet;
using PigNet.Crafting;
using PigNet.Entities;
using PigNet.Items;
using PigNet.Net;
using PigNet.Net.EnumerationsTable;
using PigNet.Net.Packets.Mcpe;
using PigNet.Utils;
using PigNet.Utils.Metadata;
using PigNet.Utils.Vectors;

namespace PigNet.Client;

public class BedrockTraceHandler(MiNetClient client) : McpeClientMessageHandlerBase(client)
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockTraceHandler));
	
	public override void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message)
	{
		Log.Warn($"Got soft enum update for {message}");
	}

	public override void HandleMcpeDisconnect(McpeDisconnect message)
	{
		Log.Warn("[Disconnect Screen] ");
		switch (message.message)
		{
			case "disconnectionScreen.notAuthenticated":
				Log.Warn("You need to authenticate to Xbox Live services to join this server.");
				break;
			case "disconnectionScreen.invalidSkin":
				Log.Warn("Invalid skin.");
				break;
			case "disconnectionScreen.serverFull":
			case "disconnectionScreen.serverFull.title":
				Log.Warn("Server is full.");
				break;
			case "disconnectionScreen.resourcePack":
				Log.Warn("Resource pack error.");
				break;
			case "disconnectionScreen.badPacket":
				Log.Warn("Client sent invalid packet.");
				break;
			default:
				Log.Warn($"Server requested disconnect with message {message.message}");
				break;
		}
		base.HandleMcpeDisconnect(message);
	}

	public override void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message)
	{
		var sb = new StringBuilder();
		sb.AppendLine();

		sb.AppendLine("Texture packs:");
		foreach (TexturePackInfo info in message.texturepacks) 
			sb.AppendLine($"ID={info.UUID}, Version={info.Version}, Unknown={info.Size}");

		Log.Debug(sb.ToString());

		base.HandleMcpeResourcePacksInfo(message);
	}

	public override void HandleMcpeResourcePackStack(McpeResourcePackStack message)
	{
		var sb = new StringBuilder();
		sb.AppendLine();

		sb.AppendLine("Resource pack stacks:");
		foreach (PackIdVersion info in message.resourcepackidversions) 
			sb.AppendLine($"ID={info.Id}, Version={info.Version}, Subpackname={info.SubPackName}");

		sb.AppendLine("Behavior pack stacks:");
		foreach (PackIdVersion info in message.behaviorpackidversions) 
			sb.AppendLine($"ID={info.Id}, Version={info.Version}, Subpackname={info.SubPackName}");

		Log.Debug(sb.ToString());

		base.HandleMcpeResourcePackStack(message);
	}

	private readonly List<ICommandExecutioner> _executioners = [new PlaceAllBlocksExecutioner()];

	private void CallPacketHandlers(Packet packet)
	{
		IEnumerable<ICommandExecutioner> wantExec = _executioners.Where(e => e is IGenericPacketHandler);
		List<Task> tasks = [];
		tasks.AddRange(from IGenericPacketHandler executioner in wantExec select Task.Run(() 
			=> executioner.HandlePacket(this, packet)));
		Task.WaitAll(tasks.ToArray());
	}

	public override void HandleMcpeText(McpeText message)
	{
		string text = message.message;
		if (string.IsNullOrEmpty(text)) return;

		IEnumerable<ICommandExecutioner> wantExec = _executioners.Where(e => e.CanExecute(text));

		foreach (ICommandExecutioner executioner in wantExec)
		{
			Log.Debug($"Executing command handler: {executioner.GetType().FullName}");
			Task.Run(() => executioner.Execute(this, text));
		}
	}

	public override void HandleMcpeInventorySlot(McpeInventorySlot message)
	{
		Log.Debug($"Inventory slot: {message.item}");
	}

	public override void HandleMcpePlayerHotbar(McpePlayerHotbar message)
	{
		CallPacketHandlers(message);
	}

	public override void HandleMcpeInventoryContent(McpeInventoryContent message)
	{
		Log.Error($"Set container content on Window ID: 0x{message.inventoryId:x2}, Count: {message.slots.Count}, ContainerName: {message.fullContainerName.ContainerId} - {message.fullContainerName.DynamicId}");
		CallPacketHandlers(message);
	}

	public override void HandleMcpeCreativeContent(McpeCreativeContent message)
	{
		Log.Warn($"[McpeCreativeContent] Received {message.input.Count} creative items");
		FileStream file = File.OpenWrite("newResources/creativeInventory.txt");
		var writer = new IndentedTextWriter(new StreamWriter(file), "\t");
		writer.WriteLine($"//Minecraft Bedrock Edition {McpeProtocolInfo.GameVersion} Creative Inventory");
		foreach (CreativeItemEntry item in message.input)
		{
			writer.WriteLine(item.Item.ExtraData == null ? $"new Item({item.Item.Id}, {item.Item.Metadata}){{ RuntimeId = {item.Item.RuntimeId}}}," 
				: $"new Item({item.Item.Id}, {item.Item.Metadata}){{ RuntimeId = {item.Item.RuntimeId}, ExtraData = {item.Item.ExtraData}}},");
		}
		Log.Warn($"[McpeCreativeContent] Done reading {message.input.Count} creative items\n");
		writer.Flush();
		file.Close();
		Log.Warn("Received creative items exported to newResources/creativeInventory.txt\n");

		FileStream file2 = File.OpenWrite("newResources/creativeGroups.txt");
		var writer2 = new IndentedTextWriter(new StreamWriter(file2), "\t");
		writer2.WriteLine("public static Dictionary<string, creativeGroup> CreativeGroups = new Dictionary<string, creativeGroup>()");
		writer2.WriteLine("		{");
		writer2.WriteLine("			//Generated with PigNet.Client (creativeGroups.txt)");

		int constructionIndex = 0;
		int equipmentIndex = 0;
		int itemsIndex = 0;
		int natureIndex = 0;
		foreach (creativeGroup group in message.groups)
		{
			if (group.Icon.Id == 0)
			{
				switch (group.Category)
				{
					case 1:
						writer2.WriteLine($"			{{\"Construction{constructionIndex++}\", new creativeGroup(1, \"\", new ItemAir())}},");
						break;
					case 2:
						writer2.WriteLine($"			{{\"Nature{equipmentIndex++}\", new creativeGroup(2, \"\", new ItemAir())}},");
						break;
					case 3:
						writer2.WriteLine($"			{{\"Equipment{itemsIndex++}\", new creativeGroup(3, \"\", new ItemAir())}},");
						break;
					case 4:
						writer2.WriteLine($"			{{\"Items{natureIndex++}\", new creativeGroup(4, \"\", new ItemAir())}},");
						break;
				}
			}
			else
			{
				string groupName = group.Name.Split('.').Last();
				writer2.WriteLine($"			{{\"{char.ToUpper(groupName[0]) + groupName.Substring(1)}\", new creativeGroup({group.Category}, \"{group.Name}\", new Item({group.Icon.Id}, {group.Icon.Metadata}))}},");
			}
		}
		writer2.WriteLine("		};");
		writer2.Flush();
		file2.Close();
		Log.Warn("Received creative groups exported to newResources/creativeGroups.txt\n");
	}

	public override void HandleMcpeAddItemEntity(McpeAddItemActor message)
	{
		CallPacketHandlers(message);
	}

	public override void HandleMcpeUpdateBlock(McpeUpdateBlock message)
	{
		CallPacketHandlers(message);
	}

	public override void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocks message)
	{
		CallPacketHandlers(message);
	}

	public override void HandleMcpeStartGame(McpeStartGame message)
	{
		Client.EntityId = message.runtimeEntityId;
		Client.NetworkEntityId = message.entityIdSelf;
		Client.SpawnPoint = message.spawn;
		Client.CurrentLocation = new PlayerLocation(Client.SpawnPoint, message.rotation.X, message.rotation.X, message.rotation.Y);

		LogGamerules(message.levelSettings.gamerules);

		Client.LevelInfo.LevelName = "Default";
		Client.LevelInfo.Version = 19133;
		Client.LevelInfo.GameType = message.levelSettings.gamemode;

		{
			var packet = McpeRequestChunkRadius.CreateObject();
			Client.ChunkRadius = 5;
			packet.chunkRadius = Client.ChunkRadius;

			Client.SendPacket(packet);
		}
	}

	public static string CodeName(string name, bool firstUpper = false)
	{
		bool upperCase = firstUpper;

		string result = string.Empty;
		for (int i = 0; i < name.Length; i++)
		{
			if (name[i] == ' ' || name[i] == '_') upperCase = true;
			else
			{
				if ((i == 0 && firstUpper) || upperCase)
				{
					result += name[i].ToString().ToUpperInvariant();
					upperCase = false;
				}
				else result += name[i];
			}
		}

		result = result.Replace(@"[]", "s");
		return result;
	}

	public override void HandleMcpeAddPlayer(McpeAddPlayer message)
	{
		if (Client.IsEmulator) return;

		Log.DebugFormat("McpeAddPlayer Entity ID: {0}", message.entityIdSelf);
		Log.DebugFormat("McpeAddPlayer Runtime Entity ID: {0}", message.runtimeEntityId);
		Log.DebugFormat("X: {0}", message.x);
		Log.DebugFormat("Y: {0}", message.y);
		Log.DebugFormat("Z: {0}", message.z);
		Log.DebugFormat("Yaw: {0}", message.yaw);
		Log.DebugFormat("Pitch: {0}", message.pitch);
		Log.DebugFormat("Velocity X: {0}", message.speedX);
		Log.DebugFormat("Velocity Y: {0}", message.speedY);
		Log.DebugFormat("Velocity Z: {0}", message.speedZ);
		Log.DebugFormat("Metadata: {0}", Client.MetadataToCode(message.metadata));
		Log.DebugFormat("Links count: {0}", message.links?.Count);
	}

	public override void HandleMcpeAddEntity(McpeAddActor message)
	{
		if (Client.IsEmulator) return;

		if (!Client.Entities.ContainsKey(message.entityIdSelf))
		{
			var entity = new Entity(message.entityType, null)
			{
				EntityId = message.runtimeEntityId,
				KnownPosition = new PlayerLocation(message.x, message.y, message.z, message.yaw, message.yaw, message.pitch),
				Velocity = new Vector3(message.speedX, message.speedY, message.speedZ)
			};
			Client.Entities.TryAdd(entity.EntityId, entity);
		}

		Log.DebugFormat("McpeAddEntity Entity ID: {0}", message.entityIdSelf);
		Log.DebugFormat("McpeAddEntity Runtime Entity ID: {0}", message.runtimeEntityId);
		Log.DebugFormat("Entity Type: {0}", message.entityType);
		Log.DebugFormat("X: {0}", message.x);
		Log.DebugFormat("Y: {0}", message.y);
		Log.DebugFormat("Z: {0}", message.z);
		Log.DebugFormat("Yaw: {0}", message.yaw);
		Log.DebugFormat("Pitch: {0}", message.pitch);
		Log.DebugFormat("Velocity X: {0}", message.speedX);
		Log.DebugFormat("Velocity Y: {0}", message.speedY);
		Log.DebugFormat("Velocity Z: {0}", message.speedZ);
		Log.DebugFormat("Metadata: {0}", Client.MetadataToCode(message.metadata));
		Log.DebugFormat("Links count: {0}", message.links?.Count);

		if (message.metadata.Contains(0))
		{
			long? value = ((MetadataLong) message.metadata[0])?.Value;
			if (value != null)
			{
				long dataValue = (long) value;
				Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString(dataValue, 2)}b ");
			}
		}

		if (Log.IsDebugEnabled)
		{
			foreach (KeyValuePair<string, EntityAttribute> attribute in message.attributes) Log.Debug($"Entity attribute {attribute}");
		}

		Log.DebugFormat("Links count: {0}", message.links);

		if (Log.IsDebugEnabled && Client._mobWriter != null)
		{
			Client._mobWriter.WriteLine("Entity Type: {0}", message.entityType);
			Client._mobWriter.Indent++;
			Client._mobWriter.WriteLine("Metadata: {0}", Client.MetadataToCode(message.metadata));
			Client._mobWriter.Indent--;
			Client._mobWriter.WriteLine();
			Client._mobWriter.Flush();
		}

		if (message.entityType != "minecraft:horse") return;
		long id = message.runtimeEntityId;
		var pos = new Vector3(message.x, message.y, message.z);
		Task.Run(BotHelpers.DoWaitForSpawn(Client))
			.ContinueWith(_ => Task.Delay(3000).Wait())
			.ContinueWith(_ =>
			{
				Log.Warn("Sending sneak for player");

				McpePlayerAction action = McpePlayerAction.CreateObject();
				action.runtimeActorId = Client.EntityId;
				action.actionId = PlayerActionType.StartSneaking;
				Client.SendPacket(action);
			})
			.ContinueWith(_ => Task.Delay(2000).Wait())
			.ContinueWith(_ =>
			{
				Log.Warn("Sending transaction for horse");

				McpeInventoryTransaction transaction = McpeInventoryTransaction.CreateObject();
				transaction.transaction = new ItemUseOnEntityTransaction()
				{
					TransactionRecords = [],
					EntityId = id,
					ActionType = 0,
					Slot = 0,
					Item = new ItemAir(),
					FromPosition = Client.CurrentLocation,
					ClickPosition = pos
				};

				Client.SendPacket(transaction);
			});
	}

	public override void HandleMcpeRemoveEntity(McpeRemoveActor message)
	{
		Log.DebugFormat("McpeAddPlayer Entity ID: {0}", message.entityIdSelf);
		Client.Entities.TryRemove(message.entityIdSelf, out _);
	}

	public override void HandleMcpeLevelEvent(McpeLevelEvent message)
	{
		int data = message.data;
		switch (message.eventId)
		{
			case LevelEventType.ParticlesDestroyBlock:
			{
				int blockId = data & 0xff;
				int metadata = data >> 12;
				Log.Debug($"BlockID={blockId}, Metadata={metadata}");
				break;
			}
			case LevelEventType.ParticlesPotionSplash:
			{
				Log.Warn($"Got effect with data: {message.data}");
				int r = (message.data >> 16) & 0xFF;
				int g = (message.data >> 8) & 0xFF;
				int b = message.data & 0xFF;

				Log.Warn($"Actual effect color R: 0x{r:x} G: 0x{g:x} B: 0x{b:x}");
				break;
			}
		}
	}

	public override void HandleMcpeUpdateAttributes(McpeUpdateAttributes message)
	{
		foreach (var playerAttribute in message.attributes)
		{
			Log.Debug($"Attribute {playerAttribute}");
		}
	}

	public override void HandleMcpeCraftingData(McpeCraftingData message)
	{
		if (Client.IsEmulator) return;

		string fileName = "newResources/recipes.txt";
		Log.Info("Writing recipes to filename: " + fileName);
		FileStream file = File.OpenWrite(fileName);

		var writer = new IndentedTextWriter(new StreamWriter(file), "\t");

		writer.WriteLine();
		writer.Indent++;
		writer.Indent++;

		writer.WriteLine("static RecipeManager()");
		writer.WriteLine("{");
		writer.Indent++;
		writer.WriteLine("Recipes = new Recipes");
		writer.WriteLine("{");
		writer.Indent++;

		foreach (Recipe recipe in message.craftingEntries)
		{
			switch (recipe)
			{
				case ShapelessRecipe shapelessRecipe:
				{
					writer.WriteLine("new ShapelessRecipe(");
					writer.Indent++;

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item itemStack in shapelessRecipe.Result) 
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item itemStack in shapelessRecipe.Input) 
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}){{ UniqueId = {itemStack.UniqueId}, RuntimeId={itemStack.RuntimeId} }},");
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapelessRecipe.Block}\"){{ UniqueId = {shapelessRecipe.UniqueId} }},");

					writer.Indent--;
					continue;
				}

				case ShapedRecipe shapedRecipe:
				{
					writer.WriteLine($"new ShapedRecipe({shapedRecipe.Width}, {shapedRecipe.Height},");
					writer.Indent++;

					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Result) 
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					writer.Indent--;
					writer.WriteLine($"}},");

					writer.WriteLine("new Item[]");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Input) 
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}, {item.Count}){{ UniqueId = {item.UniqueId}, RuntimeId={item.RuntimeId} }},");
					writer.Indent--;
					writer.WriteLine($"}}, \"{shapedRecipe.Block}\"){{ UniqueId = {shapedRecipe.UniqueId} }},");

					writer.Indent--;

					continue;
				}
				case SmeltingRecipe smeltingRecipe:
					writer.WriteLine($"new SmeltingRecipe(new Item({smeltingRecipe.Result.Id}, {smeltingRecipe.Result.Metadata}, {smeltingRecipe.Result.Count}){{ UniqueId = {smeltingRecipe.Result.UniqueId}, RuntimeId={smeltingRecipe.Result.RuntimeId} }}, new Item({smeltingRecipe.Input.Id}, {smeltingRecipe.Input.Metadata}){{ UniqueId = {smeltingRecipe.Input.UniqueId}, RuntimeId={smeltingRecipe.Input.RuntimeId} }}, \"{smeltingRecipe.Block}\"),");
					continue;
				case MultiRecipe multiRecipe:
					writer.WriteLine($"new MultiRecipe() {{ Id = new UUID(\"{recipe.Id}\"), UniqueId = {multiRecipe.UniqueId} }}, // {recipe.Id}");
					continue;
			}
		}

		writer.Indent--;
		writer.WriteLine("};");
		writer.Indent--;
		writer.WriteLine("}");

		writer.Flush();
		file.Close();
		Log.Warn("Received recipes exported to newResources/recipes.txt\n");
	}

	public override void HandleMcpeBlockEntityData(McpeBlockActorData message)
	{
		Log.DebugFormat("X: {0}", message.blockPositin.X);
		Log.DebugFormat("Y: {0}", message.blockPositin.Y);
		Log.DebugFormat("Z: {0}", message.blockPositin.Z);
		Log.DebugFormat("NBT:\n{0}", message.actorDataTags.NbtFile.RootTag);
	}

	public override void HandleMcpeLevelChunk(McpeLevelChunk message)
	{
		// TODO doesn't work anymore I guess
		if (Client.IsEmulator) return;

		if (message.blobHashes != null)
		{
			McpeClientCacheBlobStatus status = McpeClientCacheBlobStatus.CreateObject();
			Client.SendPacket(status);
		}
		else
		{
			Client.Chunks.GetOrAdd(new ChunkCoordinates(message.chunkX, message.chunkZ), _ =>
			{
				Log.Debug($"Chunk X={message.chunkX}, Z={message.chunkZ}, size={message.chunkData.Length}, Count={Client.Chunks.Count}");
				if (BlockstateGenerator.running == false) Console.WriteLine($"[McpeLevelChunk] Got chunk | X: {message.chunkX,-4} | Z: {message.chunkZ,-4} |");

				return null;
			});
		}
	}

	public override void HandleMcpeGameRulesChanged(McpeGameRulesChanged message)
	{
		GameRules rules = message.rules;
		LogGamerules(rules);
	}

	private static void LogGamerules(GameRules rules)
	{
		foreach (GameRule rule in rules)
		{
			switch (rule)
			{
				case GameRule<bool> gameRule:
					Log.Debug($"Rule: {gameRule.Name}={gameRule}");
					break;
				case GameRule<int> gameRule:
					Log.Debug($"Rule: {gameRule.Name}={gameRule}");
					break;
				case GameRule<float> gameRule:
					Log.Debug($"Rule: {gameRule.Name}={gameRule}");
					break;
				default:
					Log.Warn($"Rule: {rule.Name}={rule}");
					break;
			}
		}
	}

	public override void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message)
	{
		string fileName = Path.GetTempPath() + "ResourcePackChunkData_" + message.packageId + ".zip";
		Log.Warn("Writing ResourcePackChunkData part " + message.chunkIndex + " to filename: " + fileName);

		FileStream file = File.OpenWrite(fileName);
		file.Seek((long) message.progress, SeekOrigin.Begin);

		file.Write(message.payload, 0, message.payload.Length);
		file.Close();

		Log.Debug($"packageId={message.packageId}");
		Log.Debug($"unknown1={message.chunkIndex}");
		Log.Debug($"unknown3={message.progress}");
		Log.Debug($"Actual Lenght={message.payload.Length}");

		base.HandleMcpeResourcePackChunkData(message);
	}

	public override void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message)
	{
		foreach (NbtTag entity in (NbtList)message.namedtag.NbtFile.RootTag["idlist"])
		{
			string id = ((NbtString)entity["id"]).Value;
			int rid = ((NbtInt)entity["rid"]).Value;
			if (!Enum.IsDefined(typeof(EntityType), rid)) Log.Debug($"{{ (EntityType) {rid}, \"{id}\" }},");
		}
	}

	public override void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message)
	{
		var list = new NbtCompound("");
		foreach (NbtTag biome in (NbtCompound)message.namedtag.NbtFile.RootTag)
		{
			string biomeName = biome.Name;
			float downfall = ((NbtFloat)biome["downfall"]).Value;
			float temperature = ((NbtFloat)biome["temperature"]).Value;
			list.Add(
				new NbtCompound(biomeName)
				{
					new NbtFloat("temperature", temperature),
					new NbtFloat("downfall", downfall)
				}
			);
		}

		File.WriteAllText("newResources/biomes.txt", list.ToString());
		Log.Warn("Received biome definitions exported to newResources/biomes.txt\n");
	}

	public override void HandleMcpePlayStatus(McpePlayStatus message)
	{

		base.HandleMcpePlayStatus(message);

		if (Client.PlayerStatus != 0) return;
		McpeClientCacheStatus packet = McpeClientCacheStatus.CreateObject();
		packet.enabled = Client.UseBlobCache;
		Client.SendPacket(packet);
	}
	
	public override void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message)
	{
	}
	
	public override void HandleMcpeAvailableCommands(McpeAvailableCommands message)
	{
	}
}