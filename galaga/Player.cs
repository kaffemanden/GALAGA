using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using System;

namespace galaga{
    public class Player : IGameEventProcessor<Object>
    {
        public Entity Entity {get; private set;}
        

    public Player(DynamicShape shape, IBaseImage image)
    {
        Entity = new Entity(shape, image);
    }  
    public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
        throw new NotImplementedException();
        }
    }
}
    

    

