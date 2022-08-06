using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using UnityEngine;
using Object = UnityEngine.Object;
using Unity.Netcode;
public static class MatchmakingService {
    private const int HeartbeatInterval = 15;
    private const int LobbyRefreshRate = 2; // Rate limits at 2

    private static UnityTransport _transport;

    private static Lobby _currentLobby;
    private static CancellationTokenSource _heartbeatSource, _updateLobbySource;

    private static UnityTransport Transport {
        get => _transport != null ? _transport : _transport = Object.FindObjectOfType<UnityTransport>();
        set => _transport = value;
    }

    public static event Action<Lobby> CurrentLobbyRefreshed;

    public static void ResetStatics() {
        if (Transport != null) {
            Transport.Shutdown();
            Transport = null;
        }

        _currentLobby = null;
    }
    public static string GetLobbyId() {
        if(_currentLobby != null) {
            return _currentLobby.LobbyCode.ToString();
        }
        else{
            return "LobbyNotCreated";
        }
    }

    public static async Task CreateLobbyWithAllocation() {
        int maxPlayers = 2;
        // Create a relay allocation and generate a join code to share with the lobby
        var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        
        CreateLobbyOptions options = new CreateLobbyOptions();
        options.IsPrivate = true;
        // Create a lobby, adding the relay join code to the lobby data
        string lobbyName = "new lobby";
        _currentLobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, 2, options);
        //Transport.SetRelayServerData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        Transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

        Heartbeat();
        PeriodicallyRefreshLobby();
    }

    public static async Task LockLobby() {
        try {
            await Lobbies.Instance.UpdateLobbyAsync(_currentLobby.Id, new UpdateLobbyOptions { IsLocked = true });
        }
        catch (Exception e) {
            Debug.Log($"Failed closing lobby: {e}");
        }
    }

    private static async void Heartbeat() {
        _heartbeatSource = new CancellationTokenSource();
        while (!_heartbeatSource.IsCancellationRequested && _currentLobby != null) {
            await Lobbies.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
            await Task.Delay(HeartbeatInterval * 1000);
        }
    }

    public static async Task JoinLobbyWithAllocation(string lobbyId) {
        Debug.Log("Inside method Matchmaking.JoinLobbyWithAllocation");
        Debug.Log("JoinCode: " + lobbyId);
        _currentLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyId);
        var a = await RelayService.Instance.JoinAllocationAsync(_currentLobby.Data[Constants.JoinKey].Value);

        Transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        PeriodicallyRefreshLobby();
    }

    private static async void PeriodicallyRefreshLobby() {
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(LobbyRefreshRate * 1000);
        while (!_updateLobbySource.IsCancellationRequested && _currentLobby != null) {
            _currentLobby = await Lobbies.Instance.GetLobbyAsync(_currentLobby.Id);
            CurrentLobbyRefreshed?.Invoke(_currentLobby);
            await Task.Delay(LobbyRefreshRate * 1000);
        }
    }

    public static async Task LeaveLobby() {
        _heartbeatSource?.Cancel();
        _updateLobbySource?.Cancel();

        if (_currentLobby != null)
            try {
                if (_currentLobby.HostId == Authentication.PlayerId) await Lobbies.Instance.DeleteLobbyAsync(_currentLobby.Id);
                else await Lobbies.Instance.RemovePlayerAsync(_currentLobby.Id, Authentication.PlayerId);
                _currentLobby = null;
            }
            catch (Exception e) {
                Debug.Log(e);
            }
    }
}