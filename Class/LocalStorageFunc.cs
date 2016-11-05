using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AcFunVideo.Class
{
   public class LocalStorageFunc
    {
        static StorageFolder APPLOCALFOLDER = ApplicationData.Current.LocalFolder;

        public async static Task<bool> SaveTextFile(string fileName,string textContent,CreationCollisionOption option=CreationCollisionOption.ReplaceExisting)
        {
            try
            {
                var file = await APPLOCALFOLDER.CreateFileAsync(fileName, option);
                await FileIO.WriteTextAsync(file, textContent);
                return true;
            }
            catch
            { 
                return false;
            }
        }

        public async static Task<bool> SaveVideoFile(string fileName,string textContent,CreationCollisionOption option=CreationCollisionOption.ReplaceExisting)
        {
            try
            {
                var acFolder = await KnownFolders.VideosLibrary.CreateFolderAsync("ACFUNVideo", CreationCollisionOption.OpenIfExists);

                var file = await acFolder.CreateFileAsync(fileName,
                    option);
                await FileIO.WriteTextAsync(file, textContent);
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public async static Task DeleteFile(string fileName)
        {
            try
            {
                var file = await APPLOCALFOLDER.GetFileAsync(fileName);
                await file.DeleteAsync();
            }
            catch 
            {
            }
        }

        public async static Task DeleteVideoFile(string fileName)
        {
            try
            {
                var acFolder = await KnownFolders.VideosLibrary.CreateFolderAsync("ACFUNVideo", CreationCollisionOption.OpenIfExists);

                var file = await acFolder.GetFileAsync(fileName);
               await file.DeleteAsync();
            }
            catch 
            {
            }
        }

        public async static Task<Stream> OpenFile(string fileName)
        {
            Stream result = null;

            try
            {
                var file = await APPLOCALFOLDER.GetFileAsync(fileName);
                result = await file.OpenStreamForReadAsync();
            }
            catch
            {
            }
            return result;
        }

        public async static Task<Stream>OpenVideoFile(string fileName)
        {
            Stream result = null;
            try
            {
                var acFolder = await KnownFolders.VideosLibrary.CreateFolderAsync("ACFUNVideo", CreationCollisionOption.OpenIfExists);

                var file = await acFolder.GetFileAsync(fileName);

                result = await file.OpenStreamForReadAsync();
            }
            catch 
            {
            }
            return result;
        }

        public async static Task<string>GetVideoFileUrl(string fileName)
        {
            string result = null;
            try
            {
                var acFolder = await KnownFolders.VideosLibrary.CreateFolderAsync("ACFUNVideo", CreationCollisionOption.OpenIfExists);

                var file = await acFolder.GetFileAsync(fileName);
                result = file.Path;
               
            }
            catch
            {
            }
            return result;
        }


        public async static Task<Stream> OpenFileForWrite(string fileName)
        {
            Stream result = null;
            try
            {
                var file = await APPLOCALFOLDER.GetFileAsync(fileName);
                result =await file.OpenStreamForWriteAsync();
            }
            catch 
            {
            }
            return result;
        }


    }
}
