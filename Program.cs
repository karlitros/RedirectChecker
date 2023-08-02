internal class Program
{
    /// <summary>
    /// Fires requests to the 301 original urls, sees if it returns a 404 or not. If a 404, print it out.
    /// </summary>
    /// <param name="args">args[0] is the rewritemaps.xml filename: eg C:\\Development\\website\\config\\IISUrlRewriteMaps.xml. args[1] is the root url: eg https://mywebsite.co.uk</param>
    private static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            Console.WriteLine("Provide at least 2 args. Rewrite maps xml file, and root url to fire requests against");
            return;
        }

        var checker = new RedirectChecker.RedirectChecker(args[0], args[1]);

        checker.ProcessRepositories();

        Console.ReadLine();
    }
}