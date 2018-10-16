using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace Talkative_Cortana
{
    class Program
    {
        /// <summary>
        /// 在注册表中应用设置并返回设置结果（是否成功）。
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
            Console.WriteLine("请务必先阅读User Guide！\n正在读取txt词条文件。扫描路径：C:\\TCTexts\\Texts.txt");
            string Content = getRandomLine();
            Console.WriteLine("\n正在应用设置……");
            if (FixReg(Content))
            {
                Console.WriteLine("\n完成！输入Y来重启explorer.exe进程，输入其他文字以退出。感谢使用！");
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
                    return;
                }
                else
                {
                    Console.WriteLine("\n您需要手动重启explorer.exe或注销登陆以更新搜索栏。");
                    return;
                }
            }
            else
            {
                Console.WriteLine("\n注册表更新异常，请检查您的系统及权限设置。");
                return;
            }
        }

        static public string getRandomLine()
        {
            string Path = @"C:\TCTexts\Texts.txt", ErrorWords = "请检查C:\\TCTexts目录下的词条文本文档。", Input = "";
            if (!File.Exists(Path)) return ErrorWords;
            StreamReader txtReader = new StreamReader(Path, Encoding.UTF8);
            List<string> Lines = new List<string>();
            while ((Input = txtReader.ReadLine()) != null)
            {
                Lines.Add(Input);
            }
            Random getRandomLine = new Random();
            return Lines.Count == 0 ? ErrorWords : Lines[getRandomLine.Next(0, Lines.Count)];
        }
    }
}
