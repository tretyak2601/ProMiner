using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUps", menuName = "PowerUP Data", order = 52)]
public class PowerUpsScriptable : ScriptableObject
{
    [SerializeField] PowerUp[] powerUps;
    public PowerUp[] PowerUps { get { return powerUps; } }
}
