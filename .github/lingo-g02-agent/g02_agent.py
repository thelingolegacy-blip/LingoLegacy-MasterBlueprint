#!/usr/bin/env python3
import json, os, platform, subprocess, urllib.request

def run(cmd):
    p=subprocess.run(cmd, shell=True, text=True, capture_output=True)
    return {"command":cmd,"returncode":p.returncode,"stdout":p.stdout[-12000:],"stderr":p.stderr[-12000:]}

def gh(path):
    req=urllib.request.Request(
        "https://api.github.com"+path,
        headers={"Authorization":"Bearer "+os.environ["GITHUB_TOKEN"],"Accept":"application/vnd.github+json","X-GitHub-Api-Version":"2022-11-28"},
    )
    with urllib.request.urlopen(req, timeout=20) as r:
        return json.load(r)

repo=os.environ["GITHUB_REPOSITORY"]
run_id=os.environ.get("GITHUB_RUN_ID","")
job_name=os.environ.get("GITHUB_JOB","")
runner_name=os.environ.get("RUNNER_NAME","")
runner_id=os.environ.get("RUNNER_ID","")
os_name=platform.system()

checks=gh(f"/repos/{repo}/commits/{os.environ['GITHUB_SHA']}/check-runs")
jobs=gh(f"/repos/{repo}/actions/runs/{run_id}/jobs") if run_id else {"jobs":[]}

if os_name=="Windows":
    host=[run("Get-Service | Where-Object {$_.Name -like '*actions.runner*'} | Format-List Name,Status,StartType"),
          run(".\svc.cmd status"),
          run("Get-Process -Name Runner.Listener -ErrorAction SilentlyContinue | Select-Object Id,Path,StartTime"),
          run("Get-Content .\_diag\Runner_*.log -Tail 80 -ErrorAction SilentlyContinue")]
else:
    host=[run("sudo ./svc.sh status"),
          run("systemctl status 'actions.runner.*' --no-pager"),
          run("pgrep -af 'Runner.Listener|runsvc.sh|run.sh'"),
          run("tail -n 80 _diag/Runner_*.log 2>/dev/null || true")]

payload={
 "G02_AGENT":"executed",
 "OS":os_name,
 "RUNNER_NAME":runner_name,
 "RUNNER_ID":runner_id,
 "GITHUB_RUN_ID":run_id,
 "GITHUB_JOB":job_name,
 "HOST_CHECKS":host,
 "CHECK_RUNS":[{"name":x.get("name"),"status":x.get("status"),"conclusion":x.get("conclusion"),"id":x.get("id")} for x in checks.get("check_runs",[])],
 "JOBS":[{"name":x.get("name"),"status":x.get("status"),"runner_name":x.get("runner_name"),"runner_id":x.get("runner_id"),"steps":x.get("steps")} for x in jobs.get("jobs",[])],
}
print("G02_AGENT_EVIDENCE="+json.dumps(payload,separators=(",",":")))
