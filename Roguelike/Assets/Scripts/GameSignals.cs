using UnityEngine;
using System;

public class GameSignals : MonoBehaviour
{
    public event Action<float> OnSeenValueChanged;
}
