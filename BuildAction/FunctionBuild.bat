%SYSTEMROOT%\system32\xcopy %4%5.* %1..\Output\ciMesSolution\Current\%6\bin\ /Y/Q/E
%SYSTEMROOT%\system32\xcopy %2*.aspx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.ascx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2UserControl\*.ascx %1..\Output\ciMesSolution\Current\%6\%7\%3\UserControl\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2Web.UserControl\*.ascx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.asmx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.xml %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S /EXCLUDE:%1ciMesPortal\ExcludeFile.txt
%SYSTEMROOT%\system32\xcopy %2*.xsd %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.htm %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.html %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.ashx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.rpt %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.js %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.css %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.xlsx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %4%5.* %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\bin\ /Y/Q/E
%SYSTEMROOT%\system32\xcopy %2*.aspx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2UserControl\*.ascx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\ciMesSolution\%7\%3\UserControl\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2Web.UserControl\*.ascx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\ciMesSolution\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.ascx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.asmx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.xml %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S /EXCLUDE:%1ciMesPortal\ExcludeFile.txt
%SYSTEMROOT%\system32\xcopy %2*.xsd %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.htm %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.html %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.ashx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.rpt %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %2*.xlsx %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\%7\%3\ /Y/Q/S

%SYSTEMROOT%\system32\attrib %2bin\AjaxControlToolkit.dll +R > NUL
%SYSTEMROOT%\system32\attrib %2bin\Telerik.Web.UI.dll +R > NUL
%SYSTEMROOT%\system32\attrib %4%5.dll +R > NUL

%SYSTEMROOT%\system32\del  %2bin /S/Q
%SYSTEMROOT%\system32\del  %2obj /S/Q

%SYSTEMROOT%\system32\attrib %4%5.dll -R > NUL
%SYSTEMROOT%\system32\attrib %2bin\AjaxControlToolkit.dll -R > NUL
%SYSTEMROOT%\system32\attrib %2bin\Telerik.Web.UI.dll -R > NUL