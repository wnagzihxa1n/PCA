﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using ICSharpCode.SharpZipLib.Zip;
using System.Threading;

namespace APK_Protect_Detect
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.DragEnter += new DragEventHandler(textBox1_DragEnter);
            textBox1.DragDrop += new DragEventHandler(textBox1_DragDrop);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private bool checkJDKExist()
        {
            Process process = new Process();
            process.StartInfo.FileName = "java.exe";
            process.StartInfo.Arguments = @"-version";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
                string output = process.StandardError.ReadToEnd();
                process.WaitForExit();
                if (output.ToString().Contains("不是内部或外部命令，也不是可运行的程序"))
                {
                    return false;
                }
                else if (output.ToString().Contains("Java(TM) SE Runtime Environment"))
                {
                    return true;
                }
                else
                { 
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + Environment.NewLine);
            }
            finally
            {
                process.Close();
            }
            return false;
        }

        private void parseXML(string filePath)
        {
            if (!File.Exists(filePath))
            {
                textBox1.AppendText("不存在AndroidManifest.xml文件");
                return;
            }
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            try
            {
                process.Start();
                //textBox1.AppendText(filePath.Replace("xml", "txt") + Environment.NewLine);
                process.StandardInput.WriteLine("java.exe -jar .\\bin\\AXMLPrinter2.jar " + @filePath + " > " + @filePath.Replace("xml", "txt"));
                process.StandardInput.WriteLine("exit");
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString() + Environment.NewLine);
            }
            finally
            {
                process.Close();
            }
        }

        private void detectShell(string filePath)
        {
            string Protector_file = "";
            string judgeReason_file = "";
            string Protector_application = "";
            string judgeReason_application = "";
            textBox1.AppendText(filePath + Environment.NewLine + Environment.NewLine);
            try
            {
                ZipFile zips = new ZipFile(filePath);
                
                foreach (ZipEntry tempZip in zips)
                {
                    if (tempZip.Name.Contains("libchaosvmp.so") || tempZip.Name.Contains("libddog.so") || tempZip.Name.Contains("libfdog.so"))
                    {
                        judgeReason_file += "发现娜迦特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("娜迦")) continue;
                        Protector_file += "娜迦" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libexec.so") || tempZip.Name.Contains("libexecmain.so") || tempZip.Name.Contains("ijiami.dat"))
                    {
                        judgeReason_file += "发现爱加密特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("爱加密")) continue;
                        Protector_file += "爱加密" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libsecexe.so") || tempZip.Name.Contains("libsecmain.so"))
                    {
                        judgeReason_file += "发现梆梆特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("梆梆")) continue;
                        Protector_file += "梆梆" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libDexHelper.so") || tempZip.Name.Contains("libDexHelper-x86.so"))
                    {
                        judgeReason_file += "发现梆梆企业版特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("梆梆企业版")) continue;
                        Protector_file += "梆梆企业版" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libprotectClass.so") || tempZip.Name.Contains("libjiagu.so") || tempZip.Name.Contains("libjiagu.so") ||
                        tempZip.Name.Contains("libjiagu_art.so") || tempZip.Name.Contains("libjiagu.so") || tempZip.Name.Contains("libjiagu_x86.so") ||
                        tempZip.Name.Contains("librsprotect.so") || tempZip.Name.Contains("librsprotect_x86.so") || tempZip.Name.Contains("rsprotect.dat"))
                    {
                        judgeReason_file += "发现360加固保特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("360加固保")) continue;
                        Protector_file += "360加固保" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libegis.so") || tempZip.Name.Contains("libNSaferOnly.so"))
                    {
                        judgeReason_file += "发现通付盾特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("通付盾")) continue;
                        Protector_file += "通付盾" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libnqshield.so"))
                    {
                        judgeReason_file += "发现网秦科技特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("网秦")) continue;
                        Protector_file += "网秦科技" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libbaiduprotect.so"))
                    {
                        judgeReason_file += "发现百度特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("百度")) continue;
                        Protector_file += "百度" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("aliprotect.dat") || tempZip.Name.Contains("libsgmain.so") || tempZip.Name.Contains("libsgsecuritybody.so")
                        || tempZip.Name.Contains("cls.jar") || tempZip.Name.Contains("libmobisec.so") || tempZip.Name.Contains("libdemolish.so")
                        || tempZip.Name.Contains("libdemolishdata.so") || tempZip.Name.Contains("libsgsecuritybody.so"))
                    {
                        judgeReason_file += "发现阿里聚安全特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("阿里聚安全")) continue;
                        Protector_file += "阿里聚安全" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libtup.so") || tempZip.Name.Contains("libexec.so") || tempZip.Name.Contains("libshell.so"))
                    {
                        judgeReason_file += "发现腾讯乐固特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("腾讯乐固")) continue;
                        Protector_file += "腾讯乐固" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libtosprotection.armeabi.so") || tempZip.Name.Contains("libtosprotection.armeabi-v7a.so") || tempZip.Name.Contains("libtosprotection.x86.so"))
                    {
                        judgeReason_file += "发现腾讯御安全特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("腾讯御安全")) continue;
                        Protector_file += "腾讯御安全" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libnesec.so"))
                    {
                        judgeReason_file += "发现网易易盾特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("网易易盾")) continue;
                        Protector_file += "网易易盾" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libAPKProtect.so"))
                    {
                        judgeReason_file += "发现APKProtector特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("APKProtector")) continue;
                        Protector_file += "APKProtector" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libkwscmm.so") || tempZip.Name.Contains("libkwscr.so") || tempZip.Name.Contains("libkwslinker.so"))
                    {
                        judgeReason_file += "发现几维安全特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("几维安全")) continue;
                        Protector_file += "几维安全" + Environment.NewLine;
                    }
                    if (tempZip.Name.Contains("libx3g.so"))
                    {
                        judgeReason_file += "发现顶象科技特征文件：" + tempZip.Name + Environment.NewLine;
                        if (Protector_file.Contains("顶象科技")) continue;
                        Protector_file += "顶象科技" + Environment.NewLine;
                    }
                }
                string fileDirectory = Path.GetDirectoryName(filePath);
                string fileName = Path.GetFileName(filePath);
                ZipInputStream zipStream = new ZipInputStream(File.OpenRead(filePath));
                ZipEntry entry;
                while ((entry = zipStream.GetNextEntry()) != null)
                {
                    if (entry.Name.Equals("AndroidManifest.xml"))
                    {
                        string tempFilePath = Path.Combine(fileDirectory, entry.Name);
                        if (File.Exists(tempFilePath))
                        {
                            File.Delete(tempFilePath);
                        }
                        FileStream fileStream = File.Create(tempFilePath);
                        int dataSize = 1024;
                        byte[] data = new byte[dataSize];
                        while ((dataSize = zipStream.Read(data, 0, data.Length)) > 0)
                        {
                            fileStream.Write(data, 0, dataSize);
                        }
                        fileStream.Flush();
                        fileStream.Close();
                        break;
                    }
                }
                zipStream.Close();
                textBox1.AppendText("根据文件特征判断：" + Environment.NewLine);
                if (Protector_file.Equals(""))
                {
                    textBox1.AppendText("未发现特征文件，可能未加壳，也可能是未知壳" + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText("该APK可能被以下厂商加固" + Environment.NewLine);
                    textBox1.AppendText(Protector_file + Environment.NewLine);
                    textBox1.AppendText("判断原因：" + Environment.NewLine);
                    textBox1.AppendText(judgeReason_file + Environment.NewLine);
                }

                //解析AndroidManifest.xml文件判断入口
                textBox1.AppendText("根据Application入口判断：" + Environment.NewLine);
                if (checkJDKExist())
                {
                    parseXML(Path.Combine(fileDirectory, "AndroidManifest.xml"));
                    if (File.Exists(Path.Combine(fileDirectory, "AndroidManifest.txt")))
                    {
                        string strManifest = File.ReadAllText(Path.Combine(fileDirectory, "AndroidManifest.txt"));
                        File.Delete(Path.Combine(fileDirectory, "AndroidManifest.xml"));
                        File.Delete(Path.Combine(fileDirectory, "AndroidManifest.txt"));
                        if (strManifest.Contains("com.edog.AppWrapper"))
                        {
                            judgeReason_application += "发现娜迦Application特征：" + "com.edog.AppWrapper" + Environment.NewLine;
                            if (!Protector_application.Contains("娜迦")) 
                                Protector_application += "娜迦" + Environment.NewLine;
                        }
                        if (strManifest.Contains("com.shell.SuperApplication"))
                        {
                            judgeReason_application += "发现爱加密Application特征：" + "com.shell.SuperApplication" + Environment.NewLine;
                            if (!Protector_application.Contains("爱加密"))
                                Protector_application += "爱加密" + Environment.NewLine;
                        }
                        if (strManifest.Contains("com.secshell.shellwrapper.SecAppWrapper"))
                        {
                            judgeReason_application += "发现梆梆Application特征：" + "com.secshell.shellwrapper.SecAppWrapper" + Environment.NewLine;
                            if (!Protector_application.Contains("梆梆"))
                                Protector_application += "梆梆" + Environment.NewLine;
                        }
                        /*if (strManifest.Contains("com.secshell.shellwrapper.SecAppWrapper"))
                        {
                            judgeReason_application += "发现梆梆企业版Application特征：" + "com.secshell.shellwrapper.SecAppWrapper" + Environment.NewLine;
                            if (!Protector_application.Contains("梆梆企业版"))
                                Protector_application += "梆梆企业版" + Environment.NewLine;
                        }*/
                        if (strManifest.Contains("com.stub.StubApp"))
                        {
                            judgeReason_application += "发现360加固保Application特征：" + "com.stub.StubApp" + Environment.NewLine;
                            if (!Protector_application.Contains("360加固保"))
                                Protector_application += "360加固保" + Environment.NewLine;
                        }
                        /*if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现通付盾Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("通付盾"))
                                Protector_application += "通付盾" + Environment.NewLine;
                        }*/
                        if (strManifest.Contains("com.nqshield.NqApplication"))
                        {
                            judgeReason_application += "发现网秦科技Application特征：" + "com.nqshield.NqApplication" + Environment.NewLine;
                            if (!Protector_application.Contains("网秦科技"))
                                Protector_application += "网秦科技" + Environment.NewLine;
                        }
                        if (strManifest.Contains("baidu"))
                        {
                            judgeReason_application += "发现百度Application特征：" + "baidu" + Environment.NewLine;
                            if (!Protector_application.Contains("百度"))
                                Protector_application += "百度" + Environment.NewLine;
                        }
                        if (strManifest.Contains("com.ali.mobisecenhance.StubApplication"))
                        {
                            judgeReason_application += "发现阿里聚安全Application特征：" + "com.ali.mobisecenhance.StubApplication" + Environment.NewLine;
                            if (!Protector_application.Contains("阿里聚安全"))
                                Protector_application += "阿里聚安全" + Environment.NewLine;
                        }
                        /*if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现腾讯乐固Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("腾讯乐固"))
                                Protector_application += "腾讯乐固" + Environment.NewLine;
                        }
                        if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现腾讯御安全Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("腾讯御安全"))
                                Protector_application += "腾讯御安全" + Environment.NewLine;
                        }
                        if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现网易易盾Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("网易易盾"))
                                Protector_application += "网易易盾" + Environment.NewLine;
                        }
                        if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现APKProtectorApplication特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("APKProtector"))
                                Protector_application += "APKProtector" + Environment.NewLine;
                        }
                        if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现几维安全Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("几维安全"))
                                Protector_application += "几维安全" + Environment.NewLine;
                        }
                        if (strManifest.Contains(""))
                        {
                            judgeReason_application += "发现顶象科技Application特征：" + strManifest + Environment.NewLine;
                            if (!Protector_application.Contains("顶象科技"))
                                Protector_application += "顶象科技" + Environment.NewLine;
                        }*/
                    }
                    else
                    {
                        textBox1.AppendText("***FBI Warning***反编译AndroidManifest.xml失败" + Environment.NewLine);
                    }
                }
                else
                {
                    judgeReason_application = "未检测到Java环境，无法解析AndroidManifest.xml文件判断壳Application";
                }
                
                if (Protector_application.Equals(""))
                {
                    textBox1.AppendText("未发现特征Application，可能未加壳，也可能是未知壳" + Environment.NewLine + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText("该APK可能被以下厂商加固" + Environment.NewLine);
                    textBox1.AppendText(Protector_application + Environment.NewLine);
                    textBox1.AppendText("判断原因：" + Environment.NewLine);
                    textBox1.AppendText(judgeReason_application + Environment.NewLine);
                }
                textBox1.AppendText("Done!" + Environment.NewLine);

                if (!Protector_application.Equals("") && Protector_file.Contains(Protector_application))
                {
                    textBox1.AppendText("The mostly possible result is：" + Protector_application + Environment.NewLine);
                }
                else
                {
                    textBox1.AppendText("The mostly possible result 你自己看着办吧" + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Cannot find central directory"))
                {
                    MessageBox.Show("你的APK触发我的异常了，赔钱！" + Environment.NewLine + "或许损坏了，也或许压根不是个APK。。。。。。", "FBI Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            //MessageBox.Show(path);
            this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            textBox1.Clear();
            detectShell(path);
        }

        private void aboutAuthorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("APK Protect Detect made by 土豆夫妇", "关于作者", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "请选择要检测的APK文件";
            dialog.Filter = "APK files(*.apk)|*.apk";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                detectShell(filePath);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}

