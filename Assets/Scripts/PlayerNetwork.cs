using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<Vector2> _netPos = new(writePerm: NetworkVariableWritePermission.Owner);
    // Update is called once per frame
    void Update()
    {
        if(IsOwner) {
            _netPos.Value = transform.position;
        }
        else {
            transform.position = _netPos.Value;
        }
    }
}
