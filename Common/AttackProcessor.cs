using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLetter.Common;

public class AttackContext
{
    public AttackContext(ICharacter source, ICharacter target)
    {
        Source = source;
        Target = target;
    }

    public ICharacter Source { get; private set; }
    public ICharacter Target { get; private set; }
}

public static class AttackProcessor
{
    public static void Execute(AttackContext context, AttackModifier[] modifiers)
    {
        foreach (var modifier in modifiers)
        {
            modifier.Apply(context);
        } 
    }

    public static void Execute(ICharacter source, ICharacter target, AttackModifier[] modifiers)
    {
        var context = new AttackContext(source, target);
        Execute(context, modifiers);
    }
}
