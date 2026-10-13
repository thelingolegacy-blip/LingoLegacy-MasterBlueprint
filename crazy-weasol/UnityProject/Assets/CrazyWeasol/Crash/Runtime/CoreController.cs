using UnityEngine;
using CrazyWeasol.Core;

namespace CrazyWeasol.Crash
{
    public sealed class CoreController : MonoBehaviour
    {
        [SerializeField, Range(0f,100f)] float stability = 100f;
        public float Stability => stability;
        public bool IsFractured => stability > 0f && stability <= 40f;
        public bool IsTotalCrash => stability <= 0f;

        public void Damage(float amount)
        {
            stability = Mathf.Clamp(stability - Mathf.Max(0f, amount), 0f, 100f);
            EventBus.Publish(new CoreStabilityChangedEvent(stability, new CoreState(stability)));
        }

        public void Restore(float amount)
        {
            stability = Mathf.Clamp(stability + Mathf.Max(0f, amount), 0f, 100f);
            EventBus.Publish(new CoreStabilityChangedEvent(stability, new CoreState(stability)));
        }
    }

    public readonly struct CoreStabilityChangedEvent
    {
        public readonly float Stability;
        public readonly CoreState State;
        public CoreStabilityChangedEvent(float stability, CoreState state)
        {
            Stability = stability;
            State = state;
        }
    }
}
