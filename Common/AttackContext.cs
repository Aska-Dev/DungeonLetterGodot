using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonLetter.Common;

public class AttackContext
{
    public AttackContext(IEntity source, IEntity target, AttackModifier[] modifiers)
    {
        Source = source;
        Target = target;
        Modifiers = modifiers; 
    }

    public IEntity Source { get; private set; }
    public IEntity Target { get; private set; }
    public AttackModifier[] Modifiers { get; set; }
}
