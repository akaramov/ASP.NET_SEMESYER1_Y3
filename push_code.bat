@echo off
git add .
git commit -m "Daily automatic backup"
git push
echo Done! Pushed to remote.
pause