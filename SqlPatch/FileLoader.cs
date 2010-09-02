﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SqlPatch {
    public class FileLoader {

        private readonly string path;

        public FileLoader(string path) {
            this.path = path;
            Files = new Dictionary<Guid, ScriptFile>();
        }

        public Dictionary<Guid, ScriptFile> Files { get; private set; }

        public void LoadAllFiles(bool loadDataScripts) {
            LoadChangeScripts();
            LoadDatabaseObjects("views\\");
            LoadDatabaseObjects("sprocs\\");
            if (loadDataScripts)
                LoadDataImportScripts("data\\");
        }

        private void LoadDataImportScripts(string subdirectory)
        {
            var scripts = new DirectoryInfo(Path.Combine(path, subdirectory));
            var files = scripts.GetFiles("*.sql", SearchOption.TopDirectoryOnly)
                .Select(x => new ScriptFile(x.FullName, x.Name, ScriptType.ChangeScript)).ToList();
            foreach (var file in files)
            {
                file.Load();
                Files.Add(file.Id, file);
            }
        }

        private void LoadChangeScripts() {
            var scripts = new DirectoryInfo(path);
            var files = scripts.GetFiles("*.sql", SearchOption.TopDirectoryOnly).OrderBy(x => {
                return Int32.Parse(x.Name.Substring(0, x.Name.IndexOf('_')));
            }).Select(x => new ScriptFile(x.FullName, x.Name, ScriptType.ChangeScript)).ToList();
            foreach (var file in files) {
                file.Load();
                Files.Add(file.Id, file);
            }
        }

        private void LoadDatabaseObjects(string subdirectory) {
            var scripts = new DirectoryInfo(Path.Combine(path, subdirectory));
            var files = scripts.GetFiles("*.sql", SearchOption.TopDirectoryOnly)
                .Select(x => new ScriptFile(x.FullName, x.Name, ScriptType.DatabaseObject)).ToList();
            foreach (var file in files) {
                file.Load();
                Files.Add(file.Id, file);
            }
        }
    }
}
