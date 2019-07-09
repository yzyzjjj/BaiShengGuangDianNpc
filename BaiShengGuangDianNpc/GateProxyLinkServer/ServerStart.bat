@echo off
cd %cd%
tasklist /v %cd%
@start "%cd%" cmd /k "dotnet GateProxyLinkServer.dll"
