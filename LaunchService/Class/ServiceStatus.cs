using System;

namespace LaunchService
{
    public struct ServiceStatus
    {
        public long dwServiceType;

        public ServiceState dwCurrentState;

        public long dwControlsAccepted;

        public long dwWin32ExitCode;

        public long dwServiceSpecificExitCode;

        public long dwCheckPoint;

        public long dwWaitHint;
    }
}
