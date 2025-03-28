#region LICENSE

// The contents of this file are subject to the Common Public Attribution// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

#nullable enable
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using log4net;
using MiNET.Net.EnumerationsTable;
using MiNET.Net.Packets.Mcpe;
using MiNET.Net.Packets.RakNet;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;

namespace MiNET.Net;

public class McpeProtocolInfo
{
	public const int ProtocolVersion = 786;
	public const string GameVersion = "1.21.70";
}

public interface IMcpeMessageHandler
{
	void Disconnect(string reason, bool sendDisconnect = true);

	void HandleMcpeLogin(McpeLogin message);
	void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message);
	void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message);
	void HandleMcpeText(McpeText message);
	void HandleMcpeMoveEntity(McpeMoveActor message);
	void HandleMcpeMovePlayer(McpeMovePlayer message);
	void HandleMcpeRiderJump(McpeRiderJump message);
	void HandleMcpeEntityEvent(McpeActorEvent message);
	void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
	void HandleMcpeMobEquipment(McpeMobEquipment message);
	void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
	void HandleMcpeInteract(McpeInteract message);
	void HandleMcpeBlockPickRequest(McpeBlockPickRequest message);
	void HandleMcpeTakeItemActor(McpeActorPickRequest message);
	void HandleMcpePlayerAction(McpePlayerAction message);
	void HandleMcpeSetActorData(McpeSetActorData message);
	void HandleMcpeSetActorMotion(McpeSetActorMotion message);
	void HandleMcpeAnimate(McpeAnimate message);
	void HandleMcpeRespawn(McpeRespawn message);
	void HandleMcpeContainerClose(McpeContainerClose message);
	void HandleMcpePlayerHotbar(McpePlayerHotbar message);
	void HandleMcpeInventoryContent(McpeInventoryContent message);
	void HandleMcpeInventorySlot(McpeInventorySlot message);
	void HandleMcpeBlockEntityData(McpeBlockActorData message);
	void HandleMcpePlayerInput(McpePlayerInput message);
	void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
	void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
	void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
	void HandleMcpeCommandRequest(McpeCommandRequest message);
	void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message);
	void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message);
	void HandleMcpePurchaseReceipt(McpePurchaseReceipt message);
	void HandleMcpePlayerSkin(McpePlayerSkin message);
	void HandleMcpeNpcRequest(McpeNpcRequest message);
	void HandleMcpePhotoTransfer(McpePhotoTransfer message);
	void HandleMcpeModalFormResponse(McpeModalFormResponse message);
	void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message);
	void HandleMcpeLabTable(McpeLabTable message);
	void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message);
	void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
	void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
	void HandleMcpeNetworkSettings(McpeNetworkSettings message);
	void HandleMcpePlayerAuthInput(McpePlayerAuthInput message);
	void HandleMcpeItemStackRequest(McpeItemStackRequest message);
	void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message);
	void HandleMcpePacketViolationWarning(McpeViolationWarning message);
	void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocks message);
	void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message);
	void HandleMcpeRequestAbility(McpeRequestAbility message);
	void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message);
	void HandleMcpeEmote(McpeEmotePacket message);
	void HandleMcpeEmoteList(McpeEmoteList message);
	void HandleMcpePermissionRequest(McpeRequestPermission message);
	void HandleMcpeSetInventoryOptions(McpeSetInventoryOptions message);
	void HandleMcpeAnvilDamage(McpeAnvilDamage message);
	void HandleMcpeServerboundLoadingScreen(McpeServerboundLoadingScreen message);
}

