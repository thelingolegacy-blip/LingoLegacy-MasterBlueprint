using UnityEngine;

namespace CrazyWeasol.AI
{
    public sealed class ExperimentalGuardian : MonoBehaviour
    {
        public enum Phase { Normal, Mutation, Crash, RealityBreak }
        [SerializeField] Transform target;
        [SerializeField] float phaseHealth = 100f;
        public Phase CurrentPhase { get; private set; } = Phase.Normal;

        public void ApplyDamage(float amount)
        {
            phaseHealth -= Mathf.Max(0f, amount);
            if (phaseHealth <= 75f && CurrentPhase == Phase.Normal) CurrentPhase = Phase.Mutation;
            if (phaseHealth <= 45f && CurrentPhase == Phase.Mutation) CurrentPhase = Phase.Crash;
            if (phaseHealth <= 15f && CurrentPhase == Phase.Crash) CurrentPhase = Phase.RealityBreak;
        }

        void Update()
        {
            if (target == null) return;
            var d = target.position - transform.position; d.y = 0f;
            if (d.sqrMagnitude > .01f) transform.forward = Vector3.Slerp(transform.forward, d.normalized, 4f * Time.deltaTime);
        }
    }
}
