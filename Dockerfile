FROM debian:buster as build

ARG FAISS_VERSION=v1.7.1

RUN apt-get -y update && \
    apt-get -y install wget gnupg2 libgomp1 software-properties-common

# install intel-mkl
RUN cd /tmp && \
    wget https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB && \
    apt-key add GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB && \
    rm GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB && \
    cd / && \
    echo deb  https://apt.repos.intel.com/mkl all main > /etc/apt/sources.list.d/intel-mkl.list && \
    apt-get -y update && \
    apt-get -y install intel-mkl-64bit-2020.0-08
ENV LD_LIBRARY_PATH /opt/intel/mkl/lib/intel64:$LD_LIBRARY_PATH
ENV LIBRARY_PATH /opt/intel/mkl/lib/intel64:$LIBRARY_PATH
ENV LD_PRELOAD /usr/lib/x86_64-linux-gnu/libgomp.so.1:/opt/intel/mkl/lib/intel64/libmkl_def.so:\
/opt/intel/mkl/lib/intel64/libmkl_avx2.so:/opt/intel/mkl/lib/intel64/libmkl_core.so:\
/opt/intel/mkl/lib/intel64/libmkl_intel_lp64.so:/opt/intel/mkl/lib/intel64/libmkl_gnu_thread.so

# install latest cmake
RUN wget -O - https://apt.kitware.com/keys/kitware-archive-latest.asc 2>/dev/null | gpg --dearmor - | tee /etc/apt/trusted.gpg.d/kitware.gpg >/dev/null && \
    apt-add-repository 'deb https://apt.kitware.com/ubuntu/ bionic main' && \
    apt update && apt install -y cmake

# install gcc-c++ make
RUN apt-get -y install build-essential

# build faiss and the c api
RUN apt-get -y install git && \
    git clone -b ${FAISS_VERSION} https://github.com/facebookresearch/faiss.git /faiss && \
    cd /faiss && \
    cmake -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DCMAKE_BUILD_TYPE=Release -DFAISS_ENABLE_C_API=ON -DBUILD_SHARED_LIBS=ON -DBLA_VENDOR=Intel10_64_dyn -DBUILD_TESTING=ON -DFAISS_OPT_LEVEL=avx2 -B build . && \
    make -C build -j $(nproc) faiss && \
    make -j $(nproc) -C build install && \
    cd /

FROM mcr.microsoft.com/dotnet/core/sdk:3.1

EXPOSE 80

ADD . /src
WORKDIR /src

COPY --from=build /usr/lib/x86_64-linux-gnu/libgomp.so.1 /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_def.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_avx2.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_core.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_intel_lp64.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_sequential.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_gnu_thread.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/mkl/lib/intel64/libmkl_rt.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /faiss/build/c_api/libfaiss_c.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /faiss/build/faiss/libfaiss.so /src/FaissMask/runtimes/linux-x64/native/

ENV LD_LIBRARY_PATH=/src/FaissMask/runtimes/linux-x64/native/

CMD ["dotnet", "test", "FaissMask.Test"]