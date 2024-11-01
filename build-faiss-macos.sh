#!/bin/bash

BRANCH="main"
GITHUB_ACCOUNT="facebookresearch"

brew install libomp
brew install cmake
git clone --depth=1 --recursive --branch ${BRANCH} https://github.com/${GITHUB_ACCOUNT}/faiss.git libfaiss-src
cd libfaiss-src
cmake -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DOpenMP_libomp_LIBRARY="/opt/homebrew/opt/libomp/lib/libomp.dylib" -DOpenMP_CXX_FLAGS="-Xpreprocessor -fopenmp /opt/homebrew/opt/libomp/lib/libomp.dylib -I /opt/homebrew/opt/libomp/include" -DOpenMP_CXX_LIB_NAMES="libomp" -DBUILD_TESTING=OFF -DCMAKE_BUILD_TYPE=Release -DFAISS_ENABLE_C_API=ON -DBUILD_SHARED_LIBS=ON -B build .
make -C build -j faiss
sudo make -C build install
install_name_tool -rpath "${PWD}"/build/faiss @loader_path ./build/c_api/libfaiss_c.dylib

arch=arm64
if [[ $(uname -m) == 'x86_64' ]]; then
  arch=x64
fi

cp build/c_api/libfaiss_c.dylib ../FaissMask/runtimes/osx-$arch/native/
cp build/faiss/libfaiss.dylib ../FaissMask/runtimes/osx-$arch/native/
cd ..