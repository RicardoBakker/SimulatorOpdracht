using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models {
    public class World : IObservable<Command>, IUpdatable
    {
        // Robot List wordt Model List
        private List<Robot> worldObjects = new List<Robot>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();
        
        // Recursief modellen laten inladen
        public World() {
            Robot r = CreateRobot(0,0,0);
            r.Move(4.6, 0, 13);
        }

        private Robot CreateRobot(double x, double y, double z) {
            Robot r = new Robot(x,y,z,0,0,0);
            worldObjects.Add(r);
            return r;
        }

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer)) {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command c) {
            for(int i = 0; i < this.observers.Count; i++) {
                this.observers[i].OnNext(c);
            }
        }

        // Robot wordt model
        private void SendCreationCommandsToObserver(IObserver<Command> obs) {
            foreach(Robot m3d in worldObjects) {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }
        }

        // Robot wordt model
        public bool Update(int tick)
        {
            for(int i = 0; i < worldObjects.Count; i++) {
                Robot r = worldObjects[i];

                if(r is IUpdatable) {
                    bool needsCommand = ((IUpdatable)r).Update(tick);

                    if(needsCommand) {
                        SendCommandToObservers(new UpdateModel3DCommand(r));
                    }
                }
            }
            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose() 
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}