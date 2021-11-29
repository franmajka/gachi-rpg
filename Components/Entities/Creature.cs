using System;

using Utils;
namespace Entities {
  public abstract class Creature : Entity {
    protected static readonly double MOVE_SPEED_MULTIPLIER = 1.2;

    protected readonly ObservableProperty<uint> _strength;
    protected readonly ObservableProperty<uint> _agility;
    protected readonly ObservableProperty<uint> _intelligence;

    public uint Strength {
      get => _strength.Value;
      set => _strength.Value = value;
    }

    public uint Agility {
      get => _agility.Value;
      set => _agility.Value = value;
    }

    public uint Intelligence {
      get => _intelligence.Value;
      set => _intelligence.Value = value;
    }

    protected ComputedProperty<int> _damage;
    public virtual int Damage => _damage.Value;

    protected ComputedProperty<uint> _moveSpeed;
    public virtual uint MoveSpeed => _moveSpeed.Value;

    public Creature(string name, uint strength, uint agility, uint intelligence) : base(name) {
      _strength = new ObservableProperty<uint>(strength);
      _agility = new ObservableProperty<uint>(agility);
      _intelligence = new ObservableProperty<uint>(intelligence);

      InitComputed();
    }

    protected virtual void InitComputed() {
      // TODO: _damage = new ComputedProperty<int>()
      _moveSpeed = new ComputedProperty<uint>(() => (uint)(Agility * MOVE_SPEED_MULTIPLIER), _agility);
    }

    public virtual void DealDamage(IDamagable creature) {
      creature.TakeDamage(Damage);
    }
  }
}
