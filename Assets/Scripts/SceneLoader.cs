using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System.Linq;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private TMP_InputField _joinCode; // Join Code the player types in
    [SerializeField] private TextMeshProUGUI _lobbyCode; // Lobby Code that the player gives for others to enter into join code field.
    [SerializeField] private GameObject _createGamePanel;
    public static event Action StartTheFreakingGame;
    private void Start() {
        StartTheFreakingGame += StartGame;
    }
    public async void JoinGame(){
        string temp_join_code = _joinCode.text;
        Debug.Log("JoinCode: " + temp_join_code);
        try
        {   
            await MatchmakingService.JoinLobbyWithAllocation(temp_join_code);
            NetworkManager.Singleton.StartClient();
            //StartTheFreakingGame?.Invoke();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        } 
    }
    public async void CreateGame(){
        
        await MatchmakingService.CreateLobbyWithAllocation();
        NetworkManager.Singleton.StartHost();
        
        _lobbyCode.SetText(MatchmakingService.GetLobbyId());
        _createGamePanel.SetActive(true);
        

    }
    public async void CloseMatchmaking(){
        _createGamePanel.SetActive(false);
        await MatchmakingService.LeaveLobby();
    }
    public void ExitGame(){
        Application.Quit();
        Debug.Log("Quitting!");
    }

    public async void NewGame(){
        Debug.Log("Loading Scene!");
        await Authentication.Login();
        
        Debug.Log(Authentication.PlayerId);
        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Single);
    }

    public void Options(){
        Debug.Log("Options?");
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }
    public async void StartGame(){
        await MatchmakingService.LockLobby();
        NetworkManager.Singleton.SceneManager.LoadScene("Playertestscene", LoadSceneMode.Single);
    }
}
