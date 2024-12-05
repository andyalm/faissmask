ARG ARCH=amd64
ARG INTEL_MKL_VERSION=2025.0

FROM --platform=linux/${ARCH} debian:bullseye AS build
ENV DEBIAN_FRONTEND=noninteractive

ARG FAISS_VERSION=main
ARG GITHUB_ACCOUNT=facebookresearch
ARG INTEL_MKL_VERSION

RUN apt-get -y update && \
    apt-get -y install apt-utils gpg-agent wget gnupg2 libgomp1 software-properties-common build-essential git

# install intel-mkl
RUN cd /tmp && \
    wget -O- https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS.PUB  \
    | gpg --dearmor  \
    | tee /usr/share/keyrings/oneapi-archive-keyring.gpg > /dev/null && \
    echo "deb [signed-by=/usr/share/keyrings/oneapi-archive-keyring.gpg] https://apt.repos.intel.com/oneapi all main"  \
    | tee /etc/apt/sources.list.d/oneAPI.list && \
    apt-get -y update && \
    apt-get -y install intel-oneapi-mkl-devel-${INTEL_MKL_VERSION}

ENV MKL_ROOT=/opt/intel/oneapi/mkl/$INTEL_MKL_VERSION
ENV LD_LIBRARY_PATH=$MKL_ROOT/lib:$LD_LIBRARY_PATH

# install latest cmake
RUN wget -O - https://apt.kitware.com/keys/kitware-archive-latest.asc 2>/dev/null | gpg --dearmor - | tee /etc/apt/trusted.gpg.d/kitware.gpg >/dev/null && \
    apt-add-repository 'deb https://apt.kitware.com/ubuntu/ bionic main' && \
    apt update && apt install -y cmake

# build faiss and the c api
RUN git clone -b ${FAISS_VERSION} https://github.com/${GITHUB_ACCOUNT}/faiss.git /faiss && \
    cd /faiss && \
    sed -i 's/faiss_c PRIVATE faiss/faiss_c PRIVATE faiss_avx2/g' c_api/CMakeLists.txt && \
    cmake -DFAISS_ENABLE_GPU=OFF -DFAISS_ENABLE_PYTHON=OFF -DBUILD_TESTING=OFF -DCMAKE_BUILD_TYPE=Release -DFAISS_ENABLE_C_API=ON -DBUILD_SHARED_LIBS=ON -DFAISS_OPT_LEVEL=avx2 -B build . && \
    make -C build -j $(nproc) faiss_avx2 install

## update the rpath for the faiss libraries
RUN apt-get -y install patchelf && \
    patchelf --set-rpath '$ORIGIN' /faiss/build/c_api/libfaiss_c.so && \
    patchelf --set-rpath '$ORIGIN' /faiss/build/faiss/libfaiss_avx2.so

FROM --platform=linux/${ARCH} mcr.microsoft.com/dotnet/sdk:9.0
ARG INTEL_MKL_VERSION

ADD . /src
WORKDIR /src

RUN rm -f /src/FaissMask/runtimes/linux-x64/native/*
COPY --from=build /usr/lib/x86_64-linux-gnu/libgomp.so.1 /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_def.so.2 /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_avx2.so.2 /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_core.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_intel_lp64.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_sequential.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /opt/intel/oneapi/mkl/$INTEL_MKL_VERSION/lib/libmkl_gnu_thread.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /faiss/build/c_api/libfaiss_c.so /src/FaissMask/runtimes/linux-x64/native/
COPY --from=build /faiss/build/faiss/libfaiss_avx2.so /src/FaissMask/runtimes/linux-x64/native/
RUN echo "/src/FaissMask/runtimes/linux-x64/native/" > /etc/ld.so.conf.d/faissmask.conf && ldconfig

EXPOSE 80

CMD ["dotnet", "test", "FaissMask.Test"]