public interface IMcpeClientMessageHandler
{
	void HandleMcpePlayStatus(McpePlayStatus message);
	void HandleMcpeServerToClientHandshake(McpeServerToClientHandshake message);
	void HandleMcpeDisconnect(McpeDisconnect message);
	void HandleMcpeResourcePacksInfo(McpeResourcePacksInfo message);
	void HandleMcpeResourcePackStack(McpeResourcePackStack message);
	void HandleMcpeText(McpeText message);
	void HandleMcpeSetTime(McpeSetTime message);
	void HandleMcpeStartGame(McpeStartGame message);
	void HandleMcpeAddPlayer(McpeAddPlayer message);
	void HandleMcpeAddEntity(McpeAddActor message);
	void HandleMcpeRemoveEntity(McpeRemoveActor message);
	void HandleMcpeAddItemEntity(McpeAddItemActor message);
	void HandleMcpeTakeItemActor(McpeTakeItemActor message);
	void HandleMcpeMoveEntity(McpeMoveActor message);
	void HandleMcpeMovePlayer(McpeMovePlayer message);
	void HandleMcpeRiderJump(McpeRiderJump message);
	void HandleMcpeUpdateBlock(McpeUpdateBlock message);
	void HandleMcpeAddPainting(McpeAddPainting message);
	void HandleMcpeLevelEvent(McpeLevelEvent message);
	void HandleMcpeBlockEvent(McpeBlockEvent message);
	void HandleMcpeEntityEvent(McpeActorEvent message);
	void HandleMcpeMobEffect(McpeMobEffect message);
	void HandleMcpeUpdateAttributes(McpeUpdateAttributes message);
	void HandleMcpeInventoryTransaction(McpeInventoryTransaction message);
	void HandleMcpeMobEquipment(McpeMobEquipment message);
	void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message);
	void HandleMcpeInteract(McpeInteract message);
	void HandleMcpeHurtArmor(McpeHurtArmor message);
	void HandleMcpeSetActorData(McpeSetActorData message);
	void HandleMcpeSetActorMotion(McpeSetActorMotion message);
	void HandleMcpeSetActorLink(McpeSetActorLink message);
	void HandleMcpeSetHealth(McpeSetHealth message);
	void HandleMcpeSetSpawnPosition(McpeSetSpawnPosition message);
	void HandleMcpeAnimate(McpeAnimate message);
	void HandleMcpeRespawn(McpeRespawn message);
	void HandleMcpeContainerOpen(McpeContainerOpen message);
	void HandleMcpeContainerClose(McpeContainerClose message);
	void HandleMcpePlayerHotbar(McpePlayerHotbar message);
	void HandleMcpeInventoryContent(McpeInventoryContent message);
	void HandleMcpeInventorySlot(McpeInventorySlot message);
	void HandleMcpeContainerSetData(McpeContainerSetData message);
	void HandleMcpeCraftingData(McpeCraftingData message);
	void HandleMcpeGuiDataPickItem(McpeGuiDataPickItem message);
	void HandleMcpeBlockEntityData(McpeBlockActorData message);
	void HandleMcpeLevelChunk(McpeLevelChunk message);
	void HandleMcpeSetCommandsEnabled(McpeSetCommandsEnabled message);
	void HandleMcpeSetDifficulty(McpeSetDifficulty message);
	void HandleMcpeChangeDimension(McpeChangeDimension message);
	void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message);
	void HandleMcpePlayerList(McpePlayerList message);
	void HandleMcpeSimpleEvent(McpeSimpleEvent message);
	void HandleMcpeTelemetryEvent(McpeLegacyTelemetryEvent message);
	void HandleMcpeClientboundMapItemData(McpeClientboundMapItemData message);
	void HandleMcpeMapInfoRequest(McpeMapInfoRequest message);
	void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message);
	void HandleMcpeChunkRadiusUpdate(McpeChunkRadiusUpdate message);
	void HandleMcpeGameRulesChanged(McpeGameRulesChanged message);
	void HandleMcpeCamera(McpeCamera message);
	void HandleMcpeBossEvent(McpeBossEvent message);
	void HandleMcpeShowCredits(McpeShowCredits message);
	void HandleMcpeAvailableCommands(McpeAvailableCommands message);
	void HandleMcpeCommandOutput(McpeCommandOutput message);
	void HandleMcpeUpdateTrade(McpeUpdateTrade message);
	void HandleMcpeUpdateEquipment(McpeUpdateEquipment message);
	void HandleMcpeResourcePackDataInfo(McpeResourcePackDataInfo message);
	void HandleMcpeResourcePackChunkData(McpeResourcePackChunkData message);
	void HandleMcpeTransfer(McpeTransfer message);
	void HandleMcpePlaySound(McpePlaySound message);
	void HandleMcpeStopSound(McpeStopSound message);
	void HandleMcpeSetTitle(McpeSetTitle message);
	void HandleMcpeAddBehaviorTree(McpeAddBehaviorTree message);
	void HandleMcpeStructureBlockUpdate(McpeStructureBlockUpdate message);
	void HandleMcpeShowStoreOffer(McpeShowStoreOffer message);
	void HandleMcpePlayerSkin(McpePlayerSkin message);
	void HandleMcpeSubClientLogin(McpeSubClientLogin message);
	void HandleMcpeInitiateWebSocketConnection(McpeInitiateWebSocketConnection message);
	void HandleMcpeSetLastHurtBy(McpeSetLastHurtBy message);
	void HandleMcpeBookEdit(McpeBookEdit message);
	void HandleMcpeNpcRequest(McpeNpcRequest message);
	void HandleMcpeModalFormRequest(McpeModalFormRequest message);
	void HandleMcpeServerSettingsResponse(McpeServerSettingsResponse message);
	void HandleMcpeShowProfile(McpeShowProfile message);
	void HandleMcpeSetDefaultGameType(McpeSetDefaultGameType message);
	void HandleMcpeRemoveObjective(McpeRemoveObjective message);
	void HandleMcpeSetDisplayObjective(McpeSetDisplayObjective message);
	void HandleMcpeSetScore(McpeSetScore message);
	void HandleMcpeLabTable(McpeLabTable message);
	void HandleMcpeUpdateBlockSynced(McpeUpdateBlockSynced message);
	void HandleMcpeMoveEntityDelta(McpeMoveActorDelta message);
	void HandleMcpeSetScoreboardIdentity(McpeSetScoreboardIdentity message);
	void HandleMcpeUpdateSoftEnum(McpeUpdateSoftEnum message);
	void HandleMcpeSpawnParticleEffect(McpeSpawnParticleEffect message);
	void HandleMcpeAvailableEntityIdentifiers(McpeAvailableEntityIdentifiers message);
	void HandleMcpeNetworkChunkPublisherUpdate(McpeNetworkChunkPublisherUpdate message);
	void HandleMcpeBiomeDefinitionList(McpeBiomeDefinitionList message);
	void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message);
	void HandleMcpeLevelEventGeneric(McpeLevelEventGeneric message);
	void HandleMcpeLecternUpdate(McpeLecternUpdate message);
	void HandleMcpeClientCacheStatus(McpeClientCacheStatus message);
	void HandleMcpeOnScreenTextureAnimation(McpeOnScreenTextureAnimation message);
	void HandleMcpeMapCreateLockedCopy(McpeMapCreateLockedCopy message);
	void HandleMcpeStructureTemplateDataExportRequest(McpeStructureTemplateDataRequest message);
	void HandleMcpeStructureTemplateDataExportResponse(McpeStructureTemplateDataResponse message);
	void HandleMcpeUpdateBlockProperties(McpeUpdateBlockProperties message);
	void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message);
	void HandleMcpeClientCacheMissResponse(McpeClientCacheMissResponse message);
	void HandleMcpeNetworkSettings(McpeNetworkSettings message);
	void HandleMcpeCreativeContent(McpeCreativeContent message);
	void HandleMcpePlayerEnchantOptions(McpePlayerEnchantOptions message);
	void HandleMcpeItemStackResponse(McpeItemStackResponse message);
	void HandleMcpeItemComponent(McpeItemComponent message);
	void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocks message);
	void HandleMcpeSubChunkPacket(McpeSubChunk message);
	void HandleMcpeDimensionData(McpeDimensionData message);
	void HandleMcpeUpdateAbilities(McpeUpdateAbilities message);
	void HandleMcpeUpdateAdventureSettings(McpeUpdateAdventureSettings message);
	void HandleMcpeTrimData(McpeTrimData message);
	void HandleMcpeOpenSign(McpeOpenSign message);
	void HandleFtlCreatePlayer(FtlCreatePlayer message);
	void HandleMcpeEmote(McpeEmotePacket message);
	void HandleMcpeEmoteList(McpeEmoteList message);
	void HandleMcpePermissionRequest(McpeRequestPermission message);
	void HandleMcpePlayerFog(McpePlayerFog message);
	void HandleMcpeAnimateEntity(McpeAnimateEntity message);
	void HandleMcpeCloseForm(McpeClientboundCloseForm message);
}

