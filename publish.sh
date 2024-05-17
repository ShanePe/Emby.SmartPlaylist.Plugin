#!/bin/bash
version=2.5.1.4850
cd frontend
yarn install
yarn build
cd ../backend
rm -rv SmartPlaylist/bin/Release
dotnet publish --configuration Release
cd SmartPlaylist/bin/Release/netstandard2.0/publish
zip SmartPlaylist-$version.zip SmartPlaylist.dll
rm -v ../../../../../../Release/SmartPlaylist-$version.zip
cp SmartPlaylist-$version.zip ../../../../../../Release/
ls -lh .