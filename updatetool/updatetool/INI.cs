using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class INI
{
    public static string IniFilePath = @"tools\Settings.ini";
    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

    public INI()
	{

	}
    public static string ReadIni(string section, string key)
    {
        
        StringBuilder temp = new StringBuilder(255);
        int i = GetPrivateProfileString(section, key, "", temp, 255, IniFilePath);
        return temp.ToString();
    }
    public static void WriteIni(string section, string key, string value)
    {
        WritePrivateProfileString(section, key, value, IniFilePath);
    }
    public static long DeleteSection(string section)
    {
        return WritePrivateProfileString(section, null, null, IniFilePath);
    }
    public static long DeleteKey(string section, string key)
    {
        return WritePrivateProfileString(section, key, null, IniFilePath);
    }
    public static void DeleteFolder(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);     //删除文件   
                }
                else
                    DeleteFolder(d);    //删除文件夹
            }
            Directory.Delete(directoryPath);    //删除空文件夹
        }
    }
    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            FileInfo fi = new FileInfo(filePath);
            if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                fi.Attributes = FileAttributes.Normal;
            File.Delete(filePath);     //删除文件   
        }
    }
}
