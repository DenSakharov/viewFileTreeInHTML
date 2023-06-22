using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml.Linq;

class Program
{
    static StringBuilder TreeScan(string sDir)
    {
        StringBuilder sb = new StringBuilder();
        //List <ElementDir> elemList = new List <ElementDir>();
        /*
        foreach (string f in Directory.GetFiles(sDir))
        {
            Element elemFile = new Element();
            elemFile.name = f;
        }
        foreach (string d in Directory.GetDirectories(sDir))
        {
            TreeScan(d);
        }
        */
        try
        {


            System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(sDir);

            ElementDir elementDir = new ElementDir();
            //sb.Append("<th>");
            elementDir.ElementsD = new List<ElementDir>();
            sb.Append("<tr><td>" + DI.FullName);
            elementDir.name = DI.Name;

            elementDir.ElementsF = new List<ElementFile>();

            System.IO.FileInfo[] FI = DI.GetFiles();
            long sunDirFiles = 0;
            for (int i = 0; i < FI.Length; ++i)
            {
                ElementFile elem = new ElementFile();
                elem.name = FI[i].FullName;
                sb.Append("<tr><td>" + FI[i].Name + "</td><td>" + FI[i].Extension + "</td><td>" + FI[i].Length + "</td></tr>");
                elem.size = FI[i].Length;
                sunDirFiles += elem.size;
                elem.typeName = FI[i].Extension;
                elementDir.ElementsF.Add(elem);
            }

            System.IO.DirectoryInfo[] SubDir = DI.GetDirectories();
            for (int i = 0; i < SubDir.Length; ++i)
            {
                ElementDir elementDir2 = new ElementDir();
                elementDir2.name = SubDir[i].Name;
                elementDir.ElementsD.Add(elementDir2);
                sb.Append(TreeScan(SubDir[i].FullName));
            }

            elementDir.size = sunDirFiles;
            sb.Append("</td></tr>");

            //sb.Append("</>");
            //elemList.Add(elementDir);
            return sb;
        }
        catch
        {
            StringBuilder sbe = new StringBuilder();
            return sbe;
        }
    }
   
    static void Main(string[] args)
    {
        //Console.ReadKey();
        string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<!DOCTYPE html>\r\n<html><style>\r\n   table,thead,th,tr,td {\r\nborder: 3px solid powderblue;\r\n margin: 50px;\r\n   }\r\n  </style>\r\n <head>\r\n  <title>html </title>\r\n  <meta charset=\"utf-8\">\r\n  <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\r\n </head>\r\n <body><table>");

        //sb.AppendLine("<thead>");
        //sb.AppendLine("< th colspan = " + 2 + " > The table header</ th >");
        //sb.AppendLine("</thead>");

        sb.AppendLine("<tbody>");
        StringBuilder sb1 = TreeScan(path);
        sb.Append(sb1);
        sb.AppendLine("</tbody>");
        sb.AppendLine("</table> </body>\r\n</html>");
        //Console.WriteLine(""+sb);
        File.WriteAllText(path+@"\test.html", sb.ToString());
          
        Console.ReadKey();
        //foreach ( ElementDir elem in list.ElementsD)
        //{
        //    Console.WriteLine(elem.name +" | "+ elem.size);
        //    foreach(ElementDir elementDir in elem.ElementsD)
        //    {

        //    }
        //}
    }
}
class ElementDir
{
    public string name { get; set; }
    public long size { get; set; }
    public List<ElementFile> ElementsF { get; set; }
    public List<ElementDir> ElementsD { get; set; }
}
class ElementFile
{
    public string name { get; set; }
    public string typeName { get; set; }
    public long size { get; set; }
}
enum TypeElem
{
    File,
    Dir
}