﻿#if WINDOWS_DESKTOP || ANDROID || __IOS__

using System;
using System.IO;
using System.Threading.Tasks;

namespace KabeDon.Packaging
{
    /// <summary>
    /// <see cref="IStorage"/> の <see cref="File"/> 実装。
    /// <see cref="Environment.SpecialFolder.Personal"/> フォルダー内にファイルを置く。
    /// </summary>
    class FileStorage : IStorage
    {
        const string AppName = "KabeDon";

        public FileStorage(string levelName)
        {
            var personal = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            FullPath = Path.Combine(personal, AppName, levelName);
        }

        private FileStorage() { }

        public string FullPath
        {
            get { return _path; }
            private set
            {
                _path = value;

                if (!Directory.Exists(_path))
                    Directory.CreateDirectory(_path);
            }
        }
        private string _path;

        public Task<string[]> GetFilesAsync()
        {
            var files = Directory.GetFiles(FullPath);
            return Task.FromResult(files);
        }

        public Task<IStorage> GetSubfolderAsync(string folder)
        {
            var sub = Path.Combine(FullPath, folder);
            return Task.FromResult<IStorage>(new FileStorage { FullPath = sub });
        }

        public Task<Stream> OpenReadAsync(string file)
        {
            var path = Path.Combine(FullPath, file);
            return Task.FromResult<Stream>(File.OpenRead(path));
        }

        public Task<Stream> OpenReadAsync(Uri uri)
        {
            var path = uri.IsAbsoluteUri ? uri.AbsolutePath : Path.Combine(FullPath, uri.LocalPath);
            return Task.FromResult<Stream>(File.OpenRead(path));
        }

        public Task<Stream> OpenWriteAsync(string file)
        {
            var path = Path.Combine(FullPath, file);
            return Task.FromResult<Stream>(File.OpenWrite(path));
        }

        public Task<Stream> OpenWriteAsync(Uri uri)
        {
            var path = uri.IsAbsoluteUri ? uri.AbsolutePath : Path.Combine(FullPath, uri.LocalPath);
            return Task.FromResult<Stream>(File.OpenWrite(path));
        }
    }
}

#endif
