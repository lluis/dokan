
I made dokan_sshfs to work as a Windows service, so dokan unit continue mounted when user logouts.

DokanSSHFS.exe can be called with --installservice to install the service, or without args to edit configuration and start/stop service.

There's also an installer to install service and the executable automatilcally.

Files
-----

::

  dokan_sshfs/DokanSSHFS/bin/x86/Release/ -- compiled DokanSSHFS.exe and libs
  dokan_sshfs/DokanSSHFSInstaller/Output/ -- installer


I disabled some controls, since config file don't stores password nor passphrase, service can't get it from config file, so it only allows authentication with empty passphrase keys.

Also disabled the multiple configurations, when you click start, current configuration is saved automatically.


* I never made windows software before, nor any c#, so use it under your responsability :)
