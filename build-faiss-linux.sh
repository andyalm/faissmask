#!/bin/bash -e

FAISS_VERSION=${1:-v1.7.2}
rm -f FaissMask/runtimes/linux-x64/native/*
docker-compose build --build-arg FAISS_VERSION=$FAISS_VERSION
docker run --rm -v $PWD:/host faissmask_test bash -c 'cp /src/FaissMask/runtimes/linux-x64/native/* /host/FaissMask/runtimes/linux-x64/native/'