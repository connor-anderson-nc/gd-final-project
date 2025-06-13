using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class computer : Node
{
    [ExportGroup("sys info")]
    [Export]
    string sys_name;
    [Export]
    string os_name;
    [Export]
    int storage_space;
    [Export]
    string ram_name;
    [Export]
    int ram_capacity;
    [Export]
    string processor_name;
    [Export]
    int processor_capacity;
    [Export]
    public string start_dir;


    public FileSys files;

    public override void _Ready()
    {
        files = new FileSys(start_dir);
    }

    public void test()
    {
        Console.WriteLine("this is a test function");
    }
}

public class FileSys
{
    int max_storage;
    int used_space = 0;

    // special nodes (and active dir)
    public node root;
    public node mount;
    public node active_dir;

    public FileSys(string start_dir)
    {
        root = new node("Root", null, node.Type.Dir);
        mount = new node("Mount", root, node.Type.Dir);

        node start = ReadPath(start_dir);
        if (start.type == node.Type.Dir)
        {
            active_dir = start;
        }
        else
        {
            active_dir = root;
        }

        root.children.Add(mount._name, mount);
    }

    /////////////////////
    /// command funcs ///
    /////////////////////

    public string Change_dir(string path)
    {
        try
        {
            node temp;

            try
            {
              temp = ReadPath(path);  
            }
            catch (CustomException ex) { return ex.Message; }

            if (temp.type != node.Type.Dir) { return path + " is not a directory"; }

            active_dir = temp;
            return "current path: " + path;
        }
        catch (CustomException ex)
        {
            return ex.Message;
        }
    }

    public string Make_dir(string path)
    {
        var (p, n) = StripLast(path);
        node temp_dir;

        try
        {
            temp_dir = ReadPath(p);
        }
        catch (CustomException ex) { return ex.Message; }

        if (temp_dir.type != node.Type.Dir)
        {
            return "invalid path";
        }

        temp_dir.fsAddChild(n, node.Type.Dir);
        return "created directory in " + path;
    }

    public string Remove(string path)
    {
        node target;
        try
        {
            target = ReadPath(path);
        }
        catch (CustomException ex) { return ex.Message; }

        target.fsdelete();
        return "deleted: " + path;
    }

    public string[] List(string path = null)
    {
        if (path != null)
        {
            node temp;

            try
            {
                temp = ReadPath(path);
            }
            catch (CustomException ex) { return new string[] { ex.Message }; }

            return temp.children.Keys.ToArray();
        }
        else
        {
            return active_dir.children.Keys.ToArray();
        }
    }


    // mount funcs
    public void Mount(FileSys m)
    {
        mount.children = m.root.children;
    }

    public void UnMount()
    {
        mount.children = new Dictionary<string, node>();
    }

    /////////////////////
    /// utility funcs ///
    /////////////////////

    private node ReadPath(String _Path)
    {
        CustomException InvalidPath = new CustomException("invalid Path");

        if (string.IsNullOrEmpty(_Path))
        {
            throw InvalidPath;
        }

        string[] PathArr = (_Path).Split('/');
        node current;
        switch (PathArr[0])
        {
            case "":
                current = root;
                break;
            case ".":
                current = active_dir;
                break;
            case "..":
                current = active_dir.parent ?? throw InvalidPath;
                break;
            default:
                throw InvalidPath;
        }

        // not sure about this; strong chance it doesnt work
        if (PathArr.Length == 2 && PathArr[1] == "")
        {
            return current;
        }

        for (int i = 1; i < PathArr.Length; i++)
        {
            if (current.fsHasChild(PathArr[i]))
            {
                current = current.fsGetChild(PathArr[i]);
            }
            else if (PathArr[i] == "..")
            {
                current = current.parent ?? throw InvalidPath;
            }
            else
            {
                throw InvalidPath;
            }
        }

        return current;
    }

    private (string p, string n) StripLast(string path)
    {
        string[] PathArr = path.Split('/');
        int final = PathArr.Length - 1;
        if (PathArr.Length <= 1) { return (path, null); }

        return (string.Join("/", PathArr.Take(final)), PathArr[final]);
    }

    ////////////////////
    /// node class ///
    ////////////////////

    public class node
    {
        public enum Type { Dir, BASIC, TXT, PRGM, CONF, Null }

        public string _name;
        public node parent;
        public Type type;

        /// dir vars
        public Dictionary<string, node> children;

        // file vars 
        public int size = 0;
        public object data = null;

        // funcs
        public node(string n, node p, Type t)
        {
            _name = n;
            parent = p;
            type = t;
            children = new Dictionary<string, node>();
        }

        public void fsdelete()
        {
            // Recursively delete all children
            foreach (var child in children.Values.ToList())
            {
                child.fsdelete();
            }

            // Remove self from parent's children
            if (parent != null)
            {
                parent.children.Remove(this._name);
            }

            // Clear references
            parent = null;
            children.Clear();
        }

        public void fsAddChild(string n, Type t)
        {
            this.children.Add(n, new node(n, this, t));
        }

        public bool fsHasChild(string child) { return this.children.ContainsKey(child); }
        public node fsGetChild(string child_name) { return this.children[child_name]; }
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}