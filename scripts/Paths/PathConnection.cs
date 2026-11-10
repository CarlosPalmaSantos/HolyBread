using Godot;

namespace HolyBread.scripts.Paths;

public partial class PathConnection : Node
{
    [Export] public BaseButton A{ get; set; }
    [Export] public BaseButton B { get; set; }
    [Export] public Path Path { get; set; }

    public bool Has(BaseButton n) => n == A || n == B;
    public BaseButton GetOther(BaseButton n) =>
         n == A ? B : A;
}