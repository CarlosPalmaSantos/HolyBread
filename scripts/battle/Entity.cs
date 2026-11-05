using System;
using System.IO;
using Godot;
using HolyBread.scripts.utiles;

namespace HolyBread.scripts.battle;

[GlobalClass]
public partial class Entity : Resource, IBinary<Entity>
{
	[Export] public string Name { get; set; }
	[Export(PropertyHint.MultilineText)] public string Description { get; set; }

	[Export] public DoughType DoughType { get; set; }

	[Export] public int CurrentHp { get; set; }
	[Export] public int Hp { get; set; }
	[Export] public int Atk { get; set; }
	[Export] public int Def { get; set; }
	[Export] public int Spd { get; set; }

	[Export] public Texture2D Texture { get; set; }
	[Export] public Attack[] AttackSet { get; set; }
	[Export] public Bread[] Bread { get; set; }

	public Attack GetAttack(int attack) => AttackSet.Length > attack ? AttackSet[attack] : null;
	public Attack GetBreadAttack(int bread) => Bread.Length > bread ? Bread[bread].Attack : null;

	public void TakeDamage(float damage)
	{
		CurrentHp = Mathf.FloorToInt(Mathf.Clamp(CurrentHp - damage, 0, Hp));
		GD.Print("chp: ", CurrentHp);
	}

	public void WriteBinary(BinaryWriter writer)
	{
		
		writer.Write(Name);
		writer.Write(Description);
		DoughType.WriteBinary(writer);
		writer.Write(CurrentHp);
		writer.Write(Hp);
		writer.Write(Atk);
		writer.Write(Def);
		writer.Write(Spd);

		AttackSet.WriteBinary(writer);
		Bread.WriteBinary(writer);
	}

	public static Entity ReadBinary(BinaryReader reader)
	{
		return new Entity()
		{
			Name = reader.ReadString(),
			Description = reader.ReadString(),
			DoughType = DoughType.ReadBinary(reader),
			Hp = reader.ReadInt32(),
			Atk = reader.ReadInt32(),
			Def = reader.ReadInt32(),
			Spd = reader.ReadInt32(),
			
			AttackSet = Binary.ReadArray<Attack>(reader),
			Bread = Binary.ReadArray<Bread>(reader),
		};
	}
}
