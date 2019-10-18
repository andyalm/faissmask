# FaissSharp

FaissSharp is a package that wrap the c_api of [faiss](https://github.com/facebookresearch/faiss)

Faiss is a library for efficient similarity search and clustering of dense vectors. It contains algorithms that search in sets of vectors of any size, up to ones that possibly do not fit in RAM. It also contains supporting code for evaluation and parameter tuning. Faiss is written in C++ with complete wrappers for Python/numpy. Some of the most useful algorithms are implemented on the GPU. It is developed by [Facebook AI Research](https://research.fb.com/category/facebook-ai-research-fair/).

This version is based on faiss v1.6.0

## Try it

The `example` directory contains a demo console project.

```
$ cd example
$ dotnet run

Generating some data...
Building index
IsTrained True
Elements in index: 100000
Searching...
ID: 895fe64547e44b448779aca0dccb5964
    0 (d=.000)  97976 (d=12.460)  76821 (d=12.654)  53839 (d=12.791)  42160 (d=12.794)  
ID: d32b6996614e4c3ca251dcbd12cd3a23
    1 (d=.000)  33161 (d=13.036)  52147 (d=13.374)  58457 (d=13.425)  10414 (d=13.490)  
ID: 52ecc960ef5345ac85e9bcdd62fe4e04
    2 (d=.000)  19069 (d=12.507)   1136 (d=12.855)  98663 (d=13.401)  20122 (d=13.403)  
ID: ea32dcba2a35444fb46512f6c9794f41
    3 (d=.000)  35978 (d=12.922)  16917 (d=13.269)  31983 (d=13.439)  99038 (d=13.456)  
ID: d2ae0251fc4a46a2978ad77166808b4d
    4 (d=.000)  39897 (d=12.661)  85654 (d=12.798)  74798 (d=12.930)  41397 (d=13.207) 
```

## TODO
* Tests
* Sync native functions with faiss c_api
* Nuget Package

> This is an active project, but MR are welcome.

