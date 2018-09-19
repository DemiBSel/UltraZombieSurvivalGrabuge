using UnityEngine;
using UnityEngine.Networking;

public class InitNameMessage : MessageBase
{
    public uint netId;
    public NetworkHash128 assetId;
    public string txt_message;
    public byte[] payload;

}