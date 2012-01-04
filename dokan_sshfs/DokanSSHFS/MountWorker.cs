using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dokan;
using System.Diagnostics;

namespace DokanSSHFS
{
    public class MountWorker
    {
        private DokanOperations sshfs_;
        private DokanOptions opt_;

        public MountWorker(DokanOperations sshfs, DokanOptions opt)
        {
            sshfs_ = sshfs;
            opt_ = opt;
        }

        public void Start()
        {
            //System.IO.Directory.SetCurrentDirectory(Application.StartupPath);
            int ret = DokanNet.DokanMain(opt_, sshfs_);
            if (ret < 0)
            {
                string msg = "Dokan Error";
                switch (ret)
                {
                    case DokanNet.DOKAN_ERROR:
                        msg = "Dokan Error";
                        break;
                    case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                        msg = "Dokan drive letter error" + opt_.MountPoint;
                        break;
                    case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                        msg = "Dokan driver install error";
                        break;
                    case DokanNet.DOKAN_MOUNT_ERROR:
                        msg = "Dokan drive letter assign error";
                        break;
                    case DokanNet.DOKAN_START_ERROR:
                        msg = "Dokan driver error ,please reboot";
                        break;
                }
                //MessageBox.Show(msg, "Error");
                //Application.Exit();
            }
            Debug.WriteLine("DokanNet.Main end");
        }
    }
}
