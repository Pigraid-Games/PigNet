#region LICENSE

// The contents of this file are subject to the Common Public Attribution
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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using log4net;
using MiNET.Net.Packets.Mcpe;
using MiNET.Net.RakNet;
using MiNET.Plugins;

namespace MiNET.Net;

public class BedrockMessageHandler : BedrockMessageHandlerBase
{
	private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockMessageHandler));
	private readonly PluginManager _pluginManager;

	public BedrockMessageHandler(RakSession session, IServerManager serverManager, PluginManager pluginManager) : base(session)
	{
		_pluginManager = pluginManager;
		Handler = new LoginMessageHandler(this, session, serverManager);
	}

	public IMcpeMessageHandler Handler { get; set; }

	public override void Connected()
	{
	}

	public override void Disconnect(string reason, bool sendDisconnect = true)
	{
		Handler.Disconnect(reason, sendDisconnect);
	}

	public override Packet OnSendCustomPacket(Packet packet)
	{
		if (Handler is Player player)
		{
			Packet result = _pluginManager.PluginPacketHandler(packet, false, player);
			if (result != packet) packet.PutPool();
			packet = result;
		}

		return packet;
	}

	public override void HandleCustomPacket(Packet message)
	{
		HandleBedrockMessage(Handler, message);
	}

	private void HandleBedrockMessage(IMcpeMessageHandler handler, Packet message)
	{
		if (handler is Player player)
		{
			Packet result = _pluginManager.PluginPacketHandler(message, true, player);
			if (result != message) message.PutPool();
			message = result;
		}

		if (message == null) return;

		switch (message)
		{
			case McpeClientToServerHandshake msg:
				// Start encryption
				handler.HandleMcpeClientToServerHandshake(msg);
				break;
			case McpeResourcePackClientResponse msg:
				handler.HandleMcpeResourcePackClientResponse(msg);
				break;
			case McpeResourcePackChunkRequest msg:
				handler.HandleMcpeResourcePackChunkRequest(msg);
				break;
			case McpeSetLocalPlayerAsInitialized msg:
				handler.HandleMcpeSetLocalPlayerAsInitialized(msg);
				break;
			case McpeUpdateBlock _:
				// DO NOT USE. Will dissapear from MCPE any release. 
				// It is a bug that it leaks these messages.
				break;
			case McpeLevelSoundEvent msg:
				handler.HandleMcpeLevelSoundEvent(msg);
				break;
			case McpeClientCacheStatus msg:
				handler.HandleMcpeClientCacheStatus(msg);
				break;
			case McpeAnimate msg:
				handler.HandleMcpeAnimate(msg);
				break;
			case McpeActorEvent msg:
				handler.HandleMcpeEntityEvent(msg);
				break;
			case McpeText msg:
				handler.HandleMcpeText(msg);
				break;
			case McpeRemoveActor _:
				// Do nothing right now, but should clear out the entities and stuff
				// from this players internal structure.
				break;
			case McpeRequestNetworkSettings msg:
				handler.HandleMcpeRequestNetworkSettings(msg);
				break;
			case McpeLogin msg:
				handler.HandleMcpeLogin(msg);
				break;
			case McpeMovePlayer msg:
				handler.HandleMcpeMovePlayer(msg);
				break;
			case McpeInteract msg:
				handler.HandleMcpeInteract(msg);
				break;
			case McpeRespawn msg:
				handler.HandleMcpeRespawn(msg);
				break;
			case McpeBlockActorData msg:
				handler.HandleMcpeBlockEntityData(msg);
				break;
			case McpePlayerAction msg:
				handler.HandleMcpePlayerAction(msg);
				break;
			case McpeContainerClose msg:
				handler.HandleMcpeContainerClose(msg);
				break;
			case McpeMobEquipment msg:
				handler.HandleMcpeMobEquipment(msg);
				break;
			case McpeMobArmorEquipment msg:
				handler.HandleMcpeMobArmorEquipment(msg);
				break;
			case McpeInventoryTransaction msg:
				handler.HandleMcpeInventoryTransaction(msg);
				break;
			case McpeServerSettingsRequest msg:
				handler.HandleMcpeServerSettingsRequest(msg);
				break;
			case McpeSetPlayerGameType msg:
				handler.HandleMcpeSetPlayerGameType(msg);
				break;
			case McpePlayerHotbar msg:
				handler.HandleMcpePlayerHotbar(msg);
				break;
			case McpeInventoryContent msg:
				handler.HandleMcpeInventoryContent(msg);
				break;
			case McpeRequestChunkRadius msg:
				handler.HandleMcpeRequestChunkRadius(msg);
				break;
			case McpeMapInfoRequest msg:
				handler.HandleMcpeMapInfoRequest(msg);
				break;
			case McpeItemStackRequest nms:
				handler.HandleMcpeItemStackRequest(nms);
				break;
			case McpePlayerInput msg:
				handler.HandleMcpePlayerInput(msg);
				break;
			case McpeRiderJump msg:
				handler.HandleMcpeRiderJump(msg);
				break;
			case McpeCommandRequest msg:
				handler.HandleMcpeCommandRequest(msg);
				break;
			case McpeBlockPickRequest msg:
				handler.HandleMcpeBlockPickRequest(msg);
				break;
			case McpeActorPickRequest msg:
				handler.HandleMcpeTakeItemActor(msg);
				break;
			case McpeModalFormResponse msg:
				handler.HandleMcpeModalFormResponse(msg);
				break;
			case McpeCommandBlockUpdate msg:
				handler.HandleMcpeCommandBlockUpdate(msg);
				break;
			case McpeMoveActor msg:
				handler.HandleMcpeMoveEntity(msg);
				break;
			case McpeSetActorMotion msg:
				handler.HandleMcpeSetActorMotion(msg);
				break;
			case McpePhotoTransfer msg:
				handler.HandleMcpePhotoTransfer(msg);
				break;
			case McpeSetActorData msg:
				handler.HandleMcpeSetActorData(msg);
				break;
			case McpeNpcRequest msg:
				handler.HandleMcpeNpcRequest(msg);
				break;
			case McpeViolationWarning msg:
				handler.HandleMcpePacketViolationWarning(msg);
				break;
			case McpePlayerSkin msg:
				handler.HandleMcpePlayerSkin(msg);
				break;
			case McpeRequestAbility msg:
				handler.HandleMcpeRequestAbility(msg);
				break;
			case McpeEmotePacket msg:
				handler.HandleMcpeEmote(msg);
				break;
			case McpeEmoteList msg:
				handler.HandleMcpeEmoteList(msg);
				break;
			case McpeRequestPermission msg:
				handler.HandleMcpePermissionRequest(msg);
				break;
			case McpeSetInventoryOptions msg:
				handler.HandleMcpeSetInventoryOptions(msg);
				break;
			case McpeAnvilDamage msg:
				handler.HandleMcpeAnvilDamage(msg);
				break;
			case McpePlayerAuthInput msg:
				handler.HandleMcpePlayerAuthInput(msg);
				break;
			case McpeServerboundLoadingScreen msg:
				handler.HandleMcpeServerboundLoadingScreen(msg);
				break;

			default:
			{
				Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {_session.Username}, IP {_session.EndPoint.Address}");
				if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
				break;
			}
		}
	}
}