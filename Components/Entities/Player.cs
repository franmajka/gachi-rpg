using Utils;

namespace Entities
{
  public class Player : Creature {
    private Items.Equipment _equipment;
    private Items.Inventory _inventory;

    // Bulder pattern...
    public Player(string name, uint strength, uint agility, uint intelligence) : base(name, strength, agility, intelligence) {

    }

    protected override void InitComputed() {
      // TODO: _damage = new ComputedProperty<int>()
      _moveSpeed = new ComputedProperty<uint>(() => {
        return (uint)(Agility * MOVE_SPEED_MULTIPLIER) + _equipment.MoveSpeed;
      }, _agility, _equipment._moveSpeed);
    }
  }
}
