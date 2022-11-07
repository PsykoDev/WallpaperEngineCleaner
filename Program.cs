namespace WallpaperEngineCleaner;

internal class Program
{
    static AcfReader acfReader = new("E:\\SteamLibrary\\steamapps\\workshop\\appworkshop_431960.acf");

    static List<string> OutACF = new();
    static List<string> OutDIR = new();
    static List<string> RemDIR = new();

    static void Main(string[] args)
    {
        ReadACF();

        ReadDirectory();

        var result = OutDIR.Where(p => !OutACF.Any(l => p == l));
        foreach(var data in result)
        {
            RemDIR.Add(data);
        }

        Console.WriteLine("Acf count: "+ OutACF.Count);
        Console.WriteLine("Dir count: "+ OutDIR.Count);
        Console.WriteLine("RemDIR count: " + RemDIR.Count);

        DeleteDirectory(RemDIR);
    }

    static void ReadACF()
    {
        // Get all WorkShop id from ACF file to compare

        if (acfReader.CheckIntegrity())
        {
            foreach (KeyValuePair<string, ACF_Struct> kvp in acfReader.ACFFileToStruct().SubACF)
            {
                foreach (var data in kvp.Value.SubACF.Values.First().SubACF.Keys)
                {
                    OutACF.Add(data);
                }
            }    
        }
        else
        {
            Console.WriteLine("Wrong file or bad integrity");
        }
    }

    static void ReadDirectory()
    {
        // Get all WorkShop Dir to compare

        DirectoryInfo di = new DirectoryInfo("E:\\SteamLibrary\\steamapps\\workshop\\content\\431960");

        DirectoryInfo[] diArr = di.GetDirectories();

        foreach (DirectoryInfo dir in diArr)
        {
            OutDIR.Add(dir.Name);
        }
    }

    static void DeleteDirectory(List<string> DirPath)
    {
        // Remove all dir not in ACF file

        foreach (var data in DirPath)
        {
            string Path = "";
            try
            {
                Path = @$"E:\SteamLibrary\steamapps\workshop\content\431960\{data}";
                Directory.Delete(Path, true);
            }catch(UnauthorizedAccessException e)
            {
                Console.WriteLine($"File is probably readonly at this path\n{Path}");
            }
        }
    }

}
