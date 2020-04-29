#!/bin/bash -e

FAISS_VERSION=${1:-v1.6.1}

docker-compose build --build-arg FAISS_VERSION=$FAISS_VERSION
docker run --rm -v $PWD:/host faissmask_test bash -c 'cp /src/FaissMask/runtimes/linux-x64/native/* /host/FaissMask/runtimes/linux-x64/native/'