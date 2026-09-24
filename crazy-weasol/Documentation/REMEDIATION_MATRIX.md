# Crazy Weasol: Dark Crash — Remediation Matrix

| Area | State | Evidence | Action |
|---|---|---|---|
| Source scaffold | Hardened | Git commits | Continue implementation |
| Unity project | Present | ProjectVersion 2022.3.50f1 | Runtime verification pending |
| Build script | Present | BuildScript.PerformBuild | Runtime verification pending |
| G02 sentinel | Reproduced failure | Run 35941087615, retry job 107449970018 | Requires runner allocation |
| Runner steps | Missing | steps=[] | External Actions infrastructure boundary |
| Job logs | Missing | BlobNotFound/404 | Cannot independently verify execution |
| Artifacts | Missing | artifacts=[] | Cannot verify executable |
| AppDeploy | Not equivalent | Web QA only | Do not use as Unity evidence |
| Release | Blocked | G02 unsatisfied | Preserve fail-closed state |

## Non-negotiable transition
RUNNER ALLOCATION -> STEP EXECUTION -> SENTINEL -> LOGS -> ARTIFACT -> HASH -> INDEPENDENT VERIFICATION -> G02 PASS.
