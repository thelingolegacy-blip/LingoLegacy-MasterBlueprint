using System;
using UnityEngine;
namespace CrazyWeasol.Crash { [Serializable] public struct CoreState { [Range(0f,100f)] public float stability; public bool IsFractured=>stability>0f&&stability<=40f; public bool IsTotalCrash=>stability<=0f; public CoreState(float s){stability=Mathf.Clamp(s,0f,100f);} } }
