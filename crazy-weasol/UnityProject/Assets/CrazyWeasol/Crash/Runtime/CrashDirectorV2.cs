using UnityEngine;
using CrazyWeasol.Telemetry;

namespace CrazyWeasol.Crash
{
    public sealed class CrashDirectorV2 : MonoBehaviour
    {
        [SerializeField] CoreController core;
        [SerializeField] float fractureScale = 1.2f;
        bool active;

        void Update()
        {
            if (core == null || active || !core.IsFractured) return;
            active = true;
            Time.timeScale = 0.82f;
            transform.localScale *= fractureScale;
            CWTelemetry.Record("CRASH_EVENT", "GRAVITY_FRACTURE");
        }
    }
}
