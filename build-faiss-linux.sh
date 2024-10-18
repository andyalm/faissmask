#!/bin/bash -e

FAISS_VERSION=${1:-vector_transform_c_api}
GITHUB_ACCOUNT=${2:-makosten}
rm -f FaissMask/runtimes/linux-x64/native/*
docker-compose build --build-arg FAISS_VERSION=$FAISS_VERSION GITHUB_ACCOUNT=$GITHUB_ACCOUNT
docker run --rm -v $PWD:/host faissmask_test bash -c 'cp /src/FaissMask/runtimes/linux-x64/native/* /host/FaissMask/runtimes/linux-x64/native/'