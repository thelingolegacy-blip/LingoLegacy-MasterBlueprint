#!/usr/bin/env bash
set -euo pipefail
: "\${GITHUB_RUNNER_URL:?Set GITHUB_RUNNER_URL}"
RUNNER_DIR="\${RUNNER_DIR:-\$HOME/actions-runner}"
RUNNER_VERSION="\${RUNNER_VERSION:-2.337.0}"
RUNNER_NAME="\${RUNNER_NAME:-lingo-legacy-g02}"
cd "\$RUNNER_DIR"
if [ ! -f .runner ]; then
  : "\${GITHUB_RUNNER_TOKEN:?Set a fresh GitHub Actions runner registration token}"
  ARCH="\$(uname -m)"
  case "\$ARCH" in x86_64) PKG_ARCH=x64;; aarch64|arm64) PKG_ARCH=arm64;; *) echo "Unsupported architecture: \$ARCH" >&2; exit 1;; esac
  curl -fL --retry 3 -o actions-runner.tar.gz "https://github.com/actions/runner/releases/download/v\${RUNNER_VERSION}/actions-runner-linux-\${PKG_ARCH}-\${RUNNER_VERSION}.tar.gz"
  tar xzf actions-runner.tar.gz
  rm -f actions-runner.tar.gz
  ./config.sh --unattended --url "\$GITHUB_RUNNER_URL" --token "\$GITHUB_RUNNER_TOKEN" --name "\$RUNNER_NAME" --labels "lingo-g02" --work "_work"
fi
if [ ! -f .service ]; then sudo ./svc.sh install; fi
sudo ./svc.sh stop || true
sudo ./svc.sh start
sudo ./svc.sh status
echo "=== RUNNER PROCESS ==="
pgrep -af 'Runner.Listener|runsvc.sh|run.sh' || true
echo "=== RUNNER TELEMETRY ==="
tail -n 100 _diag/Runner_*.log 2>/dev/null || true
tail -n 100 _diag/Worker_*.log 2>/dev/null || true
