using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class MenuController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _joinCode;

    public void JoinGame() {
        Debug.Log("In Join Game!");
        Debug.Log("Join Code: " + _joinCode.text);

    }

    // Update is called once per frame
    public void Create(){
        Debug.Log("Creating New Game!");
    }
}
