using Maybe.BloomFilter;

const int MaxItems = 1000000;
const double AcceptableErrorRate = 0.01;

var clashes = GetClashes(MaxItems, AcceptableErrorRate);

Console.WriteLine($"MaxItems            = {MaxItems}");
Console.WriteLine($"AcceptableErrorRate = {AcceptableErrorRate}");
Console.WriteLine($"MaxErrors           = {MaxItems * AcceptableErrorRate}");
Console.WriteLine($"Clashes [{clashes.Count}]:");
foreach (var clash in clashes)
{
  Console.WriteLine($"  {clash}");
}

List<int> GetClashes(int maxItems, double acceptableErrorRate)
{
  var filter = new BloomFilter<Guid>(maxItems, acceptableErrorRate);
  var retval = new List<int>();
  foreach (var i in Enumerable.Range(0, maxItems))
  {
    if (filter.AddAndCheck(Guid.NewGuid()))
    {
      retval.Add(i);
    }
  }

  return retval;
}