public class McpeClientMessageDispatcher
{
	private readonly IMcpeClientMessageHandler _messageHandler;

	public McpeClientMessageDispatcher(IMcpeClientMessageHandler messageHandler)
	{
		_messageHandler = messageHandler;
	}

	public bool HandlePacket(Packet message)
	{
		switch (message)
		{
			case McpePlayStatus msg:
				_messageHandler.HandleMcpePlayStatus(msg);
				break;
			case McpeServerToClientHandshake msg:
				_messageHandler.HandleMcpeServerToClientHandshake(msg);
				break;
			case McpeDisconnect msg:
				_messageHandler.HandleMcpeDisconnect(msg);
				break;
			case McpeResourcePacksInfo msg:
				_messageHandler.HandleMcpeResourcePacksInfo(msg);
				break;
			case McpeResourcePackStack msg:
				_messageHandler.HandleMcpeResourcePackStack(msg);
				break;
			case McpeText msg:
				_messageHandler.HandleMcpeText(msg);
				break;
			case McpeSetTime msg:
				_messageHandler.HandleMcpeSetTime(msg);
				break;
			case McpeStartGame msg:
				_messageHandler.HandleMcpeStartGame(msg);
				break;
			case McpeAddPlayer msg:
				_messageHandler.HandleMcpeAddPlayer(msg);
				break;
			case McpeAddActor msg:
				_messageHandler.HandleMcpeAddEntity(msg);
				break;
			case McpeRemoveActor msg:
				_messageHandler.HandleMcpeRemoveEntity(msg);
				break;
			case McpeAddItemActor msg:
				_messageHandler.HandleMcpeAddItemEntity(msg);
				break;
			case McpeTakeItemActor msg:
				_messageHandler.HandleMcpeTakeItemActor(msg);
				break;
			case McpeMoveActor msg:
				_messageHandler.HandleMcpeMoveEntity(msg);
				break;
			case McpeMovePlayer msg:
				_messageHandler.HandleMcpeMovePlayer(msg);
				break;
			case McpeRiderJump msg:
				_messageHandler.HandleMcpeRiderJump(msg);
				break;
			case McpeUpdateBlock msg:
				_messageHandler.HandleMcpeUpdateBlock(msg);
				break;
			case McpeAddPainting msg:
				_messageHandler.HandleMcpeAddPainting(msg);
				break;
			case McpeLevelEvent msg:
				_messageHandler.HandleMcpeLevelEvent(msg);
				break;
			case McpeBlockEvent msg:
				_messageHandler.HandleMcpeBlockEvent(msg);
				break;
			case McpeActorEvent msg:
				_messageHandler.HandleMcpeEntityEvent(msg);
				break;
			case McpeMobEffect msg:
				_messageHandler.HandleMcpeMobEffect(msg);
				break;
			case McpeUpdateAttributes msg:
				_messageHandler.HandleMcpeUpdateAttributes(msg);
				break;
			case McpeInventoryTransaction msg:
				_messageHandler.HandleMcpeInventoryTransaction(msg);
				break;
			case McpeMobEquipment msg:
				_messageHandler.HandleMcpeMobEquipment(msg);
				break;
			case McpeMobArmorEquipment msg:
				_messageHandler.HandleMcpeMobArmorEquipment(msg);
				break;
			case McpeInteract msg:
				_messageHandler.HandleMcpeInteract(msg);
				break;
			case McpeHurtArmor msg:
				_messageHandler.HandleMcpeHurtArmor(msg);
				break;
			case McpeSetActorData msg:
				_messageHandler.HandleMcpeSetActorData(msg);
				break;
			case McpeSetActorMotion msg:
				_messageHandler.HandleMcpeSetActorMotion(msg);
				break;
			case McpeSetActorLink msg:
				_messageHandler.HandleMcpeSetActorLink(msg);
				break;
			case McpeSetHealth msg:
				_messageHandler.HandleMcpeSetHealth(msg);
				break;
			case McpeSetSpawnPosition msg:
				_messageHandler.HandleMcpeSetSpawnPosition(msg);
				break;
			case McpeAnimate msg:
				_messageHandler.HandleMcpeAnimate(msg);
				break;
			case McpeRespawn msg:
				_messageHandler.HandleMcpeRespawn(msg);
				break;
			case McpeContainerOpen msg:
				_messageHandler.HandleMcpeContainerOpen(msg);
				break;
			case McpeContainerClose msg:
				_messageHandler.HandleMcpeContainerClose(msg);
				break;
			case McpePlayerHotbar msg:
				_messageHandler.HandleMcpePlayerHotbar(msg);
				break;
			case McpeInventoryContent msg:
				_messageHandler.HandleMcpeInventoryContent(msg);
				break;
			case McpeInventorySlot msg:
				_messageHandler.HandleMcpeInventorySlot(msg);
				break;
			case McpeContainerSetData msg:
				_messageHandler.HandleMcpeContainerSetData(msg);
				break;
			case McpeCraftingData msg:
				_messageHandler.HandleMcpeCraftingData(msg);
				break;
			case McpeGuiDataPickItem msg:
				_messageHandler.HandleMcpeGuiDataPickItem(msg);
				break;
			case McpeBlockActorData msg:
				_messageHandler.HandleMcpeBlockEntityData(msg);
				break;
			case McpeLevelChunk msg:
				_messageHandler.HandleMcpeLevelChunk(msg);
				break;
			case McpeSetCommandsEnabled msg:
				_messageHandler.HandleMcpeSetCommandsEnabled(msg);
				break;
			case McpeSetDifficulty msg:
				_messageHandler.HandleMcpeSetDifficulty(msg);
				break;
			case McpeChangeDimension msg:
				_messageHandler.HandleMcpeChangeDimension(msg);
				break;
			case McpeSetPlayerGameType msg:
				_messageHandler.HandleMcpeSetPlayerGameType(msg);
				break;
			case McpePlayerList msg:
				_messageHandler.HandleMcpePlayerList(msg);
				break;
			case McpeSimpleEvent msg:
				_messageHandler.HandleMcpeSimpleEvent(msg);
				break;
			case McpeLegacyTelemetryEvent msg:
				_messageHandler.HandleMcpeTelemetryEvent(msg);
				break;
			case McpeClientboundMapItemData msg:
				_messageHandler.HandleMcpeClientboundMapItemData(msg);
				break;
			case McpeMapInfoRequest msg:
				_messageHandler.HandleMcpeMapInfoRequest(msg);
				break;
			case McpeRequestChunkRadius msg:
				_messageHandler.HandleMcpeRequestChunkRadius(msg);
				break;
			case McpeChunkRadiusUpdate msg:
				_messageHandler.HandleMcpeChunkRadiusUpdate(msg);
				break;
			case McpeGameRulesChanged msg:
				_messageHandler.HandleMcpeGameRulesChanged(msg);
				break;
			case McpeCamera msg:
				_messageHandler.HandleMcpeCamera(msg);
				break;
			case McpeBossEvent msg:
				_messageHandler.HandleMcpeBossEvent(msg);
				break;
			case McpeShowCredits msg:
				_messageHandler.HandleMcpeShowCredits(msg);
				break;
			case McpeAvailableCommands msg:
				_messageHandler.HandleMcpeAvailableCommands(msg);
				break;
			case McpeCommandOutput msg:
				_messageHandler.HandleMcpeCommandOutput(msg);
				break;
			case McpeUpdateTrade msg:
				_messageHandler.HandleMcpeUpdateTrade(msg);
				break;
			case McpeUpdateEquipment msg:
				_messageHandler.HandleMcpeUpdateEquipment(msg);
				break;
			case McpeResourcePackDataInfo msg:
				_messageHandler.HandleMcpeResourcePackDataInfo(msg);
				break;
			case McpeResourcePackChunkData msg:
				_messageHandler.HandleMcpeResourcePackChunkData(msg);
				break;
			case McpeTransfer msg:
				_messageHandler.HandleMcpeTransfer(msg);
				break;
			case McpePlaySound msg:
				_messageHandler.HandleMcpePlaySound(msg);
				break;
			case McpeStopSound msg:
				_messageHandler.HandleMcpeStopSound(msg);
				break;
			case McpeSetTitle msg:
				_messageHandler.HandleMcpeSetTitle(msg);
				break;
			case McpeAddBehaviorTree msg:
				_messageHandler.HandleMcpeAddBehaviorTree(msg);
				break;
			case McpeStructureBlockUpdate msg:
				_messageHandler.HandleMcpeStructureBlockUpdate(msg);
				break;
			case McpeShowStoreOffer msg:
				_messageHandler.HandleMcpeShowStoreOffer(msg);
				break;
			case McpePlayerSkin msg:
				_messageHandler.HandleMcpePlayerSkin(msg);
				break;
			case McpeSubClientLogin msg:
				_messageHandler.HandleMcpeSubClientLogin(msg);
				break;
			case McpeInitiateWebSocketConnection msg:
				_messageHandler.HandleMcpeInitiateWebSocketConnection(msg);
				break;
			case McpeSetLastHurtBy msg:
				_messageHandler.HandleMcpeSetLastHurtBy(msg);
				break;
			case McpeBookEdit msg:
				_messageHandler.HandleMcpeBookEdit(msg);
				break;
			case McpeNpcRequest msg:
				_messageHandler.HandleMcpeNpcRequest(msg);
				break;
			case McpeModalFormRequest msg:
				_messageHandler.HandleMcpeModalFormRequest(msg);
				break;
			case McpeServerSettingsResponse msg:
				_messageHandler.HandleMcpeServerSettingsResponse(msg);
				break;
			case McpeShowProfile msg:
				_messageHandler.HandleMcpeShowProfile(msg);
				break;
			case McpeSetDefaultGameType msg:
				_messageHandler.HandleMcpeSetDefaultGameType(msg);
				break;
			case McpeRemoveObjective msg:
				_messageHandler.HandleMcpeRemoveObjective(msg);
				break;
			case McpeSetDisplayObjective msg:
				_messageHandler.HandleMcpeSetDisplayObjective(msg);
				break;
			case McpeSetScore msg:
				_messageHandler.HandleMcpeSetScore(msg);
				break;
			case McpeLabTable msg:
				_messageHandler.HandleMcpeLabTable(msg);
				break;
			case McpeUpdateBlockSynced msg:
				_messageHandler.HandleMcpeUpdateBlockSynced(msg);
				break;
			case McpeMoveActorDelta msg:
				_messageHandler.HandleMcpeMoveEntityDelta(msg);
				break;
			case McpeSetScoreboardIdentity msg:
				_messageHandler.HandleMcpeSetScoreboardIdentity(msg);
				break;
			case McpeUpdateSoftEnum msg:
				_messageHandler.HandleMcpeUpdateSoftEnum(msg);
				break;
			case McpeSpawnParticleEffect msg:
				_messageHandler.HandleMcpeSpawnParticleEffect(msg);
				break;
			case McpeAvailableEntityIdentifiers msg:
				_messageHandler.HandleMcpeAvailableEntityIdentifiers(msg);
				break;
			case McpeNetworkChunkPublisherUpdate msg:
				_messageHandler.HandleMcpeNetworkChunkPublisherUpdate(msg);
				break;
			case McpeBiomeDefinitionList msg:
				_messageHandler.HandleMcpeBiomeDefinitionList(msg);
				break;
			case McpeLevelSoundEvent msg:
				_messageHandler.HandleMcpeLevelSoundEvent(msg);
				break;
			case McpeLevelEventGeneric msg:
				_messageHandler.HandleMcpeLevelEventGeneric(msg);
				break;
			case McpeLecternUpdate msg:
				_messageHandler.HandleMcpeLecternUpdate(msg);
				break;
			case McpeClientCacheStatus msg:
				_messageHandler.HandleMcpeClientCacheStatus(msg);
				break;
			case McpeOnScreenTextureAnimation msg:
				_messageHandler.HandleMcpeOnScreenTextureAnimation(msg);
				break;
			case McpeMapCreateLockedCopy msg:
				_messageHandler.HandleMcpeMapCreateLockedCopy(msg);
				break;
			case McpeStructureTemplateDataRequest msg:
				_messageHandler.HandleMcpeStructureTemplateDataExportRequest(msg);
				break;
			case McpeStructureTemplateDataResponse msg:
				_messageHandler.HandleMcpeStructureTemplateDataExportResponse(msg);
				break;
			case McpeUpdateBlockProperties msg:
				_messageHandler.HandleMcpeUpdateBlockProperties(msg);
				break;
			case McpeClientCacheBlobStatus msg:
				_messageHandler.HandleMcpeClientCacheBlobStatus(msg);
				break;
			case McpeClientCacheMissResponse msg:
				_messageHandler.HandleMcpeClientCacheMissResponse(msg);
				break;
			case McpeNetworkSettings msg:
				_messageHandler.HandleMcpeNetworkSettings(msg);
				break;
			case McpeCreativeContent msg:
				_messageHandler.HandleMcpeCreativeContent(msg);
				break;
			case McpePlayerEnchantOptions msg:
				_messageHandler.HandleMcpePlayerEnchantOptions(msg);
				break;
			case McpeItemStackResponse msg:
				_messageHandler.HandleMcpeItemStackResponse(msg);
				break;
			case McpeItemComponent msg:
				_messageHandler.HandleMcpeItemComponent(msg);
				break;
			case McpeUpdateSubChunkBlocks msg:
				_messageHandler.HandleMcpeUpdateSubChunkBlocksPacket(msg);
				break;
			case McpeSubChunk msg:
				_messageHandler.HandleMcpeSubChunkPacket(msg);
				break;
			case McpeDimensionData msg:
				_messageHandler.HandleMcpeDimensionData(msg);
				break;
			case McpeUpdateAbilities msg:
				_messageHandler.HandleMcpeUpdateAbilities(msg);
				break;
			case McpeUpdateAdventureSettings msg:
				_messageHandler.HandleMcpeUpdateAdventureSettings(msg);
				break;
			case McpeTrimData msg:
				_messageHandler.HandleMcpeTrimData(msg);
				break;
			case McpeOpenSign msg:
				_messageHandler.HandleMcpeOpenSign(msg);
				break;
			case FtlCreatePlayer msg:
				_messageHandler.HandleFtlCreatePlayer(msg);
				break;
			case McpeEmotePacket msg:
				_messageHandler.HandleMcpeEmote(msg);
				break;
			case McpeEmoteList msg:
				_messageHandler.HandleMcpeEmoteList(msg);
				break;
			case McpePlayerFog msg:
				_messageHandler.HandleMcpePlayerFog(msg);
				break;
			case McpeAnimateEntity msg:
				_messageHandler.HandleMcpeAnimateEntity(msg);
				break;
			case McpeClientboundCloseForm msg:
				_messageHandler.HandleMcpeCloseForm(msg);
				break;
			default:
				return false;
		}

		return true;
	}
}

