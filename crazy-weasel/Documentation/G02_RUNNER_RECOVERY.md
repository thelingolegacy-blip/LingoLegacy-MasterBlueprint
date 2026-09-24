# G02 Runner Recovery — lingo-g02

## Purpose

Restore the actual self-hosted GitHub Actions runner required by the G02 acceptance workflow.

This is recovery configuration, not G02 evidence. G02 remains FAIL / UNVERIFIED until a live job is allocated to the runner and produces retrievable execution evidence.

## Required runner contract

The runner must be online, repository-authorized for thelingolegacy-blip/LingoLegacy-MasterBlueprint, and carry these labels:

- self-hosted
- linux
- x64
- lingo-g02

The workflow target is:

~~~yaml
runs-on: [self-hosted, linux, x64, lingo-g02]
~~~

## Host-side checks

Run these commands on the machine that owns the lingo-g02 runner.

### 1. Locate the runner

~~~bash
cd /path/to/actions-runner
ls -la
~~~

### 2. Check runner service state

~~~bash
sudo systemctl status actions.runner.* --no-pager
~~~

Use the exact generated service name if required.

### 3. Check runner process

~~~bash
ps aux | grep -E '[R]unner.Listener|[R]unner.Worker'
~~~

A healthy active runner should have a listener process.

### 4. Check runner diagnostics

~~~bash
ls -lt _diag/
tail -n 200 _diag/Runner_*.log
tail -n 200 _diag/Worker_*.log
~~~

Look for:

~~~text
Connected to GitHub
Listening for Jobs
~~~

### 5. Restart an installed service

~~~bash
sudo systemctl restart <exact-runner-service>
sudo systemctl status <exact-runner-service> --no-pager
~~~

### 6. Interactive fallback

From the runner directory:

~~~bash
./run.sh
~~~

The required live state is:

~~~text
Connected to GitHub
Listening for Jobs
~~~

Keep the runner process alive while the G02 workflow is dispatched.

## Critical label rule

The runner registration must expose lingo-g02. Do not weaken the G02 workflow to accommodate a missing label.

## Acceptance observation

Once the daemon is online, inspect:

RUN 35944924359
JOB 107460661288

Required transition:

QUEUED
  ↓
RUNNER ALLOCATION
  ↓
STEPS INSTANTIATED
  ↓
SENTINEL EXECUTED
  ↓
LOGS + ARTIFACT
  ↓
SHA-256
  ↓
INDEPENDENT VERIFICATION
  ↓
G02 PASS

## Fail-closed rule

Do not treat runner registration, Online/Idle status without job allocation, queued jobs, workflow configuration, repository commits, AppDeploy QA, or expected labels as G02 execution evidence.

## Current blocker

Last verified observation:

RUN 35944924359
JOB 107460661288
STATUS queued
STEPS []
LOGS unavailable
ARTIFACTS []

Therefore:

G02          FAIL / UNVERIFIED
DOWNSTREAM   BLOCKED
LKG          PROTECTED
FAIL-CLOSED  ACTIVE

The host-side runner daemon must become live before positive G02 telemetry can exist.
