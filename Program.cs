namespace InBloom;

using System.Diagnostics;
using Maybe.BloomFilter;
using CommandLine;

public static class Program
{
  public static void Main(string[] args)
  {
    Parser.Default.ParseArguments<Options>(args)
      .WithParsed<Options>(opt =>
      {
        var numItems = opt.NumItems;
        var acceptableErrorRate = opt.AcceptableErrorRate;

        var proc = Process.GetCurrentProcess();

        var clashes = GetClashes(numItems, acceptableErrorRate);

        proc.Refresh();
        var maxMem = proc.PeakWorkingSet64;

        Console.WriteLine($"NumItems            = {numItems}");
        Console.WriteLine($"AcceptableErrorRate = {acceptableErrorRate * 100} %");
        Console.WriteLine($"MaxErrors           = {numItems * acceptableErrorRate}");
        Console.WriteLine($"PeakWorkingSet64    = {maxMem / (1024 * 1024)} MB");
        Console.WriteLine($"Clashes [{clashes.Count}]:");
        foreach (var clash in clashes)
        {
          Console.WriteLine($"  {clash}");
        }
      });
  }

  private static List<int> GetClashes(int numItems, double acceptableErrorRate)
  {
    var filter = new BloomFilter<Guid>(numItems, acceptableErrorRate);
    var retval = new List<int>();
    foreach (var i in Enumerable.Range(0, numItems))
    {
      if (filter.AddAndCheck(Guid.NewGuid()))
      {
        retval.Add(i);
      }
    }

    return retval;
  }

  public class Options
  {
    [Option('n', "numItems", Required = false, Default = 1000000, HelpText = "Number of items in data")]
    public int NumItems { get; set; }

    [Option('e', "errorRate", Required = false, Default = 0.01, HelpText = "Acceptable error rate (0 .. 1)")]
    public double AcceptableErrorRate { get; set; }
  }
}