public class PacketFactory
{
	public static ICustomPacketFactory CustomPacketFactory { get; set; } = null;

	public static Packet Create(short messageId, ReadOnlyMemory<byte> buffer, string ns)
	{
		Packet packet = CustomPacketFactory?.Create(messageId, buffer, ns);
		if (packet != null) return packet;

		if (ns == "raknet")
			switch (messageId)
			{
				case 0x00:
					return ConnectedPing.CreateObject().Decode(buffer);
				case 0x01:
					return UnconnectedPing.CreateObject().Decode(buffer);
				case 0x03:
					return ConnectedPong.CreateObject().Decode(buffer);
				case 0x04:
					return DetectLostConnections.CreateObject().Decode(buffer);
				case 0x1c:
					return UnconnectedPong.CreateObject().Decode(buffer);
				case 0x05:
					return OpenConnectionRequest1.CreateObject().Decode(buffer);
				case 0x06:
					return OpenConnectionReply1.CreateObject().Decode(buffer);
				case 0x07:
					return OpenConnectionRequest2.CreateObject().Decode(buffer);
				case 0x08:
					return OpenConnectionReply2.CreateObject().Decode(buffer);
				case 0x09:
					return ConnectionRequest.CreateObject().Decode(buffer);
				case 0x10:
					return ConnectionRequestAccepted.CreateObject().Decode(buffer);
				case 0x13:
					return NewIncomingConnection.CreateObject().Decode(buffer);
				case 0x14:
					return NoFreeIncomingConnections.CreateObject().Decode(buffer);
				case 0x15:
					return DisconnectionNotification.CreateObject().Decode(buffer);
				case 0x17:
					return ConnectionBanned.CreateObject().Decode(buffer);
				case 0x1A:
					return IpRecentlyConnected.CreateObject().Decode(buffer);
				case 0xfe:
					return McpeWrapper.CreateObject().Decode(buffer);
			}
		else if (ns == "ftl")
			switch (messageId)
			{
				case 0x01:
					return FtlCreatePlayer.CreateObject().Decode(buffer);
			}
		else
			switch (messageId)
			{
				case 0x01:
					return McpeLogin.CreateObject().Decode(buffer);
				case 0x02:
					return McpePlayStatus.CreateObject().Decode(buffer);
				case 0x03:
					return McpeServerToClientHandshake.CreateObject().Decode(buffer);
				case 0x04:
					return McpeClientToServerHandshake.CreateObject().Decode(buffer);
				case 0x05:
					return McpeDisconnect.CreateObject().Decode(buffer);
				case 0x06:
					return McpeResourcePacksInfo.CreateObject().Decode(buffer);
				case 0x07:
					return McpeResourcePackStack.CreateObject().Decode(buffer);
				case 0x08:
					return McpeResourcePackClientResponse.CreateObject().Decode(buffer);
				case 0x09:
					return McpeText.CreateObject().Decode(buffer);
				case 0x0a:
					return McpeSetTime.CreateObject().Decode(buffer);
				case 0x0b:
					return McpeStartGame.CreateObject().Decode(buffer);
				case 0x0c:
					return McpeAddPlayer.CreateObject().Decode(buffer);
				case 0x0d:
					return McpeAddActor.CreateObject().Decode(buffer);
				case 0x0e:
					return McpeRemoveActor.CreateObject().Decode(buffer);
				case 0x0f:
					return McpeAddItemActor.CreateObject().Decode(buffer);
				case 0x11:
					return McpeTakeItemActor.CreateObject().Decode(buffer);
				case 0x12:
					return McpeMoveActor.CreateObject().Decode(buffer);
				case 0x13:
					return McpeMovePlayer.CreateObject().Decode(buffer);
				case 0x14:
					return McpeRiderJump.CreateObject().Decode(buffer);
				case 0x15:
					return McpeUpdateBlock.CreateObject().Decode(buffer);
				case 0x16:
					return McpeAddPainting.CreateObject().Decode(buffer);
				case 0x19:
					return McpeLevelEvent.CreateObject().Decode(buffer);
				case 0x1a:
					return McpeBlockEvent.CreateObject().Decode(buffer);
				case 0x1b:
					return McpeActorEvent.CreateObject().Decode(buffer);
				case 0x1c:
					return McpeMobEffect.CreateObject().Decode(buffer);
				case 0x1d:
					return McpeUpdateAttributes.CreateObject().Decode(buffer);
				case 0x1e:
					return McpeInventoryTransaction.CreateObject().Decode(buffer);
				case 0x1f:
					return McpeMobEquipment.CreateObject().Decode(buffer);
				case 0x20:
					return McpeMobArmorEquipment.CreateObject().Decode(buffer);
				case 0x21:
					return McpeInteract.CreateObject().Decode(buffer);
				case 0x22:
					return McpeBlockPickRequest.CreateObject().Decode(buffer);
				case 0x23:
					return McpeActorPickRequest.CreateObject().Decode(buffer);
				case 0x24:
					return McpePlayerAction.CreateObject().Decode(buffer);
				case 0x26:
					return McpeHurtArmor.CreateObject().Decode(buffer);
				case 0x27:
					return McpeSetActorData.CreateObject().Decode(buffer);
				case 0x28:
					return McpeSetActorMotion.CreateObject().Decode(buffer);
				case 0x29:
					return McpeSetActorLink.CreateObject().Decode(buffer);
				case 0x2a:
					return McpeSetHealth.CreateObject().Decode(buffer);
				case 0x2b:
					return McpeSetSpawnPosition.CreateObject().Decode(buffer);
				case 0x2c:
					return McpeAnimate.CreateObject().Decode(buffer);
				case 0x2d:
					return McpeRespawn.CreateObject().Decode(buffer);
				case 0x2e:
					return McpeContainerOpen.CreateObject().Decode(buffer);
				case 0x2f:
					return McpeContainerClose.CreateObject().Decode(buffer);
				case 0x30:
					return McpePlayerHotbar.CreateObject().Decode(buffer);
				case 0x31:
					return McpeInventoryContent.CreateObject().Decode(buffer);
				case 0x32:
					return McpeInventorySlot.CreateObject().Decode(buffer);
				case 0x33:
					return McpeContainerSetData.CreateObject().Decode(buffer);
				case 0x34:
					return McpeCraftingData.CreateObject().Decode(buffer);
				case 0x36:
					return McpeGuiDataPickItem.CreateObject().Decode(buffer);
				case 0x38:
					return McpeBlockActorData.CreateObject().Decode(buffer);
				case 0x39:
					return McpePlayerInput.CreateObject().Decode(buffer);
				case 0x3a:
					return McpeLevelChunk.CreateObject().Decode(buffer);
				case 0x3b:
					return McpeSetCommandsEnabled.CreateObject().Decode(buffer);
				case 0x3c:
					return McpeSetDifficulty.CreateObject().Decode(buffer);
				case 0x3d:
					return McpeChangeDimension.CreateObject().Decode(buffer);
				case 0x3e:
					return McpeSetPlayerGameType.CreateObject().Decode(buffer);
				case 0x3f:
					return McpePlayerList.CreateObject().Decode(buffer);
				case 0x40:
					return McpeSimpleEvent.CreateObject().Decode(buffer);
				case 0x41:
					return McpeLegacyTelemetryEvent.CreateObject().Decode(buffer);
				case 0x43:
					return McpeClientboundMapItemData.CreateObject().Decode(buffer);
				case 0x44:
					return McpeMapInfoRequest.CreateObject().Decode(buffer);
				case 0x45:
					return McpeRequestChunkRadius.CreateObject().Decode(buffer);
				case 0x46:
					return McpeChunkRadiusUpdate.CreateObject().Decode(buffer);
				case 0x48:
					return McpeGameRulesChanged.CreateObject().Decode(buffer);
				case 0x49:
					return McpeCamera.CreateObject().Decode(buffer);
				case 0x4a:
					return McpeBossEvent.CreateObject().Decode(buffer);
				case 0x4b:
					return McpeShowCredits.CreateObject().Decode(buffer);
				case 0x4c:
					return McpeAvailableCommands.CreateObject().Decode(buffer);
				case 0x4d:
					return McpeCommandRequest.CreateObject().Decode(buffer);
				case 0x4e:
					return McpeCommandBlockUpdate.CreateObject().Decode(buffer);
				case 0x4f:
					return McpeCommandOutput.CreateObject().Decode(buffer);
				case 0x50:
					return McpeUpdateTrade.CreateObject().Decode(buffer);
				case 0x51:
					return McpeUpdateEquipment.CreateObject().Decode(buffer);
				case 0x52:
					return McpeResourcePackDataInfo.CreateObject().Decode(buffer);
				case 0x53:
					return McpeResourcePackChunkData.CreateObject().Decode(buffer);
				case 0x54:
					return McpeResourcePackChunkRequest.CreateObject().Decode(buffer);
				case 0x55:
					return McpeTransfer.CreateObject().Decode(buffer);
				case 0x56:
					return McpePlaySound.CreateObject().Decode(buffer);
				case 0x57:
					return McpeStopSound.CreateObject().Decode(buffer);
				case 0x58:
					return McpeSetTitle.CreateObject().Decode(buffer);
				case 0x59:
					return McpeAddBehaviorTree.CreateObject().Decode(buffer);
				case 0x5a:
					return McpeStructureBlockUpdate.CreateObject().Decode(buffer);
				case 0x5b:
					return McpeShowStoreOffer.CreateObject().Decode(buffer);
				case 0x5c:
					return McpePurchaseReceipt.CreateObject().Decode(buffer);
				case 0x5d:
					return McpePlayerSkin.CreateObject().Decode(buffer);
				case 0x5e:
					return McpeSubClientLogin.CreateObject().Decode(buffer);
				case 0x5f:
					return McpeInitiateWebSocketConnection.CreateObject().Decode(buffer);
				case 0x60:
					return McpeSetLastHurtBy.CreateObject().Decode(buffer);
				case 0x61:
					return McpeBookEdit.CreateObject().Decode(buffer);
				case 0x62:
					return McpeNpcRequest.CreateObject().Decode(buffer);
				case 0x63:
					return McpePhotoTransfer.CreateObject().Decode(buffer);
				case 0x64:
					return McpeModalFormRequest.CreateObject().Decode(buffer);
				case 0x65:
					return McpeModalFormResponse.CreateObject().Decode(buffer);
				case 0x66:
					return McpeServerSettingsRequest.CreateObject().Decode(buffer);
				case 0x67:
					return McpeServerSettingsResponse.CreateObject().Decode(buffer);
				case 0x68:
					return McpeShowProfile.CreateObject().Decode(buffer);
				case 0x69:
					return McpeSetDefaultGameType.CreateObject().Decode(buffer);
				case 0x6a:
					return McpeRemoveObjective.CreateObject().Decode(buffer);
				case 0x6b:
					return McpeSetDisplayObjective.CreateObject().Decode(buffer);
				case 0x6c:
					return McpeSetScore.CreateObject().Decode(buffer);
				case 0x6d:
					return McpeLabTable.CreateObject().Decode(buffer);
				case 0x6e:
					return McpeUpdateBlockSynced.CreateObject().Decode(buffer);
				case 0x6f:
					return McpeMoveActorDelta.CreateObject().Decode(buffer);
				case 0x70:
					return McpeSetScoreboardIdentity.CreateObject().Decode(buffer);
				case 0x71:
					return McpeSetLocalPlayerAsInitialized.CreateObject().Decode(buffer);
				case 0x72:
					return McpeUpdateSoftEnum.CreateObject().Decode(buffer);
				case 0x76:
					return McpeSpawnParticleEffect.CreateObject().Decode(buffer);
				case 0x77:
					return McpeAvailableEntityIdentifiers.CreateObject().Decode(buffer);
				case 0x79:
					return McpeNetworkChunkPublisherUpdate.CreateObject().Decode(buffer);
				case 0x7a:
					return McpeBiomeDefinitionList.CreateObject().Decode(buffer);
				case 0x7b:
					return McpeLevelSoundEvent.CreateObject().Decode(buffer);
				case 0x7c:
					return McpeLevelEventGeneric.CreateObject().Decode(buffer);
				case 0x7d:
					return McpeLecternUpdate.CreateObject().Decode(buffer);
				case 0x81:
					return McpeClientCacheStatus.CreateObject().Decode(buffer);
				case 0x82:
					return McpeOnScreenTextureAnimation.CreateObject().Decode(buffer);
				case 0x83:
					return McpeMapCreateLockedCopy.CreateObject().Decode(buffer);
				case 0x84:
					return McpeStructureTemplateDataRequest.CreateObject().Decode(buffer);
				case 0x85:
					return McpeStructureTemplateDataResponse.CreateObject().Decode(buffer);
				case 0x86:
					return McpeUpdateBlockProperties.CreateObject().Decode(buffer);
				case 0x87:
					return McpeClientCacheBlobStatus.CreateObject().Decode(buffer);
				case 0x88:
					return McpeClientCacheMissResponse.CreateObject().Decode(buffer);
				case 0x8f:
					return McpeNetworkSettings.CreateObject().Decode(buffer);
				case 0x90:
					return McpePlayerAuthInput.CreateObject().Decode(buffer);
				case 0x91:
					return McpeCreativeContent.CreateObject().Decode(buffer);
				case 0x92:
					return McpePlayerEnchantOptions.CreateObject().Decode(buffer);
				case 0x93:
					return McpeItemStackRequest.CreateObject().Decode(buffer);
				case 0x94:
					return McpeItemStackResponse.CreateObject().Decode(buffer);
				case 0x97:
					return McpeUpdatePlayerGameType.CreateObject().Decode(buffer);
				case 0x9c:
					return McpeViolationWarning.CreateObject().Decode(buffer);
				case 0xa2:
					return McpeItemComponent.CreateObject().Decode(buffer);
				case 0xac:
					return McpeUpdateSubChunkBlocks.CreateObject().Decode(buffer);
				case 0xae:
					return McpeSubChunk.CreateObject().Decode(buffer);
				case 0xaf:
					return McpeSubChunkRequestPacket.CreateObject().Decode(buffer);
				case 0xb4:
					return McpeDimensionData.CreateObject().Decode(buffer);
				case 0xbb:
					return McpeUpdateAbilities.CreateObject().Decode(buffer);
				case 0xbc:
					return McpeUpdateAdventureSettings.CreateObject().Decode(buffer);
				case 0xb8:
					return McpeRequestAbility.CreateObject().Decode(buffer);
				case 0xc1:
					return McpeRequestNetworkSettings.CreateObject().Decode(buffer);
				case 0x12e:
					return McpeTrimData.CreateObject().Decode(buffer);
				case 0x12f:
					return McpeOpenSign.CreateObject().Decode(buffer);
				case 0x8a:
					return McpeEmotePacket.CreateObject().Decode(buffer);
				case 0x98:
					return McpeEmoteList.CreateObject().Decode(buffer);
				case 0xb9:
					return McpeRequestPermission.CreateObject().Decode(buffer);
				case 0x133:
					return McpeSetInventoryOptions.CreateObject().Decode(buffer);
				case 0x136:
					return McpeClientboundCloseForm.CreateObject().Decode(buffer);
				case 0x138:
					return McpeServerboundLoadingScreen.CreateObject().Decode(buffer);
				case 0xa0:
					return McpePlayerFog.CreateObject().Decode(buffer);
				case 0x8D:
					return McpeAnvilDamage.CreateObject().Decode(buffer);
			}
		return null;
	}
}

public enum CommandPermission
{
	Normal = 0,
	Operator = 1,
	Host = 2,
	Automation = 3,
	Admin = 4
}

public enum PermissionLevel
{
	Visitor = 0,
	Member = 1,
	Operator = 2,
	Custom = 3
}

public enum ActionPermissions
{
	BuildAndMine = 0x1,
	DoorsAndSwitches = 0x2,
	OpenContainers = 0x4,
	AttackPlayers = 0x8,
	AttackMobs = 0x10,
	Operator = 0x20,
	Teleport = 0x80,
	Default = BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs,
	All = BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs | Operator | Teleport
}