@echo off

echo Docker login...
docker login

echo Building docker img...
docker build -t ramp_rage .

echo Tagging docker img...
docker tag ramp_rage:latest zvezdun/ramp_rage:latest

echo Pushing docker img to docker hub...
docker push zvezdun/ramp_rage:latest

echo Done. Successfully pushed!
pause