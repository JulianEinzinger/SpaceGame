using System;
using Godot;

namespace FirstProject.Scripts;

public abstract partial class ParentBehaviour : Node2D
{
    protected Node2D? Parent { get; private set; }

    public override void _Ready()
    {
        Parent  = GetParent<Node2D>()
            ?? throw new InvalidOperationException("No parent found!");
    }
}