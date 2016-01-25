# Webdashboard #
http://richardsbraindump.blogspot.com/2007/06/real-world-cruisecontrolnet-setup.html

# Trouble shooting CruiseControl.NET #

**PROBLEM**:
> In attempting to access http://localhost/ccnet/, the CC.NET web dashboard page does not display.
**SOLUTION**
> Ensure that ASP.NET has been registered. It is usually located at C:\WINDOWS\Microsoft.NET\Framework\v1.1.4322 (or something similar) From here, run aspnet\_regiis.exe /i. Also, ensure that the CruiseControl.NET service is installed and running by viewing the Services applet. Finally, ensure that you have installed and configured IIS (you may need to add a virtual directory for CC.NET if you install IIS after installing CC.NET.

http://www.testearly.com/2006/05/01/integrating-ccnet-with-nant-and-subversion-on-windows/