# Crazy Weasol: Dark Crash — Implementation Status

## Added
- Player locomotion
- Combat event contract
- Weasol Core stability controller
- Rot-Weasel pursuit AI
- Chaos Rank resolver
- Three-slot local save service
- Runtime telemetry log
- Unity Build-01 project/build entry point
- Runner-only G02 sentinel

## Evidence boundary
These source additions establish implementation scope only. They do not establish runtime execution.

## Current release state
G02 remains blocked until a real GitHub Actions runner allocates a job, instantiates steps, executes the sentinel, exposes retrievable logs, and produces independently verifiable build evidence.

## Required Unity evidence
1. Unity 2022.3.50f1 starts in batchmode.
2. BuildScript.PerformBuild executes.
3. Build result is Succeeded.
4. build/CrazyWeasol-CW-B01.x86_64 exists.
5. Artifact is uploaded and retrievable.
6. SHA-256 is recorded and independently reproduced.
