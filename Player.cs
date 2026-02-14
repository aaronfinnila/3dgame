using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace _3dgame {
    public class Player : GameObject {
        public float ForwardDirection { get; set; }
        public int MaxRange { get; set; }
        private Vector3 startPosition = new Vector3(0, GameConstants.HeightOffset, 0);
        public Player() : base() {
            ForwardDirection = 0.0f;
            Position = startPosition;
            MaxRange = GameConstants.MaxRange;
        }

        public void Update(KeyboardState keyboardState, float cameraYaw) {
            Vector3 futurePosition = Position;
            Vector3 movement = Vector3.Zero;

            if (keyboardState.IsKeyDown(Keys.A)) {
                movement.X = -1;
            }
            else if(keyboardState.IsKeyDown(Keys.D)) {
                movement.X = 1;
            }
            if (keyboardState.IsKeyDown(Keys.W)) {
                movement.Z = -1;
            }
            else if(keyboardState.IsKeyDown(Keys.S)) {
                movement.Z = 1;
            }

            ForwardDirection = cameraYaw;
            Matrix orientationMatrix = Matrix.CreateRotationY(ForwardDirection);

            Vector3 speed = Vector3.Transform(movement, orientationMatrix);
            speed *= GameConstants.Velocity;
            futurePosition = Position + speed;

            if (ValidateMovement(futurePosition)) {
                Position = futurePosition;
            }
        }

        private bool ValidateMovement(Vector3 futurePosition) {
            // do not allow off-terrain movement
            if ((Math.Abs(futurePosition.X) > MaxRange) || (Math.Abs(futurePosition.Z) > MaxRange)) {
                return false;
            }

            return true;
        }
    }
}