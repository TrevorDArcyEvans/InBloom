# Bloom Filters in .NET
A [bloom filter](https://en.wikipedia.org/wiki/Bloom_filter) is like a fuzzy
[hash set](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?redirectedfrom=MSDN&view=net-6.0)
but with the interesting characteristic that an item:
* **MIGHT** be in the set; or
* is definitely **NOT** in the set

This trades off correctness for compactness.  Bloom filters have found application in 'big data' where
it is impractical to hold all data in memory ie correctness.

A popular C# implementation is in [Maybe.NET](https://github.com/rmc00/Maybe)

## Results
Typical output from the sample code:

<details>

```bash
$ ./InBloom 
NumItems            = 1000000
AcceptableErrorRate = 1 %
MaxErrors           = 10000
PeakWorkingSet64    = 38 MB
Possible clashes [30]:
  774097
  782269
  806511
  813267
  841164
  844489
  871685
  873590
  889297
  892098
  919877
  929498
  934331
  951094
  951606
  952204
  956383
  957373
  969998
  975934
  977112
  978860
  979326
  983182
  984108
  989299
  991199
  995712
  996740
  997580
```

</details>

## Analysis
The implementation is error free until about 75-80% through the data.  After that, the rate of errors
increases, probably due storage filling up.

Note that the implementation maintains the error rate below the requested 1%.

On *Debian Linux*, there seems to be an overhead of ~30 MB to run the program but
this does not increase appreciably with the number of data items.

## Conclusions

The selected data type is a *GUID* which is guaranteed to be unique.  More realistic data would
probably result in the onset of errors ocurring earlier.

## Further Work
* test scaling Bloom filter
* try other implementations
  * [VDS.Common](https://github.com/dotnetrdf/vds-common)
  * [BloomFilter.NET](https://github.com/xJonathanLEI/BloomFilter.NET)
  * [BloomFilter.cs](https://gist.github.com/richardkundl/8300092)

