# G02 Evidence Contract

A G02 PASS requires all predicates below to be observed from live execution evidence:

- runner_id > 0
- runner_name populated
- job assigned to a real runner
- steps instantiated
- checkout executes
- sentinel executes
- sentinel exit_code = 0
- workflow/job logs retrievable
- evidence artifact retrievable
- evidence independently verifiable

Unity promotion additionally requires:
- Unity 2022.3.50f1 batchmode execution
- BuildScript.PerformBuild invocation
- successful StandaloneLinux64 build
- .x86_64 artifact
- artifact retrieval
- SHA-256 reproduction

No workflow definition, queued job, configuration, or AppDeploy readiness may substitute for these observations.
