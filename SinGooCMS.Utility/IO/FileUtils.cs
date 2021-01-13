using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SinGooCMS.Utility
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public class FileUtils
    {
        #region �ļ�����        

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="fileContent"></param>
        public static void CreateFile(string absolutePath, string fileContent) =>
            CreateFile(absolutePath, fileContent, "utf-8");

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="strCodeType"></param>
        public static void CreateFile(string absolutePath, string fileContent, string strCodeType)
        {
            Encoding code = Encoding.GetEncoding(strCodeType);
            StreamWriter mySream = new StreamWriter(absolutePath, false, code);
            mySream.WriteLine(fileContent);
            mySream.Flush();
            mySream.Close();
        }

        /// <summary>
        /// �첽�����ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="strCodeType"></param>
        public static async void CreateFileAsync(string absolutePath, string fileContent, string strCodeType)
        {
            Encoding code = Encoding.GetEncoding(strCodeType);
            StreamWriter mySream = new StreamWriter(absolutePath, false, code);
            await mySream.WriteLineAsync(fileContent);
            await mySream.FlushAsync();
            mySream.Close();
        }

        /// <summary>
        /// ��ȡ�ļ�����
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ReadFileContent(string absolutePath, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            string fileContent = string.Empty;
            using (StreamReader sr = new StreamReader(absolutePath, encoding))
            {
                fileContent = sr.ReadToEnd();
            }
            return fileContent;

        }

        /// <summary>
        /// �첽��ȡ�ļ�����
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static async Task<string> ReadFileContentAsync(string absolutePath, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            string fileContent = string.Empty;
            using (StreamReader sr = new StreamReader(absolutePath, encoding))
            {
                fileContent = await sr.ReadToEndAsync();
            }
            return fileContent;

        }


        /// <summary>
        /// д�ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="encoding"></param>
        /// <param name="isAppend"></param>
        public static void WriteFileContent(string absolutePath, string fileContent, bool isAppend, Encoding encoding = null)
        {
            if (!File.Exists(absolutePath))
                CreateFile(absolutePath, string.Empty);

            if (encoding == null)
                encoding = Encoding.UTF8;

            StreamWriter sw = new StreamWriter(absolutePath, isAppend, encoding);
            sw.WriteLine(fileContent);
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// �첽д�ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="fileContent"></param>
        /// <param name="isAppend"></param>
        /// <param name="encoding"></param>
        public static async void WriteFileContentAsync(string absolutePath, string fileContent, bool isAppend, Encoding encoding = null)
        {
            if (!File.Exists(absolutePath))
                CreateFile(absolutePath, string.Empty);

            if (encoding == null)
                encoding = Encoding.UTF8;

            StreamWriter sw = new StreamWriter(absolutePath, isAppend, encoding);
            await sw.WriteLineAsync(fileContent);
            await sw.FlushAsync();
            sw.Close();
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        public static void DeleteFile(string absolutePath)
        {
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);
        }

        /// <summary>
        /// �������ļ�
        /// </summary>
        /// <param name="absolutePath">�ļ����ڵ�Ŀ¼,��Ҫ����Ǹ�б������E:\\Dir\\GG</param>
        /// <param name="oldName">ԭ����</param>
        /// <param name="newName">�޸ĵ�����</param>
        /// <param name="fileType">�ļ����� 0Ϊ�ļ��� 1���ļ�</param>
        /// <returns></returns>
        public static void ReNameFile(string absolutePath, string oldName, string newName, int fileType)
        {
            if (fileType.Equals(0))
            {
                if (Directory.Exists(absolutePath + "\\" + oldName))
                    Directory.Move(absolutePath + "\\" + oldName, absolutePath + "\\" + newName.Replace(".", ""));
            }
            else
            {
                if (File.Exists(absolutePath + "\\" + oldName))
                    File.Move(absolutePath + "\\" + oldName, absolutePath + "\\" + newName);
            }
        }
        /// <summary>
        /// ��ȡ�ļ���չ��
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetExtension(string fileName) => System.IO.Path.GetExtension(fileName);

        /// <summary>
        /// ��ȡ�ļ���С
        /// </summary>
        /// <param name="length"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static string GetFileSize(decimal length, string unit = null)
        {
            decimal num = 0m;
            if (unit != null) //ָ����λ
            {
                switch (unit)
                {
                    case "GB":
                        num = Math.Round((decimal)(length / (1024m * 1024m * 1024m)), 2, MidpointRounding.AwayFromZero);
                        break;
                    case "MB":
                        num = Math.Round((decimal)(length / (1024m * 1024m)), 2, MidpointRounding.AwayFromZero);
                        break;
                    case "KB":
                    default:
                        num = Math.Round((decimal)(length / 1024m), 2, MidpointRounding.AwayFromZero);
                        break;
                }
            }
            else //��ָ����λ
            {
                if (length >= 1024m * 1024m * 1024m)
                {
                    num = Math.Round((decimal)(length / (1024m * 1024m * 1024m)), 2, MidpointRounding.AwayFromZero);
                    unit = "GB";
                }
                else if (length >= 1024m * 1024m)
                {
                    num = Math.Round((decimal)(length / (1024m * 1024m)), 2, MidpointRounding.AwayFromZero);
                    unit = "MB";
                }
                else
                {
                    num = Math.Round((decimal)(length / 1024m), 2, MidpointRounding.AwayFromZero);
                    unit = "KB";
                }
            }

            return num.ToString() + unit;
        }
        #endregion

        #region Ŀ¼����

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="absoluteDir"></param>
        /// <returns></returns>
        public static void CreateDirectory(string absoluteDir)
        {
            if (!Directory.Exists(absoluteDir))
            {
                Directory.CreateDirectory(absoluteDir);
            }
        }

        /// <summary>
        /// ɾ���ļ���
        /// </summary>
        /// <param name="absoluteDir"></param>
        /// <returns></returns>
        public static void DeleteDirectory(string absoluteDir)
        {
            if (Directory.Exists(absoluteDir))
                Directory.Delete(absoluteDir, true);
        }

        /// <summary>
        /// ��ȡĿ¼����
        /// </summary>
        /// <param name="absoluteDir"></param>
        /// <returns></returns>
        public static long GetDirectoryLength(string absoluteDir)
        {
            //�жϸ�����·���Ƿ����,������������˳�
            if (!Directory.Exists(absoluteDir))
                return 0;
            long len = 0;

            //����һ��DirectoryInfo����
            DirectoryInfo di = new DirectoryInfo(absoluteDir);

            //ͨ��GetFiles����,��ȡdiĿ¼�е������ļ��Ĵ�С
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }

            //��ȡdi�����е��ļ���,���浽һ���µĶ���������,�Խ��еݹ�
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="sourcesAbsoluteDir">Դ·��</param>
        /// <param name="destAbsoluteDir">��·��</param>
        private static void CopyFolder(string sourcesAbsoluteDir, string destAbsoluteDir)
        {
            DirectoryInfo dinfo = new DirectoryInfo(sourcesAbsoluteDir);
            //ע�������洫����·�����������ļ������Բ��ܱ�������׺���ļ�                
            foreach (FileSystemInfo f in dinfo.GetFileSystemInfos())
            {
                //Ŀ��·��destName = Ŀ���ļ���·�� + ԭ�ļ����µ����ļ�(���ļ���)����                
                //Path.Combine(string a ,string b) Ϊ�ϲ������ַ���                     
                String destName = Path.Combine(destAbsoluteDir, f.Name);
                if (f is FileInfo)
                {
                    //������ļ��͸���       
                    File.Copy(f.FullName, destName, true);//true������Ը���ͬ���ļ�                     
                }
                else
                {
                    //������ļ��оʹ����ļ���Ȼ����Ȼ��ݹ鸴��              
                    Directory.CreateDirectory(destName);
                    CopyFolder(f.FullName, destName);
                }
            }
        }

        /// <summary>
        /// ����Ŀ¼��Ŀ¼�������ļ�
        /// </summary>
        /// <param name="SourcePath"></param>
        /// <param name="DestinationPath"></param>
        public static void CopyDirectory(string SourcePath, string DestinationPath)
        {
            //����Ŀ��Ŀ¼
            if (!Directory.Exists(DestinationPath))
                Directory.CreateDirectory(DestinationPath);

            //����������Ŀ¼
            foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

            //���������ļ� ���������е��ļ�
            foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
        }

        /// <summary>
        /// �ݹ��ȡĿ¼�µ������ļ���Ϣ
        /// </summary>
        /// <param name="absoluteDir"></param>
        /// <returns></returns>
        public static IEnumerable<FileInfo> GetAllFiles(string absoluteDir)
        {
            var lst = new List<FileInfo>();
            foreach (var item in new DirectoryInfo(absoluteDir).GetFiles())
                lst.Add(item);

            foreach (var dir in new DirectoryInfo(absoluteDir).GetDirectories())
                lst.AddRange(GetAllFiles(dir.FullName));

            return lst;
        }

        #endregion     

        #region ������ָ�

        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <param name="overwrite">��Ŀ���ļ�����ʱ�Ƿ񸲸�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
                throw new FileNotFoundException(sourceFileName + "�ļ������ڣ�");

            if (!overwrite && System.IO.File.Exists(destFileName))
                return false;

            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// �����ļ�,��Ŀ���ļ�����ʱ����
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }


        /// <summary>
        /// �ָ��ļ�
        /// </summary>
        /// <param name="backupFileName">�����ļ���</param>
        /// <param name="targetFileName">Ҫ�ָ����ļ���</param>
        /// <param name="backupTargetFileName">Ҫ�ָ��ļ��ٴα��ݵ�����,���Ϊnull,���ٱ��ݻָ��ļ�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                    throw new FileNotFoundException(backupFileName + "�ļ������ڣ�");

                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                        throw new FileNotFoundException(targetFileName + "�ļ������ڣ��޷����ݴ��ļ���");
                    else
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }

        #endregion

        #region �ļ���            

        /// <summary>
        /// ���ļ���Stream
        /// </summary>
        /// <param name="fileName">Ӳ���ļ�·��</param>
        /// <returns></returns>
        public static Stream ReadFileToStream(string fileName)
        {
            byte[] bytes = ReadFileToBytes(fileName);
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// ���ļ���byte[]
        /// </summary>
        /// <param name="fileName">Ӳ���ļ�·��</param>
        /// <returns></returns>
        public static byte[] ReadFileToBytes(string fileName)
        {
            FileStream pFileStream = null;
            byte[] bytes = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader r = new BinaryReader(pFileStream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //���ļ�ָ�����õ��ļ���
                bytes = r.ReadBytes((int)r.BaseStream.Length);
                return bytes;
            }
            catch
            {
                return bytes;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }

        #endregion
    }
}