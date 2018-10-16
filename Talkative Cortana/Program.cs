using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;

namespace Talkative_Cortana
{
    class Program
    {
        /// <summary>
        /// Set values in registry and return the result.
        /// </summary>
        /// <param name="Content"></param>
        /// <returns></returns>
        static public bool FixReg(string Content)
        {
            bool Res = false;
            RegistryKey Key = Registry.CurrentUser;
            RegistryKey Key0 = Key.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search\Flighting\0\SearchBoxText");
            RegistryKey Key1 = Key.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search\Flighting\1\SearchBoxText");
            RegistryKey CheckKey0 = Key.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search\Flighting\0\SearchBoxText");
            RegistryKey CheckKey1 = Key.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Search\Flighting\1\SearchBoxText");
            Key0.SetValue("Value", Content);
            Key1.SetValue("Value", Content);
            Key0.Close();

            Key1.Close();
            if(CheckKey0.GetValue("Value").ToString() == Content && CheckKey1.GetValue("Value").ToString() == Content)
            {
                Res = true;
                CheckKey0.Close();
                CheckKey1.Close();
            }
            return Res;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Please input words U want Cortana 2 tell.");
            string Content = Console.ReadLine();
            //Under designing to be able to get words from other sourse, maybe automatically.
            Console.WriteLine("Finished！\n\nApplying...");
            if (FixReg(Content))
            {
                Console.WriteLine("\nFinished!\n\nInput Y 2 refresh explorer.exe or any other word(s) 2 close without refreshing...");
                bool RefreshOrNot = Console.ReadLine().ToUpper() == "Y" ? true : false;
                if (RefreshOrNot)
                {
                    Process LogoffCMD = new Process();
                    LogoffCMD.StartInfo.FileName = "cmd.exe";
                    LogoffCMD.StartInfo.CreateNoWindow = true;
                    LogoffCMD.StartInfo.UseShellExecute = false;
                    LogoffCMD.StartInfo.RedirectStandardInput = true;
                    LogoffCMD.StartInfo.RedirectStandardOutput = false;
                    LogoffCMD.StartInfo.RedirectStandardError = false;
                    LogoffCMD.Start();
                    LogoffCMD.StandardInput.WriteLine("taskkill /f /im explorer.exe");
                    System.Threading.Thread.Sleep(200); 
                    //Under designing to apply a better approach to refreshing without bothering users.
                    LogoffCMD.Close();
                    Process.Start(Environment.SystemDirectory + @"\..\explorer.exe");
                }
                else return;
            }
            else Console.WriteLine("Failed 2 set values. Please check Ur permission settings.\nPress any K 2 exit.");
            Console.WriteLine("\nDone. Press any K 2 exit! TKS 4 using!");
            Console.Read();
        }
    }
}
