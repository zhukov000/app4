using System;

namespace LaunchService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 1,
        SERVICE_START_PENDING,
        SERVICE_STOP_PENDING,
        SERVICE_RUNNING,
        SERVICE_CONTINUE_PENDING,
        SERVICE_PAUSE_PENDING,
        SERVICE_PAUSED
    }
}
