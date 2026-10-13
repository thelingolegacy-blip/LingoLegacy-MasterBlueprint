using UnityEngine;
namespace CrazyWeasol.Core { public sealed class GameBootstrap : MonoBehaviour { public const string BuildId="CW-B01"; [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void Initialize(){ Debug.Log("[CrazyWeasol] CW-B01 runtime initialized."); } } }
