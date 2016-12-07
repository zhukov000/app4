using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace LaunchService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private IContainer components;

        private ServiceProcessInstaller serviceProcessInstaller1;

        private ServiceInstaller serviceInstaller1;

        public ProjectInstaller()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new ServiceProcessInstaller();
            this.serviceInstaller1 = new ServiceInstaller();
            this.serviceProcessInstaller1.Account = ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            this.serviceInstaller1.Description = "Служба автозапуска сполох";
            this.serviceInstaller1.DisplayName = "Служба запуска Сполох";
            this.serviceInstaller1.ServiceName = "SpoloxLauncher";
            this.serviceInstaller1.StartType = ServiceStartMode.Automatic;
            base.Installers.AddRange(new Installer[]
            {
                this.serviceProcessInstaller1,
                this.serviceInstaller1
            });
        }
    }
}
