@echo off
cd /D "%~dp0"
cd ..

call astyle --style=allman --indent=spaces=4 --indent-namespaces --suffix=none --lineend=linux --recursive Assets/Scripts/*.cs
