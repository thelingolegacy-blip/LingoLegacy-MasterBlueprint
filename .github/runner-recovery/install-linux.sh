#!/usr/bin/env bash
set -euo pipefail

: "${GITHUB_RUNNER_URL:?Set GITHUB_RUNNER_URL, e.g. https://github.com/thelingolegacy-blip/LingoLegacy-MasterBlueprint}"
: "${GITHUB_RUNNER_TOKEN:?Set a fresh GitHub Actions runner registration token}"

RUNNER_DIR="${RUNNER_DIR:-$HOME/actions-runner}"
RUNNER_VERSION="${RUNNER_VERSION:-2.329.0}"

mkdir -p "$RUNNER_DIR"
cd "$RUNNER_DIR"

ARCH="$(uname -m)"
case "$ARCH" in
  x86_64) PKG_ARCH="x64" ;;
  aarch64|arm64) PKG_ARCH="arm64" ;;
  *) echo "Unsupported architecture: $ARCH" >&2; exit 1 ;;
esac

curl -fL -o actions-runner.tar.gz "https://github.com/actions/runner/releases/download/v${RUNNER_VERSION}/actions-runner-linux-${PKG_ARCH}-${RUNNER_VERSION}.tar.gz"
tar xzf actions-runner.tar.gz
rm -f actions-runner.tar.gz

./config.sh --unattended \
  --url "$GITHUB_RUNNER_URL" \
  --token "$GITHUB_RUNNER_TOKEN" \
  --name "lingo-legacy-g02" \
  --labels "lingo-g02" \
  --work "_work"

sudo ./svc.sh install
sudo ./svc.sh start
sudo ./svc.sh status

echo "Runner configured: lingo-legacy-g02"
echo "Required custom label: lingo-g02"
