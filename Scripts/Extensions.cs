using System;
using Godot;

namespace FirstProject.Scripts;

public static class Extensions
{
    public static T GetNodeOrThrow<T>(this Node self, NodePath path) where T : Node
    {
        return self.GetNode<T>(path)
               ?? throw new InvalidOperationException
                   ($"Unable to locate node of type {typeof(T)} at path {path}");
    }
}