using Godot;
using System;
using System.Collections.Generic;


[GlobalClass]
public partial class Computer : Node
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
}

public class FileSys
{
    int max_storage;
    int used_space;

    public static node root = new node("root", null, node.Type.Dir);
    public node active_dir;

    public FileSys(string start_dir)
    {
        node start = ReadPath(start_dir);
        if (start.type == node.Type.Dir)
        {
            active_dir = start;
        }
        else
        {
            active_dir = root; 
        }   
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
            if (current.HasChild(PathArr[i]))
            {
                current = current.GetChild(PathArr[i]);
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
        Dictionary<string, node> children;

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

        public void delete()
        {
            this.parent.children.Remove(this._name);
        }

        public void AddChild(string n, Type t)
        {
            
        }

        public bool HasChild(string child) { return this.children.ContainsKey(child); }
        public node GetChild(string child_name) { return this.children[child_name]; }
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}