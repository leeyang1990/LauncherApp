using System;
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

}
