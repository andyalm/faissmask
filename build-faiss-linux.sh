#!/bin/bash -e

FAISS_VERSION=${1:-vector_transform_c_api}
GITHUB_ACCOUNT=${2:-makosten}
arch=amd64
rm -f FaissMask/runtimes/linux-x64/native/*
docker-compose build --build-arg arch=$arch --build-arg FAISS_VERSION=$FAISS_VERSION --build-arg GITHUB_ACCOUNT=$GITHUB_ACCOUNT
docker run --platform=linux/${arch} --rm -v $PWD:/host faissmask-test bash -c 'cp /src/FaissMask/runtimes/linux-x64/native/* /host/FaissMask/runtimes/linux-x64/native/'