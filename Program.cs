using Maybe.BloomFilter;

const int MaxItems = 1000000;
const double AcceptableErrorRate = 0.01;

var filter = new BloomFilter<Guid>(MaxItems, AcceptableErrorRate);
var clashes = TestFilter(MaxItems, filter);

Console.WriteLine($"MaxItems            = {MaxItems}");
Console.WriteLine($"AcceptableErrorRate = {AcceptableErrorRate}");
Console.WriteLine($"Clashes [{clashes.Count}]:");
foreach (var clash in clashes)
{
  Console.WriteLine($"  {clash}");
}

List<int> TestFilter(int maxItems, BloomFilter<Guid> bloomFilter)
{
  var retval = new List<int>();
  foreach (var i in Enumerable.Range(0, maxItems))
  {
    if (bloomFilter.AddAndCheck(Guid.NewGuid()))
    {
      retval.Add(i);
    }
  }

  return retval;
}
