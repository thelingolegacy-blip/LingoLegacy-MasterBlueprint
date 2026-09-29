$ErrorActionPreference = "Stop"
if (-not $env:GITHUB_RUNNER_URL) { throw "Set GITHUB_RUNNER_URL" }
$RunnerDir = if ($env:RUNNER_DIR) { $env:RUNNER_DIR } else { "C:\actions-runner" }
$RunnerVersion = if ($env:RUNNER_VERSION) { $env:RUNNER_VERSION } else { "2.337.0" }
$RunnerName = if ($env:RUNNER_NAME) { $env:RUNNER_NAME } else { "lingo-legacy-g02" }
New-Item -ItemType Directory -Force -Path $RunnerDir | Out-Null
Set-Location $RunnerDir
if (-not (Test-Path ".runner")) {
  if (-not $env:GITHUB_RUNNER_TOKEN) { throw "Set GITHUB_RUNNER_TOKEN to a fresh GitHub runner registration token" }
  $zip = Join-Path $RunnerDir "actions-runner-win-x64-$RunnerVersion.zip"
  $url = "https://github.com/actions/runner/releases/download/v$RunnerVersion/actions-runner-win-x64-$RunnerVersion.zip"
  Invoke-WebRequest -Uri $url -OutFile $zip
  Expand-Archive -Path $zip -DestinationPath $RunnerDir -Force
  Remove-Item $zip -Force
  & .\config.cmd --unattended --url $env:GITHUB_RUNNER_URL --token $env:GITHUB_RUNNER_TOKEN --name $RunnerName --labels "lingo-g02" --work "_work"
}
if (-not (Test-Path ".service")) { & .\svc.cmd install }
& .\svc.cmd stop
& .\svc.cmd start
& .\svc.cmd status
Write-Host "=== RUNNER PROCESS ==="
Get-Process -Name Runner.Listener -ErrorAction SilentlyContinue | Select-Object Id,Path,StartTime
Write-Host "=== RUNNER TELEMETRY ==="
Get-Content .\_diag\Runner_*.log -Tail 100 -ErrorAction SilentlyContinue
Get-Content .\_diag\Worker_*.log -Tail 100 -ErrorAction SilentlyContinue
