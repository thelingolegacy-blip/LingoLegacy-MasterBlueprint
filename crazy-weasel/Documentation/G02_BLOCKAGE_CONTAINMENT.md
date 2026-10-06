# G02 Blockage Containment Boundary

## Purpose

Contain the unresolved G02 runner-dispatch blockage without allowing an unproven runner state to leak into downstream gates, deployment, publication, activation, or promotion.

## Containment State

```
G02 BLOCKAGE
     |
     +--> DIAGNOSE
     +--> REPAIR
     +--> OBSERVE
     +--> CAPTURE EVIDENCE
     +--> CORRELATE
     +--> VERIFY
     |
     X--> G03 EXECUTION
     X--> DEPLOYMENT
     X--> PROMOTION
     X--> ACTIVATION
     X--> PUBLICATION
```

## Allowed Inside the Container

- Runner installation/recovery diagnostics
- Runner service/process inspection
- GitHub Actions allocation telemetry
- Sentinel execution testing
- Log retrieval attempts
- Artifact capture
- SHA-256 evidence generation
- Evidence identity and scope checks
- Temporal/execution/job/artifact correlation
- Independent verification
- Repair commits on the isolated G02 recovery branch

## Explicitly Blocked by the Container

- G02 PASS without complete verified evidence
- G03 execution authorization
- Downstream gate promotion
- Deployment mutation
- Production activation
- Public publication
- Automatic state advancement
- Evidence substitution from another gate
- Inference from Online/Idle runner status
- Inference from queued jobs
- Inference from workflow configuration alone
- Inference from successful repository commits

## Exit Predicate

The container may be considered RELEASED only when:

```
RUNNER_ID > 0
AND RUNNER_NAME != ""
AND JOB_ASSIGNED = TRUE
AND STEPS_INSTANTIATED = TRUE
AND SENTINEL_SUCCESS = TRUE
AND LOGS_RETRIEVABLE = TRUE
AND ARTIFACT_RETRIEVABLE = TRUE
AND HASH_REPRODUCIBLE = TRUE
AND CORRELATION = TRUE
AND INDEPENDENT_VERIFICATION = TRUE
```

Then:

```
VERIFIED_EVIDENCE(G02)
      AND
ESTABLISHED(G02)
      AND
AUTHORIZED(G02)
      AND
PASS(G02)
      =>
G03 ELIGIBILITY ONLY
```

## Fail-Closed Rule

Any missing, empty, contradictory, stale, unretrievable, uncorrelated, or unverified predicate keeps the container closed.

```
ANY REQUIRED PREDICATE = FALSE / UNPROVEN
        =>
CONTAINER = CLOSED
        =>
G02 = NOT ESTABLISHED
        =>
DOWNSTREAM = BLOCKED
        =>
LKG = PROTECTED
```

## Current Known Condition

Repository-side recovery instrumentation has been hardened and the Linux installer variable-expansion defect has been repaired.

Current live Actions observations still show:

```
RUN = FAILURE
JOBS = NONE RETURNED
ARTIFACTS = NONE
RUNNER ALLOCATION = NOT PROVEN
SENTINEL EXECUTION = NOT PROVEN
G02 PASS = NOT ESTABLISHED
```

These observations remain contained inside G02.

## Authority Boundary

```
G02 CONTAINER
    |
    +-- evidence
    +-- verification
    +-- repair
    +-- telemetry
    |
    X-- downstream authority
```

No downstream state may be derived from the existence of this container, its repair commits, its workflow definitions, or its diagnostic outputs.

## Governing Invariants

```
NO BORROWING
NO INHERITANCE
NO SUBSTITUTION
NO STATE INFERENCE
NO IMPLIED TRANSITION
NO GATE SKIPPING
NO PARTIAL PASS
NO AUTOMATIC PROMOTION
NO AUTO-PUBLICATION
NO AUTO-ACTIVATION
```

Containment remains active until the complete G02 acceptance predicate is independently verified.
