#!/bin/bash

BRANCH="vector_transform_c_api"
GITHUB_ACCOUNT="makosten"

brew install libomp
brew install cmake
git clone --recursive --branch ${BRANCH} https://github.com/${GITHUB_ACCOUNT}/faiss.git libfaiss-src
cd libfaiss-src
cmake -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DOpenMP_libomp_LIBRARY="/opt/homebrew/opt/libomp/lib/libomp.dylib" -DBUILD_TESTING=OFF -DCMAKE_BUILD_TYPE=Release -DFAISS_ENABLE_C_API=ON -DBUILD_SHARED_LIBS=ON -B build .
make -C build -j faiss
sudo make -C build install

arch=arm64
if [[ $(uname -m) == 'x86_64' ]]; then
  arch=x64
fi

cp build/c_api/libfaiss_c.dylib ../FaissMask/runtimes/osx-$arch/native/
cp build/faiss/libfaiss.dylib ../FaissMask/runtimes/osx-$arch/native/
cd ..