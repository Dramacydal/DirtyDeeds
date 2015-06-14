using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DD.Tools
{
    using IniKeyDictionary = Dictionary<string, string>;
    using IniSectionList = List<IniSection>;

    public class IniSection
    {
        public string Name { get { return name; } }
        public IniKeyDictionary Keys { get { return keys; } }

        public IniSection(string name)
        {
            this.name = name;
        }

        public string GetValue(string key)
        {
            if (!keys.ContainsKey(key))
                return null;

            return keys[key];
        }

        public void Add(string key, string value)
        {
            keys[key] = value;
        }

        protected IniKeyDictionary keys = new IniKeyDictionary();
        protected string name;
    }

    public class IniReader
    {
        public string Path { get { return path; } }
        public IniSectionList Sections { get { return sections; } }

        protected string path;
        protected IniSectionList sections = new IniSectionList();

        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, string Key, string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);

        public IniReader(string path)
        {
            this.path = System.IO.Directory.GetCurrentDirectory() + "\\" + path;
        }

        public void Parse()
        {
            sections.Clear();

            var res = new byte[0x4000];
            var ret = GetPrivateProfileString(null, null, "", res, res.Length, path);
            var str = Encoding.ASCII.GetString(res);
            var strSections = str.Trim('\0').Split('\0');

            foreach (var strSec in strSections)
            {
                var section = new IniSection(strSec);

                Array.Clear(res, 0, res.Length);
                ret = GetPrivateProfileString(strSec, null, "", res, res.Length, path);
                var strKeys = Encoding.ASCII.GetString(res).Trim('\0').Split('\0');
                foreach (var strKey in strKeys)
                {
                    Array.Clear(res, 0, res.Length);
                    ret = GetPrivateProfileString(strSec, strKey, "", res, res.Length, path);

                    var value = Encoding.ASCII.GetString(res).Trim('\0');

                    section.Add(strKey, value);
                }

                sections.Add(section);
            }
        }
    }
}
