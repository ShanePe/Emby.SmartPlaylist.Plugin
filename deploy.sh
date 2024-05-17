#!/bin/bash
echo "====== Building ======"
./build.sh
echo "====== Stopping Service ======"
sudo service emby-server stop
echo "====== Copying ======"
sudo cp -v backend/SmartPlaylist/bin/Release/netstandard2.0/SmartPlaylist.dll /var/lib/emby/plugins/SmartPlaylist.dll
echo "====== Starting Service ======"
sudo service emby-server start
echo "====== !!DONE!! ======"