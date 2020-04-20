FROM debian:buster as build

ENV FAISS_VERSION="v1.6.1"

RUN apt-get -y update && \
    apt-get -y install wget gnupg2 libgomp1

# install intel-mkl
RUN cd /tmp && \
    wget https://apt.repos.intel.com/intel-gpg-keys/GPG-PUB-KEY-INTEL-SW-PRODUCTS-2019.PUB && \
    apt-key add GPG-PUB-KEY-INTEL-SW-PRODUCTS-2019.PUB && \
    rm GPG-PUB-KEY-INTEL-SW-PRODUCTS-2019.PUB && \
    cd / && \
    echo deb  https://apt.repos.intel.com/mkl all main > /etc/apt/sources.list.d/intel-mkl.list && \
    apt-get -y update && \
    apt-get -y install intel-mkl-2019.3-062
ENV LD_LIBRARY_PATH /opt/intel/mkl/lib/intel64:$LD_LIBRARY_PATH
ENV LIBRARY_PATH /opt/intel/mkl/lib/intel64:$LIBRARY_PATH
ENV LD_PRELOAD /usr/lib/x86_64-linux-gnu/libgomp.so.1:/opt/intel/mkl/lib/intel64/libmkl_def.so:\
/opt/intel/mkl/lib/intel64/libmkl_avx2.so:/opt/intel/mkl/lib/intel64/libmkl_core.so:\
/opt/intel/mkl/lib/intel64/libmkl_intel_lp64.so:/opt/intel/mkl/lib/intel64/libmkl_gnu_thread.so

# install gcc-c++ make swig3
RUN apt-get -y install build-essential swig3.0

# install python dev env
RUN apt-get -y install python-dev python3-numpy

# CXXFLAGS taken from makefile.inc, but removing the -g debug flag
ENV CXXFLAGS "-fPIC -m64 -Wno-sign-compare -O3 -Wall -Wextra"
RUN apt-get -y install git && \
    git clone -b ${FAISS_VERSION} https://github.com/facebookresearch/faiss.git /faiss && \
    cd /faiss && \
    ./configure --without-cuda && \
    make -j $(nproc) && \
    make install && \
    cd c_api && \
    make && \
    cd /

FROM mcr.microsoft.com/dotnet/core/sdk:3.0

EXPOSE 80

ADD . /src
WORKDIR /src/FaissSharp.Test

COPY --from=build /opt/intel/mkl/lib/intel64 /opt/intel/mkl/lib/intel64
COPY --from=build /faiss/c_api/libfaiss_c.so /src/FaissSharp/runtimes/linux/native/
ENV LD_LIBRARY_PATH="/opt/intel/mkl/lib/intel64:$LD_LIBRARY_PATH"
RUN apt-get -y update && \
    apt-get -y install libgomp1

CMD ["dotnet", "test"]
#CMD ["ldd", "/src/FaissSharp/runtimes/linux/native/libfaiss_c.so"]