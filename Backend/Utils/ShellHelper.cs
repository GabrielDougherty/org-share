using System.Diagnostics;

// source: https://loune.net/2017/06/running-shell-bash-commands-in-net-core/
namespace Backend.Utils
{
    public static class ShellHelper
    {
        public static string RunCommand(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/usr/bin/env",
                    Arguments = $" {escapedArgs}",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}