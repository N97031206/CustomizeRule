%SYSTEMROOT%\system32\xcopy %4%5.* %1..\Output\ciMesSolution\Current\%6\bin\ /Y/Q/E
%SYSTEMROOT%\system32\xcopy %2*.ascx %1..\Output\ciMesSolution\Current\%6\%7\%3\ /Y/Q/S
%SYSTEMROOT%\system32\xcopy %4%5.* %1..\Output\ciMesSolution\%date:~0,4%%date:~5,2%%date:~8,2%\%6\bin\ /Y/Q/E

%SYSTEMROOT%\system32\xcopy %4%5.* %1..\Reference\ /Y/Q/E