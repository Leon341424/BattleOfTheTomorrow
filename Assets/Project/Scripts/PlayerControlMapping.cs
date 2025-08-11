using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControlMapping", menuName = "Scriptable Objects/PlayerControlMapping")]
public class PlayerControlMapping : ScriptableObject
{
    public string lowPunch;
    public string lowKick;
    public string hardPunch;
    public string hardKick;
    public string Block;
    public string Throw;
}
