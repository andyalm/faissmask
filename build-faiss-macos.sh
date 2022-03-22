#!/bin/bash

BRANCH="v1.7.2"

brew install libomp
brew install cmake
git clone --recursive --branch $BRANCH https://github.com/facebookresearch/faiss.git libfaiss-src
cd libfaiss-src
cmake -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DBUILD_TESTING=OFF -DCMAKE_BUILD_TYPE=Release -DFAISS_ENABLE_C_API=ON -DBUILD_SHARED_LIBS=ON -B build .
make -C build -j faiss faiss_avx2
make -C build install
cp build/c_api/libfaiss_c.dylib ../FaissMask/runtimes/osx-x64/native/
cd ..