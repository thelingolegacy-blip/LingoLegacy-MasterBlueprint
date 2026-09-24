# Crazy Weasol™ — G02 Runner Restart & Recovery Runbook

## Purpose

Recover the `lingo-g02` self-hosted GitHub Actions runner from a non-dispatching state and verify the host-side daemon before retrying G02.

**Important:** restarting the host or runner is a recovery action, not G02 acceptance evidence.

## Current G02 contract

Required runner target:

```yaml
runs-on: [self-hosted, linux, x64, lingo-g02]
```

Required positive evidence:

```
LIVE RUNNER ALLOCATION
        ↓
runner_id       > 0
runner_name     ≠ ""
steps[]         ≠ []
        ↓
SENTINEL = SUCCESS
        ↓
LOGS RETRIEVABLE
        ↓
ARTIFACT RETRIEVABLE
        ↓
SHA-256 REPRODUCIBLE
        ↓
INDEPENDENT VERIFICATION = PASS
        ↓
G02 PASS
```

## 1. Locate the runner installation

Run on the `lingo-g02` host:

```bash
find ~ /opt /srv -maxdepth 4 -type f -name 'run.sh' 2>/dev/null | grep -E 'actions-runner|runner'
```

If the runner directory is known, enter it:

```bash
cd /path/to/actions-runner
```

Do not substitute a different host or runner.

## 2. Inspect registered runner services

```bash
systemctl list-units --type=service --all | grep -i 'actions.runner'
systemctl list-unit-files | grep -i 'actions.runner'
```

Inspect the exact service discovered:

```bash
sudo systemctl status actions.runner.<RUNNER_SERVICE> --no-pager
```

## 3. Inspect the runner processes

```bash
ps aux | grep -E '[R]unner.Listener|[R]unner.Worker'
pgrep -af 'Runner.Listener|Runner.Worker'
```

A healthy listener must be present.

## 4. Inspect runner diagnostics

From the runner directory:

```bash
ls -lah _diag/
tail -n 200 _diag/Runner_*.log
tail -n 200 _diag/Worker_*.log
```

Search for decisive state:

```bash
grep -RniE 'Connected to GitHub|Listening for Jobs|error|failed|authentication|unauthorized|forbidden|401|403|404|connection|WebSocket' _diag/ 2>/dev/null | tail -200
```

The critical healthy signals are:

```
Connected to GitHub
Listening for Jobs
```

## 5. Verify service configuration

```bash
sudo systemctl cat actions.runner.<RUNNER_SERVICE>
sudo systemctl is-enabled actions.runner.<RUNNER_SERVICE>
sudo systemctl is-active actions.runner.<RUNNER_SERVICE>
```

## 6. Restart the runner service

Prefer restarting the registered service:

```bash
sudo systemctl restart actions.runner.<RUNNER_SERVICE>
```

Then immediately verify:

```bash
sudo systemctl status actions.runner.<RUNNER_SERVICE> --no-pager
sudo systemctl is-active actions.runner.<RUNNER_SERVICE>
pgrep -af 'Runner.Listener|Runner.Worker'
```

## 7. If the service is unavailable, start the runner directly

Only if the runner is known to be installed and service management is unavailable:

```bash
cd /path/to/actions-runner
./run.sh
```

Do not register a duplicate runner unless registration itself has been independently diagnosed as invalid.

## 8. Verify GitHub connectivity

Inspect the newest diagnostics after restart:

```bash
grep -RniE 'Connected to GitHub|Listening for Jobs' _diag/ 2>/dev/null | tail -50
```

Required host-side state:

```
Runner.Listener
      ↓
Connected to GitHub
      ↓
Listening for Jobs
```

## 9. Verify runner identity and labels

The runner must remain the exact G02 target:

```
self-hosted
linux
x64
lingo-g02
```

Do not weaken the workflow to `ubuntu-latest` or remove `lingo-g02` merely to obtain execution.

## 10. Reboot recovery option

If the runner daemon remains unhealthy after a service restart, reboot the `lingo-g02` host:

```bash
sudo reboot
```

After the host returns:

```bash
systemctl list-units --type=service --all | grep -i 'actions.runner'
sudo systemctl status actions.runner.<RUNNER_SERVICE> --no-pager
ps aux | grep -E '[R]unner.Listener|[R]unner.Worker'
```

Then verify:

```bash
grep -RniE 'Connected to GitHub|Listening for Jobs|error|failed|authentication|unauthorized|forbidden'   /path/to/actions-runner/_diag/ 2>/dev/null | tail -200
```

## 11. GitHub-side validation

After the host reports `Connected to GitHub` and `Listening for Jobs`, inspect the controlled G02 job.

Current controlled job:

- Run: `35957977841`
- Job: `107500257954`
- Workflow: `Crazy Weasol — G02 Acceptance Sentinel`

The job must leave `queued` and become allocated to `lingo-g02`.

Required live telemetry:

```
runner_id   > 0
runner_name ≠ ""
steps[]     ≠ []
```

## 12. G02 acceptance sequence

Do not declare success merely because the runner becomes Online/Idle.

The complete chain is:

```
JOB QUEUED
    ↓
LINGO-G02 ALLOCATION
    ↓
runner_id > 0
runner_name ≠ ""
    ↓
steps[] ≠ []
    ↓
SENTINEL = SUCCESS
    ↓
LOGS RETRIEVABLE
    ↓
ARTIFACT RETRIEVABLE
    ↓
SHA-256 REPRODUCIBLE
    ↓
INDEPENDENT VERIFICATION = PASS
    ↓
G02 PASS
```

## Prohibited substitutions

The following do **not** satisfy G02:

- runner registration alone
- Online/Idle state without job allocation
- a queued job
- workflow configuration
- a successful commit
- hosted-runner execution
- AppDeploy QA
- Unity source presence
- expected labels without allocation
- a reboot without execution telemetry
- inferred or assumed execution

## Fail-closed rule

```
NO ALLOCATION
      ↓
NO EXECUTION TELEMETRY
      ↓
NO G02 ACCEPTANCE
      ↓
NO DOWNSTREAM AUTHORIZATION
      ↓
LKG PROTECTED
```

Recovery is complete only when the live G02 acceptance predicate is actually satisfied and independently verified.
