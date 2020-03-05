FROM centos:7

RUN rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm && \
    yum install -y dotnet-sdk-3.0

EXPOSE 80

ADD . /src
WORKDIR /src/FaissSharp.Test

CMD ["dotnet", "test"]