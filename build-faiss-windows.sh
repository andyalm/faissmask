#!/bin/bash
# Execute in git bash
# Please make sure you installed intel mkl (https://www.intel.com/content/www/us/en/developer/tools/oneapi/onemkl-download.html?operatingsystem=windows&windows-install=offline)
# and cmake (https://cmake.org/download/)
BRANCH=main
GITHUB_ACCOUNT=facebookresearch
MKL_ROOT="C:\Program Files (x86)\Intel\oneAPI\mkl\latest"

git clone --depth=1 --recursive --branch $BRANCH https://github.com/$GITHUB_ACCOUNT/faiss.git
cd faiss
cmake -Wno-dev -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DFAISS_ENABLE_C_API=ON -DBUILD_TESTING=OFF -DBUILD_SHARED_LIBS=ON -DCMAKE_WINDOWS_EXPORT_ALL_SYMBOLS=ON -DBLA_VENDOR=Intel10_64_dyn "-DMKL_LIBRARIES=$MKL_ROOT/lib/mkl_intel_lp64_dll.lib;$MKL_ROOT/lib/mkl_intel_thread_dll.lib;$MKL_ROOT/lib/mkl_core_dll.lib" -B build -S .
cmake --build build --config Release --target faiss faiss_c