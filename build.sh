#!/bin/bash

cd frontend
yarn install
yarn build
cd ../backend
dotnet build --configuration Release
cd ..