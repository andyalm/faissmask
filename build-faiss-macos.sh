#!/bin/bash

BRANCH="v1.6.1"

brew install libomp
git clone --recursive --branch $BRANCH https://github.com/facebookresearch/faiss.git libfaiss-src
cd libfaiss-src
./configure --without-cuda ac_cv_prog_cxx_openmp="-Xpreprocessor -fopenmp" LIBS=-lomp
make
cd c_api
sed -i '' 's/--whole-archive/-all_load/g' Makefile
sed -i '' 's/--no-whole-archive/-noall_load/g' Makefile
make
cp libfaiss_c.dylib ../../FaissMask/runtimes/osx-x64/native/
cd ../..