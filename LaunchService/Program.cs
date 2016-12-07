 using System;
using System.ServiceProcess;

namespace LaunchService
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            ServiceBase.Run(new ServiceBase[]
            {
                new SpoloxLauncher(args)
            });
        }
    }
}
