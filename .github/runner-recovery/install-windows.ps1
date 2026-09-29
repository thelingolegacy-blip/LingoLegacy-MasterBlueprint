$ErrorActionPreference = "Stop"
if (-not $env:GITHUB_RUNNER_URL) { throw "Set GITHUB_RUNNER_URL" }
if (-not $env:GITHUB_RUNNER_TOKEN) { throw "Set GITHUB_RUNNER_TOKEN to a fresh runner registration token" }
$RunnerDir = if ($env:RUNNER_DIR) { $env:RUNNER_DIR } else { "C:\actions-runner" }
$RunnerVersion = if ($env:RUNNER_VERSION) { $env:RUNNER_VERSION } else { "2.336.0" }
New-Item -ItemType Directory -Force -Path $RunnerDir | Out-Null
Set-Location $RunnerDir
$zip = Join-Path $RunnerDir "actions-runner-win-x64-$RunnerVersion.zip"
$url = "https://github.com/actions/runner/releases/download/v$RunnerVersion/actions-runner-win-x64-$RunnerVersion.zip"
Invoke-WebRequest -Uri $url -OutFile $zip
Expand-Archive -Path $zip -DestinationPath $RunnerDir -Force
Remove-Item $zip -Force
& .\config.cmd --unattended --url $env:GITHUB_RUNNER_URL --token $env:GITHUB_RUNNER_TOKEN --name "lingo-legacy-g02" --labels "lingo-g02" --work "_work"
& .\svc.cmd install
& .\svc.cmd start
& .\svc.cmd status
